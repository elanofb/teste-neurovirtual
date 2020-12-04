using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ContribuicoesPainel.ViewModels;
using WEB.Helpers;
using WEB.Areas.AssociadosContribuicoes.ViewModels;

namespace WEB.Areas.ContribuicoesPainel.Controllers {

    [OrganizacaoFilter]
    public class PainelCobrancaController : Controller {

        //Atributos

        //Propriedades

        // GET: ContribuicoesPainel/PainelCobranca
        public ActionResult Index() {

            int idContribuicao = UtilRequest.getInt32("idContribuicao");

            int ano = UtilRequest.getInt32("ano");

            bool flagBusca = !string.IsNullOrEmpty(UtilRequest.getString("search"));

            string tipoSaida = UtilRequest.getString("tipoSaida");

            var ViewModel = new PainelCobrancaVM();

            if (!flagBusca) {
                return View(ViewModel);
            }

            if (idContribuicao == 0 ) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe qual plano/contribuição deseja visualizar");

                return View(ViewModel);
            }

            if (ano == 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Informe de que o ano deseja ver as informações.");

                return View(ViewModel);
            }

            ViewModel.carregarContribuicao(idContribuicao); 

            if (ViewModel.Contribuicao.idOrganizacao !=  User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A contribuição informada não foi localizada.");

                return RedirectToAction("index");
            }

            if (ViewModel.Contribuicao.dtValidade.HasValue && ViewModel.Contribuicao.dtValidade.Value.Year < ano) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "A validade da contribuição é inferior ao ano informado.");

                return RedirectToAction("index");
            }

            ViewModel.carregarDadosContribuicao(idContribuicao, null);

            var listaBoletos = ViewModel.carregarBoletos().Select(x => new { x.idTituloReceitaPagamento, x.idAssociadoContribuicao, x.boletoUrl}).ToList();

            
            ViewModel.qtdeBoletosGerados = listaBoletos.Count;

            foreach (var Item in ViewModel.listaAssociadosPager){

                var OPgtoBoleto = listaBoletos.FirstOrDefault(x => x.idAssociadoContribuicao == Item.AssociadoContribuicao.id);

                Item.idTituloReceitaPagamento = OPgtoBoleto?.idTituloReceitaPagamento;

                Item.urlBoleto = OPgtoBoleto?.boletoUrl;

            }

            if (tipoSaida == "Excel"){
                this.baixarExcel(ViewModel.listagemFiltrada);
                return null;
            }

            return View(ViewModel);
        }

        //Exportacao do cadastro para formato EXCEL
        //Download do documento gerado
        public void baixarExcel(List<AssociadoContribuicaoItemLista> lista) {

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel(lista));

            foreach (var OItem in lista) {
                sw.WriteLine(this.gerarLinhaExcel(OItem, lista));
            }

            Response.ClearContent();

            var nomeArquivo = String.Concat("painel-cobrancas-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            Response.AddHeader("content-disposition", "attachment;filename=" + nomeArquivo);
            Response.ContentType = "text/csv; charset=ISO-8859-1";
            Response.Charset = "ISO-8859-1";
            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            Response.Write(sw.ToString());

            Response.End();

        }

        private string gerarCabecalhoExcel(List<AssociadoContribuicaoItemLista> lista) {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código Cobrança;")
                .Append("ID do Associado;")
                .Append("Número do Associado;")
                .Append("Nome do Associado;")
                .Append("Documento do Associado;")
                .Append("Status do Associado;")
                .Append("Situação Financeira;");

            var qtdTel = lista.Max(x => x.listTelefones.Count);
            var qtdMail = lista.Max(x => x.listEmails.Count);

            for (int i = 0; i < qtdTel; i++) {
                cabecalho.Append("Telefone "+(i+1)+";");
            }
            for (int i = 0; i < qtdMail; i++) {
                cabecalho.Append("E-mail " + (i + 1) + ";");
            }
            
            cabecalho.Append("Contribuição;")
                .Append("Período de Contribuição;")
                .Append("Tipo de Associado;")
                .Append("Valor Original;")
                .Append("Valor Atual;")
                .Append("Data de Vcto Original;")
                .Append("Data de Vcto Atual;")
                .Append("Data de início vigencia;")
                .Append("Data de fim vigencia;")
                .Append("Data de pgto;")
                .Append("Isento?;")
                .Append("Motivo Isenção;")
                .Append("Data de Cadastro;")
                .Append("Usuario de Cadastro;")
                .Append("Valor Total;")
                .Append("Qtde de Parcelas;")
                .Append("Valor Total Recebido;")
                .Append("Desconto de antecipação?;");                

            return cabecalho.ToString();

        }

        private string gerarLinhaExcel(AssociadoContribuicaoItemLista OContribuicao, List<AssociadoContribuicaoItemLista> lista) {

            StringBuilder linha = new StringBuilder();

            linha.Append(OContribuicao.AssociadoContribuicao.id).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.idAssociado).Append(";");
            linha.Append(OContribuicao.AssociadoExcel.nroAssociado).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.nomeAssociado).Append(";");
            linha.Append(UtilString.formatCPFCNPJ(OContribuicao.AssociadoExcel.nroDocumentoAssociado)).Append(";");
            linha.Append(OContribuicao.AssociadoExcel.statusAssociado == StatusConst.ativo ? "Ativo" : (OContribuicao.AssociadoExcel.statusAssociado == StatusConst.emAdmissao ? "Em Admissão" : "Desativado")).Append(";");
            linha.Append(OContribuicao.AssociadoExcel.situacaoFinanceira == "AD" ? "Adimplente" : "Inadimplente").Append(";");

            var qtdTel = lista.Max(x => x.listTelefones.Count);
            var qtdMail = lista.Max(x => x.listEmails.Count);

            var contTel = 0;
            foreach (var Telefone in OContribuicao.listTelefones) {
                contTel++;
                linha.Append(UtilString.formatPhone(Telefone.nroTelefone)).Append(";");
            }
            for (int i = contTel; i < qtdTel; i++) {
                linha.Append("").Append(";");
            }

            var contMail = 0;
            foreach (var Email in OContribuicao.listEmails) {
                contMail++;
                linha.Append(Email.email).Append(";");
            }
            for (int i = contMail; i < qtdMail; i++) {
                linha.Append("").Append(";");
            }
            
            linha.Append(OContribuicao.AssociadoContribuicao.descricaoContribuicao).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.descricaoPeriodoContribuicao).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.descricaoTipoAssociado).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.valorOriginal).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.valorAtual).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtVencimentoOriginal.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtVencimentoAtual.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtInicioVigencia.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtFimVigencia.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtPagamento.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.flagIsento == true ? "Sim" : "Não").Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.motivoIsencao).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.dtCadastro.exibirData()).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.nomeUsuarioCadastro).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.valorTotalTitulo).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.qtdeParcelas).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.valorTotalRecebido).Append(";");
            linha.Append(OContribuicao.AssociadoContribuicao.flagDescontoAntecipacao == true ? "Sim" : "Não").Append(";");            

            return linha.ToString();
        }
    }
}
 