using System.Collections.Generic;

namespace DAL.Arquivos {

	public class ArquivoUploadExtensao {

        public static readonly List<string> LISTATIPOIMG = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".psd", ".bmp", ".jpg2" };
        public static readonly List<string> LISTATIPODOC = new List<string> { ".csv", ".pdf", ".txt", ".xls", ".xlsx", ".doc" };
        public static readonly List<string> LISTATIPOAUDVID = new List<string> { ".mp4", ".mp3", ".wma", ".aac", ".ogg", ".mpeg" };
        
    }
}