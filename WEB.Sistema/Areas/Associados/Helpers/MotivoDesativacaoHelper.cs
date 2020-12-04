using System.Collections;
using System.Web.Mvc;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers {

    public class MotivoDesativacaoHelper {

        private static MotivoDesativacaoHelper _instance;
        private IMotivoDesativacaoBL _MotivoDesativacaoBL;

        public static MotivoDesativacaoHelper getInstance => _instance = _instance ?? new MotivoDesativacaoHelper();
        private IMotivoDesativacaoBL OMotivoDesativacaoBL => _MotivoDesativacaoBL = _MotivoDesativacaoBL ?? new MotivoDesativacaoBL();


        //Carregar combo de seleção dos tipos de associados
        public SelectList selectList(int? selected) {

            var query = OMotivoDesativacaoBL.listar("", true);            

            return new SelectList((IEnumerable)query, "id", "descricao", selected);
        }
    }
}