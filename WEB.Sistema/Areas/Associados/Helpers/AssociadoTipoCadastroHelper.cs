using System.Collections.Generic;
using System.Web.Mvc;
using DAL.Associados;

namespace WEB.Areas.Associados.Helpers{

    public class AssociadoTipoCadastroHelper{
        
        // Atributos
        private static AssociadoTipoCadastroHelper _instance;

        // Propriedades
        public static AssociadoTipoCadastroHelper getInstance => _instance = _instance ?? new AssociadoTipoCadastroHelper();
        
		//
        public SelectList selectList(int? selected){
            
            var lista = new List<object>();
            lista.Add(new { id = AssociadoTipoCadastroConst.CONSUMIDOR, descricao = "Consumidor" });
            lista.Add(new { id = AssociadoTipoCadastroConst.COMERCIANTE, descricao = "Comerciante" });
            
            return new SelectList(lista, "id", "descricao", selected);

        }

    }
}