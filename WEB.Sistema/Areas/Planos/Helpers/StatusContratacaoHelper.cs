using System.Web.Mvc;
using BLL.Planos;

namespace WEB.Areas.Planos.Helpers{

    public class StatusPlanoContratacaoHelper{

        //Atributos
        private static StatusPlanoContratacaoBL _StatusPlanoContratacaoBL;

        //Propriedades
        private static StatusPlanoContratacaoBL OStatusPlanoContratacaoBL { get { return (_StatusPlanoContratacaoBL= _StatusPlanoContratacaoBL ?? new StatusPlanoContratacaoBL());} }

        //Construtor

        //
        public static SelectList selectList(int? selected){
            var list = OStatusPlanoContratacaoBL.listar("");
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}