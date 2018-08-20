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
	public partial class TaskCreatePage : ContentPage
	{
        public TaskViewModel NewTask { get; set; }

        public TaskCreatePage(DateTime taskDate)
        {
            InitializeComponent();

            Resources["taskViewModel"] = new TaskViewModel();
            TaskDate.Date = taskDate;
        }

        protected void PrepareUserInput()
        {
            Summary.Text = Summary.Text?.Trim();
            Description.Text = Description.Text?.Trim();
        }

        protected bool ValidateUserInput()
        {
            TaskStartTime.TextColor = Color.Default;
            TaskEndTime.TextColor = Color.Default;
            Summary.PlaceholderColor = Color.Default;

            if (TaskStartTime.Time == TaskEndTime.Time || TaskStartTime.Time > TaskEndTime.Time)
            {
                TaskStartTime.TextColor = Color.Red;
                TaskEndTime.TextColor = Color.Red;
                return false;
            }

            if (String.IsNullOrEmpty(Summary.Text))
            {
                Summary.Placeholder = "Napiš název úkolu";
                Summary.PlaceholderColor = Color.Red;
                return false;
            }

            return true;
        }

        protected virtual void SaveUserInput()
        {
            NewTask = (Resources["taskViewModel"] as TaskViewModel);
            Navigation.PopModalAsync();
        }

        protected virtual void SaveButton_Clicked(object sender, EventArgs e)
        {
            PrepareUserInput();
            if (!ValidateUserInput())
                return;

            SaveUserInput();
        }

        protected virtual void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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
            switch(e.PropertyName)
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