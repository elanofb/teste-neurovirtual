using System;

namespace DAL.Planos {

	public class StatusPlanoContratacaoConst {
		public static readonly int AGUARDANDO_PAGAMENTO = Convert.ToInt32(StatusPlanoContratacaoEnum.AGUARDANDO_PAGAMENTO);
		public static readonly int EM_APROVACAO = Convert.ToInt32(StatusPlanoContratacaoEnum.EM_APROVACAO);
		public static readonly int EM_VIGENCIA = Convert.ToInt32(StatusPlanoContratacaoEnum.EM_VIGENCIA);
		public static readonly int ENCERRADO = Convert.ToInt32(StatusPlanoContratacaoEnum.ENCERRADO);
		public static readonly int PAGAMENTO_CONFIRMADO = Convert.ToInt32(StatusPlanoContratacaoEnum.PAGAMENTO_CONFIRMADO);
	}
}