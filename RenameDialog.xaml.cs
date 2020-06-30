using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickTasks
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public string NewName
        {
            get { return (string)GetValue(NewNameProperty); }
            set { SetValue(NewNameProperty, value); }
        }
        public static readonly DependencyProperty NewNameProperty =
            DependencyProperty.Register("NewName", typeof(string), typeof(RenameDialog), new PropertyMetadata("Enter new name"));

        public DelegateCommand<string> Cancel
        {
            get { return (DelegateCommand<string>)GetValue(CancelProperty); }
            set { SetValue(CancelProperty, value); }
        }
        public static readonly DependencyProperty CancelProperty =
            DependencyProperty.Register("Cancel", typeof(DelegateCommand<string>), typeof(RenameDialog));

        public DelegateCommand<string> Save
        {
            get { return (DelegateCommand<string>)GetValue(SaveProperty); }
            set { SetValue(SaveProperty, value); }
        }
        public static readonly DependencyProperty SaveProperty =
            DependencyProperty.Register("Save", typeof(DelegateCommand<string>), typeof(RenameDialog));


        public RenameDialog()
        {
            InitializeComponent();
        }

        // Handle Enter/Esc keys in rename box
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (RenameBox.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.Enter)
                {
                    NewName = RenameBox.Text;
                    SaveBtn.Command?.Execute(SaveBtn.CommandParameter);
                    e.Handled = true;  // without this, the event keeps bubbling upwards
                    this.Close();
                }
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                this.Close();
            }

        }

        // Filter allowed text in RenameBox
        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9a-zA-Zα-ωΑ-ΩάέίόύήώΆΈΎΌΉΏΊϋϊΪéáóíúýäëÿöïüÁÉÝÚÍÓÄËÜÏÖ ._#()@!&-]", "");
            textboxSender.SelectionStart = cursorPosition;
        }
    }
}
