using Newtonsoft.Json;

namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class Open {
        
        public string timestamp { get; set; }
        
        public string userAgent { get; set; }
        
        public string ipAddress { get; set; }
        
        /// <summary>
        /// Construtor
        /// </summary>
        public Open() {

        }
    }
}
