using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using BLL.Services;
using BLL.Core.Events;
using BLL.Associados.Events;
using BLL.AssociadosInstitucional.Emails;
using BLL.AssociadosOperacoes.Events;
using BLL.Email;
using BLL.Pessoas;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using DAL.Relacionamentos;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class AssociadoAcaoBL : DefaultBL, IAssociadoAcaoBL {

		//Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propriedades
	    public IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

		//Events
		private readonly EventAggregator onAdmissao = OnAdmissao.getInstance;
	    private readonly EventAggregator onDesativacao = OnDesativacao.getInstance;

        private EventAggregator onSituacaoContribuicaoAlterada = OnSituacaoContribuicaoAlterada.getInstance;

		//Construtor
		public AssociadoAcaoBL() {
		}

		//Admitir um associado que estava em processo de admissao
		public UtilRetorno admitirAssociado(int idAssociado, DateTime? dtAdmissao) {

			this.onAdmissao.subscribe( new OnAdmissaoHandler() );

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado);

			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Associado não encontrado.");
			}

			OAssociado.dtAdmissao = dtAdmissao;
			OAssociado.idUsuarioAdmissao = idUsuarioLogado;
			OAssociado.ativo = "S";
			db.SaveChanges();

			this.onAdmissao.publish( ((object) OAssociado) );

			return UtilRetorno.newInstance(false, "Associado admitido com sucesso.");
		}
        
        //Bloquear um determinado associado
		public UtilRetorno bloquearAssociado(int idAssociado, string observacoes) {
			this.onDesativacao.subscribe( new OnDesativacaoHandler() );

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado);
			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Associado não encontrado.");
			}

			OAssociado.dtDesativacao = DateTime.Now;
			OAssociado.idUsuarioDesativacao = idUsuarioLogado;
			OAssociado.ativo = "B";
			db.SaveChanges();

            OAssociado.observacoes = observacoes;
            this.onDesativacao.publish( (OAssociado as object) );

			return UtilRetorno.newInstance(false, "Associado bloqueado com sucesso.");
		}

		
		// 1 - Gerar uma nova senha para o associado
		// 2 - Atualizar a senha no banco de dados
		// 3 - Chamar o serviço de envio de e-mail para comunicar a nova senha para o associado
		public UtilRetorno reenviarSenha(int idAssociado) {

			string novaSenha = UtilString.randomString(8);
			string novaSenhaCrypt = UtilCrypt.SHA512(novaSenha);

		    var query = this.db.Associado.Where(x => x.id == idAssociado && x.dtExclusao == null);

		    query = query.condicoesSeguranca();

			Associado OAssociado = query.FirstOrDefault();

			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "O associado não foi localizado.");
			}

			if (OAssociado.ativo == "N") {
				return UtilRetorno.newInstance(true, "O Associado está desativado e não pode receber uma nova senha.");
			}

		    if (OAssociado.Pessoa.login.isEmpty()) {
                return UtilRetorno.newInstance(true, "O Associado não possui um login.");
            }

            var listaEmails = OAssociado.Pessoa.ToEmailList();

            if (!listaEmails.Any()) {
                return UtilRetorno.newInstance(true, "Não pode ser localizado nenhum e-mail para o reenvio da senha.");
            }

            OAssociado.Pessoa.senha = novaSenhaCrypt;

			this.db.SaveChanges();
		
			IEnvioSenhaAssociado EnvioEmail = EnvioSenhaAssociado.factory(OAssociado.idOrganizacao, listaEmails, null);

            EnvioEmail.enviar(OAssociado, novaSenha);

			return UtilRetorno.newInstance(false, "Foi gerada uma nova senha para e reenviada para os e-mails de cadastro do associado.");
		}

		// 1 - Gerar uma nova senha para o associado
		// 2 - Atualizar a senha no banco de dados
		// 3 - Chamar o serviço de envio de e-mail para comunicar a nova senha para o associado
		public UtilRetorno enviarLinkSenha(int idAssociado, string linkRecuperacao = "") {
			
		    var query = this.db.Associado.Where(x => x.id == idAssociado && x.dtExclusao == null);
			
		    query = query.condicoesSeguranca();
			
			Associado OAssociado = query.Select(x => new{
				x.id,
				x.idOrganizacao,
				x.ativo,
				Pessoa = new{
					x.Pessoa.nome,
					listaEmails = x.Pessoa.listaEmails.Where(e => e.email != null && e.dtExclusao == null)
				}
			}).FirstOrDefault().ToJsonObject<Associado>();
			
            if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "O cadastro informado não foi localizado.");
			}

			if (OAssociado.ativo == "N") {
				return UtilRetorno.newInstance(true, "O cadastro está desativado e não pode receber uma nova senha.");
			}
			
		    /*if (OAssociado.Pessoa.login.isEmpty()) {
                return UtilRetorno.newInstance(true, "O Associado não possui um login.");
            }*/
			
		    var listaEmails = OAssociado.Pessoa.ToEmailList();
            if (!listaEmails.Any()) {
                return UtilRetorno.newInstance(true, "Não pode ser localizado nenhum e-mail para o reenvio da senha.");
            }
			
            IEnvioLinkSenha EnvioEmail = EnvioLinkSenha.factory(OAssociado.idOrganizacao, listaEmails, null);
			
            EnvioEmail.enviar(OAssociado, linkRecuperacao);

			return UtilRetorno.newInstance(false, "Foi enviado um link para o e-mail de cadastro, através dele será possível criar uma nova senha.");
		}
        
        //Alterar o tipo de associado e registrar no histórico
	    public UtilRetorno alterarTipo(int idAssociado, int idNovoTipo, int idUsuarioAlteracao) {

	        var OAssociado = this.db.Associado.Find(idAssociado);

	        var OTipoAssociado = this.db.TipoAssociado.Find(idNovoTipo);

	        if (OAssociado == null) {
	            return UtilRetorno.newInstance(true, "Nenhum associado foi localizado com o ID informado.");
	        }

	        if (OTipoAssociado == null) {
	            return UtilRetorno.newInstance(true, "O Tipo de Associado selecionado não existe ou não está habilitado.");
	        }

	        string obsHistorico = String.Concat("O associado passou do tipo: ",(OAssociado.TipoAssociado == null ? "Nenhum" : OAssociado.TipoAssociado.nomeDisplay), " para: ", OTipoAssociado.nomeDisplay);

	        OAssociado.idTipoAssociado = idNovoTipo;

            db.SaveChanges();

	        this.OPessoaRelacionamentoBL.salvar(OAssociado.idPessoa, OcorrenciaRelacionamentoConst.idAlteracaoCadastro, idUsuarioAlteracao, obsHistorico);

            return UtilRetorno.newInstance(false, "O tipo do associado foi alterado com sucesso.");
	    }


        //Atualizacao da última data em que foi paga uma contribuição
        //Atualização do próximo vencimento do associado
	    public bool atualizarUltimoPagamentoContribuicao(AssociadoContribuicao OAssociadoContribuicao) {

	        var Contribuicao = OAssociadoContribuicao.Contribuicao ?? new Contribuicao();

	        var PeriodoContribuicao = Contribuicao.PeriodoContribuicao ?? new PeriodoContribuicao();

	        DateTime? dtProximoVencimento = null;

	        if (PeriodoContribuicao.id > 0) {

	            int qtdeMeses = PeriodoContribuicao.qtdeMeses;

	            dtProximoVencimento = OAssociadoContribuicao.dtVencimentoOriginal.AddMonths(qtdeMeses);

	        }

            this.db.Associado
                    .Where(x => x.id == OAssociadoContribuicao.idAssociado)
                    .Update(x => new Associado {
                        dtUltimoPagamentoContribuicao = OAssociadoContribuicao.dtPagamento,
                        dtProximoVencimento = dtProximoVencimento
                    }
                );

	        return true;
	    }
	}
}