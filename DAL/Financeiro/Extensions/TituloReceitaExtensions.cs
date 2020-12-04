using System;
using System.Text;
using System.Linq;

namespace DAL.Financeiro {

    public static class TituloReceitaExtensions {

        //
        public static decimal valorLiquido(this TituloReceita OTitulo) {

            decimal valorLiquido = Decimal.Subtract(UtilNumber.toDecimal(OTitulo.valorTotal), UtilNumber.toDecimal(OTitulo.valorDesconto));

            return valorLiquido;
        }

        //
        public static decimal valorTotalComDesconto(this TituloReceita OTitulo) {

            decimal valorTotal = new decimal(0);

            if (OTitulo == null) {
                return valorTotal;
            }

            var listaPagamentos = OTitulo.retornarListaPagamentos();

            decimal valorTotalJuros = listaPagamentos.Sum(x => x.valorJuros ?? 0);

            decimal valorTotalDescontoAvulso = listaPagamentos.Sum(x => x.valorDesconto ?? 0);

            decimal valorTotalDescontoAntecipacao =listaPagamentos.Sum(x => x.valorDescontoAntecipacao ?? 0);

            decimal valorTotalDescontoCupons = listaPagamentos.Sum(x => x.valorDescontoCupom ?? 0);

            decimal valorTotalDesconto = decimal.Add(decimal.Add(valorTotalDescontoAvulso, valorTotalDescontoAntecipacao), valorTotalDescontoCupons);

            valorTotal = decimal.Add(OTitulo.valorTotal.toDecimal(), valorTotalJuros);

            valorTotal = decimal.Subtract(valorTotal, valorTotalDesconto);

            return valorTotal;
        }

        //
        public static decimal valorDesconto(this TituloReceita OTitulo) {
            var valorOutrasTarifas = OTitulo.listaTituloReceitaPagamento.Sum(x => x.valorOutrasTarifas);
            var valorTarifasBancarias = OTitulo.listaTituloReceitaPagamento.Sum(x => x.valorTarifasBancarias);
            var valorTarifasTransacao = OTitulo.listaTituloReceitaPagamento.Sum(x => x.valorTarifasTransacao);

            decimal valorDesconto = Decimal.Add(UtilNumber.toDecimal(valorTarifasTransacao), Decimal.Add(UtilNumber.toDecimal(valorOutrasTarifas), UtilNumber.toDecimal(valorTarifasBancarias)));

            return valorDesconto;
        }

        //
        public static string descricaoTitulo(this TituloReceita OTitulo) {

            if (!String.IsNullOrEmpty(OTitulo.descricao)) {
                return OTitulo.descricao;
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.SOLICITACAO) {
                return "Solicitação";
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.INSCRICAO_EVENTO) {
                return "Inscrição para evento.";
            }

            if (OTitulo.idTipoReceita == TipoReceitaConst.PEDIDO) {
                return String.Format("Pedido Nº {0}", OTitulo.idReceita);
            }

            return string.Empty;
        }

        //
        public static string descricaoTitulo(this TituloReceitaPagamentoVW Opagamento) {

            if (!String.IsNullOrEmpty(Opagamento.descricaoTitulo)) {
                return Opagamento.descricaoTitulo;
            }

            if (Opagamento.idTipoReceita == TipoReceitaConst.SOLICITACAO) {
                return "Solicitação";
            }

            if (Opagamento.idTipoReceita == TipoReceitaConst.INSCRICAO_EVENTO) {
                return "Inscrição para evento.";
            }

            if (Opagamento.idTipoReceita == TipoReceitaConst.PEDIDO) {
                return String.Format("Pedido Nº {0}", Opagamento.idReceita);
            }

            return string.Empty;
        }

        //
        public static string enderecoCompletoRecibo(this TituloReceita OTitulo) {

            if (OTitulo == null) {
                return "";
            }

            StringBuilder TextoEndereco = new StringBuilder();

            if (!String.IsNullOrEmpty(OTitulo.cepRecibo)) {
                TextoEndereco.Append(OTitulo.cepRecibo).Append(" - ");
            }

            TextoEndereco.Append(OTitulo.logradouroRecibo);

            if (!String.IsNullOrEmpty(OTitulo.numeroRecibo)) {
                TextoEndereco.Append(", ").Append(OTitulo.numeroRecibo);
            }

            if (!String.IsNullOrEmpty(OTitulo.complementoRecibo)) {
                TextoEndereco.Append(", ").Append(OTitulo.complementoRecibo);
            }

            if (!String.IsNullOrEmpty(OTitulo.bairroRecibo)) {
                TextoEndereco.Append(" - ").Append(OTitulo.bairroRecibo);
            }

            if (!String.IsNullOrEmpty(OTitulo.nomeCidadeRecibo)) {
                TextoEndereco.Append(" - ").Append(OTitulo.nomeCidadeRecibo);
            }

            return TextoEndereco.ToString();
        }

        public static string descricaoCategoriaPessoa(this TituloReceita OTitulo) {

            if (OTitulo.flagCategoriaPessoa == "AS") {
                return "Associado";
            }

            if (OTitulo.flagCategoriaPessoa == "FO") {
                return "Fornecedor";
            }

            if (OTitulo.flagCategoriaPessoa == "FU") {
                return "Funcionário";
            }

            if (OTitulo.flagCategoriaPessoa == "PA") {
                return "Patrocinador";
            }

            return "";
        }

         //
        public static string classeLabelStatus(this TituloReceitaPagamentoVW Opagamento) {

            if(Opagamento.idStatusPagamento == StatusPagamentoConst.PAGO) {
                return "bg-green";
            }

            if(Opagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO ||
               Opagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO) {

                return "bg-red";
            }
            
            return "bg-yellow";

        }

        public static string numeroTransacao(this TituloReceita OTituloReceita){

            string primeiraParte = OTituloReceita.idOrganizacao.ToString().PadLeft(5, '0');

            string segundaParte = OTituloReceita.id.ToString().PadLeft(10, '0');

            string numero = $"{primeiraParte}/{segundaParte}";
            
            return numero;

        }

        /// <summary>
        /// Verificar se existe boleto bancario vinculado ao titulo de receita
        /// </summary>
        public static bool existeBoletoPendente(this TituloReceita OTituloReceita, int? idTituloReceitaPagamento = 0){

            var listaPagamentos = OTituloReceita.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

            if (idTituloReceitaPagamento > 0) {

                listaPagamentos = listaPagamentos.Where(x => x.id == idTituloReceitaPagamento).ToList();

            }

            var OBoleto = listaPagamentos.FirstOrDefault( x => x.idMeioPagamento == MeioPagamentoConst.BOLETO_BANCARIO && !string.IsNullOrEmpty(x.boletoUrl) && x.dtPagamento == null);

            return OBoleto != null;
        }


    }
}