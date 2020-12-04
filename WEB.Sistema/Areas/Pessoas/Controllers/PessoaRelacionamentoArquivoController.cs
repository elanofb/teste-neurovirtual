using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using WEB.App_Infrastructure;
using BLL.Arquivos;
using BLL.Services;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Pessoas;
using WEB.Areas.Pessoas.DTO;
using WEB.Areas.Pessoas.ViewModels;

namespace WEB.Areas.Pessoas.Controllers{

    public class PessoaRelacionamentoArquivoController : BaseSistemaController{

	    //Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL; 
	    //Atributos
	    private IArquivoUploadBL _ArquivoUploadBL; 

		//Propriedades
	    private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL());
	    private IArquivoUploadBL OArquivoUploadBL => (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL());

	    //Bloco Partial para histórico de relacionamentos
	    [HttpGet, ActionName("model-lista-arquivos")]
	    public PartialViewResult modelListaArquivos(int idPessoaRelacionamento)
	    {

		    var OOcorrenciaDTO = new PessoaRelacionamentoDTO();
		    OOcorrenciaDTO.OListaArquivos = new List<ArquivoUpload>();
				
		    var OOcorrencia = this.OPessoaRelacionamentoBL.carregar(idPessoaRelacionamento) ?? new PessoaRelacionamento();

		    if (OOcorrencia.id > 0)
		    {
			    OOcorrenciaDTO.PessoaRelacionamento = OOcorrencia;
			    OOcorrenciaDTO.OListaArquivos.AddRange(OArquivoUploadBL.listarDocumentos(idPessoaRelacionamento, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).ToList());
		    }

		    return PartialView(OOcorrenciaDTO);
	    }
	    
	    [HttpGet, ActionName("listar-galeria-documentos")]
	    public PartialViewResult listarGaleriaDocumentos(int idPessoa){
			
		    var viewModel = new PessoaRelacionamentoArquivoForm();
				
		    var listaOcorrencias = this.OPessoaRelacionamentoBL.listar(idPessoa, 0)
																.Select(x => new {
																					x.id, 
																					x.idPessoa,
																					Pessoa = new {
																									x.Pessoa.id,
																									x.Pessoa.nome
																								},
																					x.idOcorrenciaRelacionamento,
																					OcorrenciaRelacionamento = new {
																														x.OcorrenciaRelacionamento.id,
																														x.OcorrenciaRelacionamento.descricao
																													},
																					x.dtOcorrencia,
																					x.observacao,
																					x.idUsuarioCadastro,
																					UsuarioCadastro = new {
																											x.UsuarioCadastro.id,
																											x.UsuarioCadastro.nome
																										}
																				}).ToListJsonObject<PessoaRelacionamento>();
		    var idsOcorrencias = listaOcorrencias.Select(x => x.id).ToList();
		    
		    var listaArquivos = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO)
												.Where(x => idsOcorrencias.Contains(x.idReferenciaEntidade))
												 .ToList();
		    
		    foreach (var OArquivoUpload in listaArquivos){
				
			    var OOcorrencia = listaOcorrencias.FirstOrDefault(x => x.id == OArquivoUpload.idReferenciaEntidade) ?? new PessoaRelacionamento();
			    
			    var OArquivosPessoaRelacionamento = new ArquivosPessoaRelacionamentoDTO();
			    
				OArquivosPessoaRelacionamento.OArquivoUpload = OArquivoUpload;
			    
				OArquivosPessoaRelacionamento.dtOcorrencia = OOcorrencia.dtOcorrencia.Value;
			    
				OArquivosPessoaRelacionamento.descOcorrencia = OOcorrencia.OcorrenciaRelacionamento.descricao;
			    
				OArquivosPessoaRelacionamento.nomeUsuarioCadastro = OOcorrencia.UsuarioCadastro.nome;
			    			    
			    viewModel.OListaArquivosPessoaRelacionamento.Add(OArquivosPessoaRelacionamento);
		    }

		    viewModel.OListaArquivosPessoaRelacionamento = viewModel.OListaArquivosPessoaRelacionamento.OrderByDescending(x => x.dtOcorrencia).ToList();
			    
		    return PartialView(viewModel);
	    }
    }
}
