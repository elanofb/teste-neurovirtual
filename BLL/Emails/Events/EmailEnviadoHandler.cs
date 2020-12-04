using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using BLL.Associados;
using BLL.Core.Events;
using DAL.Associados;
using DAL.Relacionamentos;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Permissao.Security.Extensions;

namespace BLL.Email {

	public class EmailEnviadoHandler : IHandler<object> {

		//Atributos
		private IAssociadoBL _OAssociadoBL;

		private IPessoaRelacionamentoBL _OPessoaRelacionamentoBL;

		//Propriedades
		public IAssociadoBL OAssociadoBL => _OAssociadoBL ?? (this._OAssociadoBL = new AssociadoBL());
	    public IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _OPessoaRelacionamentoBL ?? (this._OPessoaRelacionamentoBL = new PessoaRelacionamentoBL());

        public IPrincipal User => HttpContextFactory.Current.User;

        //
        public void execute(object source) {
			UtilLog.saveLog("Envio de e-mail realizado com sucesso.", "email");

			try { 
				
				List<object> InfoEvento = (source as List<object>);
				List<string> listaEmails =  (InfoEvento[0] as List<string>);
				string assunto = (InfoEvento[0] as string);
				string corpoMensagem = (InfoEvento[1] as string);

				this.gravarHistorico(listaEmails, assunto, corpoMensagem);

			}catch(Exception ex){
				UtilLog.saveError(ex, "Erro no evento de e-mail enviado");
			}
			
		}

		// 1) Gravar o historico
		private void gravarHistorico(List<string> listaEmails, string assuntoEmail, string corpoEmail){

			foreach(string email in listaEmails){

				Associado OAssociado = this.OAssociadoBL.listar(0, "", "", "")
												.FirstOrDefault(x => x.Pessoa.ToEmailList().Contains(email));

				if(OAssociado != null){
					PessoaRelacionamento Relacionamento = new PessoaRelacionamento();
					Relacionamento.idPessoa = OAssociado.idPessoa;
					Relacionamento.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idEmailEnviado;
					Relacionamento.observacao = String.Concat("Assunto: ", assuntoEmail, "<br /><br />", corpoEmail);
					Relacionamento.dtOcorrencia = DateTime.Now;
					Relacionamento.idUsuarioCadastro = User.id();
					this.OPessoaRelacionamentoBL.salvar(Relacionamento);
				}
			};
		}
	}
}