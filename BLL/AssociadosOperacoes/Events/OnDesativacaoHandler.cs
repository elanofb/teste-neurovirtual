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

namespace BLL.AssociadosOperacoes.Events {

	public class OnDesativacaoHandler : IHandler<object> {

		//Atributos
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propridades
		private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() );

	    //Chamador das ações do evento
		public void execute(object source) {
			try {

			    var listaAssociado = (source as List<Associado>);

			    foreach (var OAssociado in listaAssociado) {

				    this.gerarOcorrencia(OAssociado);

                    this.postBack(OAssociado);

                }

			} catch (Exception ex) {
				UtilLog.saveError(ex, "");
			}
		}

		//Gerar Ocorrencia para histórico do associado
		public void gerarOcorrencia(Associado OAssociado) { 

			PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();

			Ocorrencia.dtOcorrencia = DateTime.Now;

			Ocorrencia.idPessoa = OAssociado.idPessoa;

			Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idDesativacaoAssociado;

			Ocorrencia.observacao = OAssociado.observacaoDesativacao;

			this.OPessoaRelacionamentoBL.salvar(Ocorrencia);

		}

        private void postBack(Associado OAssociado) {


            string json = JsonConvert.SerializeObject(OAssociado, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            byte[] data = Encoding.ASCII.GetBytes("associado=" + json);

            var ORequestPost = new RequestPost();

        }

    }

}