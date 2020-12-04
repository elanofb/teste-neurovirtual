using DAL.Associados;
using DAL.RelatoriosAssociados;

namespace WEB.Areas.Associados.Extensions{
    public static class AssociadoExtensions{

        //Status Associado
        public static string exibirStatus(this Associado OAssociado) {

			string descricaoAtivo = "Desativado";

            switch (OAssociado.ativo){
                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirStatus(this AssociadoEnderecoVW OAssociado) {

			string descricaoAtivo = "Desativado";

            switch (OAssociado.ativo){
                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirStatus(this AssociadoRelatorioVW OAssociado) {

            string descricaoAtivo = "Desativado";

            switch (OAssociado.ativo) {
                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }        
        
        //Status Associado
        public static string exibirStatus(this NaoAssociadoRelatorioVW OAssociado) {

            string descricaoAtivo = "Desativado";

            switch (OAssociado.ativo) {
                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }
        
        //
        public static string exibirIconeStatus(this Associado OAssociado) {

            string descricaoAtivo = (OAssociado.ativo == "E" ? "fa-clipboard" : (OAssociado.ativo == "S" ? "fa-check" : "fa-times"));

            return descricaoAtivo;
        }
        
        //
        public static string exibirClasseStatus(this Associado OAssociado) {

			string descricaoAtivo = (OAssociado.ativo == "E"? "bg-yellow": (OAssociado.ativo == "S"? "bg-green": "bg-red"));

            return descricaoAtivo;
        }
        
        //
        public static string exibirClasseTextoStatus(this Associado OAssociado) {

            string descricaoAtivo = (OAssociado.ativo == "E"? "text-yellow": (OAssociado.ativo == "S"? "text-green": "text-red"));

            return descricaoAtivo;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeSituacaoFinanceira(this Associado OAssociado) {

            string descricaoAtivo ="";// (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "fa-check" : (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "fa-check" : "fa-times"));

            return descricaoAtivo;
        }
        
        //Classes CSS situacao financeira Associado
        public static string exibirClasseSituacaoFinanceira(this Associado OAssociado) {

            string descricaoAtivo = "";// (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE? "bg-green": (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO? "bg-yellow": "bg-red"));

            return descricaoAtivo;
        }
        
        //Classes CSS situacao financeira Associado
        public static string exibirClasseTextoSituacaoFinanceira(this Associado OAssociado) {

            string descricaoAtivo = "";//(OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE? "text-green": (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO? "text-yellow": "text-red"));

            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirSituacao(this Associado OAssociado) {

            string descricaoAtivo = "";//(OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente": "Inadimplente");

            return descricaoAtivo;
        }


        //Status Associado
        public static string exibirSituacao(this AssociadoRelatorioVW OAssociado) {

            /*string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : "Inadimplente");

            return descricaoAtivo;*/

            return "";
        }

        //Status Associado
        public static string exibirSituacao(this AssociadoEnderecoVW OAssociado) {

            string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : "Inadimplente");

            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirSituacao(this NaoAssociadoRelatorioVW OAssociado) {

            //string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : "Inadimplente");

            //return descricaoAtivo;

            return "";
        }


    }
}