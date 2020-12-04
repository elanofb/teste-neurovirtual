using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro.Entities;

namespace DAL.Financeiro {

    public static class PlanoContaExtensions {

        //
        public static string descricaoCentroCusto(this CentroCusto OItem) {

            if (OItem == null) {
                return "";
            }

            return (!OItem.codigoFiscal.isEmpty() ? String.Concat(OItem.codigoFiscal, " - ", OItem.descricao) : OItem.descricao);
        }
        
        public static string descricaoMacroConta(this MacroConta OItem) {

            if (OItem == null) {
                return "";
            }

            return (!OItem.codigoFiscal.isEmpty() ? String.Concat(OItem.codigoFiscal, " - ", OItem.descricao) : OItem.descricao);
        }
        
        /// <summary>
        /// Montagem da descricao da subconta
        /// </summary>
        public static string descricaoSubConta(this CategoriaTitulo OItem, bool flagCategoriaPai = true) {

            if (OItem == null) {
                return "";
            }

            
            string descricao = string.Empty;
            string codigoFiscal = string.Empty;

            if (OItem.MacroConta != null && !OItem.MacroConta.codigoFiscal.isEmpty()) {

                codigoFiscal = $"{OItem.MacroConta.codigoFiscal}";
            }

            if (OItem.CategoriaPai != null && !OItem.CategoriaPai.codigoFiscal.isEmpty()) {
                
                codigoFiscal = $"{codigoFiscal}.{OItem.CategoriaPai.codigoFiscal}";
                
                descricao = $"{codigoFiscal}";
            }

            if (OItem.CategoriaPai != null && !OItem.CategoriaPai.descricao.isEmpty()) {
            
                descricao = $"{descricao} {OItem.CategoriaPai.descricao} > ";
            }


            if (!OItem.codigoFiscal.isEmpty()) {

                codigoFiscal = $"{codigoFiscal}.{OItem.codigoFiscal}";
                
            }

            descricao = $"{(flagCategoriaPai? descricao: "")} {codigoFiscal} {OItem.descricao}";

            return descricao;
        }       
    }
}