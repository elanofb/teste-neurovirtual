using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Core.Events;
using DAL.Atendimentos;

namespace BLL.Atendimentos {

    public class OnHistoricoMensagemCadastradoHandler : IHandler<object> {

        // Atributos Serviços
        private IAtendimentoConsultaBL _IAtendimentoConsultaBL;

        // Propriedades Serviços
        private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private AtendimentoHistorico OAtendimentoHistorico;

        public void execute(object source) {

            try {

                this.OAtendimentoHistorico = source as AtendimentoHistorico;

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao capturar os parÂmetros no evento: OnHistoricoMensagemCadastradoHandler");

            }
            
            try {

                this.dispararEmail();

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao executar o metodo dispararEmail() evento: OnHistoricoMensagemCadastradoHandler");

            }
            
        }

        //
        private void dispararEmail() {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(this.OAtendimentoHistorico.idAtendimento);

            var listaEmail = new List<string> { OAtendimento.email };

            var OEmail = EnvioMensagemAtendimento.factory(OAtendimentoHistorico.idOrganizacao.toInt(), listaEmail, null);

            OEmail.enviar(OAtendimento, this.OAtendimentoHistorico.mensagem);

        }

    }
}