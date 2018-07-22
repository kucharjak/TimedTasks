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
using TimedTasks.Utils;

namespace TimedTasks
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
			InitializeComponent();

            dateSelector.Date = DateTime.Today;

            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowAll = Settings.ShowAllSetting;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinished = Settings.ShowFinishedSetting;
            RefreshPage();
        }

        private void RefreshPage()
        {
            if ((Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowAll)
                Title = "Všechny úkoly";
            else
                Title = "Denní úkoly";

            if ((Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinished)
                ToolbarFinished.Icon = "baseline_visibility_white_48dp.png";
            else
                ToolbarFinished.Icon = "baseline_visibility_off_white_48dp.png";
        }

        private void listView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                if (listView.SelectedItem != null)
                {
                    var page = new TaskDetailsPage((TaskViewModel)listView.SelectedItem, dateSelector.Date);
                    page.Disappearing += TaskDetailsPage_Disapearing;
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
        
        private void ToolbarDaily_Activated(object sender, EventArgs e)
        {
            Settings.ShowAllSetting = false;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowAll = false;
            RefreshPage();
        }

        private void ToolbarAll_Activated(object sender, EventArgs e)
        {
            Settings.ShowAllSetting = true;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowAll = true;
            RefreshPage();
        }

        private void ToolbarFinished_Activated(object sender, EventArgs e)
        {
            var setting = !Settings.ShowFinishedSetting;
            Settings.ShowFinishedSetting = setting;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinished = setting;
            RefreshPage();
        }
    }
}
