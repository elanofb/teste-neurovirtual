using BLL.Core.Events;
using DAL.Associados;
using System;
using System.Collections.Generic;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.AssociadosOperacoes.Events {

    public class OnAdmissaoAlteradaHandler : IHandler<object> {

        //Atributos
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

        //Propridades
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

        //Chamador das ações do evento
        public void execute(object source) {

            try {

                var listaAssociado = (source as List<Associado>);

                foreach (var OAssociado in listaAssociado) {

                    this.gerarOcorrencia(OAssociado);
                    
                }

                
            } catch (Exception ex) {
                UtilLog.saveError(ex, "");
            }

        }

        //Gerar Ocorrencia para histórico do associado
        public void gerarOcorrencia(Associado OAssociado) {

            var Ocorrencia = new PessoaRelacionamento();

            Ocorrencia.dtOcorrencia = DateTime.Now;

            Ocorrencia.idPessoa = OAssociado.idPessoa;

            Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAlteracaoAdmissaoAssociado;

            Ocorrencia.observacao = OAssociado.observacoes;

            this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

        }
        
    }

}