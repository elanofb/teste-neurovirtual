using System;


namespace DAL.ContasBancarias {

    public static class ContaBancariaExtensions {

        public static string descricaoTipoConta(this ContaBancaria OContaBancaria) {

            if (OContaBancaria.tipoConta == "C") {
                return "Conta Corrente";
            }

            if (OContaBancaria.tipoConta == "P") {
                return "Conta Poupança";
            }

            return string.Empty;
        }

    }
}