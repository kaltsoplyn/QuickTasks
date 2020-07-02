using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace QuickTasks.Model
{
    class TaskList : INotifyPropertyChanged
    {
        

        private string _taskListTitle = string.Empty;
        public string TaskListTitle
        {
            get { return _taskListTitle; }
            set { _taskListTitle = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private readonly string _taskFile = @"tasks.xml";
        public string TaskFile
        {
            get { return _taskFile; }
        }

        public ObservableCollection<TaskItem> Tasks = new ObservableCollection<TaskItem>();
        

        // Constructor
        public TaskList()
        {
            PopulateTaskList();
        }

        //Methods
        
        private (XDocument, XNamespace) getXDoc()
        {
            if (File.Exists(TaskFile))
            {
                XDocument xml = XDocument.Load(TaskFile);
                return (xml, xml.Root.Name.Namespace);
            }
            else
            {
                XNamespace nmsp = "https://QuickTasks.kaltsoplyn.net";
                XDocument xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(nmsp + "TaskItemList", 
                        new XElement(nmsp + "Title", "A list of tasks"),
                        new XElement(nmsp + "TaskList", ""))
                    );

                xml.Save(TaskFile);

                xml = XDocument.Load(TaskFile);
                return (xml, xml.Root.Name.Namespace);
            }
            
        }

        public string GetTitle()
        {
            var (xDoc, ns) = getXDoc();

            var t = xDoc.Descendants(ns + "Title").SingleOrDefault();

            return (t == null ? "Tasks" : (string)t.Value);
        }

        public void SetTitle(string newTitle)
        {
            var (xDoc, ns) = getXDoc();

            try
            {
                xDoc.Descendants(ns + "Title").Single().SetValue(newTitle);
            }
            catch (Exception)
            {
                xDoc.Root.AddFirst(new XElement(ns + "Title", newTitle));
            }

            xDoc.Save(_taskFile);
        }

        public void PopulateTaskList()
        {
            TaskListTitle = GetTitle();

            Tasks.Clear();

            var (xDoc, ns) = getXDoc();

            xDoc.Descendants(ns + "TaskItem").ToList()
                .ForEach(xel => Tasks.Add(new TaskItem().XDeserialize(ns, xel)));
        }

        public void Save(string title)
        {
            TaskListTitle = title;
            SetTitle(title);

            var (xDoc, ns) = getXDoc();

            xDoc.Descendants(ns + "TaskItem").Remove();

            foreach (TaskItem task in Tasks)
            {
                xDoc.Descendants(ns + "TaskList").FirstOrDefault().Add(task.XSerialize(ns));
            }

            xDoc.Save(_taskFile);
        }
    }
}
