using Cubase.Macro.Common.Models;
using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Common.Socket;
using System.Diagnostics;

namespace Cubase.Macro.Common.Lyrics.Services.Scrolling
{
    public class ScrollerService : IScrollerService, IDisposable
    {
        private readonly CubaseMacroWebSocketClient _midiService;

        private PeriodicTimer? _timer;
        private CancellationTokenSource? _cts;
        private bool _isRunning;

        private List<BarTimeActivated>? _scrollTimes;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _nextBarIndex = 0;

        private IEnumerable<int> lyricBars;

        private int? lastBar;

        private LyricContainer currentLyric;

        // Event or Action you can subscribe to from your UI layer (Windows or MAUI)
        public Action<int>? OnGotoBar;
        public Action<TimeSpan, int>? OnTransportUpdate;

        public ScrollerService(CubaseMacroWebSocketClient webSocketClient)
        {
            this._midiService = webSocketClient;
        }

        public async Task StartMidiTimer(LyricContainer lyricContainer,
                  Action<int> onGotoBar,
                  Action<TimeSpan, int> onTransportUpdate,
                  int intervalMilliseconds = 1000)
        {
            this.currentLyric = lyricContainer;
            this.OnGotoBar = onGotoBar;
            this.OnTransportUpdate = onTransportUpdate;
            if (_isRunning) return;
            _isRunning = true;

            _stopwatch.Restart();

            _cts = new CancellationTokenSource();
            // A faster tick interval (e.g., 50ms) ensures snappier response times for bar scrolling
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(intervalMilliseconds));

            this.lyricBars = lyricContainer.GetOrderedBars();

            await this._midiService.StartTransportMonitoring((err => { }));

            var bar = await this.GetTransportBar();

            if (bar.HasValue)
            {
                this.OnGotoBar(bar.Value);
            }

            // Run the async loop on a background task thread
            _ = RunMidiScrollLoopAsync(_cts.Token);
        }

        public void StartDurationTimer(LyricContainer lyricContainer,
                          Action<int> onGotoBar,
                          Action<TimeSpan, int> onTransportUpdate,
                          int intervalMilliseconds = 50)
        {
            this.currentLyric = lyricContainer;
            this.OnGotoBar = onGotoBar;
            this.OnTransportUpdate = onTransportUpdate;
            // Get bars in sections ordered so lowest at the top, then sort by calculated timestamp
            var allBars = lyricContainer.GetOrderedBars();

            _scrollTimes = allBars.Select(bar => new BarTimeActivated
            {
                Bar = bar,
                BarTime = CalculateTargetTime(bar + (lyricContainer.CustomOptions.BarOffset * -1), lyricContainer.Bpm, (int)lyricContainer.TimeSignature)
            })
            .OrderBy(x => x.BarTime)
            .ToList();

            if (_isRunning) return;
            _isRunning = true;

            _nextBarIndex = 0;
            _stopwatch.Restart();

            _cts = new CancellationTokenSource();
            // A faster tick interval (e.g., 50ms) ensures snappier response times for bar scrolling
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(intervalMilliseconds));

            // Run the async loop on a background task thread
            _ = RunDurationScrollLoopAsync(_cts.Token);
        }

        private TimeSpan CalculateTargetTime(int targetBar, int bpm, int beatsPerBar = 4)
        {
            // Bars are 1-indexed, so elapsed bars before the target is (targetBar - 1)
            int elapsedBars = Math.Max(0, targetBar - 1);

            // Duration of a single beat in seconds
            double secondsPerBeat = 60.0 / bpm;

            // Duration of a single bar
            double secondsPerBar = secondsPerBeat * beatsPerBar;

            // Total offset time from the start of the song
            double targetSeconds = elapsedBars * secondsPerBar;

            return TimeSpan.FromSeconds(targetSeconds);
        }

        private async Task RunMidiScrollLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (await _timer!.WaitForNextTickAsync(cancellationToken))
                {
                    var transportBar = await this.GetTransportBar();
                    if (transportBar != null)
                    {
                        this.OnGotoBar?.Invoke(transportBar.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal graceful exit when stopping
            }
        }

        private async Task RunDurationScrollLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (await _timer!.WaitForNextTickAsync(cancellationToken))
                {
                    TimeSpan elapsedPlaybackTime = _stopwatch.Elapsed;

                    // Calculate current bar mathematically from elapsed time, BPM, and time signature
                    int currentBar = 1;
                    if (currentLyric != null && currentLyric.Bpm > 0)
                    {
                        double secondsPerBeat = 60.0 / currentLyric.Bpm;
                        double secondsPerBar = secondsPerBeat * (int)currentLyric.TimeSignature;

                        if (secondsPerBar > 0)
                        {
                            currentBar = (int)Math.Floor(elapsedPlaybackTime.TotalSeconds / secondsPerBar) + 1;
                        }
                    }

                    // Invoke transport update with both elapsed time and the calculated current bar
                    this.OnTransportUpdate?.Invoke(elapsedPlaybackTime, currentBar);

                    if (_scrollTimes == null || _nextBarIndex >= _scrollTimes.Count)
                        continue;

                    // Check if current time has reached or passed the next scheduled bar timestamp
                    while (_nextBarIndex < _scrollTimes.Count &&
                           elapsedPlaybackTime >= _scrollTimes[_nextBarIndex].BarTime)
                    {
                        var target = _scrollTimes[_nextBarIndex];

                        // Fire the callback on the UI/Subscriber side
                        OnGotoBar?.Invoke(target.Bar);

                        // Advance to the next bar in sequence
                        _nextBarIndex++;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal graceful exit when stopping
            }
        }

        private async Task<int?> GetTransportBar()
        {
            var transportLocation = await this._midiService.GetTransportLocation((err) => { });
            if (transportLocation == null) return null;
            if (this.lyricBars.Contains(transportLocation.BarBeatTime))
            {
                if (lastBar.HasValue)
                {
                    if (lastBar != transportLocation.BarBeatTime)
                    {
                        lastBar = transportLocation.BarBeatTime;
                        return transportLocation.BarBeatTime;
                    }
                    else
                    {
                        return null;
                    }
                }
                lastBar = transportLocation.BarBeatTime;
                return lastBar;
            }
            lastBar = transportLocation.BarBeatTime;
            return null;
        }

        public void Stop()
        {
            if (!_isRunning) return;
            _isRunning = false;

            _stopwatch.Stop();
            _cts?.Cancel();
            _timer?.Dispose();
            _cts?.Dispose();
        }

        public void Dispose()
        {
            Stop();
            _stopwatch.Reset();
        }
    }
}