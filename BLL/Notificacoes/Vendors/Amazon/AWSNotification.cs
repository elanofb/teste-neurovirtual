namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class AWSNotification {
        
        public string Type { get; set; }
        
        public string MessageId { get; set; }
        
        public string TopicArn { get; set; }
        
        public string Subject { get; set; }
        
        public Message Message { get; set; }
        
        public string Timestamp { get; set; }
        
        public string SignatureVersion { get; set; }
        
        public string Signature { get; set; }
        
        public string SigningCertURL { get; set; }
        
        public string UnsubscribeURL { get; set; }


        /// <summary>
        /// Construtor
        /// </summary>
        public AWSNotification() {

        }
    }
}
