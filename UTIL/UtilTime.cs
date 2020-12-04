using System.Globalization;

namespace System {

    public static class UtilTime {
        
        //
        public static TimeSpan? toTimeSpan(string value) {

            TimeSpan time;

            return TimeSpan.TryParse(value, out time) ? time : (TimeSpan?) null;

        }

        //
        public static bool isValid(string time) {

            TimeSpan result;

            return TimeSpan.TryParse(time.Trim(), out result);

        }
        
    }
}
