using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Documentos;

namespace DAL.Pessoas {

    public static class PessoaEnderecoExtensions {
        
        public static string formatarEnderecoCompleto(this PessoaEndereco OEndereco) {
           
            if (OEndereco == null) {
                return "";
            }

            var endereco = "";

            if (!OEndereco.logradouro.isEmpty()) {
                endereco = String.Concat(endereco, OEndereco.logradouro, ", ");
            }

            if (!OEndereco.numero.isEmpty()) {

                endereco = String.Concat(endereco, OEndereco.numero);

                if (OEndereco.complemento.isEmpty()) {
                    endereco = String.Concat(endereco, ", ");
                }
                
            }

            if (!OEndereco.complemento.isEmpty()) {
                endereco = String.Concat(endereco, " ", OEndereco.complemento, " - ");
            }

            if (!OEndereco.bairro.isEmpty()) {
                endereco = String.Concat(endereco, OEndereco.bairro, ", ");
            }

            if (!OEndereco.cep.isEmpty()) {
                endereco = String.Concat(endereco, UtilString.formatCEP(OEndereco.cep), ", ");
            }

            if (OEndereco.Cidade?.nome.isEmpty() == false) {
                endereco = String.Concat(endereco, OEndereco.Cidade?.nome, " - ");
            }

            if (OEndereco.Cidade?.Estado?.sigla.isEmpty() == false) {
                endereco = String.Concat(endereco, OEndereco.Cidade?.Estado?.sigla, " ");
            }

            return endereco;

        }
        
    }
}
