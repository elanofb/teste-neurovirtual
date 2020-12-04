using DAL.Associados;

namespace WEB.Areas.NaoAssociados.ViewModels {

    public class TornarAssociadoForm {

        //Propriedades
        public Associado NaoAssociado { get; set; }

        public string flagDesejaAdmitir { get; set;  }
        public string observacoes { get; set;  }

        //Construtor
        public TornarAssociadoForm() {
            this.flagDesejaAdmitir = "S";
        }
    }
}
