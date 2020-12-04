using System.Web.Mvc;

namespace WEB.App_Infrastructure {
    public class SincViewEngine : RazorViewEngine {

        //
        public SincViewEngine() : base() {
            ViewLocationFormats = new[] {
                "~/views/{1}/{0}.cshtml"
            };
		

            PartialViewLocationFormats = new[] {
                "~/views/{1}/{0}.cshtml",
				"~/views/shared/partial/{0}.cshtml"
            };
        }

    }
}