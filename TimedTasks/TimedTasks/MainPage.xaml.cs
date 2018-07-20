using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TimedTasks.ViewModels;
using TimedTasks.Pages;
using Xamarin.Forms;

namespace TimedTasks
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
			InitializeComponent();
		}

        private void listView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                if (listView.SelectedItem != null)
                {
                    var page = new TaskDetailsPage((TaskViewModel)listView.SelectedItem, dateSelector.Date);
                    //page.Disappearing += TaskDetailsPage_Disapearing;
                    Navigation.PushModalAsync(page);
                }
                listView.SelectedItem = null;
            }
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            var page = new TaskCreatePage(dateSelector.Date);
            page.Disappearing += CreateTaskPage_Disappearing;
            Navigation.PushModalAsync(page);
        }

        private void CreateTaskPage_Disappearing(object sender, EventArgs e)
        {
            var page = (sender as TaskCreatePage);
            if (page.NewTask != null)
                (Resources["timedTasksViewModel"] as TimedTasksViewModel).AddNewTaskCommand.Execute(page.NewTask);
        }

        private void TaskDetailsPage_Disapearing(object sender, EventArgs e)
        {
            var page = (sender as TaskDetailsPage);
            if (page.NewTask != null)
                (Resources["timedTasksViewModel"] as TimedTasksViewModel).UpdateTaskCommand.Execute(page.NewTask);
        }
    }
}
