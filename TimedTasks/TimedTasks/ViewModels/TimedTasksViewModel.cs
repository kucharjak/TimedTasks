using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimedTasks.ViewModels
{
    public class TimedTasksViewModel : ViewModelBase
    {
        private DateTime selectedDate;
        public DateTime SelectedDate { set { SetProperty(ref selectedDate, value); } get { return selectedDate; } }

        private ObservableCollection<object> tasks;
        public ObservableCollection<object> Tasks { set { SetProperty(ref tasks, value); } get { return tasks; } }
        
        private bool finishedTasksVisible;
        public bool FinishedTasksVisible { private set { SetProperty(ref finishedTasksVisible, value); } get { return finishedTasksVisible; } }

        private TaskSelectOptions taskSelectOption;
        public TaskSelectOptions TaskSelectOption { set { SetProperty(ref taskSelectOption, value); } get { return taskSelectOption; } }

        private string title;
        public string Title { private set { SetProperty(ref title, value); } get { return title; } }

        private bool groupingEnabled;
        public bool GroupingEnabled { private set { SetProperty(ref groupingEnabled, value); } get { return groupingEnabled; } }

        private bool dateSelectorVisible;
        public bool DateSelectorVisible { private set { SetProperty(ref dateSelectorVisible, value); } get { return dateSelectorVisible; } }

        public ICommand IncreaseDateByDayCommand { private set; get; }
        public ICommand DecreaseDateByDayCommand { private set; get; }
        public ICommand IncreaseDateByMonthCommand { private set; get; }
        public ICommand DecreaseDateByMonthCommand { private set; get; }

        public ICommand AddNewTaskCommand { private set; get; }
        public ICommand RemoveTaskCommand { private set; get; }
        public ICommand UpdateTaskCommand { private set; get; }

        public ICommand ShowDailyTasksCommand { private set; get; }
        public ICommand ShowAllTasksCommand { private set; get; }

        public ICommand ChangeFinishedVisibilityCommand { private set; get; }
        
        public TimedTasksViewModel()
        {
            Utils.Database.InitDB();
            Utils.Database.CreateDB();

            PropertyChanged += TimedTasksViewModel_PropertyChanged;

            tasks = new ObservableCollection<object>();

            IncreaseDateByDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(1));
            DecreaseDateByDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(-1));
            IncreaseDateByMonthCommand = new Command(() => SelectedDate = SelectedDate.AddMonths(1));
            DecreaseDateByMonthCommand = new Command(() => SelectedDate = SelectedDate.AddMonths(-1));

            AddNewTaskCommand = new Command<TaskViewModel>((task) =>
            {
                Utils.Database.InsertTask(task);
                RefreshTasks();
            });

            RemoveTaskCommand = new Command<TaskViewModel>((task) =>
            {
                Utils.Database.DeleteTask(task);
                RefreshTasks();
            });

            UpdateTaskCommand = new Command<TaskViewModel>((task) =>
            {
                Utils.Database.UpdateTask(task);
                RefreshTasks();
            });
            
            ShowDailyTasksCommand = new Command(() =>
            {
                TaskSelectOption = TaskSelectOptions.CurrentDay;
                Utils.AppData.Data.TaskSelectOption = TaskSelectOption.ToString();
            });

            ShowAllTasksCommand = new Command(() =>
            {
                TaskSelectOption = TaskSelectOptions.AllDaysByDate;
                Utils.AppData.Data.TaskSelectOption = TaskSelectOption.ToString();
            });

            ChangeFinishedVisibilityCommand = new Command(() =>
            {
                FinishedTasksVisible = !FinishedTasksVisible;

                var key = TaskSelectOption.ToString();
                if (Utils.AppData.Data.GroupVisibilitySetting.ContainsKey(key))
                    Utils.AppData.Data.GroupVisibilitySetting[key] = FinishedTasksVisible;
                else
                    Utils.AppData.Data.GroupVisibilitySetting.Add(key, FinishedTasksVisible);

                RefreshTasks();
            });
            
            LoadSettings();
            SetTaskSettings();
            RefreshTasks();
        }

        private void SetTaskSettings()
        {
            switch (TaskSelectOption)
            {
                case TaskSelectOptions.CurrentDay:
                    {
                        Title = "Denní úkoly";
                        DateSelectorVisible = true;
                        GroupingEnabled = false;
                    }
                    break;
                case TaskSelectOptions.AllDaysByDate:
                    {
                        Title = "Všechny úkoly";
                        DateSelectorVisible = false;
                        GroupingEnabled = true;
                    } break;
            }
        }

        private void LoadTaskVisibility()
        {
            if (Utils.AppData.Data.GroupVisibilitySetting != null && Utils.AppData.Data.GroupVisibilitySetting.ContainsKey(taskSelectOption.ToString()))
                FinishedTasksVisible = Utils.AppData.Data.GroupVisibilitySetting[taskSelectOption.ToString()];
            else
                FinishedTasksVisible = true;
        }

        private void LoadSettings()
        {
            if (!String.IsNullOrEmpty(Utils.AppData.Data.TaskSelectOption))
            {
                var tmpOption = new TaskSelectOptions();
                if (!Enum.TryParse(Utils.AppData.Data.TaskSelectOption, out tmpOption))
                    TaskSelectOption = tmpOption;
                else
                    TaskSelectOption = TaskSelectOptions.CurrentDay;
            }

            LoadTaskVisibility();
        }

        private void TimedTasksViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedDate):
                    {
                        RefreshTasks();
                    } break;
                case nameof(TaskSelectOption):
                    {
                        SetTaskSettings();
                        LoadTaskVisibility();
                        RefreshTasks();
                    } break;
            }
        }

        private void RefreshTasks()
        {
            var tmpTasks = (TaskSelectOption == TaskSelectOptions.AllDaysByDate) ?
                Utils.Database.SelectAllTasks(!FinishedTasksVisible) :
                Utils.Database.SelectTasks(SelectedDate.Date, !FinishedTasksVisible);

            tmpTasks = tmpTasks.OrderBy(task => task.DueDate).ThenBy(task => task.StartTime).ToList();

            if (TaskSelectOption == TaskSelectOptions.CurrentDay)
                Tasks = new ObservableCollection<object>(tmpTasks);
            else
                Tasks = new ObservableCollection<object>(GroupTasks(tmpTasks, TaskSelectOption));
        }

        private List<object> GroupTasks(List<TaskViewModel> tasks, TaskSelectOptions options)
        {
            var resultGroups = new List<object>();

            foreach (var task in tasks)
            {
                var groupName = ""; 
                switch(options)
                {
                    case TaskSelectOptions.AllDaysByDate: { groupName = task.DueDate.ToString("D"); } break;
                }

                var group = (resultGroups.Where(grp  => (grp as TaskGroupViewModel).GroupName == groupName).FirstOrDefault() as TaskGroupViewModel);
                if (group == null)
                {
                    group = new TaskGroupViewModel() { GroupName = groupName };
                    resultGroups.Add(group);
                }

                group.Add(task);
            }

            return resultGroups;
        }
        
        public enum TaskSelectOptions
        {
            CurrentDay,
            AllDaysByDate
        }
    }
}
