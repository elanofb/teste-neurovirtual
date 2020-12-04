using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using WEB.App_Infrastructure;
using BLL.Arquivos;
using DAL.Entities;
using WEB.Areas.Pessoas.ViewModels;

namespace WEB.Areas.Pessoas.Controllers{

    public class PessoaRelacionamentoExportacaoController : BaseSistemaController{

	    //Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL; 
	    private IArquivoUploadBL _ArquivoUploadBL; 

		//Propriedades
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL());
	    private IArquivoUploadBL OArquivoUploadBL => (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL());

		//Bloco Partial para histórico de relacionamentos
        public PartialViewResult exportar() {

	        var idPessoa = UtilRequest.getInt32("idPessoa");
			var valorBusca = UtilRequest.getString("valorBusca");
			var flagTemArquivos = UtilRequest.getString("flagTemArquivos");
		    
	        var query = this.OPessoaRelacionamentoBL.listar(idPessoa, 0);
	        
			if (!valorBusca.isEmpty()) {
				query = query.Where(x => x.observacao.Contains(valorBusca) || x.OcorrenciaRelacionamento.descricao.Contains(valorBusca) || x.UsuarioCadastro.nome.Contains(valorBusca));
			}

			if (flagTemArquivos == "S") {
			    
				var ids = query.Select(x => x.id).ToList();
				var idsComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => ids.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();

				query = query.Where(x => idsComArquivo.Contains(x.id));
			}

	        var listaOcorrencias = query.OrderByDescending(x => x.dtOcorrencia).ToList();

	        if (flagTemArquivos == "S") {
		        listaOcorrencias.ForEach(item => { item.flagPossuiArquivo = true; });
	        }

	        if (flagTemArquivos != "S") { 

				//Exponho as ocorrências que possuem arquivos.
				var idsOcorrencia = listaOcorrencias.Select(x => x.id).ToList();
				var idsOcorrenciaComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => idsOcorrencia.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();
	
				listaOcorrencias.ForEach(item => {
					item.flagPossuiArquivo = idsOcorrenciaComArquivo.Exists(x => x == item.id); 
				});
		        
	        }

	        var OGeradorCSV = new GeradorCsvPessoaRelacionamento();
	        
	        OGeradorCSV.baixarExcel(listaOcorrencias);
	        
	        return null;

        }

    }
	
}
