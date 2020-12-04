using System.Web.Mvc;

namespace WEB.App_Infrastructure {

    public class FilterConfig {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {

            filters.Add(new HandleErrorCustom());
            
			filters.Add(new FilterSecurity());
        }
    }
}
