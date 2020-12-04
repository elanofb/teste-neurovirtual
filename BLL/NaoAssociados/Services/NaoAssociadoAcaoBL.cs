using System;
using System.Linq;
using DAL.Associados;
using BLL.Services;
using BLL.Core.Events;
using BLL.Associados.Events;
using BLL.AssociadosInstitucional.Emails;
using BLL.Email;
using BLL.NaoAssociados.Events;
using BLL.Pessoas;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.NaoAssociados {

	public class NaoAssociadoAcaoBL : DefaultBL, INaoAssociadoAcaoBL {

		//Atributos
	    private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propriedades
	    public IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

		//Events
        private readonly EventAggregator onAdmissao = OnAdmissao.getInstance;
		private readonly EventAggregator onDesativacao = OnNaoAssociadoDesativacao.getInstance;
		private readonly EventAggregator onReativacao = OnNaoAssociadoReativacao.getInstance;
        private readonly EventAggregator onTornarAssociado = OnTornarAssociado.getInstance;

		//Desativar um determinado não associado
		public UtilRetorno desativarNaoAssociado(int idAssociado, string observacoes) {
			this.onDesativacao.subscribe( new OnNaoAssociadoDesativacaoHandler() );

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado && x.dtExclusao == null);
			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Cadastro não encontrado.");
			}

			OAssociado.dtDesativacao = DateTime.Now;
			OAssociado.idUsuarioDesativacao = idUsuarioLogado;
			OAssociado.ativo = "N";
			db.SaveChanges();

            OAssociado.observacoes = observacoes;
            this.onDesativacao.publish( (OAssociado as object) );

			return UtilRetorno.newInstance(false, "Não Associado desativado com sucesso.");
		}

		//Reativar um não associado que estava com status inativo
		public UtilRetorno reativarNaoAssociado(int idAssociado, string observacoes) {
			this.onReativacao.subscribe( new OnNaoAssociadoReativacaoHandler() );

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado && x.dtExclusao == null);
			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Cadastro não encontrado.");
			}

			OAssociado.dtReativacao = DateTime.Now;
			OAssociado.idUsuarioReativacao = idUsuarioLogado;
			OAssociado.ativo = "S";
			db.SaveChanges();

            OAssociado.observacoes = observacoes;
            this.onReativacao.publish( (OAssociado as object) );

			return UtilRetorno.newInstance(false, "Não Associado reativado com sucesso.");
		}

		// 1 - Gerar uma nova senha para o não associado
		// 2 - Atualizar a senha no banco de dados
		// 3 - Chamar o serviço de envio de e-mail para comunicar a nova senha para o associado
		public UtilRetorno reenviarSenha(int idAssociado) {

			string novaSenha = UtilString.randomString(8);
			string novaSenhaCrypt = UtilCrypt.SHA512(novaSenha);

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado && x.dtExclusao == null);
			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "O cadastro não foi localizado.");
			}

			if (OAssociado.ativo == "N") {
				return UtilRetorno.newInstance(true, "O Não Associado está desativado e não pode receber uma nova senha.");
			}

			OAssociado.Pessoa.senha = novaSenhaCrypt;
			this.db.SaveChanges();
		
			IEnvioSenhaAssociado EnvioEmail = EnvioSenhaAssociado.factory(OAssociado.idOrganizacao, OAssociado.Pessoa.emailPrincipal().ToOneList(), OAssociado.Pessoa.emailSecundario().ToOneList() );
			EnvioEmail.enviar(OAssociado, novaSenha);

			return UtilRetorno.newInstance(false, "Foi gerada uma nova senha para e reenviada para os e-mails de cadastro.");
		}

		// 1 - Gerar uma nova senha para o não associado
		// 2 - Atualizar a senha no banco de dados
		// 3 - Chamar o serviço de envio de e-mail para comunicar a nova senha para o não associado
		public UtilRetorno enviarLinkSenha(int idAssociado) {

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado && x.dtExclusao == null);
			if (OAssociado == null) { 
				return UtilRetorno.newInstance(true, "O cadastro informado não foi localizado.");
			}

			if (OAssociado.ativo == "N") {
				return UtilRetorno.newInstance(true, "O Não Associado está desativado e não pode receber uma nova senha.");
			}
		
			IEnvioLinkSenha EnvioEmail = EnvioLinkSenha.factory(OAssociado.idOrganizacao, OAssociado.Pessoa.emailPrincipal().ToOneList(), null);

            EnvioEmail.enviar(OAssociado);

			return UtilRetorno.newInstance(false, "Foi enviado um link para o e-mail do não associado, através dele será possível criar uma nova senha.");
		}

        //Excluir um não associado
		public UtilRetorno excluirNaoAssociado(int idAssociado, string observacoes) {

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == idAssociado);
			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Cadastro não encontrado.");
			}

			OAssociado.dtExclusao = DateTime.Now;
			OAssociado.idUsuarioExclusao = idUsuarioLogado;
			OAssociado.ativo = "N";
			OAssociado.observacaoDesligamento = observacoes;
			db.SaveChanges();

			Pessoa OPessoa = this.db.Pessoa.FirstOrDefault(x => x.id == OAssociado.idPessoa);
            OPessoa.ativo = "N";
            OPessoa.flagExcluido = "S";
			db.SaveChanges();

			return UtilRetorno.newInstance(false, "Não Associado reativado com sucesso.");
		}

		//Altera de não associado para associado
		public UtilRetorno tornarAssociado(Associado Associado, string observacoes) {

			int idUsuarioLogado = User.id();

			Associado OAssociado = this.db.Associado.FirstOrDefault(x => x.id == Associado.id && x.dtExclusao == null);
			if(OAssociado == null){
				return UtilRetorno.newInstance(true, "Cadastro não encontrado.");
			}

		    OAssociado.idTipoCadastro = AssociadoTipoCadastroConst.CONSUMIDOR;
            OAssociado.idTipoAssociado = Associado.idTipoAssociado;
            OAssociado.dtAlteracao = DateTime.Now;
			OAssociado.idUsuarioAlteracao = idUsuarioLogado;
		    OAssociado.ativo = Associado.ativo;
            OAssociado.dtAdmissao = Associado.dtAdmissao;
		    OAssociado.nroAssociado = this.proximoId();
			db.SaveChanges();

            OAssociado.observacoes = observacoes;

            this.onTornarAssociado.subscribe( new OnTornarAssociadoHandler() );
            this.onTornarAssociado.publish( (OAssociado as object) );

            if (OAssociado.dtAdmissao != null) {
                this.onAdmissao.subscribe( new OnAdmissaoHandler() );
                this.onAdmissao.publish( (OAssociado as object) );
            }

		    return UtilRetorno.newInstance(false, "Não Associado desativado com sucesso.");
		}

        //Alterar o tipo de associado e registrar no historico
	    public UtilRetorno alterarTipo(int idAssociado, int idNovoTipo, int idUsuarioAlteracao) {

	        var OAssociado = this.db.Associado.Find(idAssociado);

	        var OTipoAssociado = this.db.TipoAssociado.Find(idNovoTipo);

	        if (OAssociado == null) {
	            return UtilRetorno.newInstance(true, "Nenhum cadastro foi localizado com o ID informado.");
	        }

	        if (OTipoAssociado == null) {
	            return UtilRetorno.newInstance(true, "O Tipo de Não Associado selecionado não existe ou não está habilitado.");
	        }

	        string obsHistorico = String.Concat("O não associado passou do tipo: ",(OAssociado.TipoAssociado == null ? "Nenhum" : OAssociado.TipoAssociado.nomeDisplay), " para: ", OTipoAssociado.nomeDisplay);

	        OAssociado.idTipoAssociado = idNovoTipo;

            db.SaveChanges();

	        this.OPessoaRelacionamentoBL.salvar(OAssociado.idPessoa, OcorrenciaRelacionamentoConst.idAlteracaoCadastro, idUsuarioAlteracao, obsHistorico);

            return UtilRetorno.newInstance(false, "O tipo de não associado foi alterado com sucesso.");
	    }

		//Verificar se já existe um registro para evitar duplicidades
		private int proximoId() {

		    int nroProximoId = db.Associado.Max(x => x.nroAssociado) ?? 0;

		    if (nroProximoId == 0) {
		        return 1;
		    }

		    nroProximoId = nroProximoId + 1;
			return nroProximoId;
		}	
	}
}