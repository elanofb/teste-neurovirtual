using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using Excel;

namespace WEB.Areas.Financeiro.Controllers {

    public class ImportacaoController : Controller {

        public ActionResult importar() {
            return View();
        }

        public ActionResult importarDespesaAtualizar() {
            return View();
        }

        public ActionResult importarReceita() {
            return View();
        }

        [HttpPost]
        public ActionResult importar(HttpPostedFileBase arquivoExcel) {

            var lista = new List<ImportacaoFinanceiroDTO>();

            string extensao = UTIL.Upload.UploadConfig.getExtension(arquivoExcel);
            string pathExcelTemporario = Path.Combine(UtilConfig.pathAbsTempFiles, string.Concat(UtilString.onlyNumber(DateTime.Now.ToString()), extensao));
            arquivoExcel.SaveAs(pathExcelTemporario);

            using (FileStream stream = System.IO.File.Open(pathExcelTemporario, FileMode.Open, FileAccess.Read)) {

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                foreach (DataTable table in result.Tables) {
                    for (int i = 0; i < table.Rows.Count; i++) {

                        var OImportacao = new ImportacaoFinanceiroDTO();

                        OImportacao.ano = getCampo(table, i, 0);
                        OImportacao.mes = getCampo(table, i, 1);
                        OImportacao.dia = getCampo(table, i, 2);
                        OImportacao.flagFixa = getCampo(table, i, 3);
                        OImportacao.descricao = getCampo(table, i, 4);
                        OImportacao.centroCusto = getCampo(table, i, 5);
                        //  OImportacao.CentroCusto = getCampo(table, i, 6);
                        OImportacao.categoria = getCampo(table, i, 7);
                        OImportacao.tipoCategoria = getCampo(table, i, 8);
                        OImportacao.detalheCategoria = getCampo(table, i, 9);
                        OImportacao.qtdeParcela = getCampo(table, i, 10);
                        OImportacao.formaPagamento = getCampo(table, i, 11);
                        OImportacao.descricaoFormaPagamento = getCampo(table, i, 12);
                        OImportacao.valor = getCampo(table, i, 13);

                        lista.Add(OImportacao);
                    }
                }
            }

            if (lista.Count > 0) {
                foreach (var item in lista) {
                    if (!String.IsNullOrEmpty(item.ano)) {
                        var OTituloDespesaBL = new ContasAPagarBL();

                        var dtPagamento = new DateTime(UtilNumber.toInt32(item.ano), UtilNumber.toInt32(item.mes), UtilNumber.toInt32(item.dia));
                        var idCategoria = this.getCategoria(item.categoria);
                        var idTipoCategoria = this.getTipoCategoria(item.tipoCategoria);
                        var idDetalheTipoCategoria = this.getDetalheCategoria(item.detalheCategoria);
                        var idCentroCusto = this.getCentroCusto(item.centroCusto);
                        var idFormaPagamento = this.getFormaPagamento(item.formaPagamento);

                        TituloDespesa OTituloDespesa = new TituloDespesa();
                        OTituloDespesa.descricao = item.descricao;
                        OTituloDespesa.idDespesa = 0;
                        OTituloDespesa.idCategoria = idCategoria;
                        OTituloDespesa.idTipoCategoria = idTipoCategoria;
                        OTituloDespesa.idDetalheTipoCategoria = idDetalheTipoCategoria;
                        OTituloDespesa.idCentroCusto = idCentroCusto;
                        OTituloDespesa.idPeriodoRepeticao = 1;
                        OTituloDespesa.idAgrupador = 0;
                        OTituloDespesa.nroDocumento = "";
                        OTituloDespesa.qtdeRepeticao = 4;
                        OTituloDespesa.valorTotal = Convert.ToDecimal(item.valor);
                        OTituloDespesa.qtdeRepeticao = 1;
                        OTituloDespesa.dtQuitacao = dtPagamento;

                        TituloDespesaPagamento OTituloDespesaPagamento = new TituloDespesaPagamento();
                        OTituloDespesaPagamento.dtPagamento = dtPagamento;
                        OTituloDespesaPagamento.idFormaPagamento = idFormaPagamento;
                        //OTituloDespesaPagamento.descricao = OTituloDespesa.descricao;
                        OTituloDespesaPagamento.descParcela = item.descricaoFormaPagamento;
                        OTituloDespesaPagamento.dtVencimento = dtPagamento;
                        OTituloDespesaPagamento.flagPago = "S";
                        OTituloDespesaPagamento.valorOriginal = OTituloDespesa.valorTotal.Value;
                        OTituloDespesaPagamento.valorPago = OTituloDespesa.valorTotal.Value;

                        OTituloDespesa.listaTituloDespesaPagamento = new List<TituloDespesaPagamento>();
                        OTituloDespesa.listaTituloDespesaPagamento.Add(OTituloDespesaPagamento);

                        //if (!OTituloDespesaBL.exists(x => x.dtQuitacao == OTituloDespesa.dtQuitacao
                        //                                  && x.descricao == OTituloDespesa.descricao
                        //                                  && x.valorTotal == OTituloDespesa.valorTotal))
                        //{
                        // OTituloDespesaBL.salvar(OTituloDespesa);
                        //}                        
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult importarDespesaAtualizar(HttpPostedFileBase arquivoExcel) {

            var lista = new List<ImportacaoFinanceiroDTO>();

            string extensao = UTIL.Upload.UploadConfig.getExtension(arquivoExcel);
            string pathExcelTemporario = Path.Combine(UtilConfig.pathAbsTempFiles, string.Concat(UtilString.onlyNumber(DateTime.Now.ToString()), extensao));
            arquivoExcel.SaveAs(pathExcelTemporario);

            using (FileStream stream = System.IO.File.Open(pathExcelTemporario, FileMode.Open, FileAccess.Read)) {

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                foreach (DataTable table in result.Tables) {
                    for (int i = 0; i < table.Rows.Count; i++) {

                        var OImportacao = new ImportacaoFinanceiroDTO();

                        OImportacao.ano = getCampo(table, i, 0);
                        OImportacao.mes = getCampo(table, i, 1);
                        OImportacao.dia = getCampo(table, i, 2);
                        OImportacao.flagFixa = getCampo(table, i, 3);
                        OImportacao.descricao = getCampo(table, i, 4);
                        OImportacao.centroCusto = getCampo(table, i, 5);
                        //OImportacao.CentroCusto = getCampo(table, i, 6);
                        OImportacao.categoria = getCampo(table, i, 7);
                        OImportacao.tipoCategoria = getCampo(table, i, 8);
                        OImportacao.detalheCategoria = getCampo(table, i, 9);
                        OImportacao.qtdeParcela = getCampo(table, i, 10);
                        OImportacao.formaPagamento = getCampo(table, i, 11);
                        OImportacao.descricaoFormaPagamento = getCampo(table, i, 12);
                        OImportacao.valor = getCampo(table, i, 13);

                        lista.Add(OImportacao);
                    }
                }
            }

            if (lista.Count > 0) {
                foreach (var item in lista) {
                    if (!String.IsNullOrEmpty(item.ano) && item.flagFixa != "V") {
                        var OTituloDespesaBL = new ContasAPagarBL();

                        var idCentroCusto = 1;//this.getCentroCusto(item.CentroCusto);

                        var valorTotal = Decimal.Round(UtilNumber.toDecimal(item.valor), 2);

                        var listOTituloDespesa = OTituloDespesaBL.listar("").Where(x => x.idCentroCusto == idCentroCusto && x.descricao == item.descricao && x.valorTotal == valorTotal).ToList();

                        if (listOTituloDespesa.Count > 0) {
                            foreach (var OTituloDespesa in listOTituloDespesa) {
                                OTituloDespesaBL.atualizar(OTituloDespesa.id, "flagFixa", (item.flagFixa != "V") ? "S" : "N");
                            }
                        }
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult importarReceita(HttpPostedFileBase arquivoExcel) {
            var lista = new List<ImportacaoFinanceiroReceitaDTO>();

            string extensao = UTIL.Upload.UploadConfig.getExtension(arquivoExcel);
            string pathExcelTemporario = Path.Combine(UtilConfig.pathAbsTempFiles, string.Concat(UtilString.onlyNumber(DateTime.Now.ToString()), extensao));
            arquivoExcel.SaveAs(pathExcelTemporario);

            using (FileStream stream = System.IO.File.Open(pathExcelTemporario, FileMode.Open, FileAccess.Read)) {

                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                foreach (DataTable table in result.Tables) {
                    for (int i = 0; i < table.Rows.Count; i++) {

                        var OImportacao = new ImportacaoFinanceiroReceitaDTO();

                        OImportacao.flagFoiPago = getCampo(table, i, 0);
                        OImportacao.dtRecebimento = getCampo(table, i, 1);
                        OImportacao.descricao = getCampo(table, i, 2);
                        OImportacao.dtVencimento = getCampo(table, i, 3);
                        OImportacao.valor = getCampo(table, i, 4);
                        OImportacao.centroCusto = getCampo(table, i, 5);
                        //  OImportacao.CentroCusto = getCampo(table, i, 6);
                        OImportacao.categoria = getCampo(table, i, 7);
                        OImportacao.tipoCategoria = getCampo(table, i, 8);
                        OImportacao.detalheCategoria = getCampo(table, i, 9);

                        lista.Add(OImportacao);
                    }
                }
            }

            if (lista.Count > 0) {
                foreach (var item in lista) {
                    if (!String.IsNullOrEmpty(item.dtRecebimento)) {
                        var OTituloDespesaBL = new ContasAReceberBL();

                        var itemDataRecebimento = item.dtRecebimento.Replace(".", "/");
                        var itemDataVencimento = item.dtVencimento.Replace(".", "/");

                        var dtPagamento = UtilDate.cast(itemDataRecebimento);
                        var dtVencimento = UtilDate.cast(itemDataVencimento);
                        //var idCentroCusto = this.getCentroCusto(item.CentroCusto);
                        var idCategoria = this.getCategoria(item.categoria);
                        var idTipoCategoria = this.getTipoCategoria(item.tipoCategoria);
                        var idDetalheTipoCategoria = this.getDetalheCategoria(item.detalheCategoria);
                        var idCentroCusto = this.getCentroCusto(item.centroCusto);

                        var OTituloReceita = new TituloReceita();
                        OTituloReceita.descricao = item.descricao;
                        OTituloReceita.idCentroCusto = idCentroCusto;
                        OTituloReceita.idReceita = 0;
                        OTituloReceita.idCategoria = idCategoria;
                        OTituloReceita.idCentroCusto = idCentroCusto;
                        OTituloReceita.idPeriodoRepeticao = 1;
                        OTituloReceita.nroDocumento = "";
                        OTituloReceita.qtdeRepeticao = 4;
                        OTituloReceita.valorTotal = Convert.ToDecimal(item.valor);
                        OTituloReceita.valorDesconto = 0;
                        OTituloReceita.qtdeRepeticao = 1;

                        var OTituloReceitaPagamento = new TituloReceitaPagamento();
                        //                        OTituloReceitaPagamento.descricao = item.descricao;
                        OTituloReceitaPagamento.dtPagamento = dtPagamento;
                        //                        OTituloReceitaPagamento.descParcela = OTituloReceita.descricao;
                        OTituloReceitaPagamento.dtVencimento = dtVencimento;
                        //                        OTituloReceitaPagamento.flagPago = (item.flagFoiPago == "Sim") ? "S" : "N";
                        OTituloReceitaPagamento.valorOriginal = OTituloReceita.valorTotal.Value;
                        OTituloReceitaPagamento.valorRecebido = OTituloReceita.valorTotal.Value;

                        OTituloReceita.listaTituloReceitaPagamento = new List<TituloReceitaPagamento>();
                        OTituloReceita.listaTituloReceitaPagamento.Add(OTituloReceitaPagamento);

                        OTituloDespesaBL.inserir(OTituloReceita);
                    }
                }
            }

            return View();
        }


        private string getCampo(DataTable table, int indice, int posicao) {
            try {
                return Convert.ToString(table.Rows[indice][posicao]);
            } catch (Exception) {
                return "";
            }
        }

        private int getCentroCusto(string CentroCusto) {

            var idCentroCusto = 0;

            CentroCusto = UtilString.onlyAlphaNumber(UtilString.cleanAccents(CentroCusto)).ToUpper();

            var listaCentroCusto = new CentroCustoBL().listar("", true).ToList();

            listaCentroCusto.ForEach(item => {
                item.descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(item.descricao)).ToUpper();
            });

            var OCentroCusto = listaCentroCusto.FirstOrDefault(x => x.descricao == CentroCusto);

            if (OCentroCusto != null) {
                idCentroCusto = OCentroCusto.id;
            }

            return idCentroCusto;
        }

        private int getCategoria(string descricao) {
            var id = 0;
            descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(descricao)).ToUpper();

            var lista = new CategoriaTituloBL().listar(0, "", "S").ToList();
            lista.ForEach(item => {
                item.descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(item.descricao)).ToUpper();
            });

            var Objeto = lista.Where(x => x.descricao == descricao).FirstOrDefault();

            if (Objeto != null) id = Objeto.id;

            return id;
        }

        private int getTipoCategoria(string descricao) {
            var id = 0;
            descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(descricao)).ToUpper();

            var lista = new TipoCategoriaTituloBL().listar(0, 0, "", "S").ToList();
            lista.ForEach(item => {
                item.descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(item.descricao)).ToUpper();
            });

            var Objeto = lista.Where(x => x.descricao == descricao).ToList().FirstOrDefault();

            if (Objeto != null) id = Objeto.id;

            return id;
        }

        private int getDetalheCategoria(string descricao) {
            var id = 0;
            descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(descricao)).ToUpper();

            var lista = new DetalheTipoCategoriaTituloBL().listar(0, 0, 0, "", "S").ToList();
            lista.ForEach(item => {
                item.descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(item.descricao)).ToUpper();
            });

            var Objeto = lista.Where(x => x.descricao == descricao).FirstOrDefault();

            if (Objeto != null) id = Objeto.id;

            return id;
        }



        private byte? getFormaPagamento(string descricao) {
            byte id = 0;
            descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(descricao)).ToUpper();

            var lista = new FormaPagamentoBL().listar("", "S").ToList();
            lista.ForEach(item => {
                item.descricao = UtilString.onlyAlphaNumber(UtilString.cleanAccents(item.descricao)).ToUpper();
            });

            var Objeto = lista.Where(x => x.descricao == descricao).FirstOrDefault();

            if (Objeto != null) id = Objeto.id;

            return id;
        }
    }
}