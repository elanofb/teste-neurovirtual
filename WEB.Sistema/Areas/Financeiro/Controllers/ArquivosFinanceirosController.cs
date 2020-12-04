using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using BLL.Services;
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class ArquivosFinanceirosController : BaseSistemaController {

        // Atributos Serviços
        private IReceitasDespesasArquivosVWBL _IReceitasDespesasArquivosVWBL;
		
        // Propriedades Serviços
        private IReceitasDespesasArquivosVWBL OReceitasDespesasArquivosVWBL => _IReceitasDespesasArquivosVWBL = _IReceitasDespesasArquivosVWBL ?? new ReceitasDespesasArquivosVWBL();
        
        //
        public ActionResult Index(ArquivosFinanceirosConsultaVM ViewModel) {
            
            ViewModel.montarLista();
            
            return View(ViewModel);
            
        }
        
        //
        [HttpPost, ActionName("gerar-zip-selecionados")]
        public ActionResult geraZipSelecionados(GeradorZipArquivosFinanceiros ViewModel) {

            var listaArquivos = this.OReceitasDespesasArquivosVWBL.listar().Where(x => ViewModel.idsArquivos.Contains(x.id))
                                    .Select(x => new { x.id, x.idOrganizacao, x.path })
                                    .OrderByDescending(x => x.id).ToListJsonObject<ReceitaDespesaArquivoVW>();
            
            var caminhoZip = ViewModel.gerarZip(listaArquivos);

            return Json(new { error = false, nomeArquivo = caminhoZip, totalRegistros = listaArquivos.Count }, JsonRequestBehavior.AllowGet);            
        }
        
        //
        [HttpPost, ActionName("gerar-zip-todos")]
        public ActionResult geraZipTodos(ArquivosFinanceirosConsultaVM ViewModel) {

            var listaArquivos = ViewModel.montarQuery().Select(x => new { x.id, x.idOrganizacao, x.path })
                                         .OrderByDescending(x => x.id).ToListJsonObject<ReceitaDespesaArquivoVW>();

            var OGeradorZip = new GeradorZipArquivosFinanceiros();
            var caminhoZip = OGeradorZip.gerarZip(listaArquivos);

            return Json(new {error = false, nomeArquivo = caminhoZip, totalRegistros = listaArquivos.Count }, JsonRequestBehavior.AllowGet);            
        }
        
        //
        [ActionName("download-zip")]
        public FileResult downloadZip(string nomeArquivo) {
            return File(UtilConfig.pathAbsTempFiles + nomeArquivo, "application/zip", nomeArquivo);
        }

    }
    
}