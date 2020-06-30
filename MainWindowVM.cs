using QuickTasks.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickTasks
{
    class MainWindowVM : INotifyPropertyChanged
    {
        #region Properties, Events, and DelegateCommands
        
        // Informational (logLevel, info message) tuple to pass around
        public enum Log { Info, Warning, Error};
        public (Log, string) InfoMsg = (Log.Info, string.Empty);

        // depth of undo list
        private int undoLevels = 10;

        // State of the application and HasUnsavedChanges boolean
        public enum States { Idling, Shifting, Renaming };

        private States _state = States.Idling;
        public States State {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); } 
        }

        private bool _hasUnsavedChanges = false;
        public bool HasUnsavedChanges { 
            get {return _hasUnsavedChanges; }
            set { _hasUnsavedChanges = value ; 
                _saveToDisk.RaiseCanExecuteChanged();
                _reloadFromDisk.RaiseCanExecuteChanged();
            } }

        // Commands from the View
        private readonly DelegateCommand<string> _saveToDisk;
        public DelegateCommand<string> SaveToDisk { get { return _saveToDisk; } }
        private DelegateCommand<string> SaveToDiskCmd()
        {
            return new DelegateCommand<string>(
                (s) => { TL.Save(TitleText); HasUnsavedChanges = false; },
                (s) => { return HasUnsavedChanges; }
            );
        }

        private readonly DelegateCommand<string> _reloadFromDisk;
        public DelegateCommand<string> ReloadFromDisk { get { return _reloadFromDisk; } }
        private DelegateCommand<string> ReloadFromDiskCmd()
        {
            return new DelegateCommand<string>(
                (s) => { TL.PopulateTaskList(); HasUnsavedChanges = false; },
                (s) => { return HasUnsavedChanges; }
            );
        }

        private readonly DelegateCommand<string> _removeItem;
        public DelegateCommand<string> RemoveItem { get { return _removeItem; } }
        private DelegateCommand<string> RemoveItemCmd()
        {
            return new DelegateCommand<string>(
              (s) => { tasks.RemoveAt(tasks.Count-1); if (!HasUnsavedChanges) HasUnsavedChanges = true; },
              (s) => { return true; }
          );
        }

        private readonly DelegateCommand<string> _titleRename;
        public DelegateCommand<string> TitleRename { get { return _titleRename; } }
        private DelegateCommand<string> TitleRenameCmd()
        {
            return new DelegateCommand<string>(
                (s) => { TitleText = s; HasUnsavedChanges = true; },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _addItem;
        public DelegateCommand<string> AddItem { get { return _addItem; } }
        private DelegateCommand<string> AddItemCmd()
        {
            return new DelegateCommand<string>(
                (s) => 
                {
                    string _path = Helpers.GetPathOfNewTask();
                    if (_path == string.Empty)
                    {
                        InfoMsg = (Log.Error, "Path not resolved.");
                        return;
                    }
                    AddTask(new TaskItem(_path));
                    if (!HasUnsavedChanges) HasUnsavedChanges = true;
                },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _redo;
        public DelegateCommand<string> Redo { get { return _redo; } }
        private DelegateCommand<string> RedoCmd()
        {
            return new DelegateCommand<string>(
                (s) => { new NotImplementedException(); },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _undo;
        public DelegateCommand<string> Undo { get { return _undo; } }
        private DelegateCommand<string> UndoCmd()
        {
            return new DelegateCommand<string>(
                (s) => { new NotImplementedException(); },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _quit;
        public DelegateCommand<string> Quit { get { return _quit; } }
        public event EventHandler QuitRequest;
        protected void OnQuitRequest()
        {
            this.QuitRequest?.Invoke(this, EventArgs.Empty);
        }
        private DelegateCommand<string> QuitCmd()
        {
            return new DelegateCommand<string>(
                (s) => { OnQuitRequest(); },
                (s) => { return true; }
                );
        }




        private string _titleText;
        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public ObservableCollection<TaskItem> tasks { get; set; }
        private List<TaskItem> tasksOnDisk { get; set; }
        public ObservableCollection<ObservableCollection<TaskItem>> tasksHistory = new ObservableCollection<ObservableCollection<TaskItem>>();

        private TaskList TL;

        #endregion

        #region Constructor
        public MainWindowVM() 
        {

            TL = new TaskList();
            tasks = TL.Tasks;
            tasksOnDisk = new List<TaskItem>(tasks.Count);
            tasks.ToList<TaskItem>().ForEach((task) => tasksOnDisk.Add(new TaskItem().CopyFrom(task)));

            TitleText = TL.TaskListTitle;

            TL.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "TaskListTitle":
                        TitleText = TL.TaskListTitle;
                        break;
                }
            };

            TaskItem t = new TaskItem();

            _titleRename = TitleRenameCmd();

            _saveToDisk = SaveToDiskCmd();

            _reloadFromDisk = ReloadFromDiskCmd();

            _redo = RedoCmd();
            _undo = UndoCmd();

            _addItem = AddItemCmd();
            _removeItem = RemoveItemCmd();

            _quit = QuitCmd();
        }
        #endregion

        #region Methods

        public bool IsIdling(States state)
        {
            return state == States.Idling;
        }
        public bool IsShifting(States state)
        {
            return state == States.Shifting;
        }
        public bool IsRenaming(States state)
        {
            return state == States.Idling;
        }


        public bool Contains(TaskItem task) // caution! this can cause confusion with the built-in List<T>.Contains(T) method
        {
            List<string> _paths = tasks.Select(x => x.Path).ToList();

            return _paths.Contains(task.Path);
        }

        public void RemoveTask(TaskItem task)
        {
            if (tasks.Remove(task)) 
                InfoMsg = (Log.Info, "A task was removed.");
            // I don't handle failure here, because I can't see a way for relayed task to not be in the list
        }

        public void AddTask(TaskItem task)
        {
            if (Contains(task))
            {
                InfoMsg = (Log.Warning, "Task already in list."); 
            }
            else
            {
                tasks.Add(task);
                InfoMsg = (Log.Info, "A new task was added.");
            }
        }

        public void AddTaskAfter(TaskItem taskToAdd, TaskItem taskToAddAfter)
        {
            if (Contains(taskToAddAfter) && !Contains(taskToAdd))
            {
                int _index = tasks.IndexOf(taskToAddAfter);
                tasks.Insert(_index + 1, taskToAdd);
            }
            else
            {
                InfoMsg = (Log.Error, taskToAdd.Name + " could not be inserted after " + taskToAddAfter.Name);
            }
        }

        public void AddTaskBefore(TaskItem taskToAdd, TaskItem taskToAddBefore)
        {
            if (Contains(taskToAddBefore) && !Contains(taskToAdd))
            {
                int _index = tasks.IndexOf(taskToAddBefore);
                tasks.Insert(_index, taskToAdd);
            }
            else
            {
                InfoMsg = (Log.Error, taskToAdd.Name + " could not be inserted after " + taskToAddBefore.Name);
            }
        }

        #endregion
    }
}

