using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

            vm.PropertyChanged += async (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "InfoMsg":
                        ucInfo.Message = vm.InfoMsg.Item2;
                        Brush _bg = this.Background;
                        switch (vm.InfoMsg.Item1)
                        {
                            case MainWindowVM.Log.Info:
                                ucInfo.Background = Brushes.Aquamarine;
                                ucInfo.LogLbl.Background = Brushes.DarkGreen;
                                ucInfo.LogLevel = ucInfo.LogSigns["Info"];
                                break;
                            case MainWindowVM.Log.Warning:
                                ucInfo.Background = Brushes.LightGoldenrodYellow;
                                ucInfo.LogLbl.Background = Brushes.DarkOrange;
                                ucInfo.LogLevel = ucInfo.LogSigns["Warning"];
                                break;
                            case MainWindowVM.Log.Error:
                                ucInfo.Background = Brushes.DeepPink;
                                ucInfo.LogLbl.Background = Brushes.DarkRed;
                                ucInfo.LogLevel = ucInfo.LogSigns["Error"];
                                break;
                        }
                        await Task.Delay(2000);
                        ucInfo.Background = _bg;
                        ucInfo.LogLbl.Background = _bg;
                        ucInfo.LogLevel = string.Empty;
                        ucInfo.Message = string.Empty;
                        break;
                    default:
                        break;
                }
            };

        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
            Left = System.Windows.SystemParameters.WorkArea.Width / 3;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Top = System.Windows.SystemParameters.WorkArea.Height - ActualHeight;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (Keyboard.Modifiers == ModifierKeys.Shift && MainTaskList.SelectedIndex != -1)
            {
                e.Handled = true;
                TaskItem task = MainTaskList.SelectedItem as TaskItem;
                int curIndex = MainTaskList.SelectedIndex;
                if (e.Key == Key.Down)
                {
                    vm.MoveDown.Execute(task.UID);
                    MainTaskList.Focus();
                    MainTaskList.SelectedIndex = curIndex < vm.tasks.Count - 1? curIndex + 1 : curIndex;
                }
                else if (e.Key == Key.Up)
                {
                    vm.MoveUp.Execute(task.UID);
                    MainTaskList.Focus();
                    MainTaskList.SelectedIndex = curIndex > 0 ? curIndex - 1 : curIndex;
                }
                ListViewItem lvi = MainTaskList.ItemContainerGenerator.ContainerFromIndex(MainTaskList.SelectedIndex) as ListViewItem;
                lvi?.Focus();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Quit on Esc or Exit renaming session
            if (e.Key == Key.Escape)
            {
                if (!vm.HasUnsavedChanges) Close();
            }


            if (!MainTaskList.IsKeyboardFocused && (e.Key == Key.Down || e.Key == Key.Up))
            {
                MainTaskList.Focus();
                MainTaskList.SelectedIndex = (MainTaskList.SelectedIndex == -1) ?
                    e.Key == Key.Down ? 0 : (MainTaskList.Items.Count > 0 ? MainTaskList.Items.Count - 1 : 0) :
                    MainTaskList.SelectedIndex;
                                
                ListViewItem lvi = MainTaskList.ItemContainerGenerator.ContainerFromIndex(MainTaskList.SelectedIndex) as ListViewItem;
                lvi?.Focus();
            }



            // Launch selected item on enter
            if (MainTaskList.SelectedItem != null && e.Key == Key.Enter)
            {
                string _path = ((TaskItem)MainTaskList.SelectedItem).Path;
                vm.LaunchAndQuit.Execute(_path);
            }

            // Delete selected item on del
            if (MainTaskList.SelectedItem != null && e.Key == Key.Delete)
            {
                string _uid = ((TaskItem)MainTaskList.SelectedItem).UID;
                vm.RemoveItem.Execute(_uid);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            try
            {
                this.DragMove();
            }
            catch
            {
                // do nothing
            }
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

        public void LVI_Drag(object sender, MouseEventArgs e)
        {
            FrameworkElement lvi = sender as FrameworkElement;
            string taskUID = ((TaskItem)lvi.DataContext).UID;
            if (lvi != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(lvi, taskUID, DragDropEffects.Move);
            }
        }

        public void LVI_Drop(object sender, DragEventArgs e)
        {
            string thisTaskUID = ((sender as FrameworkElement).DataContext as TaskItem)?.UID ?? String.Empty; // this is fucking brilliant
            string dropTask = (string)e.Data.GetData(DataFormats.StringFormat)?? (e.Data.GetData(DataFormats.FileDrop) as string[]).First();

            if (File.Exists(dropTask) || Directory.Exists(dropTask))
            {
                vm.DragDrop.Execute(new string[] { "", thisTaskUID, dropTask });
            }
            else if (Guid.Parse(dropTask) != null)
            {
                vm.DragDrop.Execute(new string[] { dropTask, thisTaskUID });
            } 
            else
            {
                vm.DragDrop.Execute(new string[] { });
            }

        }

    }
}
