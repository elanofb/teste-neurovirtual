using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.Helpers{

    public class TipoVencimentoHelper{

        //Constantes
        private static TipoVencimentoHelper _instance; 

        //Atributos
         private static ITipoVencimentoBL _TipoVencimentoBL;

        //Propriedades
        public static TipoVencimentoHelper getInstance => _instance = _instance ?? new TipoVencimentoHelper();
        private ITipoVencimentoBL OTipoVencimentoBL   => _TipoVencimentoBL = _TipoVencimentoBL ?? new TipoVencimentoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {


            var listaItens = OTipoVencimentoBL.listar(true)
                                    .OrderBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.descricao
                                    }).ToList();

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}