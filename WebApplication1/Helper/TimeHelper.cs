using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Helper
{
    public interface ITimeHelper
    {
        public string Converter(string date);
        public string Converter(DateTime date); 
    }
    public class TimeHelper : ITimeHelper
    {
        public string Converter(string date)
        {
            DateTime datetime = Convert.ToDateTime(date);
            TimeSpan time = DateTime.Now.Subtract(datetime);
            if (time.TotalSeconds < 60.00)
            {
                return ((int)time.TotalSeconds == 1) ? (int)time.TotalSeconds + " " + " secondo fa" : (int)time.TotalSeconds + " " + " secondi fa";
            }
            if (time.TotalMinutes < 60.00)
            {
                return ((int)time.TotalMinutes == 1) ? (int)time.TotalMinutes + " " + " minuto fa" : (int)time.TotalMinutes + " " + " minuti fa";
            }
            if (time.TotalHours < 24.00)
            {

                return ((int)time.TotalHours == 1) ? (int)time.TotalHours + " " + " ora fa" : (int)time.TotalHours + " " + " ore fa";
            }
            return datetime.ToString("dd MMM");
            throw new NotImplementedException();
        }

        public string Converter(DateTime date)
        {
            TimeSpan time = DateTime.Now.Subtract(date);
            if (time.TotalSeconds < 60.00)
            {
                return ((int)time.TotalSeconds == 1) ? (int)time.TotalSeconds + " " + " secondo fa" : (int)time.TotalSeconds + " " + " secondi fa";
            }
            if (time.TotalMinutes < 60.00)
            {
                return ((int) time.TotalMinutes==1)? (int)time.TotalMinutes + " " + " minuto fa" : (int)time.TotalMinutes + " " + " minuti fa";
            }
            if (time.TotalHours < 24.00)
            {
                
                return ((int) time.TotalHours==1)? (int)time.TotalHours + " " + " ora fa" : (int)time.TotalHours + " " + " ore fa";
            }
            return date.ToString("dd MMM");
            throw new NotImplementedException();
        }
    }
}
