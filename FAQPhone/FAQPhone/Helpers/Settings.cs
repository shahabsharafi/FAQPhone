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
        const string languageKey = "language";
        private static readonly string languageDefault = "Fa";
        const string localPassword = "localPassword";
        private static readonly string localPasswordDefault = null;
        const string usernameKey = "user";
        private static readonly string usernameDefault = null;
        const string passwordKey = "password";
        private static readonly string passwordDefault = null;

        #endregion

        public static string Language
        {
            get { return AppSettings.GetValueOrDefault(languageKey, languageDefault); }
            set { AppSettings.AddOrUpdateValue(languageKey, value); }
        }

        public static string LocalPassword
        {
            get { return AppSettings.GetValueOrDefault(localPassword, localPasswordDefault); }
            set { AppSettings.AddOrUpdateValue(localPassword, value); }
        }

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
