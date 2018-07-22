using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimedTasks.Utils
{
    public static class Settings
    {
        const string showAllSettingName = "showAll";
        const string showFinishedSettingsName = "showFinished";

        public static bool ShowAllSetting
        {
            get { return (bool)GetSetting(showAllSettingName, false); }
            set { SetSettings(showAllSettingName, value); }
        }

        public static bool ShowFinishedSetting
        {
            get { return (bool)GetSetting(showFinishedSettingsName, false); }
            set { SetSettings(showFinishedSettingsName, value); }
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
