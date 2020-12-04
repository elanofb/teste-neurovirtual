using BLL.Core.Events;
using DAL.Associados;
using System;
using DAL.Pessoas;
using BLL.Pessoas;
using DAL.Relacionamentos;

namespace BLL.NaoAssociados.Events {

	public class OnNaoAssociadoDesativacaoHandler : IHandler<object> {

		//Atributos
		private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;

		//Propridades
		private Associado OAssociado { get; set;}
		private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => (this._PessoaRelacionamentoBL = this._PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL() );

	    //Chamador das ações do evento
		public void execute(object source) {
			try {

				this.OAssociado = (source as Associado);

				this.gerarOcorrencia();

			} catch (Exception ex) {
				UtilLog.saveError(ex, "");
			}
		}

		//Gerar Ocorrencia para histórico do associado
		public void gerarOcorrencia() { 
			PessoaRelacionamento Ocorrencia = new PessoaRelacionamento();
			Ocorrencia.dtOcorrencia = DateTime.Now;
			Ocorrencia.idPessoa = OAssociado.idPessoa;
			Ocorrencia.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idDesativacaoNaoAssociado;
			Ocorrencia.observacao = this.OAssociado.observacoes;

			this.OPessoaRelacionamentoBL.salvar(Ocorrencia);
		}
	}
}