using System;
using System.Collections.Generic;
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
using System.Media;
using System.IO;
using System.Reflection;

namespace fappy_bird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        private static readonly MediaPlayer _backgroundMusic = new MediaPlayer();

        double score;
        int gravity = 8;
        bool gameover;
        string over;
        string Exit;
        Rect BirdHitbox;

        public MainWindow()
        {
            InitializeComponent();
            PlaySound1();




        timer.Tick += MainEventTimer;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            gamescore.Content = "Score: " + score;
            overgame.Content = "" + over;
            exit.Content = "" + Exit;
            BirdHitbox = new Rect(Canvas.GetLeft(bird), Canvas.GetTop(bird), bird.Width, bird.Height);
            Canvas.SetTop(bird, Canvas.GetTop(bird) + gravity);

            if (Canvas.GetTop(bird) < 0 || Canvas.GetTop(bird) > 458)
            {
                EndGame();
            }

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "a1" || (string)x.Tag == "a2" || (string)x.Tag == "a3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);
                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        score += .5;
                        //PlaySound2();
                    }

                    Rect PipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (BirdHitbox.IntersectsWith(PipeHitBox))
                    {
                        EndGame();
                    }
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);
                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }

            }


        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                bird.RenderTransform = new RotateTransform(-20, bird.Width / 2, bird.Height / 2);
                gravity = -8;
            }

            if (e.Key == Key.R && gameover == true)
            {
                StartGame();
            }

            if (e.Key == Key.E && gameover == true)
            {
                Application.Current.Shutdown();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

            bird.RenderTransform = new RotateTransform(5, bird.Width / 2, bird.Height / 2);
            gravity = 8;
        }

        private void StartGame()
        {
            MyCanvas.Focus();

            int temp = 300;
            score = 0;
            gameover = false;
            Canvas.SetTop(bird, 190);

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "a1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "a2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "a3")
                {
                    Canvas.SetLeft(x, 1100);
                }
                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }
            }
            timer.Start();
        }

        private void EndGame()
        {
            timer.Stop();
            gameover = true;
            overgame.Content = "Game Over! Press R to try again!";
            exit.Content = "Press E to exit!";
        }

         private void SoundEffect()
         {
             Task.Factory.StartNew(PlaySound1);
             Task.Factory.StartNew(PlaySound2);
         }

         private void PlaySound1()
         {
             SoundPlayer wowSound = new SoundPlayer("Flappy Bird Theme Song.wav");
             wowSound.PlayLooping();
         }

         private void PlaySound2()
         {
             SoundPlayer countingSound = new SoundPlayer("score.wav");
             countingSound.Play();
         }

        }
    }
