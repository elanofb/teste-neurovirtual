using System.Collections.Generic;

namespace System.Json {

    public class JsonMessage{

        public bool error { get; set; }
        public string message { get; set; }
        public IList<string> listMessage {get; set;}
        public object extraInfo { get; set; }
        
        public JsonMessage(){
            this.listMessage = new List<string>();
        }
    }

    public class JsonMessageStatus: JsonMessage{
        public string active { get; set; }
    }
}
