using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LangtonsAnt_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap canvas;
        Ant ant;
        DispatcherTimer timer;
        Stopwatch sw;
        System.Drawing.Color previousColour;
        bool isProccessed;
        int stepCounter;
        int bitmapSize;
        Dictionary<System.Drawing.Color, ColourAndRotation> coloursDic;

        public MainWindow()
        {
            InitializeComponent();
            bitmapSize = 300;
            isProccessed = false;
            sw = new Stopwatch();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isProccessed)
            {
                if(stepCounter > 1000000)
                {
                    timer.Stop();
                }
                isProccessed = false;
                previousColour = canvas.GetPixel(ant.coord.X, ant.coord.Y);               
                canvas = LangtonAnt.Ant(canvas, ref ant, bitmapSize, coloursDic);
                previousColour = canvas.GetPixel(ant.coord.X, ant.coord.Y);
                canvas.SetPixel(ant.coord.X, ant.coord.Y, System.Drawing.Color.Red);
                _AntCanvas.Source = ConvertBitmap(canvas);
                canvas.SetPixel(ant.coord.X, ant.coord.Y, previousColour);
                System.Threading.Thread.Sleep(0);
                stepCounter++;
                TimeSpan ts = sw.Elapsed;
                Decimal t = Convert.ToDecimal(stepCounter / ts.TotalMilliseconds);
                _stepCounter.Content = string.Format("Step taken: {0}. @{1} steps/milisecond", stepCounter.ToString(), (Math.Round(t, 2)).ToString());
                if(stepCounter % 1000 == 0)
                {
                    canvas.Save(@"C:\Users\Jericho Masigan\Documents\Test Environment\langtonant\A " + stepCounter.ToString() + ".bmp");
                }
                isProccessed = true;
                
            }
        }

        private void InitColour()
        {
            coloursDic = new Dictionary<System.Drawing.Color, ColourAndRotation>();
            ColourAndRotation black;
            black.Colour = System.Drawing.Color.FromArgb(255, 255, 255);
            black.Direction = TurnDirection.Clockwise;

            ColourAndRotation magenta;
            magenta.Colour = System.Drawing.Color.FromArgb(255, 0, 255);
            magenta.Direction = TurnDirection.Clockwise;

            ColourAndRotation lime;
            lime.Colour = System.Drawing.Color.FromArgb(0, 255, 0);
            lime.Direction = TurnDirection.AntiClockwise;

            ColourAndRotation orangered;
            orangered.Colour = System.Drawing.Color.FromArgb(255, 69, 0);
            orangered.Direction = TurnDirection.AntiClockwise;

            ColourAndRotation cyan;
            cyan.Colour = System.Drawing.Color.FromArgb(0, 255, 255);
            cyan.Direction = TurnDirection.AntiClockwise;

            ColourAndRotation dodgerblue;
            dodgerblue.Colour = System.Drawing.Color.FromArgb(30, 144, 255);
            dodgerblue.Direction = TurnDirection.Clockwise;

            ColourAndRotation goldenrod;
            goldenrod.Colour = System.Drawing.Color.FromArgb(218, 165, 32);
            goldenrod.Direction = TurnDirection.AntiClockwise;

            ColourAndRotation white;
            white.Colour = System.Drawing.Color.FromArgb(0, 0, 0);
            white.Direction = TurnDirection.Clockwise;

            coloursDic.Add(white.Colour, black);
            coloursDic.Add(black.Colour, magenta);
            coloursDic.Add(magenta.Colour, lime);
            coloursDic.Add(lime.Colour, orangered);
            coloursDic.Add(orangered.Colour, cyan);
            coloursDic.Add(cyan.Colour, dodgerblue);
            coloursDic.Add(dodgerblue.Colour, goldenrod);
            coloursDic.Add(goldenrod.Colour, white);
        }

        public BitmapImage ConvertBitmap(Bitmap src)
        {
            System.IO.MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void _StartButton_Click(object sender, RoutedEventArgs e)
        {
            InitColour();
            isProccessed = true;
            canvas = new Bitmap(bitmapSize, bitmapSize, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics gfx = Graphics.FromImage(canvas))
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, bitmapSize, bitmapSize);
            }
            ant = new Ant(bitmapSize/2, bitmapSize/2);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            sw.Start();
            timer.Start();
        }
    }
}
