using DAL.Associados;
using DAL.RelatoriosAssociados;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Extensions {

    public static class CompradorExtensions {

        //Status Associado
        public static string exibirStatus(this DadosCompradorDTO OAssociado) {

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
        public static string exibirIconeStatus(this DadosCompradorDTO OAssociado) {

            string descricaoAtivo = (OAssociado.ativo == "E" ? "fa-clipboard" : (OAssociado.ativo == "S" ? "fa-check" : "fa-times"));

            return descricaoAtivo;
        }

        //
        public static string exibirClasseStatus(this DadosCompradorDTO OAssociado) {

            string descricaoAtivo = (OAssociado.ativo == "E" ? "text-yellow" : (OAssociado.ativo == "S" ? "text-green" : "text-red"));

            return descricaoAtivo;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeSituacaoFinanceira(this DadosCompradorDTO OAssociado) {

            string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "fa-check" : (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "fa-check" : "fa-times"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseSituacaoFinanceira(this DadosCompradorDTO OAssociado) {

            string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "text-green" : (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "text-yellow" : "text-red"));

            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirSituacao(this DadosCompradorDTO OAssociado) {

            string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "Isento" : "Inadimplente"));

            return descricaoAtivo;
        }

        //Status Associado
        public static string exibirSituacao(this AssociadoEnderecoVW OAssociado) {

            string descricaoAtivo = (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : (OAssociado.flagSituacaoContribuicao == SituacaoContribuicaoConst.ISENTO ? "Isento" : "Inadimplente"));

            return descricaoAtivo;
        }

    }

}