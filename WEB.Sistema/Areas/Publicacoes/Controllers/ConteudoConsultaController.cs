using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using PagedList;
using WEB.App_Infrastructure;

namespace WEB.Areas.Publicacoes.Controllers {

    [OrganizacaoFilter]
    public class ConteudoConsultaController : BaseSistemaController {
        
        //Atributos
        private IConteudoConsultaBL _ConteudoConsultaBL;        
        
        //Propriedades
        private IConteudoConsultaBL OConteudoConsultaBL => _ConteudoConsultaBL = _ConteudoConsultaBL ?? new ConteudoConsultaBL();        
        
        //Construtor
        public ConteudoConsultaController() {

        }
                
        //
        public ActionResult listar() {
            
            string descricao = UtilRequest.getString("valorBusca");
            
            bool? ativo = UtilRequest.getBool("ativo");
            
            var listaConteudos = this.OConteudoConsultaBL.listar(descricao, ativo).OrderBy(x => x.dtCadastro);
            
            return View(listaConteudos.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
            
        }
       
    }
}