using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using DAL.Financeiro.Entities;

namespace DAL.Contribuicoes {

    public static class ContribuicaoExtensions {

        public static List<VigenciaDTO> retornarVigencia(this Contribuicao Item, List<Contribuicao> listaContribuicao) {

            DateTime dtVigencia = new DateTime(Convert.ToInt32(Item.anoInicioVigencia), Convert.ToInt32(Item.mesInicioVigencia), 1, 00, 00, 00);

            if (dtVigencia > DateTime.Today) {
                return new List<VigenciaDTO>();
            }

            var ProximaContribuicao = listaContribuicao
                .Where(x => x.dtInicioVigencia > Item.dtInicioVigencia)
                .OrderBy(x => x.dtInicioVigencia)
                .FirstOrDefault();

            int ano = ProximaContribuicao != null ? Convert.ToInt32(ProximaContribuicao.anoInicioVigencia) : DateTime.Today.Year;
            int mes = ProximaContribuicao != null ? Convert.ToInt32(ProximaContribuicao.mesInicioVigencia) : 12;

            DateTime startDate = new DateTime(Convert.ToInt32(Item.anoInicioVigencia), Convert.ToInt32(Item.mesInicioVigencia), 1, 00, 00, 00);
            DateTime endDate = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes), 23, 59, 59);

            //Acerta a contribuição já vencida
            //Acerta a contribuição quando houver outra
            if (!Item.flagVigente || ProximaContribuicao != null) {
                endDate = endDate.AddMonths(-1);
            }

            Item.listaVigencia = Enumerable.Range(0, Int32.MaxValue)
                                .Select(e => startDate.AddMonths(e))
                                .TakeWhile(e => e < endDate)
                                .Select(e => new VigenciaDTO { ano = e.Year, mes = e.Month, descricaoMes = e.ToString("MMMM") }).ToList();

            return Item.listaVigencia;
        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static ContribuicaoVencimento retornarProximoVencimento(this Contribuicao Item, DateTime? dtVencimentoInformado = null) {

            var listaContribuicaoVencimento = Item.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            if (Item.flagVencimentoVariado()) {

                var ONovoVencimento = listaContribuicaoVencimento.FirstOrDefault() ?? new ContribuicaoVencimento();

                ONovoVencimento.dtVencimento = dtVencimentoInformado;

                if (dtVencimentoInformado.HasValue) {

                    ONovoVencimento.dtInicioVigencia = dtVencimentoInformado;

                    ONovoVencimento.dtFimVigencia = dtVencimentoInformado.Value.AddYears(1);

                    ONovoVencimento.mesVencimento = dtVencimentoInformado.Value.Month.toByte();

                    ONovoVencimento.diaVencimento = dtVencimentoInformado.Value.Day.toByte();
                }

                return ONovoVencimento;
            }


            var listaVencimentos = new List<ContribuicaoVencimento>();

            int anoVigencia = DateTime.Today.Year;

            if (dtVencimentoInformado.HasValue) {

                anoVigencia = dtVencimentoInformado.Value.Year;

            }

            listaContribuicaoVencimento.ForEach(v => {

                var dtVencimento = new DateTime(anoVigencia, UtilNumber.toInt32(v.mesVencimento), UtilNumber.toInt32(v.diaVencimento));

                var dtInicioVigencia = new DateTime(anoVigencia, UtilNumber.toInt32(v.mesInicioVigencia), UtilNumber.toInt32(v.diaInicioVigencia));

                var dtFimVigencia = new DateTime(anoVigencia, UtilNumber.toInt32(v.mesFimVigencia), UtilNumber.toInt32(v.diaFimVigencia));

                if (dtInicioVigencia > dtFimVigencia) {

                    dtFimVigencia = dtFimVigencia.AddYears(1);

                    dtVencimento = dtVencimento.AddYears(1);
                }

                var ItemVencimento = new ContribuicaoVencimento {
                    id = v.id, dtVencimento = dtVencimento, dtInicioVigencia = dtInicioVigencia,
                    dtFimVigencia = dtFimVigencia, mesVencimento = v.mesVencimento, diaVencimento = v.diaVencimento };

                listaVencimentos.Add(ItemVencimento);

            });

            if (dtVencimentoInformado.HasValue) {

                var ORetornoVencimento = listaVencimentos.FirstOrDefault(x => x.dtVencimento.GetValueOrDefault().Date == dtVencimentoInformado.GetValueOrDefault().Date) ?? new ContribuicaoVencimento();

                if (ORetornoVencimento.id > 0) {

                    return ORetornoVencimento;

                }
            }

            var dtHoje = DateTimeFactory.Today;

            var OVencimento = listaVencimentos.FirstOrDefault(x => x.dtInicioVigencia <= dtHoje && x.dtFimVigencia >= dtHoje) ?? new ContribuicaoVencimento();

            OVencimento.dtVencimento = OVencimento.dtVencimento ?? DateTime.MinValue;

            return OVencimento;
        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static ContribuicaoVencimento retornarProximoVencimento(this Contribuicao Item, DateTime dtVencimento) {

            byte diaVencimento = (byte)dtVencimento.Day;

            byte mesVencimento = (byte)dtVencimento.Month;

            var OVencimento = Item.listaContribuicaoVencimento.FirstOrDefault(x => x.dtExclusao == null && x.diaVencimento == diaVencimento && x.mesVencimento == mesVencimento);

            if (OVencimento == null) {
                return null;
            }

            OVencimento.dtVencimento = dtVencimento;

            OVencimento.dtInicioVigencia = new DateTime(dtVencimento.Year, UtilNumber.toInt32(OVencimento.mesInicioVigencia), UtilNumber.toInt32(OVencimento.diaInicioVigencia));

            OVencimento.dtFimVigencia = new DateTime(dtVencimento.Year, UtilNumber.toInt32(OVencimento.mesFimVigencia), UtilNumber.toInt32(OVencimento.diaFimVigencia));

            if (OVencimento.dtInicioVigencia > OVencimento.dtFimVigencia) {

                OVencimento.dtFimVigencia = OVencimento.dtFimVigencia.Value.AddYears(1);

            }

            return OVencimento;
        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static List<ContribuicaoVencimento> retornarListaVencimento(this Contribuicao Item) {

            if (Item.idTipoVencimento != TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {
                return new List<ContribuicaoVencimento>();
            }

            var listaContribuicaoVencimento = Item.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            listaContribuicaoVencimento.ForEach(v => {

                var dtVencimento = new DateTime(DateTime.Today.Year, UtilNumber.toInt32(v.mesVencimento), UtilNumber.toInt32(v.diaVencimento));

                var dtInicioVigencia = new DateTime(DateTime.Today.Year, UtilNumber.toInt32(v.mesInicioVigencia), UtilNumber.toInt32(v.diaInicioVigencia));

                var dtFimVigencia = new DateTime(DateTime.Today.Year, UtilNumber.toInt32(v.mesFimVigencia), UtilNumber.toInt32(v.diaFimVigencia));

                if (dtInicioVigencia > dtFimVigencia) {

                    dtFimVigencia = dtFimVigencia.AddYears(1);

                    dtVencimento = dtVencimento.AddYears(1);
                }

                v.dtVencimento = dtVencimento;

                v.dtInicioVigencia = dtInicioVigencia;

                v.dtFimVigencia = dtFimVigencia;

            });

            return listaContribuicaoVencimento;

        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static ContribuicaoTabelaPreco retornarTabelaVigente(this Contribuicao Item) {

            var Retorno = new ContribuicaoTabelaPreco();

            if (Item == null) {
                return Retorno;
            }

            Item.listaTabelaPreco = Item.listaTabelaPreco ?? new List<ContribuicaoTabelaPreco>();

            Retorno = Item.listaTabelaPreco.Where(x => x.flagExcluido == false && x.dtInicioVigencia <= DateTime.Today)
                                            .OrderByDescending(x => x.dtInicioVigencia)
                                            .FirstOrDefault();

            Retorno = Retorno ?? new ContribuicaoTabelaPreco();

            return Retorno;
        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static ContribuicaoPreco retornarPreco(this ContribuicaoTabelaPreco TabelaPreco, int idTipoAssociado) {

            var Retorno = new ContribuicaoPreco();

            if (TabelaPreco == null) {
                return Retorno;
            }

            Retorno = TabelaPreco.listaPrecos.FirstOrDefault(x => x.idTipoAssociado == idTipoAssociado && x.flagExcluido == "N");

            Retorno = Retorno ?? new ContribuicaoPreco();

            return Retorno;
        }


        //Extension para pegar o preço a partir do tipo do associado
        //Verificar se a cobranca deve ser feita pro-rata e calcular o valor
        public static ContribuicaoPreco retornarPrecoAtual(this ContribuicaoTabelaPreco TabelaPreco, int idTipoAssociado, DateTime? dtCobranca) {

            var Preco = new ContribuicaoPreco();

            if (TabelaPreco == null) {
                return Preco;
            }

            Preco = TabelaPreco.listaPrecos.FirstOrDefault(x => x.idTipoAssociado == idTipoAssociado && x.flagExcluido == "N");

            Preco = Preco ?? new ContribuicaoPreco();

            var OContribuicao = TabelaPreco.Contribuicao;

            var flagCobrancaProRata = OContribuicao.flagCobrancaProRata == true;

            var OVencimento = OContribuicao.retornarProximoVencimento();

            var dtFimVigencia = OVencimento.dtFimVigencia;

            if (!dtFimVigencia.HasValue || !dtCobranca.HasValue) {

                return Preco;

            }

            if (!flagCobrancaProRata || OContribuicao.idTipoVencimento != TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO && Preco.id == 0) {

                return Preco;

            }

            int qtdeDiasRestante = dtFimVigencia.Value.Date.Subtract(dtCobranca.Value.Date).Days;

            int qtdeDiasTotal = OContribuicao.PeriodoContribuicao.qtdeDias;

            decimal valorPorDia = decimal.Divide(UtilNumber.toDecimal(Preco.valorFinal), UtilNumber.toDecimal(qtdeDiasTotal));

            decimal valorProRata = decimal.Multiply(valorPorDia, UtilNumber.toDecimal(qtdeDiasRestante));

            Preco.valorFinal = valorProRata;

            return Preco;
        }

        /// <summary>
        /// Verificar se existe tabela de desconto por antecipacao de pagamentos e retornar a tabela 
        /// </summary>
        public static List<TituloReceitaDescontoAntecipacao> retornarListaDescontos(this ContribuicaoTabelaPreco OTabelaPreco, int idTipoAssociado, DateTime dtVencimento) {

            var listaDescontos = new List<TituloReceitaDescontoAntecipacao>();

            if (OTabelaPreco == null) {
                return listaDescontos;
            }

            var Preco = OTabelaPreco.listaPrecos.FirstOrDefault(x => x.idTipoAssociado == idTipoAssociado && x.flagExcluido == "N");


            if (Preco == null) {
                return listaDescontos;
            }

            var listaPrecoDescontos = Preco.listaDesconto.Where(x => x.dtExclusao == null).ToList();

            if (!listaPrecoDescontos.Any()) {

                return listaDescontos;

            }

            foreach (var OPrecoDesconto in listaPrecoDescontos) {

                var ItemDesconto = new TituloReceitaDescontoAntecipacao();

                ItemDesconto.dtLimiteDesconto = dtVencimento.AddDays(-Math.Abs(OPrecoDesconto.qtdeDiasAntecipacao));

                ItemDesconto.valor = OPrecoDesconto.valorDesconto;

                listaDescontos.Add(ItemDesconto);
            }


            return listaDescontos;
        }

        //Extension para pegar a tabela de preços vigente para a contribuição
        public static bool flagVencimentoVariado(this Contribuicao Item) {

            if (Item.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO || Item.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELO_ULTIMO_PAGAMENTO) {
                return true;
            }

            return false;
        }
    }
}