using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Planos;
using BLL.Services;
using WEB.App_Infrastructure;
using DAL.Planos;
using PagedList;

namespace WEB.Areas.Planos.Controllers {

    [OrganizacaoFilter]
    public class PlanoCarreiraConsultaController : BaseSistemaController {
    
        //Constantes
        
        //Atributos
        private IPlanoCarreiraConsultaBL _IPlanoCarreiraConsultaBL;        
        //Propriedades
        private IPlanoCarreiraConsultaBL OPlanoCarreiraConsultaBL => _IPlanoCarreiraConsultaBL = _IPlanoCarreiraConsultaBL ?? new PlanoCarreiraConsultaBL();
        
        //
        [HttpGet]
        public ActionResult Index() {
            
            var ativo = UtilRequest.getBool("ativo");
            
            var valorBusca = UtilRequest.getString("valorBusca");
            
            var listaPlanos = this.OPlanoCarreiraConsultaBL.listar(valorBusca, ativo)
                                    .Select(x => new {
                                        x.id, 
                                        x.ativo, 
                                        x.descricao,
                                        x.pontuacao,
                                        x.dtCadastro
                                    }).OrderBy(x => x.pontuacao).ToListJsonObject<PlanoCarreira>();           
            
            return View(listaPlanos.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
            
        }

    }
    
}
