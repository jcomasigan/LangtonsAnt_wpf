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
        public MainWindow()
        {
            InitializeComponent();
            bitmapSize = 200;
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
                isProccessed = false;
                previousColour = canvas.GetPixel(ant.coord.X, ant.coord.Y);
                canvas = LangtonAnt.Ant(canvas, ref ant, bitmapSize);
                previousColour = canvas.GetPixel(ant.coord.X, ant.coord.Y);
                canvas.SetPixel(ant.coord.X, ant.coord.Y, System.Drawing.Color.Red);
                _AntCanvas.Source = ConvertBitmap(canvas);
                canvas.SetPixel(ant.coord.X, ant.coord.Y, previousColour);
                System.Threading.Thread.Sleep(0);
                stepCounter++;
                TimeSpan ts = sw.Elapsed;
                Decimal t = Convert.ToDecimal(stepCounter / ts.TotalMilliseconds);
                _stepCounter.Content = string.Format("Step taken: {0}. @{1} steps/milisecond", stepCounter.ToString(), (Math.Round(t, 2)).ToString());
                if(stepCounter == 11000)
                {
                    canvas.Save(@"C:\Users\Jericho Masigan\Documents\Test Environment\langtonant\11000steps.bmp");
                }
                isProccessed = true;
            }
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
