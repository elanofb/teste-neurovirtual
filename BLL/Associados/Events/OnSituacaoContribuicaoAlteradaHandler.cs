using System;
using System.Collections.Generic;
using BLL.Core.Events;
using DAL.Associados;
using DAL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.Associados.Events {

    public class OnSituacaoContribuicaoAlteradaHandler : IHandler<object> {

        //Atributos
        private IAssociadoRelacionamentoBL _AssociadoRelacionamentoBL;

        //Propriedades
        private IAssociadoRelacionamentoBL OAssociadoRelacionamentoBL => _AssociadoRelacionamentoBL = _AssociadoRelacionamentoBL ?? new AssociadoRelacionamentoBL();

        private int idAssociado { get; set; }
        private string motivoAlteracao { get; set; }

        //
        public void execute(object source) {

            try {

                var listaParametros = (source as List<object>);

                this.idAssociado = Convert.ToInt32(listaParametros[0]);

                this.motivoAlteracao = listaParametros[1].ToString();

                this.salvarHistorico();

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro no manipulador de evento: OnSituacaoContribuicaoAlteradaHandler");
            }
        }

        //Salva o histórico
        private void salvarHistorico() {

            Associado OAssociado = new AssociadoBL().carregar(idAssociado);

            if (OAssociado == null) {

                return;

            }


        }
    }
}