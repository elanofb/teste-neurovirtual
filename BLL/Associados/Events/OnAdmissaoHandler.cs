using BLL.Core.Events;
using DAL.Associados;
using System.Collections.Generic;
using System;
using System.Text;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;
using BLL.Request;
using Newtonsoft.Json;

namespace BLL.Associados.Events {

    public class OnAdmissaoHandler : IHandler<object> {

        //Atributos
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

        //Propridades
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();

        //Chamador das ações do evento
        public void execute(object source) {
            try {

                var listaParametros = source as List<object>;

                var listaAssociados = listaParametros[0] as List<Associado>;

                var observacoes = listaParametros[1] as string;

                listaAssociados.ForEach(x => {

                    this.gerarOcorrencia(x, observacoes);


                });

            } catch (Exception ex) {
                UtilLog.saveError(ex, "");
            }
        }

        //Gerar Ocorrencia para histórico do associado
        public void gerarOcorrencia(Associado OAssociado, string observacoes) {

            PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();
            Ocorrencia.dtOcorrencia = OAssociado.dtAdmissao;
            Ocorrencia.idPessoa = OAssociado.idPessoa;
            Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idAdmissaoAssociado;
            Ocorrencia.observacao = observacoes;

            this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
        }

    }
}