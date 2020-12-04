using System.Collections.Generic;
using Newtonsoft.Json;

namespace BLL.Notificacoes.Vendors.Amazon {
    
    public class Tags {
        
        [JsonProperty("ses_operation")]
        public List<string> sesoperation { get; set; }
        
        [JsonProperty("ses_configuration-set")]
        public List<string> sesconfigurationset { get; set; }
        
        [JsonProperty("ses_source-ip")]
        public List<string> sessourceip { get; set; }
        
        [JsonProperty("ses_from-domain")]
        public List<string> sesfromdomain { get; set; }
        
        [JsonProperty("ses_caller-identity")]
        public List<string> sescalleridentity { get; set; }
        
//        [JsonProperty("ses:operation")]
//        public IList<string> ses:operation { get; set; }
//
//        [JsonProperty("ses:configuration-set")]
//        public IList<string> ses:configuration-set { get; set; }
//
//        [JsonProperty("ses:source-ip")]
//        public IList<string> ses:source-ip { get; set; }
//
//        [JsonProperty("ses:from-domain")]
//        public IList<string> ses:from-domain { get; set; }
//
//        [JsonProperty("ses:caller-identity")]
//        public IList<string> ses:caller-identity { get; set; }
        
        /// <summary>
        /// Construtor
        /// </summary>
        public Tags() {

        }
    }
}
