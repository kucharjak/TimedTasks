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
        private bool showFinishedTasks;
        private TimedTasksDateOption dateOption;
        private TimedTasksGroupOption groupOption;

        public DateTime SelectedDate { set { SetProperty(ref selectedDate, value); } get { return selectedDate; } }
        public ObservableCollection<TaskViewModel> Tasks { set { SetProperty(ref tasks, value); } get { return tasks; } }
        public ObservableCollection<TaskGroupViewModel> GroupedTasks { set { SetProperty(ref groupedTasks, value); } get { return groupedTasks; } }
        public bool ShowFinishedTasks { set { SetProperty(ref showFinishedTasks, value); } get { return showFinishedTasks; } }
        public TimedTasksDateOption DateOption { set { SetProperty(ref dateOption, value); } get { return dateOption; } }
        public TimedTasksGroupOption GroupOption { set { SetProperty(ref groupOption, value); } get { return groupOption; } }

        public ICommand IncreaseDateByDayCommand { private set; get; }
        public ICommand DecreaseDateByDayCommand { private set; get; }
        public ICommand IncreaseDateByMonthCommand { private set; get; }
        public ICommand DecreaseDateByMonthCommand { private set; get; }

        public ICommand AddNewTaskCommand { private set; get; }
        public ICommand RemoveTaskCommand { private set; get; }
        public ICommand UpdateTaskCommand { private set; get; }

        public ICommand ShowDailyTasksCommand { private set; get; }
        public ICommand ShowAllTasksCommand { private set; get; }

        public TimedTasksViewModel()
        {
            Utils.Database.InitDB();
            Utils.Database.CreateDB();

            PropertyChanged += TimedTasksViewModel_PropertyChanged;

            tasks = new ObservableCollection<TaskViewModel>();
            groupedTasks = new ObservableCollection<TaskGroupViewModel>();
            //SelectedDate = DateTime.Today;

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
                DateOption = TimedTasksDateOption.TasksForSelectedDate;
                GroupOption = TimedTasksGroupOption.None;
            });

            ShowAllTasksCommand = new Command(() =>
            {
                DateOption = TimedTasksDateOption.AllTasksEver;
                GroupOption = TimedTasksGroupOption.GroupByLongDate;
            });
        }

        private void TimedTasksViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(GroupOption):
                case nameof(DateOption):
                case nameof(ShowFinishedTasks):
                case nameof(SelectedDate):
                    {
                        RefreshTasks();
                    } break;
            }
        }

        private void RefreshTasks()
        {
            var tasks = (DateOption == TimedTasksDateOption.AllTasksEver) ?
                Utils.Database.SelectAllTasks(!ShowFinishedTasks) :
                Utils.Database.SelectTasks(SelectedDate.Date, !ShowFinishedTasks);

            tasks = tasks.OrderBy(task => task.DueDate).ThenBy(task => task.StartTime).ToList();

            if (GroupOption == TimedTasksGroupOption.None)
            {
                GroupedTasks.Clear();
                Tasks = new ObservableCollection<TaskViewModel>(tasks);
            }
            else
            {
                Tasks.Clear();
                GroupedTasks = GroupTasks(tasks, GroupOption);
            }
        }

        private ObservableCollection<TaskGroupViewModel> GroupTasks(List<TaskViewModel> tasks, TimedTasksGroupOption grouping)
        {
            var resultGroups = new ObservableCollection<TaskGroupViewModel>();

            foreach (var task in tasks)
            {
                var groupName = "";
                switch (grouping)
                {
                    case TimedTasksGroupOption.GroupByDate:
                        {
                            groupName = task.DueDate.ToString("d");
                        }
                        break;
                    case TimedTasksGroupOption.GroupByLongDate:
                        {
                            groupName = task.DueDate.ToString("D");
                        }
                        break;
                    case TimedTasksGroupOption.GroupByDay:
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case TimedTasksGroupOption.GroupByTime:
                        {
                            groupName = task.StartTime.TotalHours.ToString() + ":00";
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

        public enum TimedTasksDateOption
        {
            AllTasksEver,
            TasksForSelectedDate
        }

        public enum TimedTasksGroupOption
        {
            None,
            GroupByTime,
            GroupByDate,
            GroupByLongDate,
            GroupByDay
        }
    }
}
