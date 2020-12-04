namespace DAL.RedeAfiliados.DTO {

    public class NovoMembroRede {
        
        public int idMembro { get; set; }
        
        public int idMembroPai { get; set; }
        
        public bool? flagEsquerda { get; set; }
        
        public bool? flagDireita { get; set; }
    }

}
