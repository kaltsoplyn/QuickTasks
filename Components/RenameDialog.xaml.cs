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
        private string _originalName;

        public string NewName
        {
            get { return (string)GetValue(NewNameProperty); }
            set { SetValue(NewNameProperty, value); }
        }
        public static readonly DependencyProperty NewNameProperty =
            DependencyProperty.Register("NewName", typeof(string), typeof(RenameDialog), new PropertyMetadata("Enter new name"));

        //public string Identifier
        //{
        //    get { return (string)GetValue(IdentifierProperty); }
        //    set { SetValue(IdentifierProperty, value); }
        //}
        //public static readonly DependencyProperty IdentifierProperty =
        //    DependencyProperty.Register("Identifier", typeof(string), typeof(RenameDialog), new PropertyMetadata(string.Empty));



        //public DelegateCommand<string> Cancel
        //{
        //    get { return (DelegateCommand<string>)GetValue(CancelProperty); }
        //    set { SetValue(CancelProperty, value); }
        //}
        //public static readonly DependencyProperty CancelProperty =
        //    DependencyProperty.Register("Cancel", typeof(DelegateCommand<string>), typeof(RenameDialog));

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

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            _originalName = NewName;
        }

        // Handle Enter/Esc keys in rename box
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (RenameBox.IsKeyboardFocusWithin)
            {
                if (e.Key == Key.Enter)
                {
                    SaveHandler();
                    e.Handled = true;  // normally I'd need this to keep the event from bubbling upwards
                }
            }

            if (e.Key == Key.Escape)
            {
                CancelHandler(CancelBtn, new RoutedEventArgs());
                e.Handled = true;
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


        // I handle Cancel as a click event because it does nothing; it just closes the dialog
        private void CancelHandler(object sender, RoutedEventArgs e)
        {
            modRenameDialog.Close();
        }

        // I use a Click event handler for Save, and execute the Save command through it, because I also want to close the dialog.
        private void SaveHandler() 
        {
            SaveHandler(SaveBtn, new RoutedEventArgs());
        } 
        private void SaveHandler(object sender, RoutedEventArgs e)
        {
            if ((RenameBox.Text != "") && (RenameBox.Text != _originalName))
            {
                NewName = RenameBox.Text;
                Save.Execute(NewName);
            }
            else
            {
                CancelHandler(CancelBtn, new RoutedEventArgs());
            }
            modRenameDialog.Close();
        }
    }
}
