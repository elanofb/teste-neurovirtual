using DAL.Associados;
using DAL.Localizacao;
using DAL.Pessoas;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BLL.AssociadosDependentes;
using DAL.Enderecos;
using WEB.Areas.Associados.Extensions;
using WEB.Helpers;
using WEB.Areas.AssociadosDependentes.ViewModels;

namespace WEB.Areas.AssociadosDependentes.Controllers {

    public class AssociadoDependenteController : Controller {

        //Atributos
        private IAssociadoDependenteBL _IAssociadoDependenteBL;

        //Propriedades
        private IAssociadoDependenteBL OAssociadoDependenteBL => _IAssociadoDependenteBL = _IAssociadoDependenteBL ?? new AssociadoDependenteBL();

        //Listagem dos associados do sistema
        public ActionResult listar() {
            
            var valorBusca = UtilRequest.getString("valorBusca");

            var ativo = UtilRequest.getString("flagAtivo");

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");
            
            var flagTipoSaida = UtilRequest.getString("tipoSaida");

            var queryDependentes = this.OAssociadoDependenteBL.listar(0, valorBusca, ativo);

            if (idTipoAssociado > 0){
                queryDependentes = queryDependentes.Where(x => x.idTipoAssociado == idTipoAssociado);
            }

            if(flagTipoSaida == TipoSaidaHelper.EXCEL) {

                this.baixarExcel(queryDependentes.ToList());
                return null;
            }

            var query = queryDependentes.Select(x => new ItemListaDependente {
                id = x.id,
                flagTipoPessoa = x.Pessoa.flagTipoPessoa,
                nome = x.Pessoa.nome,
                descricaoTipoAssociado = x.TipoAssociado.descricao,
                razaoSocial = x.Pessoa.razaoSocial,
                nroDocumento = x.Pessoa.nroDocumento,
                rg = x.Pessoa.rg,
                dtCadastro = x.dtCadastro,
                ativo = x.ativo
            });

            query = query.OrderBy(x => x.nome);

            var lista = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(lista);
        }

        [ActionName("partial-lista-associado-dependentes")]
        public ActionResult partialListaAssociadoDependente(int idAssociado){

            var queryDependentes = this.OAssociadoDependenteBL.listar(idAssociado, "", "S");

            var query = queryDependentes.Select(x => new ItemListaDependente{
                id = x.id,
                flagTipoPessoa = x.Pessoa.flagTipoPessoa,
                nome = x.Pessoa.nome,
                descricaoTipoAssociado = x.TipoAssociado.descricao,
                razaoSocial = x.Pessoa.razaoSocial,
                nroDocumento = x.Pessoa.nroDocumento,
                rg = x.Pessoa.rg,
                dddTelefonePrincipal = x.Pessoa.dddTelPrincipal,
                nroTelefonePrincipal = x.Pessoa.nroTelPrincipal,
                dtCadastro = x.dtCadastro,
                ativo = x.ativo
            });

            query = query.OrderBy(x => x.nome);

            var lista = query.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(lista);
        }

        //Exportacao do cadastro para formato EXCEL
        //Download do documento gerado
        public void baixarExcel(List<Associado> listaDependentes) {
          
            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel(listaDependentes));

            foreach (var OItem in listaDependentes) {
                sw.WriteLine(this.gerarLinhaExcel(OItem, listaDependentes));
            }

            Response.ClearContent();

            var nomeArquivo = String.Concat("relatorios-dependentes-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            Response.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            Response.ContentType = "text/csv; charset=ISO-8859-1";
            Response.Charset = "ISO-8859-1";
            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            Response.Write(sw.ToString());

            Response.End();
        }

        private string gerarCabecalhoExcel(List<Associado> lista) {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código Sistema;")
                .Append("Tipo Pessoa;")
                .Append("Nome;")
                .Append("Associado Estipulante;")
                .Append("N. Documento;")
                .Append("RG/I.E;")
                .Append("Nascto.;")
                .Append("1º E-mail;")
                .Append("2º E-mail;")
                .Append("Receber Comunicados ?;")
                .Append("1º Telefone;")
                .Append("2º Telefone;")
                .Append("3º Telefone;")
                .Append("1º Endereço CEP;")
                .Append("1º Endereço Logradouro;")
                .Append("1º Endereço Numero;")
                .Append("1º Endereço Complemento;")
                .Append("1º Endereço Bairro;")
                .Append("1º Endereço Cidade;")
                .Append("1º Endereço UF;")
                .Append("2º Endereco CEP;")
                .Append("2º Endereço Logradouro;")
                .Append("2º Endereço Numero;")
                .Append("2º Endereço Complemento;")
                .Append("2º Endereço Bairro;")
                .Append("2º Endereço Cidade;")
                .Append("2º Endereço UF;")
                .Append("Login;")
                .Append("Status;")
                .Append("Data Cadastro;")
                .Append("Observações;");

                if (lista.Exists(x => x.flagEstudante())) {
                    cabecalho.Append("Matricula;");
                    cabecalho.Append("Universidade/Instituicao;");
                }


            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Associado OAssociado, List<Associado> lista) {

            var EnderecoPrincipal = OAssociado.Pessoa.listaEnderecos.FirstOrDefault(x => x.idTipoEndereco == TipoEnderecoConst.PRINCIPAL);
            var EnderecoComercial = OAssociado.Pessoa.listaEnderecos.FirstOrDefault(x => x.idTipoEndereco == TipoEnderecoConst.COMERCIAL);

            StringBuilder linha = new StringBuilder();

            linha.Append(OAssociado.id).Append(";");
            linha.Append(OAssociado.Pessoa.flagTipoPessoa == "J" ? "Jurídica" : "Física").Append(";");
            linha.Append(UtilString.limparParaCSV(OAssociado.Pessoa.nome)).Append(";");
            linha.Append(UtilString.formatCPFCNPJ(OAssociado.Pessoa.nroDocumento)).Append(";");
            linha.Append(OAssociado.Pessoa.rg).Append(";");
            linha.Append(OAssociado.Pessoa.dtNascimento.exibirData()).Append(";");
            linha.Append(UtilString.limparParaCSV(OAssociado.Pessoa.emailPrincipal())).Append(";");
            linha.Append(UtilString.limparParaCSV(OAssociado.Pessoa.emailSecundario())).Append(";");
            linha.Append(OAssociado.flagInformativosOnline == "S" ? "Sim" : "Não").Append(";");
            linha.Append(OAssociado.Pessoa.formatarTelPrincipal()).Append(";");
            linha.Append(OAssociado.Pessoa.formatarTelSecundario()).Append(";");
            linha.Append(OAssociado.Pessoa.formatarTelTerciario()).Append(";");

            if (EnderecoPrincipal != null) {

                var CidadePrincipal = EnderecoPrincipal.Cidade ?? new Cidade();
                var EstadoPrincipal = CidadePrincipal.Estado ?? new Estado();

                linha.Append(UtilString.limparParaCSV(EnderecoPrincipal.logradouro)).Append(";");
                linha.Append(UtilString.formatCEP(EnderecoPrincipal.cep)).Append(";");
                linha.Append(EnderecoPrincipal.numero).Append(";");
                linha.Append(UtilString.limparParaCSV(EnderecoPrincipal.complemento)).Append(";");
                linha.Append(UtilString.limparParaCSV(EnderecoPrincipal.bairro)).Append(";");
                linha.Append(UtilString.limparParaCSV(CidadePrincipal.nome)).Append(";");
                linha.Append(UtilString.limparParaCSV(EstadoPrincipal.sigla)).Append(";");
            }else {
                linha.Append(";;;;;;;");
            }

            if (EnderecoComercial != null) {

                var CidadeComercial = EnderecoComercial.Cidade ?? new Cidade();
                var EstadoComercial = CidadeComercial.Estado ?? new Estado();

                linha.Append(UtilString.limparParaCSV(EnderecoComercial.logradouro)).Append(";");
                linha.Append(UtilString.formatCEP(EnderecoComercial.cep)).Append(";");
                linha.Append(EnderecoComercial.numero).Append(";");
                linha.Append(EnderecoComercial.complemento).Append(";");
                linha.Append(UtilString.limparParaCSV(EnderecoComercial.bairro)).Append(";");
                linha.Append(UtilString.limparParaCSV(CidadeComercial.nome)).Append(";");
                linha.Append(UtilString.limparParaCSV(EstadoComercial.sigla)).Append(";");
            }else {
                linha.Append(";;;;;;;");
            }

            linha.Append(OAssociado.Pessoa.login).Append(";");
            linha.Append(OAssociado.exibirStatus()).Append(";");
            linha.Append(OAssociado.dtCadastro.exibirData()).Append(";");
            linha.Append(UtilString.limparParaCSV(OAssociado.observacoes)).Append(";");

            if (lista.Exists(x => x.flagEstudante())) {
                linha.Append(UtilString.limparParaCSV(OAssociado.Pessoa.nroMatriculaEstudante)).Append(";");
                linha.Append(UtilString.limparParaCSV(OAssociado.Pessoa.instituicaoFormacao)).Append(";");
            }


            return linha.ToString();
        }
        
    }
}