namespace System {

	public class DateTimeFactory {

		private static DateTime? _datetimeNow;

		private static DateTime? _datetimeToday;

        //Wrapper Today
		public static DateTime Today {
			get {
			    if (_datetimeToday == null) {
			        return DateTime.Today;
			    }
					
				return _datetimeToday.Value;
			}
		}

        //Wrapper Now
		public static DateTime Now {
			get {
			    if (_datetimeNow == null) {
			        return DateTime.Today;
			    }
					
				return _datetimeNow.Value;
			}
		}

        //
		public static void setCurrentToday(DateTime date) {
			_datetimeToday = date;
		}

        //
		public static void setCurrentNow(DateTime date) {
			_datetimeNow = date;
		}
	}
}