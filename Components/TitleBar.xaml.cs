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
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TitleBar), new PropertyMetadata("My list of tasks"));

        public DelegateCommand<string[]> Rename
        {
            get { return (DelegateCommand<string[]>)GetValue(RenameProperty); }
            set { SetValue(RenameProperty, value); }
        }
        public static readonly DependencyProperty RenameProperty =
            DependencyProperty.Register("Rename", typeof(DelegateCommand<string[]>), typeof(TitleBar));

        public DelegateCommand<string> Quit
        {
            get { return (DelegateCommand<string>)GetValue(QuitProperty); }
            set { SetValue(QuitProperty, value); }
        }
        public static readonly DependencyProperty QuitProperty =
            DependencyProperty.Register("Quit", typeof(DelegateCommand<string>), typeof(TitleBar));

        public DelegateCommand<string> Save
        {
            get { return (DelegateCommand<string>)GetValue(SaveProperty); }
            set { SetValue(SaveProperty, value); }
        }
        public static readonly DependencyProperty SaveProperty =
            DependencyProperty.Register("Save", typeof(DelegateCommand<string>), typeof(TitleBar));

        public DelegateCommand<string> Reload
        {
            get { return (DelegateCommand<string>)GetValue(ReloadProperty); }
            set { SetValue(ReloadProperty, value); }
        }
        public static readonly DependencyProperty ReloadProperty =
            DependencyProperty.Register("Reload", typeof(DelegateCommand<string>), typeof(TitleBar));

        public DelegateCommand<string> Add
        {
            get { return (DelegateCommand<string>)GetValue(AddProperty); }
            set { SetValue(AddProperty, value); }
        }
        public static readonly DependencyProperty AddProperty =
            DependencyProperty.Register("Add", typeof(DelegateCommand<string>), typeof(TitleBar));

        public DelegateCommand<string> Redo
        {
            get { return (DelegateCommand<string>)GetValue(RedoProperty); }
            set { SetValue(RedoProperty, value); }
        }
        public static readonly DependencyProperty RedoProperty =
            DependencyProperty.Register("Redo", typeof(DelegateCommand<string>), typeof(TitleBar));

        public DelegateCommand<string> Undo
        {
            get { return (DelegateCommand<string>)GetValue(UndoProperty); }
            set { SetValue(UndoProperty, value); }
        }
        public static readonly DependencyProperty UndoProperty =
            DependencyProperty.Register("Undo", typeof(DelegateCommand<string>), typeof(TitleBar));


        public TitleBar()
        {
            InitializeComponent();
        }

        

        private void OpenRenameDialog(object sender, MouseButtonEventArgs e)
        {
            RenameDialog RD = new RenameDialog();
            RD.NewName = Title;   // assign NewName dep prop of RenameDialog to Title prop of TitleBar which is bound to TitleText of VM
            RD.Identifier = "#";
            RD.Save = Rename;  // assign Save delegate com of RenameDialog to Rename del com of TitleBar which is bound to TitleRename of VM
            Point pos = this.PointToScreen(new Point(0, 0));
            RD.Left = pos.X;
            RD.Top = Math.Min(pos.Y, System.Windows.SystemParameters.WorkArea.Height - RD.Height);
            RD.ShowDialog();
        }
    }
}
