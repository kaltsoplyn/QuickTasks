using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickTasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int FlyInOutDuration = 100;

        MainWindowVM vm;

        public MainWindow()
        {
            InitializeComponent();

            vm = new MainWindowVM();
            this.DataContext = vm;

            vm.QuitRequest += (sender, e) => this.Close();


        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void FlyIn(FrameworkElement e)
        {
            e.Visibility = Visibility.Visible;
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = e.Height,
                Duration = TimeSpan.FromMilliseconds(FlyInOutDuration),
                DecelerationRatio = 0.2,
                FillBehavior = FillBehavior.Stop
            };
            e.BeginAnimation(HeightProperty, animation);
        }

        private async void FlyOut(FrameworkElement e)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = e.Height,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(2 * FlyInOutDuration),
                DecelerationRatio = 0.2,
                FillBehavior = FillBehavior.Stop
            };
            //animation.Completed += onCompleted;
            e.BeginAnimation(HeightProperty, animation);

            await Task.Delay(FlyInOutDuration);
            e.Visibility = Visibility.Collapsed;
        }

        
    }
}
