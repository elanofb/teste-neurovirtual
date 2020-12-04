using System.Collections.Generic;

namespace System {

    public class UtilRetorno {
        
        public bool         flagError { get; set; }
        public List<string> listaErros { get; set; }
        public object       info { get; set; }
        
        private static UtilRetorno _instance;
        
        public static UtilRetorno getInstance() 
            => _instance = _instance 
            ?? new UtilRetorno();
        
        public UtilRetorno() {
            this.listaErros = new List<string>();
        }
        
        public static UtilRetorno newInstance(bool flagErro, string message = "", object info = null) {
            var OUtilRetorno = new UtilRetorno();

            OUtilRetorno.flagError = flagErro;

            if (!String.IsNullOrEmpty(message)) {
                OUtilRetorno.listaErros.Add(message);
            }

            if (info != null) {
                OUtilRetorno.info = info;
            }

            return OUtilRetorno;
        }
    }

}