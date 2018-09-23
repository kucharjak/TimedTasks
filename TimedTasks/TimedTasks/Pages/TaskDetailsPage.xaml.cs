using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimedTasks.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimedTasks.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetailsPage : ContentPage
    {
        public TaskViewModel ResultTask = null;

        public TaskDetailsPage(string title)
        {
            InitializeComponent();

            Title = title;
        }

        public TaskDetailsPage(string title, DateTime startDate)
        {
            InitializeComponent();

            Title = title;
            Resources["taskViewModel"] = new TaskViewModel();
            TaskDate.Date = startDate;
        }

        public TaskDetailsPage(string title, TaskViewModel taskViewModel)
        {
            InitializeComponent();

            Title = title;
            Resources["taskViewModel"] = taskViewModel.Copy(true);
        }

        private void ToolbarOK_Clicked(object sender, EventArgs e)
        {
            PrepareUserInput();
            if (!ValidateUserInput())
                return;

            SaveUserInput();
        }

        protected void PrepareUserInput()
        {
            Summary.Text = Summary.Text?.Trim();
            Description.Text = Description.Text?.Trim();
        }

        protected bool ValidateUserInput()
        {
            TaskStartTime.TextColor = Color.White;
            TaskEndTime.TextColor = Color.White;
            Summary.PlaceholderColor = Color.White;

            if (TaskStartTime.Time == TaskEndTime.Time || TaskStartTime.Time > TaskEndTime.Time)
            {
                TaskStartTime.TextColor = Color.Red;
                TaskEndTime.TextColor = Color.Red;
                return false;
            }

            if (String.IsNullOrEmpty(Summary.Text))
            {
                Summary.Placeholder = "Doplň název úkolu";
                Summary.PlaceholderColor = Color.Red;
                return false;
            }

            return true;
        }

        protected virtual void SaveUserInput()
        {
            ResultTask = (Resources["taskViewModel"] as TaskViewModel);
            Navigation.PopAsync();
        }

        private void TaskEndTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TimePicker.Time):
                    {
                        if (TaskEndTime.Time < TaskStartTime.Time)
                            TaskStartTime.Time = TaskEndTime.Time;
                    }
                    break;
            }
        }

        private void TaskStartTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TimePicker.Time):
                    {
                        if (TaskStartTime.Time > TaskEndTime.Time)
                            TaskEndTime.Time = TaskStartTime.Time;
                    }
                    break;
            }
        }
    }
}