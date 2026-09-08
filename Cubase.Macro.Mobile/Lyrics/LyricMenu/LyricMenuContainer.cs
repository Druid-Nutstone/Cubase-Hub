using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Mobile.Controls;
using Cubase.Macro.Mobile.Services.Lyrics;

namespace Cubase.Macro.Mobile.Lyrics.LyricMenu
{
    public class LyricMenuContainer : VerticalStackLayout
    {
        private readonly ILyricService lyricService;

        private VerticalStackLayout lyrics = new VerticalStackLayout();

        private VerticalStackLayout setlists = new VerticalStackLayout();

        private HorizontalStackLayout setListMenu = new HorizontalStackLayout() { HorizontalOptions = LayoutOptions.Fill };

        private List<Grid> setListGridList;

        private Func<LyricContainer, bool, Task> OnLyricClick;

        private Func<SetListContainer, Task> OnSetListClick;

        public LyricMenuContainer(ILyricService lyricService) : base()
        {
            this.lyricService = lyricService;
        }

        public async Task Initialise(Func<LyricContainer, bool, Task> onLyricClick, Func<SetListContainer, Task> onSetListClick)
        {
            this.Children.Clear();
            this.OnLyricClick = onLyricClick;
            this.OnSetListClick = onSetListClick;
            await this.SetupLyricMenu();
            await this.SetupSetlistMenu();
        }

        public async Task SetupTopNavigation(string title)
        {
            this.setListMenu.Clear();
            var backButton = new BaseButton()
            {
                Text = "◄",
                FontSize = 16,
                TextColor = Colors.Green,
                BackgroundColor = Color.FromArgb("#2e2e2e")
            };
            this.setListMenu.Children.Add(backButton);
            backButton.Clicked += async (sender, e) =>
            {
                this.Children.Clear();
                await this.SetupLyricMenu();
                await this.SetupSetlistMenu();
            };

            var titleLabel = new Label()
            {
                Text = title,
                FontSize = 16,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(20, 0, 5, 0)
            };
            this.setListMenu.Children.Add(titleLabel);

            this.Add(this.setListMenu);
        }

        public async Task SetupLyricMenu()
        {
            this.lyrics.Children.Clear();
            this.lyrics.Children.Add(this.GetTitle("Lyrics"));
            var availableLyricFile = await this.lyricService.GetLyricFiles();
            foreach (var file in availableLyricFile)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var fileLabel = this.GetItemButton(name);
                fileLabel.Clicked += async (s, e) =>
                {
                    await this.OnLyricClick?.Invoke(LyricContainer.Load(file, (err) => { }), true);
                };
                this.lyrics.Children.Add(fileLabel);
            }
            this.Children.Add(lyrics);
        }

        public async Task SetupSetlistMenu()
        {
            this.setlists.Children.Clear();
            this.setlists.Children.Add(this.GetTitle("Set List"));
            var availableSetLists = await this.lyricService.GetSetlists();
            foreach (var file in availableSetLists)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var fileLabel = this.GetItemButton(name);
                fileLabel.Clicked += async (s, e) =>
                {
                    var setListContainer = SetListContainer.Load(file);
                    await this.DisplaySetList(setListContainer);
                    await this.OnSetListClick?.Invoke(setListContainer);
                };
                this.setlists.Children.Add(fileLabel);
            }

            this.Children.Add(setlists);
        }

        private async Task DisplaySetList(SetListContainer setList)
        {
            this.Children.Clear();

            await this.SetupTopNavigation(setList.Title);
            this.setListGridList = new List<Grid>();
            foreach (var item in setList.Songs.OrderBy(x => x.Index))
            {
                var contentLine = new Grid() { Padding = new Thickness(4, 10, 4, 10) };
                contentLine.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                     { new ColumnDefinition() { Width = GridLength.Auto} },
                     { new ColumnDefinition() { Width = GridLength.Auto } },
                     { new ColumnDefinition() { Width = GridLength.Star } },
                };

                var itemIndex = new Label()
                {
                    Text = $"{item.Index + 1}.",
                    TextColor = Colors.Yellow,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(0, 0, 25, 0)
                };

                var pointer = this.GetPointer();

                var itemButton = new BaseButton()
                {
                    Text = item.Title,
                    TextColor = Colors.White,
                    BackgroundColor = Color.FromRgba("#2e2e2e")
                };
                contentLine.Add(itemIndex, 0);
                contentLine.Add(pointer, 1);
                contentLine.Add(itemButton, 2);
                this.Children.Add(contentLine);

                this.setListGridList.Add(contentLine);

                itemButton.Clicked += async (s, e) =>
                {
                    await SetCurrentSetListLyric(item.Index);
                    setList.CurrentSong = item.Index;
                    await this.OnLyricClick(await this.lyricService.LoadSetListLyric(item), false);
                };

            }
        }

        public async Task SetCurrentSetListLyric(int index)
        {
            if (this.setListGridList != null)
            {
                for (int i = 0; i < this.setListGridList.Count; i++)
                {
                    ((Image)setListGridList[i].Children[1]).IsVisible = i == index;
                }

            }
        }

        private Label GetTitle(string text)
        {
            return new Label()
            {
                TextColor = Colors.Yellow,
                Text = text,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start
            };
        }

        private Button GetItemButton(string text)
        {
            return new BaseButton()
            {
                Text = text,
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = CubaseMacroMobileConstants.DefaultBackgroundColour,
                TextColor = Colors.White,
            };
        }

        private Image GetPointer()
        {
            return new Image
            {
                Source = "pointer.png",
                WidthRequest = 16,
                HeightRequest = 16,
                IsVisible = false
            };
        }

    }
}
