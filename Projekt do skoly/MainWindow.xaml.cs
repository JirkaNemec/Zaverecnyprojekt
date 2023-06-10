using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AimLabClone
{
    public partial class MainWindow : Window
    {
        private Random random;
        private int score;
        private int ammo;
        private Ellipse target;
        private Timer timer;
        private Line verticalLine;
        private Line horizontalLine;
        private Ellipse smallerTarget;
        private double smallerTargetSize;
        private Ellipse smallestTarget;

        
        
        
        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            score = 0;
            ammo = 10;

            timer = new Timer(2000);
            timer.Elapsed += TimerElapsed;

            gameCanvas.SizeChanged += GameCanvasSizeChanged;

            UpdateAmmoText();
        }

       
        
        
        
        
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                score--;
                scoreText.Text = $"Score: {score}";

                RemoveTarget();
                AddTarget();
            });
        }

       
        
        
        private void GameCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RemoveTarget();
            AddTarget();
        }

       
        
        
        
        private void gameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (ammo > 0)
                {
                    Point mousePosition = e.GetPosition(gameCanvas);
                    CheckTargetHit(mousePosition);
                    ammo--;
                    UpdateAmmoText();
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ammo = 10;
                UpdateAmmoText();
            }
        }
       
        
        
        
        private void CheckTargetHit(Point mousePosition)
        {
            if (IsMouseOverTarget(mousePosition, target))
            {
                score++;
                scoreText.Text = $"Score: {score}";
            }
            else
            {
                score--;
                scoreText.Text = $"Score: {score}";
            }

            RemoveTarget();
            AddTarget();

            timer.Stop();
            timer.Start();
        }

        
        
        
        private bool IsMouseOverTarget(Point mousePosition, Ellipse target)
        {
            Point targetPosition = target.TranslatePoint(new Point(), gameCanvas);
            double targetRadius = target.Width / 2;
            double dx = mousePosition.X - (targetPosition.X + targetRadius);
            double dy = mousePosition.Y - (targetPosition.Y + targetRadius);
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance <= targetRadius;

        }


       
        
        
        private void AddTarget()
        {

            double canvasWidth = gameCanvas.ActualWidth;
            double canvasHeight = gameCanvas.ActualHeight;
            double targetSize = random.Next(30, 80);

            target = new Ellipse
            {
                Width = targetSize,
                Height = targetSize,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 2

               
            };
 

            
            double left = random.NextDouble() * (canvasWidth - targetSize);
            double top = random.NextDouble() * (canvasHeight - targetSize);

            Canvas.SetLeft(target, left);
            Canvas.SetTop(target, top);


            
            
            
            gameCanvas.Children.Add(target);



            // Vytvoření druhé elipsy
            Ellipse smallerTarget = new Ellipse
            {
                Width = targetSize / 2,
                Height = targetSize / 2,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            double smallerLeft = left + targetSize / 4;
            double smallerTop = top + targetSize / 4;

            Canvas.SetLeft(smallerTarget, smallerLeft);
            Canvas.SetTop(smallerTarget, smallerTop);

            gameCanvas.Children.Add(smallerTarget);

            verticalLine = new Line
            {
                X1 = left + targetSize / 2,
                Y1 = top,
                X2 = left + targetSize / 2,
                Y2 = top + targetSize,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            gameCanvas.Children.Add(verticalLine);

            horizontalLine = new Line
            {
                X1 = left,
                Y1 = top + targetSize / 2,
                X2 = left + targetSize,
                Y2 = top + targetSize / 2,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            gameCanvas.Children.Add(horizontalLine);
            timer.Start();
        }

       
        
        
        
        
        private void RemoveTarget()
        {
            gameCanvas.Children.Remove(target);
            gameCanvas.Children.Remove(verticalLine);
            gameCanvas.Children.Remove(horizontalLine);

            // Odstranění menší elipsy a souvisejícího kříže
            foreach (UIElement element in gameCanvas.Children)
            {
                if (element is Ellipse && element != target)
                {
                    gameCanvas.Children.Remove(element);

                    // Odstranění kříže spojeného s menší elipsou ((((((už je to jedno není potřeba tahle část kodu))))))
                    foreach (UIElement childElement in gameCanvas.Children)
                    {
                        if (childElement is Line)
                        {
                            gameCanvas.Children.Remove(childElement);
                            break;
                        }
                    }

                    break;
                }
            }
        }
    
    

        private void UpdateAmmoText()
        {
            ammoText.Text = $"Ammo: {ammo}";
        }
    }
}
