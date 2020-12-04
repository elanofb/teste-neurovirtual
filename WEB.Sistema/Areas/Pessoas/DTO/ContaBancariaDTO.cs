namespace WEB.Areas.Pessoas.ViewModels {
    
    public class ContaBancariaDTO {
        
        public int id {get; set; }
        
        public int idPessoa {get; set; }
        
        public int idBanco { get; set; }
        
        public string nomeBanco { get; set; }
        
        public string nroAgencia { get; set; }
        
        public string nroDigitoAgencia {get; set; }
        
        public string nroContaBancaria {get; set; }
        
        public bool? flagContaCorrente {get; set; }
        
        public bool? flagContaPoupanca {get; set; }

    }
    
}
