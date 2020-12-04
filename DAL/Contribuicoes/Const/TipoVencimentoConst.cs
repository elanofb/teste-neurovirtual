using System;

namespace DAL.Contribuicoes {

	public static class TipoVencimentoConst {

        public static readonly int FIXO_PELA_CONTRIBUICAO = Convert.ToInt32(TipoVencimentoEnum.FIXO_PELA_CONTRIBUICAO);

        public static readonly int VENCIMENTO_PELA_ADMISSAO_ASSOCIADO = Convert.ToInt32(TipoVencimentoEnum.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO);

        public static readonly int VENCIMENTO_PELA_DATA_GERACAO = Convert.ToInt32(TipoVencimentoEnum.VENCIMENTO_PELA_DATA_GERACAO);

        public static readonly int VENCIMENTO_PELO_ULTIMO_PAGAMENTO = Convert.ToInt32(TipoVencimentoEnum.VENCIMENTO_PELO_ULTIMO_PAGAMENTO);
        
	}
}