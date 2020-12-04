using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using BLL.Historicos.Interfaces;
using BLL.Historicos.Services;
using BLL.Services;
using DAL.Historicos;

namespace WEB.Areas.Associados.Controllers {
    [OrganizacaoFilter]
    public class PreAtualizacaoFinalizacaoController : Controller{

        //Atributos
        private IHistoricoAtualizacaoConsultaBL _HistoricoAtualizacaoConsultaBL;
        private IHistoricoAtualizacaoFinalizacaoBL _HistoricoAtualizacaoFinalizacaoBL;
        
        //Propriedades
        private IHistoricoAtualizacaoConsultaBL OHistoricoAtualizacaoConsultaBL => _HistoricoAtualizacaoConsultaBL = _HistoricoAtualizacaoConsultaBL ?? new HistoricoAtualizacaoConsultaBL();
        private IHistoricoAtualizacaoFinalizacaoBL OHistoricoAtualizacaoFinalizacaoBL => _HistoricoAtualizacaoFinalizacaoBL = _HistoricoAtualizacaoFinalizacaoBL ?? new HistoricoAtualizacaoFinalizacaoBL();
        
        [HttpPost, ActionName("finalizar-analise")]
        public ActionResult finalizarAnalise(int? idAssociado, bool flagAprovado){

            int idAssociadoParam = idAssociado.toInt();
            
            HistoricoAtualizacao OHistoricoAtualizacao = this.OHistoricoAtualizacaoConsultaBL.query(User.idOrganizacao())
                                                             .Select(x => new { x.id, x.idAssociado, x.dtAnalise })
                                                             .FirstOrDefault(x => x.idAssociado == idAssociadoParam && x.dtAnalise == null)                                                             
                                                             .ToJsonObject<HistoricoAtualizacao>() ?? new HistoricoAtualizacao();
            
            if (OHistoricoAtualizacao.id == 0){
                return Json(new { error = true, message = "Histórico de alteração não encontrado!" });    
            }
            
            bool flagFinalizacao = this.OHistoricoAtualizacaoFinalizacaoBL.finalizarAnalise(new[]{OHistoricoAtualizacao.id}, flagAprovado);

            if (flagAprovado == false){
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Análise finalizada com sucesso!");
            }                                                   
            
            return Json(new{error = !flagFinalizacao, message = "Análise finalizada com sucesso!", urlRedirecionamento = Url.Action("index", "PreAtualizacaoConsulta") });            

        }
        
        [HttpPost, ActionName("reprovar-lote")]
        public ActionResult finalizarLote(int[] ids){
            
            if(!ids.Any()){
                return Json(new { error = true, message = "Informe ao menos 1 item para reprovação!" });
            }
            
            bool flagFinalizacao = this.OHistoricoAtualizacaoFinalizacaoBL.finalizarAnalise(ids, false);
            
            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Análise finalizada com sucesso!");
            
            return Json(new{error = !flagFinalizacao, message = "Análises finalizadas com sucesso!", urlRedirecionamento = Url.Action("index", "PreAtualizacaoConsulta") });            
            
        }
       
    }
}