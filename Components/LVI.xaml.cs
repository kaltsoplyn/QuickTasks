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


        // I hate this. I'm using a command to extract the new name coming from the RenameDialog, and then I'sending this over
        // to the view model with a new command Rename with parameters [new name, task UID]
        // Why can't I just intercept the command parameters from the RenameDialog?
        private string _newName;
        private readonly DelegateCommand<string> _renameCommandHelper;
        public DelegateCommand<string> RenameCommandHelper {  get { return _renameCommandHelper; } }
        private DelegateCommand<string> RenameCommandHelperCmd()
        {
            return new DelegateCommand<string>(
                (s) =>
                {
                    _newName = s;
                    Rename.Execute(new string[] { _newName, LVItem.UID });
                }, 
                (s) => { return true; });
        } 

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





        public LVI()
        {
            InitializeComponent();

            _renameCommandHelper = RenameCommandHelperCmd();
        }

        private void InitiateLaunch(object sender, MouseButtonEventArgs e)
        {
            Launch.Execute(LVItem.Path);
        }

        private void OpenRenameDialog(object sender, RoutedEventArgs e)
        {
            RenameDialog RD = new RenameDialog();
            RD.NewName = LVItem.Name;
            RD.Save = RenameCommandHelper;
            Point pos = this.PointToScreen(new Point(0, 0));
            RD.Left = pos.X;
            RD.Top = Math.Min(pos.Y, System.Windows.SystemParameters.WorkArea.Height - RD.Height);
            RD.ShowDialog();
        }
    }
}
