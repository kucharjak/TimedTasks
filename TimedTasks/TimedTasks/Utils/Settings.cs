using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using TimedTasks.ViewModels;

namespace TimedTasks.Utils
{
    public static class Settings
    {
        const string showAllTasksSettingName = "showAllTasks";
        const string showFinishedSettingName = "showFinished";

        public static bool ShowAllTasks
        {
            get { return (bool)GetSetting(showAllTasksSettingName, false); }
            set { SetSettings(showAllTasksSettingName, value); }
        }

        public static bool ShowFinished
        {
            get { return (bool)GetSetting(showFinishedSettingName, false); }
            set { SetSettings(showFinishedSettingName, value); }
        }

        private static void SetSettings(string settingName, object value)
        {
            var properties = Application.Current.Properties;
            if (properties.ContainsKey(settingName))
                properties[settingName] = value;
            else
                properties.Add(settingName, value);
        }

        private static object GetSetting(string settingName, object defaultValue)
        {
            var properties = Application.Current.Properties;
            if (properties.ContainsKey(settingName))
                return properties[settingName];

            return defaultValue;
        }
    }
}
