namespace DAL.Procedures {

	public class SpNomes {

        public static readonly string spAssociadosSemAnuidade = "sp_associados_sem_anuidade";

        public static readonly string spAnuidadeTotalizadores = "sp_anuidade_totalizadores";

        public static readonly string spDadosLancamentoBoleto = "sp_dados_lancamento_boleto";

        //Contribuicoes
        public static readonly string spAssociadosNaoVinculadosContribuicao = "sp_associados_nao_vinculados_contribuicao";
        public static readonly string spAssociadosNaoVinculadosContribuicaoAdmissao = "sp_associados_nao_vinculados_contribuicao_admissao";
        public static readonly string spAssociadosNaoVinculadosContribuicaoUltimaContribuicao = "sp_associados_nao_vinculados_contribuicao_ultima_contribuicao";

        // Relatorios de Associados
	    public static readonly string spAssociadosPesquisaRapida = "sp_associados_pesquisa_rapida";

        public static readonly string spRelatorioEmissaoCarteirinha = "sp_relatorio_emissao_carteirinha";

        // Relatórios de Pedidos
        public static readonly string spRelatorioPedidosPorPeriodo = "sp_relatorio_pedidos_por_periodo";

        // Relatórios de Eventos
        public static readonly string spRelatorioGeralEventos = "sp_relatorio_geral_eventos";

        // Financeiro
        public static readonly string spContasBancariasSaldoAtual = "sp_conta_bancaria_saldo_atual";


    }
}