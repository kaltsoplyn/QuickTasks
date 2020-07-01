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

namespace QuickTasks
{
    /// <summary>
    /// Interaction logic for LVI.xaml
    /// </summary>
    public partial class LVI : UserControl
    {

        public TaskItem LVItem
        {
            get { return (TaskItem)GetValue(LVItemProperty); }
            set { SetValue(LVItemProperty, value); }
        }
        public static readonly DependencyProperty LVItemProperty =
            DependencyProperty.Register("LVItem", typeof(TaskItem), typeof(LVI), new PropertyMetadata(new TaskItem()));

        public DelegateCommand<string> Launch
        {
            get { return (DelegateCommand<string>)GetValue(LaunchProperty); }
            set { SetValue(LaunchProperty, value); }
        }
        public static readonly DependencyProperty LaunchProperty =
            DependencyProperty.Register("Launch", typeof(DelegateCommand<string>), typeof(LVI));

        public DelegateCommand<string[]> Rename
        {
            get { return (DelegateCommand<string[]>)GetValue(RenameProperty); }
            set { SetValue(RenameProperty, value); }
        }
        public static readonly DependencyProperty RenameProperty =
            DependencyProperty.Register("Rename", typeof(DelegateCommand<string[]>), typeof(LVI));


        public DelegateCommand<string> Delete
        {
            get { return (DelegateCommand<string>)GetValue(DeleteProperty); }
            set { SetValue(DeleteProperty, value); }
        }
        public static readonly DependencyProperty DeleteProperty =
            DependencyProperty.Register("Delete", typeof(DelegateCommand<string>), typeof(LVI));



        private Brush _bgColor;

        public LVI()
        {
            InitializeComponent();
            _bgColor = this.Background;
        }

        private void InitiateLaunch(object sender, MouseButtonEventArgs e)
        {
            Launch.Execute(LVItem.Path);
        }

        private void OpenRenameDialog(object sender, RoutedEventArgs e)
        {
            RenameDialog RD = new RenameDialog();
            RD.NewName = LVItem.Name;
            RD.Identifier = LVItem.UID;
            RD.Save = Rename;
            Point pos = this.PointToScreen(new Point(0, 0));
            RD.Left = pos.X;
            RD.Top = Math.Min(pos.Y, System.Windows.SystemParameters.WorkArea.Height - RD.Height);
            RD.ShowDialog();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            this.Background = Brushes.Gray;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            this.Background = _bgColor;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            this.Background = _bgColor;
        }
    }
}
