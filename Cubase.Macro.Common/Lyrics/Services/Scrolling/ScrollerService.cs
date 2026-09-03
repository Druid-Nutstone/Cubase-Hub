using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cubase.Macro.Common.Lyrics.Services.Scrolling
{
    public class ScrollerService : IScrollerService, IDisposable
    {
        private readonly IlyricMidiService _midiService;

        private PeriodicTimer? _timer;
        private CancellationTokenSource? _cts;
        private bool _isRunning;

        private List<BarTimeActivated>? _scrollTimes;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _nextBarIndex = 0;

        // Event or Action you can subscribe to from your UI layer (Windows or MAUI)
        public Action<int>? OnGotoBar;
        public Action<TimeSpan>? OnTransportUpdate;
        
        public ScrollerService(IlyricMidiService midiService)
        {
            this._midiService = midiService;

        }

        public void StartDurationTimer(LyricContainer lyricContainer, 
                          Action<int> onGotoBar, 
                          Action<TimeSpan> onTransportUpdate,
                          int intervalMilliseconds = 50)
        {
            this.OnGotoBar = onGotoBar;
            this.OnTransportUpdate = onTransportUpdate;
            // Get bars in sections ordered so lowest at the top, then sort by calculated timestamp
            var allBars = lyricContainer.GetOrderedBars();

            _scrollTimes = allBars.Select(bar => new BarTimeActivated
            {
                Bar = bar,
                BarTime = CalculateTargetTime(bar, lyricContainer.Bpm, (int)lyricContainer.TimeSignature)
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

        private async Task RunDurationScrollLoopAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (await _timer!.WaitForNextTickAsync(cancellationToken))
                {
                    if (_scrollTimes == null || _nextBarIndex >= _scrollTimes.Count)
                        continue;

                    TimeSpan elapsedPlaybackTime = _stopwatch.Elapsed;
                    this.OnTransportUpdate?.Invoke(elapsedPlaybackTime);

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