using System.Linq;
using System.Web.Mvc;
using BLL.Transacoes;

namespace WEB.Areas.Transacoes.Helpers {

    public class TipoTransacaoHelper {

        //Constanctes
        private static TipoTransacaoHelper _instance;
        private        ITipoTransacaoConsultaBL    _ConsultaBL;

        //Atributos

        //Propriedades
        public static TipoTransacaoHelper getInstance     => _instance = _instance ?? new TipoTransacaoHelper();
        private ITipoTransacaoConsultaBL ConsultaBL => _ConsultaBL = _ConsultaBL ?? new TipoTransacaoConsultaBL();

        /// <summary>
        /// 
        /// </summary>
        public SelectList selectList(int? selected) {

            var lista = this.ConsultaBL.query()
                            .Where(x => x.ativo == true)
                            .Select(x => new { id = x.id, descricao = x.descricao })
                            .ToList();

            return new SelectList(lista, "id", "descricao", selected);
        }

    }
}