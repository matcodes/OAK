using Oak.Classes.Enums;
using Oak.Controls;
using Oak.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Oak.Classes.Converters;

namespace Oak.Views
{
    #region StartPage
    public class StartPage : OakPage
    {
        #region Static members
        private static uint ANIMATION_TIME = 500;

        private static Color BUTTON_TEXT_COLOR = Color.Black;

        public static BindableProperty StateProperty = BindableProperty.Create("State", typeof(StartPageStates), typeof(StartPage), StartPageStates.Starting, BindingMode.TwoWay);
        public static BindableProperty IsConnectingVisibleProperty = BindableProperty.Create("IsConnectingVisible", typeof(bool), typeof(StartPage), false, BindingMode.TwoWay);
        #endregion

        private Grid _mainPanel = null;

        private View _startingPanel = null;
        private View _waitConnectionPanel = null;
        private View _selectProductPanel = null;
        private View _cameraHelpPanel = null;
        private View _cameraPanel = null;
        private View _scanPanel = null;
        private View _storePanel = null;
        private View _keepPanel = null;
        private View _programsPanel = null;
        private View _rescanPanel = null;
        private View _comparePanel = null;
        private View _checkPanel = null;

        private View _phone = null;
        private View _scanner = null;

        private AppActivityIndicator _connectingIndicator = null;
        private View _connectedCheck = null;

        private StackLayout _upText = null;
        private StackLayout _pairConnectDownText = null;
        private StackLayout _connectingDownText = null;
        private StackLayout _connectedDownText = null;

        private StartPageStates _oldState = StartPageStates.Starting;

        private ReverseBoolConverter _reverseBoolConverter = new ReverseBoolConverter();

        public StartPage() : base()
        {
            this.BackgroundImage = "background";

            this.SetBinding(StartPage.StateProperty, "State");

            this.BindingContext = new StartViewModel();
        }

        protected override View CreateContent()
        {
            _startingPanel = this.CreateStartingPanel();

            _mainPanel = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0
            };
            _mainPanel.Children.Add(_startingPanel);

            this.ShowStartingPanel();

            return _mainPanel;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "State")
            {
                if (this.State == StartPageStates.WaitConnection)
                    this.ShowWaitConnectionPanel();
                else if (this.State == StartPageStates.Connecting)
                    this.ShowConnectingPanel();
                else if (this.State == StartPageStates.Connected)
                    this.ShowConnectedPanel();
                else if (this.State == StartPageStates.SelectProduct)
                    this.ShowSelectProductPanel();
                else if (this.State == StartPageStates.CameraHelp)
                    this.ShowCameraHelpPanel();
                else if (this.State == StartPageStates.Camera)
                    this.ShowCameraPanel();
                else if (this.State == StartPageStates.Scan)
                    this.ShowScanPanel();
                else if (this.State == StartPageStates.Store)
                    this.ShowStorePanel();
                else if (this.State == StartPageStates.Keep)
                    this.ShowKeepPanel();
                else if (this.State == StartPageStates.Programs)
                    this.ShowProgramsPanel();
                else if (this.State == StartPageStates.Rescan)
                    this.ShowRescanPanel();
                else if (this.State == StartPageStates.Compare)
                    this.ShowComparePanel();
                else if (this.State == StartPageStates.Check)
                    this.ShowCheckPanel();

                if ((this.State == StartPageStates.Keep) || (this.State == StartPageStates.Programs) || 
                    (this.State == StartPageStates.Compare) || (this.State == StartPageStates.Check))
                    _oldState = this.State;

            }
        }

        protected override bool OnBackButtonPressed()
        {
            var result = false;
            if (this.State == StartPageStates.CameraHelp)
            {
                this.State = StartPageStates.SelectProduct;
                result = true;
            }
            else if (this.State == StartPageStates.Camera)
            {
                this.State = StartPageStates.CameraHelp;
                result = true;
            }
            else if (this.State == StartPageStates.Scan)
            {
                this.State = StartPageStates.Camera;
                result = true;
            }
            else if (this.State == StartPageStates.Store)
            {
                this.State = StartPageStates.Scan;
                result = true;
            }
            else if (this.State == StartPageStates.Keep)
            {
                this.State = StartPageStates.Store;
                result = true;
            }
            else if (this.State == StartPageStates.Programs)
            {
                this.State = StartPageStates.Keep;
                result = true;
            }
            else if (this.State == StartPageStates.Rescan)
            {
                this.State = _oldState;
                result = true;
            }
            else if (this.State == StartPageStates.Compare)
            {
                this.State = StartPageStates.Store;
                result = true;
            }
            else if (this.State == StartPageStates.Check)
            {
                this.State = StartPageStates.Compare;
                result = true;
            }
            return result;
        }

        private void ShowStartingPanel()
        {
            Task.Run(() => {
                Task.Delay(500).Wait();
                _startingPanel.FadeTo(1, ANIMATION_TIME);

                _waitConnectionPanel = this.CreateWaitConnectionPanel();
                this.AddView(_waitConnectionPanel, false);

                _selectProductPanel = this.CreateSelectProductPanel();
                this.AddView(_selectProductPanel, false);

                _cameraHelpPanel = this.CreateCameraHelpPanel();
                this.AddView(_cameraHelpPanel, false);

                _cameraPanel = this.CreateCameraPanel();
                this.AddView(_cameraPanel, false);

                _scanPanel = this.CreateScanPanel();
                this.AddView(_scanPanel, false);

                _storePanel = this.CreateStorePanel();
                this.AddView(_storePanel, false);

                _keepPanel = this.CreateKeepPanel();
                this.AddView(_keepPanel, false);

                _programsPanel = this.CreateProgramsPanel();
                this.AddView(_programsPanel, false);

                _rescanPanel = this.CreateRescanPanel();
                this.AddView(_rescanPanel, false);

                _comparePanel = this.CreateComparePanel();
                this.AddView(_comparePanel, false);

                _checkPanel = this.CreateCheckPanel();
                this.AddView(_checkPanel, true);
            });
        }

        private void AddView(View view, bool isAll)
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.Children.Add(view);

                if (isAll)
                {
                    _mainPanel.RaiseChild(_startingPanel);
                    this.State = StartPageStates.WaitConnection;
                }
            });
        }

        private void ShowWaitConnectionPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.LowerChild(_startingPanel);
                _mainPanel.RaiseChild(_waitConnectionPanel);
                _startingPanel.FadeTo(0, ANIMATION_TIME);
                _waitConnectionPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowConnectingPanel()
        {
            _pairConnectDownText.FadeTo(0, ANIMATION_TIME);
            _connectingDownText.FadeTo(1, ANIMATION_TIME);
            this.MovePhone();
            this.MoveScanner();
        }

        private void ShowConnectedPanel()
        {
            Task.Run(() => {
                _connectingDownText.FadeTo(0, ANIMATION_TIME);
                _connectingIndicator.FadeTo(0, ANIMATION_TIME);
                _connectedDownText.FadeTo(1, ANIMATION_TIME);
                _connectedCheck.FadeTo(1, ANIMATION_TIME);
                Task.Delay(3000).Wait();
                _connectedCheck.FadeTo(0, ANIMATION_TIME);
                this.IsConnectingVisible = false;
                _upText.LayoutTo(new Rectangle(_upText.Bounds.Left, 0 - _upText.Bounds.Height, _upText.Bounds.Width, _upText.Bounds.Height), ANIMATION_TIME);
                _connectedDownText.LayoutTo(new Rectangle(_connectedDownText.Bounds.Left, this.Height, _connectedDownText.Bounds.Width, _connectedDownText.Bounds.Height), ANIMATION_TIME);
                _phone.LayoutTo(new Rectangle(0 - _phone.Bounds.Width, _phone.Bounds.Top, _phone.Bounds.Width, _phone.Bounds.Height), ANIMATION_TIME);
                _scanner.LayoutTo(new Rectangle(this.Width, _scanner.Bounds.Top, _scanner.Bounds.Width, _scanner.Bounds.Height), ANIMATION_TIME);
                _upText.FadeTo(0, ANIMATION_TIME);
                _connectedDownText.FadeTo(0, ANIMATION_TIME);
                _scanner.FadeTo(0, ANIMATION_TIME);
                this.FadePhone();
                _mainPanel.LowerChild(_waitConnectionPanel);
            });
        }

        private void ShowSelectProductPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_selectProductPanel);
                _cameraHelpPanel.FadeTo(0, ANIMATION_TIME);
                _selectProductPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowCameraHelpPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_cameraHelpPanel);
                _selectProductPanel.FadeTo(0, ANIMATION_TIME);
                _cameraPanel.FadeTo(0, ANIMATION_TIME);
                _cameraHelpPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowCameraPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_cameraPanel);
                _cameraHelpPanel.FadeTo(0, ANIMATION_TIME);
                _scanPanel.FadeTo(0, ANIMATION_TIME);
                _cameraPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowScanPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_scanPanel);
                _cameraPanel.FadeTo(0, ANIMATION_TIME);
                _storePanel.FadeTo(0, ANIMATION_TIME);
                _scanPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowStorePanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_storePanel);
                _scanPanel.FadeTo(0, ANIMATION_TIME);
                _rescanPanel.FadeTo(0, ANIMATION_TIME);
                _keepPanel.FadeTo(0, ANIMATION_TIME);
                _comparePanel.FadeTo(0, ANIMATION_TIME);
                _storePanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowKeepPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_keepPanel);
                _storePanel.FadeTo(0, ANIMATION_TIME);
                _programsPanel.FadeTo(0, ANIMATION_TIME);
                _keepPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowProgramsPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_programsPanel);
                _keepPanel.FadeTo(0, ANIMATION_TIME);
                _rescanPanel.FadeTo(0, ANIMATION_TIME);
                _programsPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowRescanPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_rescanPanel);
                _keepPanel.FadeTo(0, ANIMATION_TIME);
                _comparePanel.FadeTo(0, ANIMATION_TIME);
                _programsPanel.FadeTo(0, ANIMATION_TIME);
                _checkPanel.FadeTo(0, ANIMATION_TIME);
                _rescanPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowComparePanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_comparePanel);
                _storePanel.FadeTo(0, ANIMATION_TIME);
                _checkPanel.FadeTo(0, ANIMATION_TIME);
                _comparePanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void ShowCheckPanel()
        {
            Device.BeginInvokeOnMainThread(() => {
                _mainPanel.RaiseChild(_checkPanel);
                _comparePanel.FadeTo(0, ANIMATION_TIME);
                _rescanPanel.FadeTo(0, ANIMATION_TIME);
                _checkPanel.FadeTo(1, ANIMATION_TIME);
            });
        }

        private void MovePhone()
        {
            Task.Run(async() => {
                await _phone.LayoutTo(new Rectangle(_phone.Bounds.Left + 85, _phone.Bounds.Top, _phone.Bounds.Width, _phone.Bounds.Width), ANIMATION_TIME);
            });
        }

        private void MoveScanner()
        {
            Task.Run(async () => {
                await _scanner.LayoutTo(new Rectangle(_scanner.Bounds.Left - 85, _scanner.Bounds.Top, _scanner.Bounds.Width, _scanner.Bounds.Width), ANIMATION_TIME);
                this.IsConnectingVisible = true;
            });
        }

        private void FadePhone()
        {
            Task.Run(async () => {
                await _phone.FadeTo(0, ANIMATION_TIME);
                this.State = StartPageStates.SelectProduct;
            });
        }

        private View CreateStartingPanel()
        {
            var logoWidth = this.Width / 5 * 3;
            var logoHeight = logoWidth / 2;

            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = logoWidth,
                HeightRequest = logoHeight,
                Aspect = Aspect.AspectFit,
                Source = "logo"
            };

            var activityIndicator = new AppActivityIndicator {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                IndicatorColor = AppActivityIndicatorColors.Green,
                IsRunning = true
            };

            var loading = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Text = "LOADING",
                AppFont = AppFonts.MontserratBold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#15FF15")
            };

            var bottom = this.Height / 5;

            var stack = new StackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Spacing = 10,
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(0, 0, 0, bottom)
            };
            stack.Children.Add(activityIndicator);
            stack.Children.Add(loading);

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Opacity = 0
            };
            grid.Children.Add(logo);
            grid.Children.Add(stack);

            return grid;
        }

        private View CreateWaitConnectionPanel()
        {
            var welcome = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratLight,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#A1A1A1"),
                Text = "WELCOME TO"
            };

            var oak = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratBold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2.5,
                TextColor = Color.FromHex("#FFFFFF"),
                Text = "O A K"
            };

            _upText = new StackLayout {
               HorizontalOptions = LayoutOptions.Center,
               VerticalOptions = LayoutOptions.Center,
               Orientation = StackOrientation.Vertical,
               Spacing = 0
            };
            _upText.Children.Add(welcome);
            _upText.Children.Add(oak);

            _phone = new Image {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Source = "circle_phone"
            };
            _phone.SetBinding(Image.IsVisibleProperty, new Binding("IsConnectingVisible", BindingMode.OneWay, _reverseBoolConverter, null, null, this));
            _phone.SizeChanged += (sender, args) => {
                _phone.LayoutTo(new Rectangle(_phone.Bounds.Left - 50, _phone.Bounds.Top, _phone.Bounds.Width, _phone.Bounds.Width), 500);
            };

            _scanner = new Image {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Source = "circle_scaner"
            };
            _scanner.SetBinding(Image.IsVisibleProperty, new Binding("IsConnectingVisible", BindingMode.OneWay, _reverseBoolConverter, null, null, this));
            _scanner.SizeChanged += (sender, args) => {
                _scanner.LayoutTo(new Rectangle(_scanner.Bounds.Left + 50, _scanner.Bounds.Top, _scanner.Bounds.Width, _scanner.Bounds.Width), 500);
            };

            var connection = new Image {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Source = "connect"
            };
            connection.SetBinding(Image.IsVisibleProperty, new Binding("IsConnectingVisible", BindingMode.OneWay, null, null, null, this));

            _connectingIndicator = new AppActivityIndicator {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IndicatorColor = AppActivityIndicatorColors.Black,
                IsRunning = true
            };
            _connectingIndicator.SetBinding(Image.IsVisibleProperty, new Binding("IsConnectingVisible", BindingMode.OneWay, null, null, null, this));

            _connectedCheck = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0,
                Source = "check"
            };

            var waitConnectionContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };
            waitConnectionContent.Children.Add(_phone);
            waitConnectionContent.Children.Add(_scanner);
            waitConnectionContent.Children.Add(connection);
            waitConnectionContent.Children.Add(_connectingIndicator);
            waitConnectionContent.Children.Add(_connectedCheck);

            var pairConnectFirstLabel = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratLight,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#A1A1A1"),
                Text = "Please turn on your scanner and"
            };

            var pairConnectSecondLabel = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratLight,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#A1A1A1"),
                Text = "pair with your smartphone"
            };

            var pairConnectStartConnection = new AppTextCommand {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                AppFont = AppFonts.MontserratBold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#15FF15"),
                Text = "PAIR WITH SCANNER"
            };
            pairConnectStartConnection.SetBinding(AppTextCommand.CommandProperty, "StartConnectionCommand");

            var pairConnectStartConnectionContent = new ContentView {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 20, 0, 0),
                Content = pairConnectStartConnection
            };

            _pairConnectDownText = new StackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Orientation = StackOrientation.Vertical
            };
            _pairConnectDownText.Children.Add(pairConnectFirstLabel);
            _pairConnectDownText.Children.Add(pairConnectSecondLabel);
            _pairConnectDownText.Children.Add(pairConnectStartConnectionContent);

            var connecting = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratBold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#15FF15"),
                Text = "CONNECTING..."
            };

            _connectingDownText = new StackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0,
                Spacing = 0,
                Orientation = StackOrientation.Vertical
            };
            _connectingDownText.Children.Add(connecting);

            var connectedFirstLabel = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratLight,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#A1A1A1"),
                Text = "Scanner has been"
            };

            var connectedSecondLabel = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                AppFont = AppFonts.MontserratLight,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#A1A1A1"),
                Text = "successfully connected"
            };

            var connected = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                AppFont = AppFonts.MontserratBold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromHex("#15FF15"),
                Text = "CONNECTED"
            };

            var connectedContent = new ContentView {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 20, 0, 0),
                Content = connected
            };

            _connectedDownText = new StackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Orientation = StackOrientation.Vertical,
                Opacity = 0
            };
            _connectedDownText.Children.Add(connectedFirstLabel);
            _connectedDownText.Children.Add(connectedSecondLabel);
            _connectedDownText.Children.Add(connectedContent);

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill, 
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 5,
                ColumnSpacing = 0,
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(_upText, 0, 0);
            grid.Children.Add(waitConnectionContent, 0, 1);
            grid.Children.Add(_pairConnectDownText, 0, 2);
            grid.Children.Add(_connectingDownText, 0, 2);
            grid.Children.Add(_connectedDownText, 0, 2);

            return grid;
        }

        private View CreateSelectProductPanel()
        {
            #region First line
            var pageLogo = new Image {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Source = "page_icon"
            };

            var oak = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                Text = "O A K"
            };

            var firstRowContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = new Thickness(40, 40, 40, 20)
            };
            firstRowContent.Children.Add(pageLogo);
            firstRowContent.Children.Add(oak);
            #endregion

            #region Second line
            var text = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                Text = "SELECT PRODUCT"
            };

            var textContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 20, 40, 20),
                Content = text
            };
            #endregion

            #region Buttons
            var alcohol = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "ALCOHOL"
            };
            alcohol.SetBinding(AppButton.CommandProperty, "AlcoholCommand");

            var milk = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "MILK"
            };
            milk.SetBinding(AppButton.CommandProperty, "MilkCommand");

            var water = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "WATER"
            };
            water.SetBinding(AppButton.CommandProperty, "WaterCommand");

            var oil = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "OIL"
            };
            oil.SetBinding(AppButton.CommandProperty, "OilCommand");

            var buttonsContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill, 
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(80, 0, 80, 0),
                Spacing = 30
            };
            buttonsContent.Children.Add(alcohol);
            buttonsContent.Children.Add(milk);
            buttonsContent.Children.Add(water);
            buttonsContent.Children.Add(oil);
            #endregion

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(firstRowContent, 0, 0);
            grid.Children.Add(textContent, 0, 1);
            grid.Children.Add(buttonsContent, 0, 2);

            return grid;
        }

        private View CreateCameraHelpPanel()
        {
            #region First line
            var pageLogo = new Image {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Source = "page_icon"
            };

            var oak = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                Text = "O A K"
            };

            var firstRowContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = new Thickness(40, 40, 40, 20)
            };
            firstRowContent.Children.Add(pageLogo);
            firstRowContent.Children.Add(oak);
            #endregion

            #region Second line
            var firstText = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                Text = "POINT YOUR CAMERA"
            };

            var secondText = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                Text = "AT PRODUCT"
            };

            var textContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 20, 0, 20),
                Orientation = StackOrientation.Vertical
            };
            textContent.Children.Add(firstText);
            textContent.Children.Add(secondText);
            #endregion

            #region Image
            var image = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "camera_help"
            };
            #endregion

            #region Command line
            var takePhoto = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "TAKE A PICTURE"
            };
            takePhoto.SetBinding(AppButton.CommandProperty, "TakePhotoCommand");

            var takePhotoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(80, 40, 80, 40),
                Content = takePhoto
            };
            #endregion

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            grid.Children.Add(firstRowContent, 0, 0);
            grid.Children.Add(textContent, 0, 1);
            grid.Children.Add(image, 0, 2);
            grid.Children.Add(takePhotoContent, 0, 3);

            return grid;
        }

        private View CreateCameraPanel()
        {
            #region First line
            var pageLogo = new Image {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Source = "page_icon"
            };

            var oak = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                Text = "O A K"
            };

            var firstRowContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                RowSpacing = 0,
                ColumnSpacing = 0,
                Padding = new Thickness(40, 40, 40, 20)
            };
            firstRowContent.Children.Add(pageLogo);
            firstRowContent.Children.Add(oak);
            #endregion

            #region Second line
            var firstText = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                Text = "POINT YOUR CAMERA"
            };

            var secondText = new AppLabel {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White,
                Text = "AT PRODUCT"
            };

            var textContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 20, 0, 20),
                Orientation = StackOrientation.Vertical
            };
            textContent.Children.Add(firstText);
            textContent.Children.Add(secondText);
            #endregion

            #region Camera
            var camera = new AppCameraPreview {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var cameraContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Padding = new Thickness(0, 5, 0, 0),
                BackgroundColor = Color.FromHex("#00C500"),
                Content = camera
            };
            #endregion

            #region Command line
            var takePhoto = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "TAKE A PICTURE"
            };
            takePhoto.SetBinding(AppButton.CommandProperty, "TakePhotoCommand");

            var takePhotoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(80, 40, 80, 40),
                Content = takePhoto
            };
            #endregion

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            grid.Children.Add(firstRowContent, 0, 0);
            grid.Children.Add(textContent, 0, 1);
            grid.Children.Add(cameraContent, 0, 2);
            grid.Children.Add(takePhotoContent, 0, 3);

            return grid;
        }

        private View CreateScanPanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var scan = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "SCAN"
            };
            scan.SetBinding(AppButton.CommandProperty, "ScanCommand");

            var scanContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(80, 40, 80, 40),
                Orientation = StackOrientation.Vertical
            };
            scanContent.Children.Add(scan);
            #endregion

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(scanContent, 0, 1);

            return grid;
        }

        private View CreateStorePanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var store = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "STORE"
            };
            store.SetBinding(AppButton.CommandProperty, "StoreCommand");

            var test = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "TEST"
            };
            test.SetBinding(AppButton.CommandProperty, "TestCommand");

            var storeContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(80, 40, 80, 40),
                Spacing = 30,
                Orientation = StackOrientation.Vertical
            };
            storeContent.Children.Add(store);
            storeContent.Children.Add(test);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(storeContent, 0, 1);

            return grid;
        }

        private View CreateKeepPanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var keepInPhone = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "KEEP IN PHONE"
            };
            keepInPhone.SetBinding(AppButton.CommandProperty, "KeepInPhoneCommand");

            var transferFile = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "TRANSFER FILE"
            };
            transferFile.SetBinding(AppButton.CommandProperty, "TransferFileCommand");

            var keepContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(80, 40, 80, 40),
                Spacing = 30,
                Orientation = StackOrientation.Vertical
            };
            keepContent.Children.Add(keepInPhone);
            keepContent.Children.Add(transferFile);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(keepContent, 0, 1);

            return grid;
        }

        private View CreateProgramsPanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var program1 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "1"
            };
            program1.SetBinding(AppButton.CommandProperty, "Program1Command");

            var program2 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "2"
            };
            program2.SetBinding(AppButton.CommandProperty, "Program2Command");

            var program3 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "3"
            };
            program3.SetBinding(AppButton.CommandProperty, "Program3Command");

            var program4 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "4"
            };
            program4.SetBinding(AppButton.CommandProperty, "Program4Command");

            var program5 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "5"
            };
            program5.SetBinding(AppButton.CommandProperty, "Program5Command");

            var program6 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "6"
            };
            program6.SetBinding(AppButton.CommandProperty, "Program6Command");

            var program7 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "7"
            };
            program7.SetBinding(AppButton.CommandProperty, "Program7Command");

            var program8 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "8"
            };
            program8.SetBinding(AppButton.CommandProperty, "Program8Command");

            var programsContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(40, 0, 40, 0),
                RowSpacing = 30,
                ColumnSpacing = 40,
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            programsContent.Children.Add(program1, 0, 0);
            programsContent.Children.Add(program2, 1, 0);
            programsContent.Children.Add(program3, 0, 1);
            programsContent.Children.Add(program4, 1, 1);
            programsContent.Children.Add(program5, 0, 2);
            programsContent.Children.Add(program6, 1, 2);
            programsContent.Children.Add(program7, 0, 3);
            programsContent.Children.Add(program8, 1, 3);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(programsContent, 0, 1);

            return grid;
        }

        private View CreateRescanPanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var rescan = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "RESCAN"
            };
            rescan.SetBinding(AppButton.CommandProperty, "RescanCommand");

            var close = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "CLOSE"
            };
            close.SetBinding(AppButton.CommandProperty, "CloseCommand");

            var rescanContent = new StackLayout {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(80, 40, 80, 40),
                Spacing = 30,
                Orientation = StackOrientation.Vertical
            };
            rescanContent.Children.Add(rescan);
            rescanContent.Children.Add(close);
            #endregion

            var grid = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(rescanContent, 0, 1);

            return grid;
        }

        private View CreateComparePanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var compare = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "COMPARE"
            };
            compare.SetBinding(AppButton.CommandProperty, "CompareCommand");

            var transferFile = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "TRANSFER FILE"
            };
            transferFile.SetBinding(AppButton.CommandProperty, "TransferFileCommand");

            var compareContent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(80, 40, 80, 40),
                Spacing = 30,
                Orientation = StackOrientation.Vertical
            };
            compareContent.Children.Add(compare);
            compareContent.Children.Add(transferFile);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(compareContent, 0, 1);

            return grid;
        }

        private View CreateCheckPanel()
        {
            #region Logo
            var logo = new Image {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Source = "icon"
            };

            var logoContent = new ContentView {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(40, 40, 40, 40),
                Content = logo
            };
            #endregion

            #region Buttons
            var program1 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "1"
            };
            program1.SetBinding(AppButton.CommandProperty, "Program1Command");

            var program2 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "2"
            };
            program2.SetBinding(AppButton.CommandProperty, "Program2Command");

            var program3 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "3"
            };
            program3.SetBinding(AppButton.CommandProperty, "Program3Command");

            var program4 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "4"
            };
            program4.SetBinding(AppButton.CommandProperty, "Program4Command");

            var program5 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "5"
            };
            program5.SetBinding(AppButton.CommandProperty, "Program5Command");

            var program6 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "6"
            };
            program6.SetBinding(AppButton.CommandProperty, "Program6Command");

            var program7 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "7"
            };
            program7.SetBinding(AppButton.CommandProperty, "Program7Command");

            var program8 = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "8"
            };
            program8.SetBinding(AppButton.CommandProperty, "Program8Command");

            var check = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "CHECK"
            };
            check.SetBinding(AppButton.CommandProperty, "CheckCommand");

            var next = new AppButton {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                AppFont = AppFonts.MontserratRegular,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = BUTTON_TEXT_COLOR,
                Text = "NEXT"
            };
            next.SetBinding(AppButton.CommandProperty, "NextCommand");

            var checkContent = new Grid {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(40, 0, 40, 0),
                RowSpacing = 10,
                ColumnSpacing = 40,
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };
            checkContent.Children.Add(program1, 0, 0);
            checkContent.Children.Add(program2, 1, 0);
            checkContent.Children.Add(program3, 0, 1);
            checkContent.Children.Add(program4, 1, 1);
            checkContent.Children.Add(program5, 0, 2);
            checkContent.Children.Add(program6, 1, 2);
            checkContent.Children.Add(program7, 0, 3);
            checkContent.Children.Add(program8, 1, 3);
            checkContent.Children.Add(check, 0, 4);
            checkContent.Children.Add(next, 1, 4);
            #endregion

            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowSpacing = 0,
                ColumnSpacing = 0,
                BackgroundColor = Color.FromHex("#3D3D3D"),
                Opacity = 0,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                }
            };
            grid.Children.Add(logoContent, 0, 0);
            grid.Children.Add(checkContent, 0, 1);

            return grid;
        }

        public new StartViewModel ViewModel
        {
            get { return (this.BindingContext as StartViewModel); }
        }

        public StartPageStates State
        {
            get { return (StartPageStates)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        public bool IsConnectingVisible
        {
            get { return (bool)this.GetValue(IsConnectingVisibleProperty); }
            set { this.SetValue(IsConnectingVisibleProperty, value); }
        }
    }
    #endregion
}
