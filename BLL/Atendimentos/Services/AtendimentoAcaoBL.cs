using System;
using BLL.Core.Events;
using BLL.Permissao;
using BLL.Services;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;

namespace BLL.Atendimentos {

	public class AtendimentoAcaoBL : DefaultBL, IAtendimentoAcaoBL {

        // Atributos Servicos
	    private IAtendimentoConsultaBL _IAtendimentoConsultaBL;
	    private IAtendimentoCadastroBL _IAtendimentoCadastroBL;
        private IAtendimentoHistoricoBL _IAtendimentoHistoricoBL;

        // Propriedades Serviços
	    private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();
	    private IAtendimentoCadastroBL OAtendimentoCadastroBL => _IAtendimentoCadastroBL = _IAtendimentoCadastroBL ?? new AtendimentoCadastroBL();
        private IAtendimentoHistoricoBL OAtendimentoHistoricoBL => _IAtendimentoHistoricoBL = _IAtendimentoHistoricoBL ?? new AtendimentoHistoricoBL();

        // Events
	    private readonly EventAggregator onAtendimentoIniciado = new OnAtendimentoIniciado();
        private readonly EventAggregator onHistoricoCadastrado = new OnHistoricoCadastrado();
	    private readonly EventAggregator onHistoricoMensagemCadastrado = new OnHistoricoMensagemCadastrado();

        //
        public void iniciarAtendimento(int idAtendimento) {

            var OAtendimentoHistorico = new AtendimentoHistorico();
            
            OAtendimentoHistorico.idAtendimento = idAtendimento;

            OAtendimentoHistorico.idStatusAtendimento = AtendimentoStatusConst.EM_ATENDIMENTO;

            OAtendimentoHistorico.nome = User.name();

            OAtendimentoHistorico.mensagem = "<strong><i class=\"fa fa-sign-in\"></i> <i>O atendimento foi iniciado.</i></strong>";

            var flagSucesso = this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);

            if (flagSucesso) {

                this.onAtendimentoIniciado.subscribe(new OnAtendimentoIniciadoHandler());

                this.onAtendimentoIniciado.publish((object) idAtendimento);

            }

        }

        //
        public void enviarMensagem(AtendimentoHistorico OAtendimentoHistorico) {

            OAtendimentoHistorico.nome = User.name();

            var flagSucesso = this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);

            if (flagSucesso) {

                this.onHistoricoMensagemCadastrado.subscribe(new OnHistoricoMensagemCadastradoHandler());

                this.onHistoricoMensagemCadastrado.publish((object)OAtendimentoHistorico);

            }

        }

        //
	    public void aguardarRetorno(AtendimentoHistorico OAtendimentoHistorico) {

	        OAtendimentoHistorico.nome = User.name();

            OAtendimentoHistorico.idStatusAtendimento = AtendimentoStatusConst.AGUARDANDO_RETORNO;

            var flagSucesso = this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);

            if (flagSucesso) {

                this.onHistoricoCadastrado.subscribe(new OnHistoricoCadastradoHandler());

                this.onHistoricoCadastrado.publish((object) OAtendimentoHistorico);

            }

	    }

	    //
	    public void finalizar(AtendimentoHistorico OAtendimentoHistorico) {

	        OAtendimentoHistorico.nome = User.name();

	        OAtendimentoHistorico.idStatusAtendimento = AtendimentoStatusConst.FINALIZADO;

	        var flagSucesso = this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);

	        if (flagSucesso) {

	            this.onHistoricoCadastrado.subscribe(new OnHistoricoCadastradoHandler());

	            this.onHistoricoCadastrado.publish((object)OAtendimentoHistorico);

	        }

	    }

	    //
	    public void sair(AtendimentoHistorico OAtendimentoHistorico) {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(OAtendimentoHistorico.idAtendimento);

            var idStatusAtual = OAtendimento.idStatusAtendimento;

            OAtendimento.idStatusAtendimento = OAtendimento.idStatusAtendimentoAnterior.toInt();

            OAtendimento.idStatusAtendimentoAnterior = idStatusAtual;

            this.OAtendimentoCadastroBL.salvar(OAtendimento);

            
            OAtendimentoHistorico.idStatusAtendimento = OAtendimento.idStatusAtendimento;

	        OAtendimentoHistorico.nome = User.name();

            OAtendimentoHistorico.mensagem = String.Concat("<strong><i class=\"fa fa-sign-out\"></i> <i>O atendimento está temporariamente encerrado:</i></strong> ", OAtendimentoHistorico.mensagem);

            this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);
            
	    }

    }
}