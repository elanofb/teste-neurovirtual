using System;
using BLL.Core.Events;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;

namespace BLL.Atendimentos {

    public class OnAtendimentoIniciadoHandler : IHandler<object> {

        // Atributos Serviços
        private IAtendimentoConsultaBL _IAtendimentoConsultaBL;
        private IAtendimentoCadastroBL _IAtendimentoCadastroBL;

        // Propriedades Serviços
        private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private IAtendimentoCadastroBL OAtendimentoCadastroBL => _IAtendimentoCadastroBL = _IAtendimentoCadastroBL ?? new AtendimentoCadastroBL();


        public void execute(object source) {

            try {

                var idAtendimento = source.toInt();

                this.registrarInicioAtendimento(idAtendimento);

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro no manipulador de evento: OnAtendimentoIniciadoHandler");

            }
        }

        private void registrarInicioAtendimento(int idAtendimento) {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(idAtendimento);

            try {

                OAtendimento.dtInicioAtendimento = OAtendimento.dtInicioAtendimento ?? DateTime.Now;

                OAtendimento.idUsuarioInicioAtendimento = OAtendimento.idUsuarioInicioAtendimento ?? HttpContextFactory.Current.User.id();

                OAtendimento.idUltimoUsuarioAtendimento = HttpContextFactory.Current.User.id();

                OAtendimento.dtUltimoAtendimento = DateTime.Now;

                OAtendimento.idStatusAtendimentoAnterior = OAtendimento.idStatusAtendimento;

                OAtendimento.idStatusAtendimento = AtendimentoStatusConst.EM_ATENDIMENTO;

                this.OAtendimentoCadastroBL.salvar(OAtendimento);

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao registrar o primeiro histórico do atendimento { OAtendimento.id }: OnAtendimentoIniciadoHandler");

            }
        }
    }
}