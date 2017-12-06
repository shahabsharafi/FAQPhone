using System;
using Android.App;
using System.Globalization;
using Infrastructure = FAQPhone.Droid.Infrastructure;

[assembly: Xamarin.Forms.Dependency(typeof(Infrastructure.PersianCalendarService))]
namespace FAQPhone.Droid.Infrastructure
{
    [Activity(Label = "PersianCalendarService")]
    public class PersianCalendarService: FAQPhone.CalendarService.IPersianCalendarService
    {
        public Calendar GetCalendar()
        {
            PersianCalendar pc = new PersianCalendar();
            return pc;
        }
    }
}