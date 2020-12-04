using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Notificacoes;

namespace WEB.Areas.PessoasDevices.ViewModels {

    public class GeradorCsvDispositivosRegistrados {
        
        //
        public GeradorCsvDispositivosRegistrados() {
            
        }
        
        //
        public void baixarExcel(List<PessoaDevice> listaDispositivos) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaDispositivos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatório-dispositivos-registrados", ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Identificador;")
                     .Append("Nome;")
                     .Append("Código do Aparelho;")
                     .Append("Plataforma;")
                     .Append("Versão;")
                     .Append("Data de Cadastro;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(PessoaDevice ODispositivo) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(ODispositivo.id).Append(";")
                 .Append(ODispositivo.Pessoa?.nome).Append(";")
                 .Append(ODispositivo.idDevice).Append(";")
                 .Append(ODispositivo.flagAndroid == true ? "Android" : (ODispositivo.flagIOS == true ? "IOS" : "Não Identificado")).Append(";")
                 .Append(ODispositivo.versao).Append(";")
                 .Append(ODispositivo.dtCadastro.exibirData()).Append(";");
                
            return linha.ToString();
        }

    }
}