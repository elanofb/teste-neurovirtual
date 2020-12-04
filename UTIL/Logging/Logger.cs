using System;
using System.Diagnostics;
using System.Text;

namespace UTIL {
	public class Logger : ILogger {

		public void information(string message) {
			Trace.TraceInformation(message);
		}

		public void information(string fmt, params object[] vars) {
			Trace.TraceInformation(fmt, vars);
		}

		public void information(Exception exception, string fmt, params object[] vars) {
			Trace.TraceInformation(FormatExceptionMessage(exception, fmt, vars));
		}

		public void warning(string message) {
			Trace.TraceWarning(message);
		}

		public void warning(string fmt, params object[] vars) {
			Trace.TraceWarning(fmt, vars);
		}

		public void warning(Exception exception, string fmt, params object[] vars) {
			Trace.TraceWarning(FormatExceptionMessage(exception, fmt, vars));
		}

		public void error(string message) {
			Trace.TraceError(message);
		}

		public void error(string fmt, params object[] vars) {
			Trace.TraceError(fmt, vars);
		}

		public void error(Exception exception, string fmt, params object[] vars) {
			Trace.TraceError(FormatExceptionMessage(exception, fmt, vars));
		}

		public void traceApi(string componentName, string method, TimeSpan timespan) {
			traceApi(componentName, method, timespan, "");
		}

		public void traceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars) {
			traceApi(componentName, method, timespan, string.Format(fmt, vars));
		}

		public void traceApi(string componentName, string method, TimeSpan timespan, string properties) {
			string message = String.Concat("Component:", componentName, ";Method:", method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
			Trace.TraceInformation(message);
		}

		private static string FormatExceptionMessage(Exception exception, string fmt, object[] vars) {
			UtilLog.saveError(exception, fmt);
			var sb = new StringBuilder();
			sb.Append(string.Format(fmt, vars));
			sb.Append(" Exception: ");
			sb.Append(exception.ToString());
			return sb.ToString();
		}
	}
}