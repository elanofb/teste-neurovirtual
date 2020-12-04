using DAL.RedeAfiliados.DTO;

namespace WEB.Areas.RedeAfiliados.Models.ViewModels {

    public class PontuacaoMembroVM {
        
        public PontuacaoMembroDTO Pontuacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PontuacaoMembroVM() {
            
            this.Pontuacao = new PontuacaoMembroDTO();
        }
    }

}
