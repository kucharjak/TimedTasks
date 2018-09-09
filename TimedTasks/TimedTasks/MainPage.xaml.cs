using System;
using Xamarin.Forms;
using TimedTasks.Utils;
using TimedTasks.Pages;
using TimedTasks.ViewModels;
using static TimedTasks.ViewModels.TimedTasksViewModel;
using TimedTasks.Converters;

namespace TimedTasks
{
	public partial class MainPage : ContentPage
	{
        bool listViewAnimationRunning = false;
        const int listViewAnimationTime = 100;

        TimedTasksViewModel tasks;

        public MainPage()
        {
			InitializeComponent();

            dateSelector.Date = DateTime.Today;

            tasks = (Resources["timedTasksViewModel"] as TimedTasksViewModel);
            
            ToolbarFinished.Command = tasks.ChangeFinishedVisibilityCommand;

            ToolbarFinished.BindingContext = tasks;
            ToolbarFinished.SetBinding(MenuItem.IconProperty, "FinishedTasksVisible", converter: new SetValueIfTrueConverter()
                {
                    TrueValue = ImageSource.FromFile("baseline_visibility_white_36dp.png"),
                    FalseValue = ImageSource.FromFile("baseline_visibility_off_white_36dp.png")
                });

            this.BindingContext = tasks;
            this.SetBinding(ContentPage.TitleProperty, "Title");
            this.ToolbarItems.Add(new ToolbarItem() { Text = "Denní úkoly", Order = ToolbarItemOrder.Secondary, Command = tasks.ShowDailyTasksCommand });
            this.ToolbarItems.Add(new ToolbarItem() { Text = "Všechny úkoly", Order = ToolbarItemOrder.Secondary, Command = tasks.ShowAllTasksCommand });
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
                                item.FinishOrResumeCommand.Execute(tasks);
                            }
                            break;
                        case "Smazat":
                            {
                                if (await DisplayAlert("Opravdu?", "Opravdu chcete úkol smazat?", "Ano", "Ne"))
                                {
                                    item.RemoveCommand.Execute(tasks);
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
                tasks.AddNewTaskCommand.Execute(page.NewTask);
        }

        private void TaskDetailsPage_Disapearing(object sender, EventArgs e)
        {
            var page = (sender as TaskDetailsPage);
            if (page.NewTask != null)
                tasks.UpdateTaskCommand.Execute(page.NewTask);
        }
        
        private async void Button_DecreaseDay_Clicked(object sender, EventArgs e)
        {
            if (listViewAnimationRunning)
                return;

#pragma warning disable  CS4014 
            listViewAnimationRunning = true;
            listView.FadeTo(0.2, listViewAnimationTime);
            await listView.TranslateTo(listView.Width, 0, listViewAnimationTime, Easing.SinIn);

            tasks.DecreaseDateByDayCommand.Execute(null);

            listView.TranslationX = -1 * listView.Width;
            listView.FadeTo(1, listViewAnimationTime);
            await listView.TranslateTo(0, 0, listViewAnimationTime, Easing.SinOut);
            listViewAnimationRunning = false;
#pragma warning restore
        }

        private async void Button_IncreaseDay_Clicked(object sender, EventArgs e)
        {
            if (listViewAnimationRunning)
                return;

#pragma warning disable  CS4014 
            listViewAnimationRunning = true;
            listView.FadeTo(0.2, listViewAnimationTime);
            await listView.TranslateTo(-1 * listView.Width, 0, listViewAnimationTime, Easing.SinIn);

            tasks.IncreaseDateByDayCommand.Execute(null);

            listView.TranslationX = listView.Width;
            listView.FadeTo(1, listViewAnimationTime);
            await listView.TranslateTo(0, 0, listViewAnimationTime, Easing.SinOut);
            listViewAnimationRunning = false;
#pragma warning restore
        }
    }
}
