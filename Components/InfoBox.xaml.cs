using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for InfoBox.xaml
    /// </summary>
    public partial class InfoBox : UserControl
    {
        public Dictionary<string, string> LogSigns = new Dictionary<string, string>() 
        { 
            { "Info", "info" }, 
            { "Warning", "warn" }, 
            { "Error", "error" } 
        };
         

        public string LogLevel
        {
            get { return (string)GetValue(LogLevelProperty); }
            set { SetValue(LogLevelProperty, value); }
        }
        public static readonly DependencyProperty LogLevelProperty =
            DependencyProperty.Register("LogLevel", typeof(string), typeof(InfoBox), new PropertyMetadata(string.Empty));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(InfoBox), new PropertyMetadata(string.Empty));



        public InfoBox()
        {
            InitializeComponent();
        }

        private Brush _bgColor = Brushes.Gray;
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            this.Background = Brushes.Gray;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);
            this.Background = Brushes.Transparent;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            this.Background = Brushes.Transparent;
        }

    }
}
