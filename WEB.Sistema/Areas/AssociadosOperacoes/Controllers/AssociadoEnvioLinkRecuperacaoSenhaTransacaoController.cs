using System;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosOperacoes;
using BLL.NaoAssociados;
using BLL.Pessoas;
using BLL.Services;
using DAL.Associados.DTO;
using DAL.Pessoas;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoEnvioLinkRecuperacaoSenhaTransacaoController : Controller {

		//Atributos
		private IAssociadoRecuperacaoSenhaTransacaoNotificacaoBL _IAssociadoRecuperacaoSenhaTransacaoNotificacaoBL;
		private IPessoaEmailConsultaBL _PessoaEmailConsultaBL;
		private INaoAssociadoBL _NaoAssociadoBL;

		//Propriedades
		private IAssociadoRecuperacaoSenhaTransacaoNotificacaoBL OAssociadoRecuperacaoSenhaTransacaoNotificacaoBL => _IAssociadoRecuperacaoSenhaTransacaoNotificacaoBL = _IAssociadoRecuperacaoSenhaTransacaoNotificacaoBL ?? new AssociadoRecuperacaoSenhaTransacaoNotificacaoBL();
		private IPessoaEmailConsultaBL OPessoaEmailConsultaBL => _PessoaEmailConsultaBL = _PessoaEmailConsultaBL ?? new PessoaEmailConsultaBL(); 
		private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL(); 

        //
        [HttpPost, ActionName("enviar-link-senha-transacao")]
	    public ActionResult enviarLinkSenhaTransacao(AssociadoFiltroVM DadosConsulta) {
            

            var listaAssociados = DadosConsulta.montarQuery()
                                                .Select(x => new ItemListaAssociado {
                                                    id = x.id, 
		            								idPessoa = x.idPessoa,
		            							    nome = x.nome,
													ativo = x.ativo
                                                }).OrderBy(x => x.nome)
	            								.ToList();

	        if (!listaAssociados.Any())
	        {

		        listaAssociados = ONaoAssociadoBL.listar("", "S").Where(x => DadosConsulta.idsAssociados.Contains(x.id)).Select(x => new ItemListaAssociado {
				        id = x.id, 
				        idPessoa = x.idPessoa,
				        nome = x.Pessoa.nome,
				        ativo = x.ativo
			        }).OrderBy(x => x.nome)
			        .ToList();

		        if (!listaAssociados.Any())
		        {
			        return Json(new { error = true, message = "Nenhum membro foi encontrado para enviar o link de recuperação de senha." }, JsonRequestBehavior.AllowGet);
		        }
	        }

	        var idsPessoas = listaAssociados.Select(x => x.idPessoa).ToList();
	        
	        var listaEmail = this.OPessoaEmailConsultaBL.listar(0)
														.Where(x => idsPessoas.Contains(x.idPessoa))
														.Select(x => new {x.id, x.idPessoa, x.email})
														.ToListJsonObject<PessoaEmail>();            
	        
	        foreach (var item in listaAssociados) {

		        var Email = listaEmail.FirstOrDefault(x => x.idPessoa == item.idPessoa);

		        if (Email == null) {
			        continue;
		        }

		        item.email = Email.email; 
                
		        var EmailSecundario = listaEmail.Skip(1).FirstOrDefault(x => x.idPessoa == item.idPessoa);
                
		        if (EmailSecundario == null) {
			        continue;
		        }

		        item.emailSecundario = EmailSecundario.email;                
	        }

	        if (listaAssociados.Where(x => !x.email.isEmpty()).ToList().Count == 0)
	        {		        
		        return Json(new { error = true, message = "Não existem e-mails cadastrados para serem notificados." }, JsonRequestBehavior.AllowGet);
	        }
            
            var ORetorno = this.OAssociadoRecuperacaoSenhaTransacaoNotificacaoBL.registrarEmails(listaAssociados);

	        if (!ORetorno.flagError) {

	            return Json(new { error = false, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	        }

	        return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	    }
        
	}

}
