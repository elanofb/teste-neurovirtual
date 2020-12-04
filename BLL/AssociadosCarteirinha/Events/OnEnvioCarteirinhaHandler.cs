using BLL.Associados;
using BLL.Core.Events;
using DAL.Associados;
using DAL.AssociadosCarteirinha;
using DAL.Pessoas;
using DAL.Relacionamentos;
using System;

namespace BLL.AssociadosCarteirinha.Events {

    public class OnEnvioCarteirinhaHandler : IHandler<object> {

		//Atributos
		private IAssociadoBL _AssociadoBL;
		private IAssociadoRelacionamentoBL _AssociadoRelacionamentoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL { get{ return (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL() ); } }
        private IAssociadoRelacionamentoBL OAssociadoRelacionamentoBL { get{ return (this._AssociadoRelacionamentoBL = this._AssociadoRelacionamentoBL ?? new AssociadoRelacionamentoBL() ); } }

		//
		public void execute(object source) {

            try {

                var OAssociadoCarteirinha = (source as AssociadoCarteirinha);
				this.salvarHistorico(OAssociadoCarteirinha);

			} catch (Exception ex) {
				UtilLog.saveError(ex, "Erro no manipulador de evento: OnEnvioCarteirinhaHandlerHandler");
			}
		}

		//Salva o histórico
		private void salvarHistorico(AssociadoCarteirinha OAssociadoCarteirinha) { 

			Associado OAssociado = this.OAssociadoBL.carregar(OAssociadoCarteirinha.idAssociado);

		    string descricaoObservacao = "Data do Envio: " + OAssociadoCarteirinha.dtEnvio.ToShortDateString() + " | " + 
                                         "Tipo de Emissão: " + OAssociadoCarteirinha.flagTipoEmissao() + " | " + 
                                         "Tipo de Envio: " + OAssociadoCarteirinha.flagTipoEnvio() + " | " + 
                                         "OBS.: " + OAssociadoCarteirinha.observacao;

			PessoaRelacionamento Relacionamento = new PessoaRelacionamento();
			Relacionamento.idPessoa = OAssociado.idPessoa;
			Relacionamento.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoConst.idEnvioCarteirinha;
			Relacionamento.observacao = String.Concat(descricaoObservacao);
			Relacionamento.dtOcorrencia = DateTime.Now;

			this.OAssociadoRelacionamentoBL.salvar(Relacionamento);
		}
	}
}