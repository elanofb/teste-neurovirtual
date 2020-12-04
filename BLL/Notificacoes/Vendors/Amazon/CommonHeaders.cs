using System.Collections.Generic;

namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class CommonHeaders {
        
        public List<string> from { get; set; }
        
        public List<string> to { get; set; }
        
        public string messageId { get; set; }
        
        public string subject { get; set; }


        /// <summary>
        /// Construtor
        /// </summary>
        public CommonHeaders() {

        }
    }
}
