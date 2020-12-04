using BLL.Contatos;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;

namespace WEB.Areas.Contatos.Controllers {
    
    [OrganizacaoFilter]
    public class TipoContatoConsultaController : BaseSistemaController {
        
        // Atributos
        private ITipoContatoVWConsultaBL _ITipoContatoVWConsultaBL;

        // Propriedades
        private ITipoContatoVWConsultaBL OTipoContatoVWConsultaBL => _ITipoContatoVWConsultaBL = _ITipoContatoVWConsultaBL ?? new TipoContatoVWConsultaBL();

        // Listagem dos associados do sistema
        public ActionResult Index() {

            var valorBusca = UtilRequest.getString("valorBusca");

            var ativo = UtilRequest.getBool("ativo");
            
            var query = this.OTipoContatoVWConsultaBL.listar(valorBusca, ativo);
            
            var listaTipos = query.OrderBy(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
            
            return View(listaTipos);
            
        }
        
    }
    
}