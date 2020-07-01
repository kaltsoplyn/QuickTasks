using QuickTasks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace QuickTasks
{
    public class TaskItem : INotifyPropertyChanged
    {
        // default and not-found images
        private ImageSource _defaultTaskImg = new BitmapImage(new Uri("pack://application:,,,/Images/AppGear.png"));
        private ImageSource _NotFoundTaskImg = new BitmapImage(new Uri("pack://application:,,,/Images/AppNotFound.png"));

        // properties. Name change raises a PropertyChanged Event
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Path { get; set; }
        public ImageSource Ico { get; set; }
        public string UID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Constructors
        public TaskItem()
        {
            Name = string.Empty;
            Path = string.Empty;
            Ico = _defaultTaskImg;
            UID = string.Empty;
        }

        public TaskItem(string path) : this()
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = path;
            Ico = Helpers.IconToImageSource(Helpers.GetIconFromPath(path));
            UID = Guid.NewGuid().ToString();
        }

        public TaskItem(string name, string path) : this(path)
        {
            Name = name;
        }

        public TaskItem(string name, string path, string uid) : this(name, path)
        {
            UID = uid;
        }

        public TaskItem(string name, string path, ImageSource ico, string uid) : this(name, path, uid)
        {
            Name = name;
            Path = path;
            Ico = ico;
            UID = uid;
        }


        // Methods
        // Deep copy factory
        public TaskItem CopyFrom(TaskItem task)
        {
            return new TaskItem(task.Name, task.Path, task.Ico, task.UID);
        }

        // LINQ XML Serialize
        public XElement XSerialize(XNamespace nmsp)
        {
            return new XElement(nmsp + "TaskItem",
                new XElement(nmsp + "Name", this.Name),
                new XElement(nmsp + "Path", this.Path),
                new XElement(nmsp + "UID", this.UID));
        }
        public XElement XSerialize()
        {
            return this.XSerialize(XNamespace.None);
        }

        // LINQ XML Deserialize
        public TaskItem XDeserialize(XNamespace nmsp, XElement xel)
        {
            if (!((string)xel.Name.LocalName == "TaskItem")) throw new NotSupportedException();

            TaskItem task = new TaskItem(
                (string)xel.Element(nmsp + "Name"),
                (string)xel.Element(nmsp + "Path"),
                (string)xel.Element(nmsp + "UID")
                );

            return task;
        }

    }
}
