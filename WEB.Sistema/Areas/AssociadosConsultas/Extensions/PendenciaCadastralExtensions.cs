using DAL.Associados;

namespace WEB.Areas.AssociadosConsultas.Extensions{

    public static class PendenciaCadastralExtensions{
        
        //Status Associado
        public static string exibirStatus(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = "Desativado";

            switch (OAssociado.ativo) {

                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;

                case "B":
                    descricaoAtivo = "Bloqueado";
                    break;
            }


            return descricaoAtivo;
        }
        
        //
		public static string exibirIconeStatus(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = (OAssociado.ativo == "E"? "fa-clipboard": (OAssociado.ativo == "S"? "fa-check": "fa-times"));

            return descricaoAtivo;
        }

        //
		public static string exibirClasseStatus(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = (OAssociado.ativo == "E"? "text-yellow": (OAssociado.ativo == "S"? "text-green": "text-red"));

            return descricaoAtivo;
        }
        
        //Icone FA situacao financeira Associado
        public static string exibirIconeSituacaoFinanceira(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "fa-check": (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO? "fa-check": "fa-times"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseSituacaoFinanceira(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE? "text-green": (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO? "text-yellow": "text-red"));

            return descricaoAtivo;
        }
        
        //Status Associado
        public static string exibirSituacao(this PendenciaCadastralVW OAssociado) {

			string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente": (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "Isento": "Inadimplente"));

            return descricaoAtivo;
        }
    }
}