namespace DAL.Financeiro {

	public class TipoReceitaConst {

		public static readonly byte SOLICITACAO = (byte)TipoReceitaEnum.SOLICITACAO;

        public static readonly byte PEDIDO = (byte)TipoReceitaEnum.PEDIDO;

        public static readonly byte INSCRICAO_EVENTO = (byte)TipoReceitaEnum.INSCRICAO_EVENTO;

        public static readonly byte TRANSFERENCIA = (byte)TipoReceitaEnum.TRANSFERENCIA;

        public static readonly byte PLANO = (byte)TipoReceitaEnum.PLANO;

        public static readonly byte TAXA_INSCRICAO = (byte)TipoReceitaEnum.TAXA_INSCRICAO;

        public static readonly byte CONTRIBUICAO = (byte)TipoReceitaEnum.CONTRIBUICAO;

        public static readonly byte OUTROS = (byte)TipoReceitaEnum.OUTROS;
		
		public static readonly byte PROCESSO_AVALIACAO = (byte)TipoReceitaEnum.PROCESSO_AVALIACAO;
    }
}