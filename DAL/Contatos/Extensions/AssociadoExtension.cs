
using System;

namespace DAL.Contatos {

    public static class PessoaContatoVWExtension {

        //Status Associado
        public static string exibirStatus(this PessoaContatoVW OPessoaContatoVW) {

            string descricaoAtivo = "Desativado";

            switch (OPessoaContatoVW.ativo) {
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

    }

}
