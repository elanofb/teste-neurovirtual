using SimpleInjector;
using UTIL.FileSystem;

namespace WEB.App_Infrastructure.Core.DIComponents {

    public class UTIL_IoC {

        /// <summary>
        /// 
        /// </summary>
        public static void mapear(ref Container container) {

            container.Register<IUtilDirectory, UtilDirectory>(Lifestyle.Singleton);
            container.Register<IUtilFile, UtilFile>(Lifestyle.Singleton);

        }
    }
}
