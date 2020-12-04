using System;
using BLL.Core.Events;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

    public class OnAtendimentoCadastradoHandler : IHandler<object> {

        // Atributos Serviços
        private IAtendimentoHistoricoBL _IAtendimentoHistoricoBL;

        // Propriedades Serviços
        private IAtendimentoHistoricoBL OAtendimentoHistoricoBL => _IAtendimentoHistoricoBL = _IAtendimentoHistoricoBL ?? new AtendimentoHistoricoBL();


        public void execute(object source) {

            try {

                var OAtendimento = source as Atendimento;

                this.gerarPrimeiroHistorico(OAtendimento);

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro no manipulador de evento: OnAtendimentoCadastradoHandler");

            }
        }

        private void gerarPrimeiroHistorico(Atendimento OAtendimento) {

            try {

                var OAtendimentoHistorico = new AtendimentoHistorico();

                OAtendimentoHistorico.idOrganizacao = OAtendimento.idOrganizacao;

                OAtendimentoHistorico.idAtendimento = OAtendimento.id;

                OAtendimentoHistorico.nome = OAtendimento.nome;

                OAtendimentoHistorico.mensagem = OAtendimento.mensagem;

                OAtendimentoHistorico.idStatusAtendimento = OAtendimento.idStatusAtendimento;

                this.OAtendimentoHistoricoBL.salvar(OAtendimentoHistorico);

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao registrar o primeiro histórico do atendimento { OAtendimento.id }: OnAtendimentoCadastradoHandler");

            }
        }
    }
}