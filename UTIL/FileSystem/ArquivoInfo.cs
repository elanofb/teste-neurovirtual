namespace UTIL.FileSystem {

    public class ArquivoInfo {
        
        public int idOrganizacao { get; set; }
        
        public string nome { get; set; }
        
        public string extensao { get; set; }
        
        public string nomeDiretorioPai { get; set; }
        
        public bool flagSobreporSeExistir { get; set; }
        
        public string conteudoTexto { get; set; }
    }

}
