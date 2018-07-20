using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimedTasks.ViewModels
{
    public class TimedTasksViewModel : ViewModelBase
    {
        private DateTime selectedDate;
        private ObservableCollection<TaskViewModel> tasks;
        private bool showFinished;

        public DateTime SelectedDate { set { SetProperty(ref selectedDate, value); } get { return selectedDate; } }
        public ObservableCollection<TaskViewModel> Tasks { set { SetProperty(ref tasks, value); } get { return tasks; } }
        public bool ShowFinished { set { SetProperty(ref showFinished, value); } get { return showFinished; } }

        public ICommand IncreaseDateByDayCommand { private set; get; }
        public ICommand DecreaseDateByDayCommand { private set; get; }
        public ICommand IncreaseDateByMonthCommand { private set; get; }
        public ICommand DecreaseDateByMonthCommand { private set; get; }

        public ICommand AddNewTaskCommand { private set; get; }
        public ICommand RemoveTaskCommand { private set; get; }
        public ICommand UpdateTaskCommand { private set; get; }

        public TimedTasksViewModel()
        {
            Utils.Database.InitDB();
            Utils.Database.CreateDB();

            PropertyChanged += TimedTasksViewModel_PropertyChanged;

            tasks = new ObservableCollection<TaskViewModel>();
            //SelectedDate = DateTime.Today;

            IncreaseDateByDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(1));
            DecreaseDateByDayCommand = new Command(() => SelectedDate = SelectedDate.AddDays(-1));
            IncreaseDateByMonthCommand = new Command(() => SelectedDate = SelectedDate.AddMonths(1));
            DecreaseDateByMonthCommand = new Command(() => SelectedDate = SelectedDate.AddMonths(-1));

            AddNewTaskCommand = new Command<TaskViewModel>((task) => 
            {
                Utils.Database.InsertTask(task);
                RefreshTasks();
                //Tasks.Add(task);
            });

            RemoveTaskCommand = new Command<TaskViewModel>((task) =>
            {
                Utils.Database.DeleteTask(task);
                RefreshTasks();
                //Tasks.Remove(task);
            });

            UpdateTaskCommand = new Command<TaskViewModel>((task) =>
            {
                Utils.Database.UpdateTask(task);
                RefreshTasks();
            });
        }

        private void TimedTasksViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedDate": { RefreshTasks(); } break;
            }
        }

        private void RefreshTasks()
        {
            Tasks = new ObservableCollection<TaskViewModel>(Utils.Database.SelectTasks(SelectedDate.Date, !ShowFinished));
        }
    }
}
