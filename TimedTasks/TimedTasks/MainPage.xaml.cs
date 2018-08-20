using System;
using Xamarin.Forms;
using TimedTasks.Utils;
using TimedTasks.Pages;
using TimedTasks.ViewModels;
using static TimedTasks.ViewModels.TimedTasksViewModel;

namespace TimedTasks
{
	public partial class MainPage : ContentPage
	{
        bool listViewAnimationRunning = false;
        const int listViewAnimationTime = 100;

        public MainPage()
        {
			InitializeComponent();

            dateSelector.Date = DateTime.Today;

            (Resources["timedTasksViewModel"] as TimedTasksViewModel).PropertyChanged += MainPage_PropertyChanged;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinishedTasks = Settings.ShowFinished;

            RefreshVisibilityIcon();

            if (Settings.ShowAllTasks)
                ShowAllTasks();
            else
                ShowDailyTasks();
        }

        private void MainPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(TimedTasksViewModel.ShowFinishedTasks): RefreshVisibilityIcon(); break;
            } 
        }

        private void RefreshVisibilityIcon()
        {
            if ((Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinishedTasks)
                ToolbarFinished.Icon = "baseline_visibility_white_36dp.png";
            else
                ToolbarFinished.Icon = "baseline_visibility_off_white_36dp.png";
        }

        private async void listView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                if (listView.SelectedItem != null)
                {
                    var item = (listView.SelectedItem as TaskViewModel);
                    listView.SelectedItem = null;
                    var result = await DisplayActionSheet(
                        item.Summary,
                        null,
                        null,
                        "Upravit",
                        !item.Finished ? "Dokončit" : "Obnovit",
                        "Smazat");

                    switch(result)
                    {
                        case "Upravit":
                            {
                                var page = new TaskDetailsPage(item, dateSelector.Date);
                                page.Disappearing += TaskDetailsPage_Disapearing;
                                await Navigation.PushModalAsync(page);
                            }
                            break;
                        case "Dokončit":
                        case "Obnovit":
                            {
                                item.FinishOrResumeCommand.Execute((Resources["timedTasksViewModel"] as TimedTasksViewModel));
                            }
                            break;
                        case "Smazat":
                            {
                                if (await DisplayAlert("Opravdu?", "Opravdu chcete úkol smazat?", "Ano", "Ne"))
                                {
                                    item.RemoveCommand.Execute((Resources["timedTasksViewModel"] as TimedTasksViewModel));
                                }
                            }
                            break;
                    }   
                }
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
            Settings.ShowAllTasks = false;
            ShowDailyTasks();
            
        }

        private void ToolbarAll_Activated(object sender, EventArgs e)
        {
            Settings.ShowAllTasks = true;
            ShowAllTasks();
        }

        private void ShowDailyTasks()
        {
            Title = "Denní úkoly";
            listView.SetBinding(ListView.ItemsSourceProperty, "Tasks");
            listView.IsGroupingEnabled = false;
            dateView.IsVisible = true;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowDailyTasksCommand.Execute(null);
        }

        private void ShowAllTasks()
        {
            Title = "Všechny úkoly";
            listView.SetBinding(ListView.ItemsSourceProperty, "GroupedTasks");
            listView.IsGroupingEnabled = true;
            dateView.IsVisible = false;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowAllTasksCommand.Execute(null);
        }

        private void ToolbarFinished_Activated(object sender, EventArgs e)
        {
            var setting = !Settings.ShowFinished;
            Settings.ShowFinished = setting;
            (Resources["timedTasksViewModel"] as TimedTasksViewModel).ShowFinishedTasks = setting;
        }

        private async void Button_DecreaseDay_Clicked(object sender, EventArgs e)
        {
            if (listViewAnimationRunning)
                return;

            listViewAnimationRunning = true;
            listView.FadeTo(0.2, listViewAnimationTime);
            await listView.TranslateTo(listView.Width, 0, listViewAnimationTime, Easing.SinIn);

            (Resources["timedTasksViewModel"] as TimedTasksViewModel).DecreaseDateByDayCommand.Execute(null);

            listView.TranslationX = -1 * listView.Width;
            listView.FadeTo(1, listViewAnimationTime);
            await listView.TranslateTo(0, 0, listViewAnimationTime, Easing.SinOut);
            listViewAnimationRunning = false;
        }

        private async void Button_IncreaseDay_Clicked(object sender, EventArgs e)
        {
            if (listViewAnimationRunning)
                return;

            listViewAnimationRunning = true;
            listView.FadeTo(0.2, listViewAnimationTime);
            await listView.TranslateTo(-1 * listView.Width, 0, listViewAnimationTime, Easing.SinIn);

            (Resources["timedTasksViewModel"] as TimedTasksViewModel).IncreaseDateByDayCommand.Execute(null);

            listView.TranslationX = listView.Width;
            listView.FadeTo(1, listViewAnimationTime);
            await listView.TranslateTo(0, 0, listViewAnimationTime, Easing.SinOut);
            listViewAnimationRunning = false;
        }
    }
}
