using QuickTasks.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private (Log, string) infoMsg = (Log.Info, string.Empty);
        public (Log, string) InfoMsg { 
            get { return infoMsg; }
            set 
            { 
                infoMsg = value; 
                OnPropertyChanged();
            } }

        // Definition of main list 'tasks', plus the extra undo prereqs
        public ObservableCollection<TaskItem> tasks { get; set; }
        private List<TaskItem> tasksBackup { get; set; }
        public List<List<TaskItem>> tasksHistory = new List<List<TaskItem>>(undoLevels);

        // depth of undo list and current state index
        private static int undoLevels = 10;
        private int currentUndoLevel = 0;
        public int CurrentUndoLevel
        {
            get
            {
                return currentUndoLevel;
            }
            set
            {
                currentUndoLevel = value;
                _undo.RaiseCanExecuteChanged();
                _redo.RaiseCanExecuteChanged();
            }
        }
        

        private void AddHistoryEntry()
        {
            tasksBackup = new List<TaskItem>(tasks.Count);
            tasks.ToList<TaskItem>().ForEach((task) => tasksBackup.Add(new TaskItem().CopyFrom(task)));

            if (tasksHistory.Count == tasksHistory.Capacity)
            {
                tasksHistory.RemoveAt(0);
            }
            else
            {
                CurrentUndoLevel += 1;
            }

            tasksHistory = tasksHistory.Take(CurrentUndoLevel).Append(tasksBackup).ToList();

            HasUnsavedChanges = true;
        }

        private bool _hasUnsavedChanges = false;
        public bool HasUnsavedChanges { 
            get {return _hasUnsavedChanges; }
            set { _hasUnsavedChanges = value ; 
                _saveToDisk.RaiseCanExecuteChanged();
                _reloadFromDisk.RaiseCanExecuteChanged();
                _launchAndQuit.RaiseCanExecuteChanged();
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
                (s) => 
                { 
                    TL.PopulateTaskList();
                    AddHistoryEntry();
                    HasUnsavedChanges = false; 
                },
                (s) => { return HasUnsavedChanges; }
            );
        }

        private readonly DelegateCommand<string> _removeItem;
        public DelegateCommand<string> RemoveItem { get { return _removeItem; } }
        private DelegateCommand<string> RemoveItemCmd()
        {
            return new DelegateCommand<string>(
              (s) => 
              { 
                  TaskItem t = GetTaskByUID(s);
                  if (RemoveTask(t))
                    AddHistoryEntry(); 
              },
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
                    if (AddTask(new TaskItem(_path)))
                        AddHistoryEntry();
                },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _redo;
        public DelegateCommand<string> Redo { get { return _redo; } }
        private DelegateCommand<string> RedoCmd()
        {
            return new DelegateCommand<string>(
                (s) => 
                {
                    CurrentUndoLevel += 1;
                    tasks.Clear();
                    tasksHistory[CurrentUndoLevel].ForEach(task => tasks.Add(task));
                    HasUnsavedChanges = true;
                },
                (s) => { return CurrentUndoLevel < tasksHistory.Count - 1; }
                );
        }

        private readonly DelegateCommand<string> _undo;
        public DelegateCommand<string> Undo { get { return _undo; } }
        private DelegateCommand<string> UndoCmd()
        {
            return new DelegateCommand<string>(
                (s) => 
                {
                    CurrentUndoLevel -= 1;
                    tasks.Clear();
                    tasksHistory[CurrentUndoLevel].ForEach(task => tasks.Add(task));
                    HasUnsavedChanges = true;
                },
                (s) => { return CurrentUndoLevel > 0; }
                );
        }

        private readonly DelegateCommand<string> _moveUp;
        public DelegateCommand<string> MoveUp { get { return _moveUp; } }
        private DelegateCommand<string> MoveUpCmd()
        {
            return new DelegateCommand<string>(
                (uid) => 
                { 
                    if (MoveTaskUp(GetTaskByUID(uid)))
                        AddHistoryEntry();
                },
                (uid) => { return true; }
                );
        }

        private readonly DelegateCommand<string> _moveDown;
        public DelegateCommand<string> MoveDown { get { return _moveDown; } }
        private DelegateCommand<string> MoveDownCmd()
        {
            return new DelegateCommand<string>(
                (uid) =>
                {
                    if (MoveTaskDown(GetTaskByUID(uid)))
                        AddHistoryEntry();
                },
                (uid) => { return true; }
                );
        }

        private readonly DelegateCommand<string[]> _dragDrop;
        public DelegateCommand<string[]> DragDrop { get { return _dragDrop; } }
        private DelegateCommand<string[]> DragDropCmd()
        {
            return new DelegateCommand<string[]>(
                (uids) =>
                {
                    if (uids.Length == 0) { InfoMsg = (Log.Error, "Dropped task neither task nor exe"); return; }

                    TaskItem targetTask = uids[1] == String.Empty ? new TaskItem() : GetTaskByUID(uids[1]);
                    TaskItem droppedTask = new TaskItem();
                    switch (uids.Length)
                    {
                        case 2:
                            droppedTask = GetTaskByUID(uids[0]);
                            break;
                        case 3:
                            droppedTask = new TaskItem(uids[2]);
                            break;
                        //default:
                        //    break;
                    }
                    if (DropTaskAt(droppedTask, targetTask))
                        AddHistoryEntry();

                },
                (uids) => { return true; }
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

        private readonly DelegateCommand<string> _launchAndQuit;
        public DelegateCommand<string> LaunchAndQuit { get { return _launchAndQuit; } }
        private DelegateCommand<string> LaunchAndQuitCmd()
        {
            return new DelegateCommand<string>(
                (path) =>
                {
                    try
                    {
                        Process.Start(path);
                        OnQuitRequest();
                    }
                    catch (ObjectDisposedException)
                    {
                        throw;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(String.Format("Path didn't lead to an executable.\nFile not found.\n{0}", e.Message), "Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        //throw;
                    }
                },
                (path) => { return true; }
                );
        }


        private readonly DelegateCommand<string[]> _titleRename;
        public DelegateCommand<string[]> TitleRename { get { return _titleRename; } }
        private DelegateCommand<string[]> TitleRenameCmd()
        {
            return new DelegateCommand<string[]>(
                (s) => {
                    TitleText = s[0];
                    InfoMsg = (Log.Info, "The list was renamed");
                    HasUnsavedChanges = true;
                },
                (s) => { return true; }
                );
        }

        private readonly DelegateCommand<string[]> _rename;
        public DelegateCommand<string[]> Rename { get { return _rename; } }
        private DelegateCommand<string[]> RenameCmd()
        {
            return new DelegateCommand<string[]>(
                (s) => 
                {
                    TaskItem t = GetTaskByUID(s[1]);
                    RenameTask(t, s[0]);
                    AddHistoryEntry(); 
                },
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


        private TaskList TL;

        #endregion

        #region Constructor
        public MainWindowVM() 
        {

            TL = new TaskList();
            tasks = TL.Tasks;

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

            // Command registration
            _titleRename = TitleRenameCmd();
            _rename = RenameCmd();

            _saveToDisk = SaveToDiskCmd();
            _reloadFromDisk = ReloadFromDiskCmd();

            _redo = RedoCmd();
            _undo = UndoCmd();

            _addItem = AddItemCmd();
            _removeItem = RemoveItemCmd();

            _moveUp = MoveUpCmd();
            _moveDown = MoveDownCmd();
            _dragDrop = DragDropCmd();

            _launchAndQuit = LaunchAndQuitCmd();
            _quit = QuitCmd();

            AddHistoryEntry();
            CurrentUndoLevel = 0; // This is just for initialization
            HasUnsavedChanges = false;
        }
        #endregion

        #region Methods

        public TaskItem GetTaskByUID(string uid)
        {
            return tasks.Where(t => t.UID == uid).Single();
        }

        public TaskItem GetTaskByPath(string path)
        {
            return tasks.Where(t => t.Path == path).Single();
        }

        public bool Contains(TaskItem task) // caution! this can cause confusion with the built-in List<T>.Contains(T) method
        {
            List<string> _paths = tasks.Select(x => x.Path).ToList();

            return _paths.Contains(task.Path);
        }

        public void RenameTask(TaskItem task, string newName)
        {
            task.Name = newName;
            InfoMsg = (Log.Info, "A task was renamed.");
        }

        public bool RemoveTask(TaskItem task)
        {
            if (tasks.Remove(task))
            {
                InfoMsg = (Log.Info, "A task was removed.");
                return true;
            }
            else
            {
                InfoMsg = (Log.Warning, "Task could not be removed.");
                return false;
            }
        }

        public bool AddTask(TaskItem task)
        {
            if (Contains(task))
            {
                InfoMsg = (Log.Warning, "Task already in list.");
                return false;
            }
            else
            {
                tasks.Add(task);
                InfoMsg = (Log.Info, "A new task was added.");
                return true;
            }
        }

        public bool AddTaskAfter(TaskItem taskToAdd, TaskItem taskToAddAfter)
        {
            if (Contains(taskToAddAfter) && !Contains(taskToAdd))
            {
                int _index = tasks.IndexOf(taskToAddAfter);
                tasks.Insert(_index + 1, taskToAdd);
                return true;
            }
            else
            {
                InfoMsg = (Log.Error, taskToAdd.Name + " could not be inserted after " + taskToAddAfter.Name);
                return false;
            }
        }

        public bool AddTaskBefore(TaskItem taskToAdd, TaskItem taskToAddBefore)
        {
            if (taskToAddBefore.UID == string.Empty) // a string.Empty as UID of target means "add at the end"
            {
                return(AddTask(taskToAdd));
            }
            else if (Contains(taskToAddBefore) && !Contains(taskToAdd))
            {
                int _index = tasks.IndexOf(taskToAddBefore);
                tasks.Insert(_index, taskToAdd);
                InfoMsg = (Log.Info, "A task was added.");
                return true;
            }
            else
            {
                InfoMsg = (Log.Error, taskToAdd.Name + " could not be added");
                return false;
            }
        }

        public bool DropTaskAt(TaskItem dropTask, TaskItem taskToDropBefore)
        {
            if (!Contains(dropTask)) { return(AddTaskBefore(dropTask, taskToDropBefore)); }

            int _oldIndex = tasks.IndexOf(dropTask);
            if (_oldIndex == -1)
            {
                InfoMsg = (Log.Warning, "Task already in list.");
                return false;
            }

            if (taskToDropBefore.UID == string.Empty)
            {
                tasks.Move(_oldIndex, tasks.Count - 1);
            }
            else if (Contains(taskToDropBefore))
            {
                int _newIndex = tasks.IndexOf(taskToDropBefore);
                
                if (_oldIndex == _newIndex)
                {
                    InfoMsg = (Log.Warning, "Won't move task on itself.");
                    return false;
                }
                tasks.Move(_oldIndex, _newIndex);
            }
            else
            {
                InfoMsg = (Log.Error, dropTask.Name + " could not be moved.");
                return false;
            }

            InfoMsg = (Log.Info, "A task was moved.");
            return true;
        }

        public bool MoveTaskUp(TaskItem task)
        {
            int index = tasks.IndexOf(task);
            if (index > 0)
            {
                tasks.Move(index, index - 1);
                InfoMsg = (Log.Info, "Task moved up.");
                return true;
            }
            else
            {
                InfoMsg = (Log.Warning, "Task already at the top.");
                return false;
            }
        }

        public bool MoveTaskDown(TaskItem task)
        {
            int index = tasks.IndexOf(task);
            if (index < tasks.Count - 1)
            {
                tasks.Move(index, index + 1);
                InfoMsg = (Log.Info, "Task moved down.");
                return true;
            }
            else
            {
                InfoMsg = (Log.Warning, "Task already at the bottom.");
                return false;
            }
        }

        #endregion

        #region Undo/Redo handling

        


        #endregion
    }
}

