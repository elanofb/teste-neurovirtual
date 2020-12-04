using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Pessoas;
using BLL.Core.Events;
using DAL.Associados;
using BLL.Associados;
using BLL.AssociadosInstitucional.Emails;
using BLL.AssociadosInstitucional.Events;
using EntityFramework.Extensions;

namespace BLL.AssociadosInstitucional {

	public class AssociadoAcessoBL : DefaultBL, IAssociadoAcessoBL {
		
		//Constantes

		//Atributos
		private IAssociadoBL _AssociadoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL { get { return (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL()); } }

		//Events
		private EventAggregator onSenhaAlterada = OnSenhaAlterada.getInstance;
		private EventAggregator onSenhaTransacaoAlterada = OnSenhaTransacaoAlterada.getInstance;

		//Construtor
		public AssociadoAcessoBL() {
		}



		//Login para o associado na área institucional
		public Associado login(string login, string senha, int idTipoCadastro, int? idOrganizacaoParam = null) {
			
		    if (idOrganizacaoParam == 0) {

		        idOrganizacaoParam = idOrganizacao;
		    }

			string cryptSenha = UtilCrypt.SHA512(senha);
		
		    string loginCPF = login.onlyNumber();
			
			var loginNumerico = loginCPF.toInt();
			
			var query = from Ass in db.Associado
									.Include(x => x.Pessoa)
									.Include(x => x.PlanoCarreira)
						where 
							(
								Ass.Pessoa.login == login || 
							   (Ass.Pessoa.nroDocumento == loginCPF && !string.IsNullOrEmpty(loginCPF)) ||
							   (Ass.nroAssociado == loginNumerico && loginNumerico > 0)
							) && 
							Ass.Pessoa.senha == cryptSenha && 
							Ass.dtExclusao == null &&
                            Ass.idTipoCadastro == idTipoCadastro && 
                            Ass.ativo != "N" && 
                            Ass.Pessoa.flagExcluido == "N"
						select
							Ass;

		    if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

			var OAssociado = query.FirstOrDefault();
			return OAssociado;
		}

		//Recuperacao de senha para o associado
		//Gerar uma nova senha e enviar para o e-mail do associado
		public UtilRetorno recuperarSenha(string login) {

			var query = from Ass in db.Associado
									.Include(x => x.Pessoa)
						where 
							(Ass.Pessoa.login == login || Ass.Pessoa.emailSecundario == login) && 
							Ass.dtExclusao == null
						select
							Ass;

			var OAssociado = query.FirstOrDefault();

			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "Desculpe, não foi localizado nenhum associado com os dados informados.");
			}

			if (OAssociado.ativo == "N") { 
				return UtilRetorno.newInstance(true, "Desculpe, os dados informados pertencem à um associado que está desativado.");
			}
			
			string novaSenha = UtilString.randomString(8);
			OAssociado.Pessoa.senha = UtilCrypt.SHA512(novaSenha);
			db.SaveChanges();

			var EnvioEmail = EnvioRecuperacaoSenha.factory(OAssociado.idOrganizacao, OAssociado.Pessoa.ToEmailList(), null);
			EnvioEmail.enviar(OAssociado, novaSenha);

			return UtilRetorno.newInstance(false, "Geramos uma nova senha para sua conta com sucesso. Você irá recebê-la nos e-mails de seu cadastro em alguns instantes.");
		}

		//Alteracao de senha solicitada pelo usuario
		public UtilRetorno alterarSenha(int idAssociado, string senhaAtual, string novaSenha, bool flagValidarSenhaAtual = true, int? idOrganizacaoParam = null) {

		    if (idOrganizacao > 0 && idOrganizacaoParam == null) {
		        idOrganizacaoParam = idOrganizacao;
		    }

			var query = from Ass in db.Associado
									.Include(x => x.Pessoa)
						where 
							Ass.id == idAssociado && 
							Ass.dtExclusao == null
						select
							Ass;

			var OAssociado = query.FirstOrDefault();

			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "Desculpe, não foi localizado nenhum associado com os dados informados.");
			}
            
            query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            
			if (!UtilCrypt.SHA512(senhaAtual).Equals(OAssociado.Pessoa.senha) && flagValidarSenhaAtual) { 
				return UtilRetorno.newInstance(true, "A senha atual informada está incorreta.");
			}
			
			OAssociado.Pessoa.senha = UtilCrypt.SHA512(novaSenha);
			db.SaveChanges();
			
			//Assinatura e disparo do evento
			this.onSenhaAlterada.subscribe(new OnSenhaAlteradaHandler());
			this.onSenhaAlterada.publish( (OAssociado as object) );

			return UtilRetorno.newInstance(false, "A senha foi alterada com sucesso!");
		}
		
		//Alteracao de senha solicitada pelo usuario
		public UtilRetorno alterarSenhaTransacao(int idAssociado, string senhaAtual, string novaSenha, bool flagValidarSenhaAtual = true, int? idOrganizacaoParam = null) {

			if (idOrganizacao > 0 && idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			var query = from Ass in db.Associado
				where 
					Ass.id == idAssociado && 
					Ass.dtExclusao == null
				select
					Ass;

			var OAssociado = query.Select(x => new {x.id, 
														x.idPessoa, 
														x.senhaTransacao, 
														Pessoa = new { x.Pessoa.id, x.Pessoa.nome}
														
													})
								.FirstOrDefault()
								.ToJsonObject<Associado>();

			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "Desculpe, não foi localizado nenhum associado com os dados informados.");
			}
            
			query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

			if (!UtilCrypt.SHA512(senhaAtual).Equals(OAssociado.senhaTransacao) && flagValidarSenhaAtual) { 
				return UtilRetorno.newInstance(true, "A senha atual informada está incorreta.");
			}

			OAssociado.senhaTransacao = UtilCrypt.SHA512(novaSenha);

			db.Associado.Where(x => x.id == OAssociado.id)
						.Update(x => new Associado {senhaTransacao = OAssociado.senhaTransacao});
			
			//Assinatura e disparo do evento
			this.onSenhaTransacaoAlterada.subscribe(new OnSenhaTransacaoAlteradaHandler());
			this.onSenhaTransacaoAlterada.publish( (OAssociado as object) );

			return UtilRetorno.newInstance(false, "A senha foi alterada com sucesso!");
		}

        //Enviar o link de troca de senha para o associado
	    public UtilRetorno enviarLinkTrocaSenha(int idAssociado) {
			var query = from Ass in db.Associado
									.Include(x => x.Pessoa)
						where 
							Ass.id == idAssociado && 
							Ass.dtExclusao == null
						select
							Ass;

			var OAssociado = query.FirstOrDefault();

			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "Desculpe, o seu cadastro não foi localizado no sistema.");
			}

            return UtilRetorno.newInstance(false, "O link para alteração de senha foi enviado com sucesso!");
        }
	}
}