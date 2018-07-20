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

        protected bool ValidateValues()
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

        protected virtual void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (!ValidateValues())
                return;

            NewTask = (Resources["taskViewModel"] as TaskViewModel);
            Navigation.PopModalAsync();
        }

        protected virtual void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}