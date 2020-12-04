using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class ModoPagamentoDespesaConsultaController : BaseSistemaController {

        //Atributos
        private IModoPagamentoDespesaConsultaBL _ModoPagamentoDespesaConsultaBL;

        //Propriedades
        private IModoPagamentoDespesaConsultaBL OModoPagamentoDespesaConsultaBL => (this._ModoPagamentoDespesaConsultaBL = this._ModoPagamentoDespesaConsultaBL ?? new ModoPagamentoDespesaConsultaBL());

        [ActionName("verificar-conta-bancaria")]
        public ActionResult verificarContaBancaria(int id = 0){
            
            var flagContaBancaria = OModoPagamentoDespesaConsultaBL.query().Any(x => x.id == id && x.flagContaBancaria == true);
            
            return Json(flagContaBancaria, JsonRequestBehavior.AllowGet);
            
        }
        
        [ActionName("listar-modo-pagamento")]
        public ActionResult listarModoPagamento(int idTipoDespesa = 0) {

            var query = this.OModoPagamentoDespesaConsultaBL.query().Where(x => x.ativo == true);
            
            if (idTipoDespesa == TipoDespesaConst.TRIBUTOS) {
                query = query.Where(x => x.flagImposto == true);
            }
            
            if (idTipoDespesa != TipoDespesaConst.TRIBUTOS) {
                query = query.Where(x => x.flagImposto != true);
            }
            
            var listaModoPagamento = query.Select(x => new{
                                        value = x.id,
                                        text = x.descricao
                                     }).ToList();

            return Json(new {listaModoPagamento, error = false}, JsonRequestBehavior.AllowGet);
            
        }
    }
}