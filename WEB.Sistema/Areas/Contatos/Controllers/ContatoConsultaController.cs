using BLL.Contatos;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Contatos.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Contatos.Controllers {
    
    public class ContatoConsultaController : Controller {
        
        // Atributos
        private IPessoaContatoVWBL _PessoaContatoVWBL;

        // Propriedades
        private IPessoaContatoVWBL OPessoaContatoVWBL => _PessoaContatoVWBL = _PessoaContatoVWBL ?? new PessoaContatoVWBL();

        // Listagem dos associados do sistema
        public ActionResult index(ContatoConsulta ViewModel) {

            string valorBusca = UtilRequest.getString("valorBusca");

            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            var listaPessoaContato = OPessoaContatoVWBL.listar(ViewModel.ativo, ViewModel.flagSituacaoContribuicao, ViewModel.idsTipoContato, ViewModel.idsTipoAssociado, valorBusca).OrderBy(x => x.nomeAssociado);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OContatoConsultaExportacao = new ContatoConsultaExportacao();
                
                OContatoConsultaExportacao.baixarExcel(listaPessoaContato.ToList());

                return null;
            }

            ViewModel.listaPessoaContato = listaPessoaContato.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View("index", ViewModel);
        }
    }
}