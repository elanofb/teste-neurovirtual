using System.Collections.Generic;
using System.Web.Mvc;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers {

    public class MotivoDesligamentoHelper {

        private static MotivoDesligamentoHelper _instance;
        private IMotivoDesligamentoBL _MotivoDesligamentoBL;

        public static MotivoDesligamentoHelper getInstance => _instance = _instance ?? new MotivoDesligamentoHelper();
        private IMotivoDesligamentoBL OMotivoDesligamentoBL => _MotivoDesligamentoBL = _MotivoDesligamentoBL ?? new MotivoDesligamentoBL();


        //Carregar combo de seleção dos tipos de associados
        public SelectList selectList(int? selected) {

            var query = OMotivoDesligamentoBL.listar("", true);

            return new SelectList(query, "id", "descricao", selected);
        }

        //Carregar combo de seleção dos tipos de associados
        public MultiSelectList selectMultList(List<int> selected) {

            var query = OMotivoDesligamentoBL.listar("", true);

            return new MultiSelectList(query, "id", "descricao", selected);
        }
    }
}