using System;
using System.Collections.Generic;
using System.Text;

namespace TimedTasks.ViewModels
{
    public class TaskGroupViewModel : List<TaskViewModel>
    {
        public string GroupName { get; set; }
        //public string ShortDate { get; set; }
    }
}
