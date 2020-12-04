using System.Collections.Generic;
using System.Web.Mvc;
namespace System{

    public static class UtilMessage{

        public const string TYPE_MESSAGE_SUCCESS = "success";
        public const string TYPE_MESSAGE_DANGER = "danger";
        public const string TYPE_MESSAGE_WARNING = "warning";
        public const string TYPE_MESSAGE_INFO = "info";
        public const string TYPE_MESSAGE_ERROR = "error";

        private const string MESSAGE_SUCCESS = "<h4><i class=\"icon fa fa-check\"></i> {0}</h4>{1}";
        private const string MESSAGE_SUCCESS_SMILE = "<div class='pull-left' style='font-size:3.5em;margin-right:15px;'><i class=\"icon fal fa-smile\"></i></div><div><h4> {0}</h4>{1}</div><div class='clearfix'></div>";
        private const string MESSAGE_WARNING = "<h4><i class=\"icon fa fa-warning\"></i> {0}</h4>{1}";
        private const string MESSAGE_ERROR = "<h4><i class=\"icon fa fa-ban\"></i> {0}</h4>{1}";
        private const string MESSAGE_ERROR_FACEDOWN = "<div class='pull-left' style='font-size:3.5em;margin-right:15px;'><i class=\"icon fal fa-frown\"></i></div><div><h4 class='text-red'> {0}</h4>{1}</div><div class='clearfix'></div>";
        private const string MESSAGE_WARNING_TRIANGLE = "<div class='pull-left' style='font-size:3.5em;margin-right:15px;'><i class=\"icon far fa-exclamation-triangle\"></i></div><div><h4 class='text-yellow'> {0}</h4>{1}</div><div class='clearfix'></div>";

        public static IList<string> listErrors = new List<string>();
        public static IList<string> listSuccess = new List<string>();
        public static IList<string> listWarnings = new List<string>();
        public static IList<string> listInfos = new List<string>();
        public static TempDataDictionary TempData = new TempDataDictionary();

        //
        public static string success(string title, string message) {
            return string.Format(MESSAGE_SUCCESS, title, message);
        }
        
        //
        public static string successSmile(string title, string message) {
            return string.Format(MESSAGE_SUCCESS_SMILE, title, message);
        }

        //
        public static string error(string title, string message) {
            return string.Format(MESSAGE_ERROR, title, message);
        }

        //
        public static string errorFaceDown(string title, string message) {
            return string.Format(MESSAGE_ERROR_FACEDOWN, title, message);
        }

        //
        public static string warning(string title, string message) {
            return string.Format(MESSAGE_WARNING, title, message);
        }
        
        //
        public static string warningTriangle(string title, string message) {
            return string.Format(MESSAGE_WARNING_TRIANGLE, title, message);
        }
        


        public static void clear() {
            TempData.Clear();
            listErrors.Clear();
            listWarnings.Clear();
            listSuccess.Clear();
            listInfos.Clear();
        }
    }

}
