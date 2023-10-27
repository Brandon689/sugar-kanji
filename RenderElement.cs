using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace SugarKanji
{
    public class RenderElement : FrameworkElement
    {
        DispatcherTimer slideshow = new();
        DispatcherTimer slideshow2 = new();
        public string frmTextKanji="z";
        public string ALL ="z";
        double left = 0;
        double top = -55;
        double wi;
        double hi;
        int fontsize = 360;
        string engl="x";
        double opac=1;
        double minus = 0.0038;
        double initialMinus = 0.00001;
        public RenderElement()
        {
            slideshow.Interval = new TimeSpan(0, 0, 0, 0, 26);
            slideshow.Tick += Slideshow_Tick;

            slideshow2.Interval = new TimeSpan(0, 0, 0, 11, 500);
            slideshow2.Tick += Slideshow2_Tick;
        }

        private void Slideshow2_Tick(object? sender, EventArgs e)
        {
            slideshow2.Stop();
            minus = -0.04;
            opac = 0.01;
            slideshow.Start();
        }

        private void Slideshow_Tick(object? sender, EventArgs e)
        {
            if (minus < 0 && opac > .85)
                slideshow.Stop();
                //return;
            opac -= minus;
            minus += 0.000019;

            if (minus > 0.00015)
            {
                minus += 0.00001;
            }
            if (opac <= 0)
            {
                slideshow.Stop();
            }
            else
            {// disabled move temp
                //top += 0.11;
            }
            InvalidateVisual();
        }

        public void Rezero(string yagamisan, double w, double h, string eng)
        {
            opac = 1;
            top = -55;
            minus = initialMinus;
            slideshow.Start();
            slideshow2.Start();
            wi = w;
            hi = h;
            engl = eng;
            frmTextKanji = yagamisan;
            //Console.WriteLine(w);
            if (w < 370)
            {
                fontsize = 280;
                left = -5;
            }
            else if (w < 400)
            {
                fontsize = 290;
                left = -1;
            }
            else if (w > 400)
            {
                fontsize = 350;
                left = 0;
            }
            else if (w > 450)
            {
                fontsize = 390;
                left = 0;
            }
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            FormattedText text = new FormattedText(frmTextKanji, Thread.CurrentThread.CurrentUICulture,
                FlowDirection.LeftToRight, new Typeface("Noto Sans JP"), fontsize, Brushes.Black, 2.5);

            //main
            FormattedText engtext = new FormattedText(engl,
               Thread.CurrentThread.CurrentUICulture,
               FlowDirection.LeftToRight,
               //new Typeface("UD Digi Kyokasho N-B"),
               new Typeface("Noto Sans JP"),
               15,
               Brushes.Black, 2.5);

            var width = text.Width;
            left = (wi - width) / 2;
      
            Geometry textGeometry = text.BuildGeometry(new Point(left, top));
            Rect rx = new(0, 0, 1000, 1000);
            RectangleGeometry r = new(rx);
            //Console.WriteLine(top);
            var geometry = Geometry.Combine(textGeometry, r, GeometryCombineMode.Xor, null);
                  
            //if (opac < 0.7 || minus < 0)
            if (minus < 0)
            {
                Geometry textGeometry2 = engtext.BuildGeometry(new Point(14, hi - 25));
                geometry = Geometry.Combine(geometry, textGeometry2, GeometryCombineMode.Xor, null);
            }

            //test
            //string res = string.Join("   ", ALL.ToCharArray());
            //FormattedText text3 = new FormattedText(res, Thread.CurrentThread.CurrentUICulture,
            // FlowDirection.LeftToRight, new Typeface("Noto Sans JP"), 15, Brushes.Black, 2.5);
            //Geometry textGeometry3 = text3.BuildGeometry(new Point(14, hi - 55));
            //so = Geometry.Combine(so, textGeometry3, GeometryCombineMode.Xor, null);
            int x = 14;
            for (int i = 0; i < ALL.Length; i++)
            {
                int fontSz = 14;
                double y = hi - 55;
                if (ALL[i].ToString()==frmTextKanji)
                {
                    fontSz = 19;
                    y -= 3;
                }
                FormattedText text3 = new FormattedText(ALL[i].ToString(), Thread.CurrentThread.CurrentUICulture,
                 FlowDirection.LeftToRight, new Typeface("Noto Sans JP"), fontSz, Brushes.Black, 2.5);
                
                Geometry textGeometry3 = text3.BuildGeometry(new Point(x, y));
                geometry = Geometry.Combine(geometry, textGeometry3, GeometryCombineMode.Xor, null);
                x += 30;


                var p = textGeometry3.GetWidenedPathGeometry(new Pen(Brushes.Black, 4));
                DoubleAnimation so = new();
                
            }

            SolidColorBrush c = new();
            c = new SolidColorBrush(Colors.Black);
            c.Opacity = opac;
            Pen pp = new();
            pp.Brush = Brushes.Black;
            pp.Thickness = 0;
            drawingContext.DrawGeometry(c, pp, geometry);

            base.OnRender(drawingContext);
        }
    }
}
