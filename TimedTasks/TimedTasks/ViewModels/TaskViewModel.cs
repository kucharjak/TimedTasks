using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace TimedTasks.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private int id;
        private DateTime dueDate;
        private TimeSpan startTime, endTime;
        private string summary, description;
        private bool finished;
        private Color backgroundColor;

        [PrimaryKey, AutoIncrement]
        public int Id { set { SetProperty(ref id, value); } get { return id; } }

        public DateTime DueDate { set { SetProperty(ref dueDate, value); } get { return dueDate; } }
        public TimeSpan StartTime { set { SetProperty(ref startTime, value); } get { return startTime; } }
        public TimeSpan EndTime { set { SetProperty(ref endTime, value); } get { return endTime; } }

        [MaxLength(50)]
        public string Summary { set { SetProperty(ref summary, value); } get { return summary; } }
        [MaxLength(2500)]
        public string Description { set { SetProperty(ref description, value); } get { return description; } }

        public bool Finished { set { SetProperty(ref finished, value); } get { return finished; } }

        [Ignore]
        public Color BackgroundColor { set { SetProperty(ref backgroundColor, value); } get { return backgroundColor; } }

        public TaskViewModel()
        {
        }

        public TaskViewModel Copy(bool copyId)
        {
            var copy = new TaskViewModel();
            copy.Id = Id;
            copy.DueDate = DueDate;
            copy.StartTime = StartTime;
            copy.EndTime = EndTime;
            copy.Summary = Summary;
            copy.Description = Description;
            copy.Finished = Finished;

            copy.BackgroundColor = BackgroundColor;

            return copy;
        }

        public TaskViewModel PopulateTask(TaskViewModel task)
        {
            task.Id = Id;
            task.DueDate = DueDate;
            task.StartTime = StartTime;
            task.EndTime = EndTime;
            task.Summary = Summary;
            task.Description = Description;
            task.Finished = Finished;

            task.BackgroundColor = BackgroundColor;

            return task;
        }
    }
}
