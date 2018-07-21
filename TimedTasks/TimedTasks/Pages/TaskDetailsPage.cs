using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimedTasks.ViewModels;
using Xamarin.Forms;

namespace TimedTasks.Pages
{
    public class TaskDetailsPage : TaskCreatePage
    {
        protected TaskViewModel task;

        public TaskDetailsPage(TaskViewModel task, DateTime taskDate) : base(taskDate)
        {
            this.task = task;
            Resources["taskViewModel"] = task.Copy(true);
        }

        protected override void SaveUserInput()
        {
            NewTask = (Resources["taskViewModel"] as TaskViewModel).PopulateTask(task);
            Navigation.PopModalAsync();
        }
    }
}