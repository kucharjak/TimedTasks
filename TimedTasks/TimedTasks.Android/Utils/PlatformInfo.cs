using System;
using Android.OS;
using Xamarin.Forms;
using TimedTasks.Interfaces;

[assembly: Dependency(typeof(TimedTasks.Droid.Utils.PlatformInfo))]

namespace TimedTasks.Droid.Utils
{
    public class PlatformInfo : IPlatformInfo
    {
        public string GetPersonalPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}