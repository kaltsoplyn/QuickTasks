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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Quit on Esc or Exit renaming session
            if (e.Key == Key.Escape)
            {
                if (!vm.HasUnsavedChanges) Close();
            }

            //// give keyboard focus on up/down (down -> focus @ top item | up -> focus @ bottom item )
            //if (!MainTaskList.IsKeyboardFocused && (e.Key == Key.Down || e.Key == Key.Up))
            //{
            //    MainTaskList.Focus();
            //    int focusedElement = e.Key == Key.Down ? 0 : (MainTaskList.Items.Count > 0 ? MainTaskList.Items.Count - 1 : 0);
            //    MainTaskList.SelectedIndex = focusedElement;
            //    ListViewItem lvi = MainTaskList.ItemContainerGenerator.ContainerFromIndex(focusedElement) as ListViewItem;
            //    lvi?.Focus();
            //}

            //// Launch selected item on enter
            //if (MainTaskList.SelectedItem != null && e.Key == Key.Enter)
            //{
            //    string _path = ((TaskItem)MainTaskList.SelectedItem).Path;
            //    LaunchAndQuit(_path);
            //}

            //// Delete selected item on del
            //if (MainTaskList.SelectedItem != null && e.Key == Key.Delete)
            //{
            //    RemoveItem((TaskItem)MainTaskList.SelectedItem, new System.Windows.RoutedEventArgs());
            //}
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    // Begin dragging the window
        //    this.DragMove();
        //}

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
