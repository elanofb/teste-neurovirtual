using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.Financeiro.Controllers {
    public class SubContaConsultaController : Controller {

        //Atributos
        private ICategoriaTituloBL _CategoriaTituloBL;

        //Propriedades
        private ICategoriaTituloBL OCategoriaTituloBL => _CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL();

        /// <summary>
        /// Listagem
        /// </summary>
        public ActionResult listar() {

            var idMacroConta = UtilRequest.getInt32("idMacroConta");
            var idCategoriaPai = UtilRequest.getInt32("idCategoriaPai");
            var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getString("flagAtivo");

            var listaCategoriaTitulo = this.OCategoriaTituloBL.listar(idMacroConta, descricao, ativo);

            if (idCategoriaPai > 0) {
                listaCategoriaTitulo = listaCategoriaTitulo.Where(x => x.idCategoriaPai == idCategoriaPai);
            }

            listaCategoriaTitulo = listaCategoriaTitulo.OrderBy(x => x.descricao);

            return View(listaCategoriaTitulo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        [ActionName("listar-ajax")]
        public ActionResult listarAjax(int? idMacroConta, bool? flagSomentePai, int idExclude = 0) {

            var query = this.OCategoriaTituloBL.listar(0, "", "S");

            if (idMacroConta > 0) {
                query = query.Where(x => x.idMacroConta == idMacroConta);
            }

            if (flagSomentePai == true) {
                query = query.Where(x => x.idCategoriaPai <= 0 || x.idCategoriaPai == null);
            }

            if (idExclude > 0) {
                query = query.Where(x => idExclude != x.id);
            }
            
            var list = query.ToList().Select(x => new {
                    value = x.id, 
                    x.idMacroConta, 
                    x.codigoFiscal, 
                    codigoFiscalPai = x.idCategoriaPai > 0 ? x.CategoriaPai.codigoFiscal: "", 
                    text = x.descricaoSubConta()
                }).OrderBy(x => x.codigoFiscalPai).ThenBy(x => UtilString.onlyAlphaNumber(x.codigoFiscal).toInt()).ThenBy(x => x.text);
                                   
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}