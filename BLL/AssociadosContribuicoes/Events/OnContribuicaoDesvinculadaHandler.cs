using BLL.Core.Events;
using System;
using BLL.Associados;
using DAL.AssociadosContribuicoes;
using BLL.Financeiro;
using DAL.Associados;
using DAL.Financeiro;
using DAL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.AssociadosContribuicoes.Events {

    public class OnContribuicaoDesvinculadaHandler : IHandler<object> {

        //Atributos

        //Propridades

        //Chamador das ações do evento
        public void execute(object source) {

            try {

                var OContribuicaoExcluida = (source as AssociadoContribuicao) ?? new AssociadoContribuicao();

                this.salvarHistorico(OContribuicaoExcluida);

                var OTituloReceitaExclusaoBL = new TituloReceitaExclusaoBL();

                OTituloReceitaExclusaoBL.excluir(TipoReceitaConst.CONTRIBUICAO, OContribuicaoExcluida.id, "Contribuicao desvinculada");

            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro no manipulador de evento: OnContribuicaoDesvinculadaHandler");

            }
        }

        //Salva o histórico
        private void salvarHistorico(AssociadoContribuicao OAssociadoContribuicao) {

            var OAssociadoBL = new AssociadoBL();

            Associado OAssociado = OAssociadoBL.carregar(OAssociadoContribuicao.idAssociado);

            PessoaRelacionamento Relacionamento = new PessoaRelacionamento();

            Relacionamento.idPessoa = OAssociado.idPessoa;

            Relacionamento.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idDesvinculoContribuicao;

            Relacionamento.observacao = string.Concat(OAssociadoContribuicao.Contribuicao.descricao, " removida, motivo: ", OAssociadoContribuicao.motivoExclusao);

            Relacionamento.dtOcorrencia = DateTime.Now;

            var OAssociadoRelacionamentoBL = new AssociadoRelacionamentoBL();
            
            OAssociadoRelacionamentoBL.salvar(Relacionamento);
        }
    }
}