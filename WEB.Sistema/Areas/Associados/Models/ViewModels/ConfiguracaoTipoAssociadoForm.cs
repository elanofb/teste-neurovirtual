using DAL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    public class ConfiguracaoTipoAssociadoForm
    {

        //Atributos

        //Servicos

        //Propriedades
        public int idTipoAssociado  { get; set; }
        public ConfiguracaoTipoAssociado ConfiguracaoTipoAssociado { get; set; }

        //Construtor
        public ConfiguracaoTipoAssociadoForm()
        {

            this.ConfiguracaoTipoAssociado = new ConfiguracaoTipoAssociado();
        }
    }
}