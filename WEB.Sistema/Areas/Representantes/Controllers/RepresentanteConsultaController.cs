using System;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using PagedList;
using BLL.Representantes;
using BLL.Services;
using DAL.Representantes;

namespace WEB.Areas.Representantes.Controllers {

    [OrganizacaoFilter]
    public class RepresentanteConsultaController : BaseSistemaController {

        //Constantes

        //Atributos
        private IRepresentanteConsultaBL _IRepresentanteConsultaBL;

        //Propriedades
        private IRepresentanteConsultaBL ORepresentanteConsultaBL => _IRepresentanteConsultaBL = _IRepresentanteConsultaBL ?? new RepresentanteConsultaBL();
        
        //
        [HttpGet]
        public ActionResult Index() {

            var ativo = UtilRequest.getBool("ativo");
            
            var valorBusca = UtilRequest.getString("valorBusca");

            var listaRepresentantes = this.ORepresentanteConsultaBL.listar(valorBusca, ativo)
                                    .Select(x => new {
                                        x.id, x.ativo, x.dtCadastro,
                                        Pessoa = new {
                                            x.Pessoa.nome, x.Pessoa.nroDocumento, 
                                            x.Pessoa.emailPrincipal,
                                            x.Pessoa.nroTelPrincipal, x.Pessoa.nroTelSecundario
                                        }
                                    }).OrderBy(x => x.Pessoa.nome).ToListJsonObject<Representante>();

            return View(listaRepresentantes.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

    }
    
}
