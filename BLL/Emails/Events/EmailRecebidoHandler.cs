using BLL.Associados;
using BLL.Core.Events;
using DAL.Entities;

namespace BLL.Email {

	public class EmailRecebidoHandler : IHandler<LogEmail> {

		//Atributos
		private AssociadoBL _OAssociadoBL;

		private AssociadoRelacionamentoBL _OAssociadoRelacionamentoBL;

		//Propriedades
		public AssociadoBL OAssociadoBL {
			get { return _OAssociadoBL ?? (this._OAssociadoBL = new AssociadoBL()); }
		}

		public AssociadoRelacionamentoBL OAssociadoRelacionamentoBL {
			get { return _OAssociadoRelacionamentoBL ?? (this._OAssociadoRelacionamentoBL = new AssociadoRelacionamentoBL()); }
		}

		/**
		*
		*/

		public void execute(LogEmail source) {
			//Pessoa OAssociado = this.OAssociadoBL.listar("", "").Where(x => x.emailPrincipal == source.emailRemetente || x.emailSecundario == source.emailRemetente).FirstOrDefault();

			//if(OAssociado != null){
			//	PessoaRelacionamento Relacionamento = new PessoaRelacionamento();
			//	Relacionamento.idPessoa = OAssociado.id;
			//	Relacionamento.idOcorrenciaRelacionamento = OcorrenciaRelacionamentoBL.idEmailRecebido;
			//	Relacionamento.observacao = String.Concat("Assunto: ", source.assunto, "<br /><br />", source.corpoMensagem);
			//	Relacionamento.dtOcorrencia = source.dtEnvio;
			//	this.OAssociadoRelacionamentoBL.salvar(Relacionamento, null);
			//}
		}
	}
}