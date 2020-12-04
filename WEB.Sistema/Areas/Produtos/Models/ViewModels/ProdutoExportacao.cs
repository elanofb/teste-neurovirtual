using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Produtos;
using WEB.Helpers;

namespace WEB.Areas.Associacoes.ViewModels {

    public class ProdutoExportacao {
        
        //Atributos

        // Propriedades

        //
        public ProdutoExportacao() {
            
        }
        
        //
        public void baixarExcel(List<Produto> listaProdutos) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaProdutos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-produtos-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();
                
            cabecalho.Append("Código Sistema;")
                .Append("Tipo de Produto/Serviço;")
                .Append("Produto Status;")                
                .Append("Nome do Produto;")                
                                
                .Append("Valor;")
                .Append("Pontos para Plano de Carreira;")
                .Append("Ganhos Diários;")
                .Append("Duração em Dias;")
                
                .Append("Rede de Afiliados?;")
                .Append("Teto Binário;")
                .Append("Plano Recomendado?;")                
                    
                .Append("Data de Cadastro;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Produto OProduto) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OProduto.id).Append(";")
                .Append(OProduto.TipoProduto?.descricao).Append(";")
                .Append(OProduto.ativo ?? false ? "Sim" : "Não").Append(";")                
                .Append(OProduto.nome).Append(";")                                
                .Append(OProduto.valor.ToString("C")).Append(";")                                                               
                
                .Append(OProduto.qtdePontosPlanoCarreira.toDecimal().ToString("C")).Append(";")
                .Append(OProduto.valorGanhoDiario.toDecimal().ToString("C")).Append(";")
                .Append(OProduto.qtdeDiasDuracao).Append(";")
                
                .Append(OProduto.flagRedeAfiliados == true ? "Sim" : "Não").Append(";")
                .Append(OProduto.qtdeMaximoBinario.toDecimal().ToString("C")).Append(";")
                .Append(OProduto.flagPlanoRecomendado == true ? "Sim" : "Não").Append(";")
                
                .Append(OProduto.dtCadastro.exibirData()).Append(";");

            return linha.ToString();
        }

    }
}