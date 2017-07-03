// Helpers/Settings.cs
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace FAQPhone.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        const string usernameKey = "user";
        private static readonly string usernameDefault = null;
        const string passwordKey = "password";
        private static readonly string passwordDefault = null;

        #endregion

        public static string Username
        {
            get { return AppSettings.GetValueOrDefault(usernameKey, usernameDefault); }
            set { AppSettings.AddOrUpdateValue(usernameKey, value); }
        }

        public static string Password
        {
            get { return AppSettings.GetValueOrDefault(passwordKey, passwordDefault); }
            set { AppSettings.AddOrUpdateValue(passwordKey, value); }
        }
    }
}
