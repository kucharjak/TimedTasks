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
        private ObservableCollection<TaskViewModel> tasks;
        private ObservableCollection<TaskGroupViewModel> groupedTasks;
        private bool finishedTasksVisible;
        private TaskSelectOptions taskSelectOption;

        public DateTime SelectedDate { set { SetProperty(ref selectedDate, value); } get { return selectedDate; } }
        public ObservableCollection<TaskViewModel> Tasks { set { SetProperty(ref tasks, value); } get { return tasks; } }
        public ObservableCollection<TaskGroupViewModel> GroupedTasks { set { SetProperty(ref groupedTasks, value); } get { return groupedTasks; } }
        public bool FinishedTasksVisible { private set { SetProperty(ref finishedTasksVisible, value); } get { return finishedTasksVisible; } }
        public TaskSelectOptions TaskSelectOption { set { SetProperty(ref taskSelectOption, value); } get { return taskSelectOption; } }

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

            tasks = new ObservableCollection<TaskViewModel>();
            groupedTasks = new ObservableCollection<TaskGroupViewModel>();

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
                LoadTaskVisibility();
                RefreshTasks();
            });

            ShowAllTasksCommand = new Command(() =>
            {
                TaskSelectOption = TaskSelectOptions.AllDaysByDate;

                Utils.AppData.Data.TaskSelectOption = TaskSelectOption.ToString();
                LoadTaskVisibility();
                RefreshTasks();
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
            RefreshTasks();
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
            }
        }

        private void RefreshTasks()
        {
            var tasks = (TaskSelectOption == TaskSelectOptions.AllDaysByDate) ?
                Utils.Database.SelectAllTasks(!FinishedTasksVisible) :
                Utils.Database.SelectTasks(SelectedDate.Date, !FinishedTasksVisible);

            tasks = tasks.OrderBy(task => task.DueDate).ThenBy(task => task.StartTime).ToList();

            if (TaskSelectOption == TaskSelectOptions.CurrentDay)
            {
                GroupedTasks.Clear();
                Tasks = new ObservableCollection<TaskViewModel>(tasks);
            }
            else
            {
                Tasks.Clear();
                GroupedTasks = GroupTasks(tasks, TaskSelectOption);
            }
        }

        private ObservableCollection<TaskGroupViewModel> GroupTasks(List<TaskViewModel> tasks, TaskSelectOptions grouping)
        {
            var resultGroups = new ObservableCollection<TaskGroupViewModel>();

            foreach (var task in tasks)
            {
                var groupName = "";
                switch (grouping)
                {
                    case TaskSelectOptions.AllDaysByDate:
                        {
                            groupName = task.DueDate.ToString("D");
                        }
                        break;
                }

                var group = resultGroups.Where(grp => grp.GroupName == groupName).FirstOrDefault();
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
