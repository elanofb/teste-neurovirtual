using System;
using System.Collections.Generic;

namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class Mail {
        
        public DateTime? timestamp { get; set; }
        
        public string source { get; set; }
        
        public string sourceArn { get; set; }
        
        public string sendingAccountId { get; set; }
        
        public string messageId { get; set; }
        
        public List<string> destination { get; set; }
        
        public string headersTruncated { get; set; }
        
        public List<Header> headers { get; set; }
        
        public CommonHeaders commonHeaders { get; set; }
        
        public Tags tags { get; set; }
        

        /// <summary>
        /// Construtor
        /// </summary>
        public Mail() {

        }
    }
}
