using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Wacton.Desu.Kanji;

namespace SugarKanji
{
    public partial class MainWindow : Window
    {
        DispatcherTimer slideshow = new();
        //List<Notes> notesAll;
        int start = 0;
        int num;
        int MAXSLIDES = 3;
        public List<IKanjiEntry> kanji;
        string dir = @"C:\Pictures\wallpapers\";
        
        string[] slides;
        //Dictionary<string, Notes> dic;
        int picOffset = 120;
        double srcdiv = 2.2;

        public MainWindow()
        {
            num = start;
            InitializeComponent();
            slideshow.Interval = new TimeSpan(0, 0, 0, 15);
            slideshow.Tick += Slideshow_Tick;
            //dic = new();
            //notesAll = new();
            //notesAll.AddRange(ImportAnkiDB());
            //for (int i = 0; i < notesAll.Count; i++)
            //{
            //    dic.Add(notesAll[i].Fields[0], notesAll[i]);
            //}
            slides = Directory.GetFiles(dir);
            Loaded += MainWindow_Loaded;
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void ImagePlacing()
        {
            var x = new BitmapImage(new Uri(slides[num + picOffset]));
            Img.Source = x;
            //Console.WriteLine(girls[num]);
            //Console.WriteLine("zz:" + Img.Source.Height);
            //Console.WriteLine("zz:" + Img.Source.Width);
            Img.Width = x.PixelWidth / srcdiv;
            Img.Height = x.PixelHeight / srcdiv;
            //Console.WriteLine("hei: " + Img.Height);
            //Console.WriteLine("www: " + Img.Source.Width);

            //Img.Height = Img.

            if (Img.Height > 1300)
            {
                //Console.WriteLine("13131313");
                Img.Width = x.PixelWidth / 4.8;
                Img.Height = x.PixelHeight / 4.8;
            }
            else if (Img.Height > 1100)
            {
                //Console.WriteLine("111111111");
                Img.Width = x.PixelWidth / 4;
                Img.Height = x.PixelHeight / 4;
            }
            else if (Img.Height > 900)
            {
                //Console.WriteLine("999999999999999990");
                //double ratio = Img.Height / Img.Width;
                //Img.Height = 900;
                //Img.Width = Img.Width / ratio;
                Img.Width = x.PixelWidth / 3;
                Img.Height = x.PixelHeight / 3;
            }
            //else
            //{
            //    Img.Width = Img.Source.Width / 2;
            //    Img.Height = Img.Source.Height / 2;
            //}
            //Console.WriteLine(Img.Source.Width);
            //Console.WriteLine(Img.Source.Height);
            this.Height = Img.Height;
            this.Width = Img.Width;
            /////////this.Left = 0;
            //this.Left = 1526 - Img.Width;
            //////////this.Top = 28;
            if (num == slides.Length) num = start;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KanjiDict kd = new();
            await kd.Out();
            kanji = kd.kanji.Where(x => x.JLPT == 3).ToList();
            //Console.WriteLine(kanji.Count);
            kanji = kanji.OrderBy(x => x.Frequency).ToList().GetRange(start, start + MAXSLIDES);
            string loop = "";
            for (int i = num; i < num + MAXSLIDES; i++)
            {
                loop += kanji[i].Literal;
            }
            RenderEl.ALL = loop;
            //var loop = string.Join(" ", kanji.Select(x => x.Literal));
            //t.Setter(loop, num - start);
            NextSlide();
            slideshow.Start();
        }

        private void Slideshow_Tick(object? sender, EventArgs e)
        {
            NextSlide();
        }

        public void NextSlide()
        {
            var z = String.Join("    ", kanji[num].Meanings.Where(x => x.Language == Wacton.Desu.Enums.Language.English).Select(x => x.Term));
            var ji = kanji[num].Literal;
            kanjiTitle.Text = ji;
            //char c = char.Parse(kanji[num].Literal);
            ImagePlacing();
            //Console.WriteLine(Img.Width);
            //Console.WriteLine(Img.Height);
            RenderEl.Rezero(kanji[num].Literal, Img.Width, Img.Height, z);
            Subtext.Text = num + "/" + kanji.Count + " " + z;  
            num++;

            if (num == kanji.Count) num = start;
        }

        private List<Notes> ImportAnkiDB()
        {
            string dbFile = @"E:\anki\Kanji" + "\\collection.anki21";
            var databasePath = Path.Combine(dbFile);
            var db = new SQLiteConnection(databasePath);
            //Console.WriteLine(databasePath);
            var noTes = db.Table<Notes>().ToList();
            return noTes;
        }
    }

    public class Notes : Hana
    {
        public List<string>? Fields => flds?.Split("").ToList();
    }

    public class Hana
    {
        [PrimaryKey]
        public int id { get; set; }
        public int mid { get; set; }
        public int mod { get; set; }
        public int usn { get; set; }
        public int sfld { get; set; }
        public int csum { get; set; }
        public int flags { get; set; }
        public string? data { get; set; }
        public string? guid { get; set; }
        public string? tags { get; set; }
        public string? flds { get; set; }
    }
}
