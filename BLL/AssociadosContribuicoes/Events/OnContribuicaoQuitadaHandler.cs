using BLL.Core.Events;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BLL.Associados;
using BLL.AssociadosContribuicoes.Emails;
using BLL.Financeiro;
using BLL.Services;
using BLL.Tarefas;
using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using DAL.Pessoas;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes.Events {

    public class OnContribuicaoQuitadaHandler : DefaultBL, IHandler<object> {

        //Atributos
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;
        private IAssociadoAcaoBL _AssociadoAcaoBL;


        //Propridades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => (this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL());
        private ITituloReceitaBL OTituloReceitaBL => new TituloReceitaContribuicaoBL();
        private IAssociadoAcaoBL OAssociadoAcaoBL => (this._AssociadoAcaoBL = this._AssociadoAcaoBL ?? new AssociadoAcaoBL());

        private AssociadoContribuicao AssociadoContribuicao { get; set; }

        //Chamador das ações do evento
        public void execute(object source) {

            try {

                int idTituloReceita = UtilNumber.toInt32(source);

                TituloReceita OTituloReceita = OTituloReceitaBL.carregar(idTituloReceita);

                this.AssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(UtilNumber.toInt32(OTituloReceita.idReceita));

                this.registrarPagamento(OTituloReceita);

                this.enviarEmailPagamento(AssociadoContribuicao);

                this.OAssociadoAcaoBL.atualizarUltimoPagamentoContribuicao(this.AssociadoContribuicao);

                new TarefaInadimplencia().executar();

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro no manipulador de evento: OnContribuicaoQuitadaHandler");
            }
        }

        /// <summary>
        /// Registar a data da quitacao na tb_associado_contribuicao
        /// </summary>
        private void registrarPagamento(TituloReceita OTituloReceita) {

            int idAssociadoContribuicao = OTituloReceita.idReceita.toInt();

            db.AssociadoContribuicao
                    .Where(x => x.id == idAssociadoContribuicao)
                    .Update(x => new AssociadoContribuicao {
                        dtPagamento = OTituloReceita.dtQuitacao

                    });

        }

        /// <summary>
        /// Enviar e-mail de confirmação do pagamento da anuidade
        /// </summary>
        private void enviarEmailPagamento(AssociadoContribuicao OAssociadoContribuicao) {

            if (OAssociadoContribuicao.flagImportado) {
                return;
            }

            var listaEmails = OAssociadoContribuicao.Associado.Pessoa.ToEmailList();

            listaEmails = listaEmails.Where(UtilValidation.isEmail).ToList();

            if (!listaEmails.Any()) {

                return;
            }

            IEnvioPagamentoContribuicao EmailEnvio = EnvioPagamentoContribuicao.factory(AssociadoContribuicao.idOrganizacao, listaEmails, null);

            EmailEnvio.enviar(OAssociadoContribuicao);

        }


    }
}