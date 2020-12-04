using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using DAL.Entities;
using Microsoft.Ajax.Utilities;
using WEB.App_Infrastructure;
using WEB.Areas.Relacionamentos.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Relacionamentos.Controllers {
        
    [OrganizacaoFilter]
    public class RelacionamentoConsultaController : BaseSistemaController {
        

        //Atributos
        private IArquivoUploadBL _ArquivoUploadBL; 

        //Propriedades
        private IArquivoUploadBL OArquivoUploadBL => (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL());
        
        //
        public ActionResult Index() {

            var ViewModel = new RelacionamentoConsultaVM();
            
            var query = ViewModel.montarQuery();
            
            var flagTipoSaida = UtilRequest.getString("flagTipoSaida");
            var flagTemArquivos = UtilRequest.getString("flagTemArquivos");

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OGerador = new GeradorCsvRelacionamentoConsulta();
                
                OGerador.baixarExcel(query.OrderByDescending(x => x.dtOcorrencia).ToList());

                return null;
            }

            ViewModel.listaPessoaRelacionamentos = query.OrderByDescending(x => x.dtOcorrencia).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            if (flagTemArquivos == "S"){
				
                ViewModel.listaPessoaRelacionamentos.ForEach(item => { item.flagPossuiArquivo = true; });
                return View("Index", ViewModel);
            }

            //Exponho as ocorrências que possuem arquivos.
            var idsOcorrencia = ViewModel.listaPessoaRelacionamentos.Select(x => x.id).ToList();
            var idsOcorrenciaComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => idsOcorrencia.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();

            ViewModel.listaPessoaRelacionamentos.ForEach(item => {
                item.flagPossuiArquivo = idsOcorrenciaComArquivo.Exists(x => x == item.id); 
            });
            
            return View("Index", ViewModel);
            
        }
        
    }
    
}