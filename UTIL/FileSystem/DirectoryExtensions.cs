using System;
using System.IO;

namespace UTIL.FileSystem {

    public static class DirectoryExtensions {

        
        /// <summary>
        /// 
        /// </summary>
        public static string virtualPath(this FileInfo OFileInfo) {
            
            string  dirApp = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "");

            string virtualPath = OFileInfo.FullName.Replace(dirApp, "");

            return virtualPath;

        }
    }

}
