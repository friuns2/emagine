using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using System.Windows.Markup;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Globalization;
using System.Text;
using System.Collections.ObjectModel;
using WaveMSS;
using cspeex;
using java.io;


namespace EmagineClient
{
    public partial class Page : UserControl
    {

        #region Private

        bool drawing = false;
        DrawingArea drawArea;
        DuplexClientHelper<DrawData> proxy;
        string userName = string.Empty;
        #endregion
        private CaptureSource _captureSource;
        private StreamAudioSink _audioSink;

        #region Ctor and Methods

        public Page()
        {
            InitializeComponent();

            if (App.Current.InstallState != InstallState.Installed)
            {
                this.LoginButtonPanel.Visibility = System.Windows.Visibility.Collapsed;
                this.InstallButtonPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.LoginButtonPanel.Visibility = System.Windows.Visibility.Visible;
                this.InstallButtonPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            
            //audio hookup
            _captureSource = new CaptureSource();
            _audioSink = new StreamAudioSink();

            //drawing stuff
            drawArea = new DrawingArea(this.DrawingCanvas);
            drawArea.Tool = CurrentTool.Pencil;
            drawArea.StrokeColor = new SolidColorBrush(Colors.Gray);
            drawArea.FillColor = new SolidColorBrush(Colors.Red);
            drawArea.StrokeWidth = 5;
            this.DataContext = drawArea;

            drawArea.DataAdded += (data) =>
            {
                try
                {
                    proxy.Client.DrawAsync(data);
                }
                catch { }
            };
        }


        /// <summary>
        /// Initialize the connection
        /// </summary>
        void InitServiceConnection()
        {
            proxy = new DuplexClientHelper<DrawData>();
            //string endPointAddress =  "http://"
            //  + App.Current.Host.Source.DnsSafeHost
            //  + ":"
            //  + App.Current.Host.Source.Port.ToString(CultureInfo.InvariantCulture)
            //  + "/DuplexDrawService.svc";

            //string endPointAddress = "http://192.168.0.102/EmagineServer/DuplexDrawService.svc";
            string endPointAddress = "http://gobind-pc/EmagineServer/DuplexDrawService.svc";

            proxy.GotNotification += (operation, data) =>
            {
                if (data != null)
                {
                    if (data.Content.StartsWith("@draw:"))
                    {
                        if (data.From != userName)
                        {
                            drawArea.AddObjects(data.Content.Substring("@draw:".Length), data.From);
                            ShowNotification(data.From + " has drawn something");
                        }
                    }
                    else if (data.Content.StartsWith("@added:"))
                    {
                        ShowNotification(data.Content.Substring("@added:".Length) + " Joined");
                    }
                }
            };

            proxy.GotError += (operation, data) => ShowNotification("Error:" + data.Message);
            proxy.Initialize(endPointAddress);
            proxy.Client.RegisterAsync(this.InputUser.Text);
        }

        /// <summary>
        /// Shows a notification from server
        /// </summary>
        /// <param name="data"></param>
        public void ShowNotification(string data)
        {
            Notification.Text = "Notification: " + data;
        }


        #endregion

        #region Events

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            this.ColorPanel.Visibility = Visibility.Collapsed;
        }

        private void ButtonTool_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null && btn.Tag is string)
            {
                drawArea.Tool = (CurrentTool)Enum.Parse(typeof(CurrentTool), btn.Tag as string, true);
            }
        }

        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawArea.PrevPoint = e.GetPosition(this.DrawingCanvas);
            drawArea.StartPoint = drawArea.PrevPoint;
            drawArea.TempHolder.Clear();

            if (drawing)
                drawing = false;
            else
            {
                drawing = true;
                var cupt = e.GetPosition(this.DrawingCanvas);
                e.Handled = true;
            }

        }

        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawing = false;
            var cupt = e.GetPosition(this.DrawingCanvas);
            drawArea.HideVirtualLine();

            cupt = drawArea.DrawOnComplete(cupt);

        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing && drawArea.PrevPoint != null)
            {
                var cupt = e.GetPosition(this.DrawingCanvas);
                cupt = drawArea.DrawOnMove(cupt);
            }

        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitServiceConnection();
                drawArea.MessageCollection.Clear();
                this.DrawingCanvas.Children.Clear();
                this.userName = InputUser.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect: " + ex.Message + ". Continuing with out connecting to server!!", "Can't Connect", MessageBoxButton.OK);
                UpdateButton.IsEnabled = false;
            }

            this.LayoutRoot.Visibility = Visibility.Visible;
            this.LoginPanel.Visibility = Visibility.Collapsed;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = this.TalkText.Text;
                this.TalkText.Text = string.Empty;
                var talk = new ScreenObject() { Text = txt, Type = ScreenObjectType.Text };
                drawArea.MessageCollection.Insert(0, new TalkItem()
                {
                    Text = talk.Text,
                    Time = DateTime.Now.ToShortTimeString(),
                    From = "Me"
                });
                var obj = new ScreenObject[] { talk };
                proxy.Client.DrawAsync(obj.JsonSerialize());
            }
            catch { }
        }

        private void ButtonColor_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas)
            {
                this.ColorPanel.Visibility = Visibility.Visible;
                ColorPanel.Tag = sender;
                ColorPicker.ColorChanged += (s, c) =>
                {
                    (ColorPanel.Tag as Canvas).Background = c.newColor;
                };
            }
        }

        private void ButtonVideo_Click(object sender, RoutedEventArgs e)
        {
            //do something
        }

        #endregion

        private void StartAudioCapture_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_captureSource != null)
            {
                _captureSource.Stop();

                _captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
                _audioSink.CaptureSource = _captureSource;

                if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                {
                    _captureSource.Start();
                }
            }
        }

        private void StopAudioCapture_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_captureSource != null)
            {
                _captureSource.Stop();

                string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "testFile.wav");
                
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                System.IO.Stream stream = System.IO.File.Create(path);

                using (stream)
                {
                    JSpeexDec decoder = new JSpeexDec();
                    decoder.setDestFormat(JSpeexDec.FILE_FORMAT_WAVE);
                    decoder.setStereo(true);

                    System.IO.Stream memStream = _audioSink.MemFile.InnerStream;
                    memStream.Position = 0;

                    decoder.decode(new RandomInputStream(memStream), new RandomOutputStream(stream));

                    stream.Close();
                }
            }
        }
        
        private void PlayAudioCapture_Button_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            Uri location = new Uri(@"http://gobind-pc/EmagineAudio/testFile.wav");
            client.OpenReadCompleted +=new OpenReadCompletedEventHandler(OnDownloadComplete);
            client.OpenReadAsync(location);
        }

        void OnDownloadComplete(object o, OpenReadCompletedEventArgs e)
        {
            try
            {
                System.IO.Stream stream = e.Result;
                WaveMediaStreamSource wave = new WaveMediaStreamSource(stream);
                this.Media.SetSource(wave);
                this.Media.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Install();
        }

        




    }
}
