using System;
using BLL.Core.Events;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

    public class OnHistoricoCadastradoHandler : IHandler<object> {

        // Atributos Serviços
        private IAtendimentoConsultaBL _IAtendimentoConsultaBL;
        private IAtendimentoCadastroBL _IAtendimentoCadastroBL;

        // Propriedades Serviços
        private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private IAtendimentoCadastroBL OAtendimentoCadastroBL => _IAtendimentoCadastroBL = _IAtendimentoCadastroBL ?? new AtendimentoCadastroBL();


        public void execute(object source) {

            try {

                var OAtendimentoHistorico = source as AtendimentoHistorico;

                this.alterarStatus(OAtendimentoHistorico);

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro no manipulador de evento: OnHistoricoCadastradoHandler");

            }
        }

        private void alterarStatus(AtendimentoHistorico OAtendimentoHistorico) {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(OAtendimentoHistorico.idAtendimento);

            try {

                OAtendimento.idStatusAtendimentoAnterior = OAtendimento.idStatusAtendimento;

                OAtendimento.idStatusAtendimento = OAtendimentoHistorico.idStatusAtendimento.toInt();

                OAtendimento.dtUltimoAtendimento = DateTime.Now;

                if (OAtendimento.idStatusAtendimento == AtendimentoStatusConst.FINALIZADO) {

                    OAtendimento.dtFinalizacaoAtendimento = DateTime.Now;

                }

                if (OAtendimentoHistorico.flagAtendido.HasValue) {

                    OAtendimento.flagAtendido = OAtendimentoHistorico.flagAtendido;

                }

                this.OAtendimentoCadastroBL.salvar(OAtendimento);

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao alterar o status do atendimento { OAtendimento.id } para { OAtendimentoHistorico.idStatusAtendimento }: OnHistoricoCadastradoHandler");

            }
        }
    }
}