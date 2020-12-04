using System;

namespace UTIL{

    public interface ILogger{
        void information(string message);
        void information(string fmt, params object[] vars);
        void information(Exception exception, string fmt, params object[] vars);

        void warning(string message);
        void warning(string fmt, params object[] vars);
        void warning(Exception exception, string fmt, params object[] vars);

        void error(string message);
        void error(string fmt, params object[] vars);
        void error(Exception exception, string fmt, params object[] vars);

        void traceApi(string componentName, string method, TimeSpan timespan);
        void traceApi(string componentName, string method, TimeSpan timespan, string properties);
        void traceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars);

    }
}