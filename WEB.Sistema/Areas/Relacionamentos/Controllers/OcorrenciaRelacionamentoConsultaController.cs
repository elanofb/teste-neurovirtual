using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Relacionamentos;
using WEB.App_Infrastructure;

namespace WEB.Areas.Relacionamentos.Controllers {
    
    [OrganizacaoFilter]
    public class OcorrenciaRelacionamentoConsultaController : BaseSistemaController {
        
        // Atributos
        private IOcorrenciaRelacionamentoVWConsultaBL _IOcorrenciaRelacionamentoVWConsultaBL;

        // Propriedades
        private IOcorrenciaRelacionamentoVWConsultaBL OOcorrenciaRelacionamentoVWConsultaBL => _IOcorrenciaRelacionamentoVWConsultaBL = _IOcorrenciaRelacionamentoVWConsultaBL ?? new OcorrenciaRelacionamentoVWConsultaBL();

        // Listagem dos associados do sistema
        public ActionResult Index() {

            var valorBusca = UtilRequest.getString("valorBusca");

            var ativo = UtilRequest.getBool("ativo");
            
            var query = this.OOcorrenciaRelacionamentoVWConsultaBL.listar(valorBusca, ativo);
            
            var listaTipos = query.OrderBy(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
            
            return View(listaTipos);
            
        }
        
    }
    
}