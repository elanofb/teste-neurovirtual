namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class Message {
        
        public string eventType { get; set; }
        
        public Mail mail { get; set; }
        
        public Open open { get; set; }
        
//        public string send { get; set; }


        /// <summary>
        /// Construtor
        /// </summary>
        public Message() {

        }
    }
}
