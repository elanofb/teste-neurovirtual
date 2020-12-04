using System;
using System.Json;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.Pessoas;
using DAL.Pessoas;

namespace WEB.Areas.Financeiro.Controllers {
    public class GeracaoBoletoController : Controller {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;
        private IPessoaVWBL _PessoaVWBL;

        //Propriedades
        private ITituloReceitaBL OTituloReceitaBL => (_TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL());
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => (_TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL());
        private IPessoaVWBL OPessoaVWBL => this._PessoaVWBL = this._PessoaVWBL ?? new PessoaVWBL();


        //Gerar boleto
        [HttpPost]
        public ActionResult gerar(int id) {

            JsonMessage Retorno = new JsonMessage() {
                error = false,
                message = "O Boleto Bancário foi gerado com sucesso!"
            };

            var OPagamento = this.OTituloReceitaPagamentoBL.carregar(id);

            var OTitulo = OPagamento.TituloReceita;

            if (OTitulo.idPessoa == null) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = "Informe para quem deve ser gerado o Boleto Bancário."
                };

                return Json(Retorno);
            }

            if (OPagamento.dtVencimento <= DateTime.Now) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = "A data de vencimento deve ser superior a hoje."
                };

                return Json(Retorno);
            }

            //Chama o metodo que realiza a atualização dos dados da pessoa
            //OTituloReceitaBL.atualizarDadosPessoa(OTitulo.id);
            
            var OTituloAtualizado = OTituloReceitaBL.carregar(OTitulo.id);

            var OPessoaVW = OPessoaVWBL.carregar(Convert.ToInt32(OTituloAtualizado.idPessoa));

            if (OPessoaVW == null) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = "Informe para quem deve ser gerado o Boleto Bancário."
                };

                return Json(Retorno);
            }

            if (String.IsNullOrEmpty(OTituloAtualizado.documentoPessoa)) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = $"<strong>Cadastro Incompleto</strong><br><br><a href=" + this.getUrlCadastro(OPessoaVW) + " target=\"_blank\">Clique aqui</a> para preencher o CPF/CNPJ do " + OPessoaVW.descricaoCategoriaPessoa + "."
                };

                return Json(Retorno);
            }

            if (String.IsNullOrEmpty(OTituloAtualizado.nomePessoa)) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = $"<strong>Cadastro Incompleto</strong><br><br><a href=" + this.getUrlCadastro(OPessoaVW) + " target=\"_blank\">Clique aqui</a> para preencher o Nome Completo do " + OPessoaVW.descricaoCategoriaPessoa + "."
                };

                return Json(Retorno);
            }

            if (String.IsNullOrEmpty(OTituloAtualizado.cepRecibo) || String.IsNullOrEmpty(OTituloAtualizado.logradouroRecibo) || String.IsNullOrEmpty(OTituloAtualizado.numeroRecibo) || String.IsNullOrEmpty(OTituloAtualizado.bairroRecibo) || OTituloAtualizado.CidadeRecibo == null) {
                Retorno = new JsonMessage() {
                    error = true,
                    message = $"<strong>Cadastro Incompleto</strong><br><br><a href=" + this.getUrlCadastro(OPessoaVW) + " target=\"_blank\">Clique aqui</a> para preencher o Endereço do " + OPessoaVW.descricaoCategoriaPessoa + "."
                };

                return Json(Retorno);
            }

            if (!Retorno.error) {

                OPagamento.TituloReceita = OTituloAtualizado;

                //this.OTituloReceitaPagamentoBL.gerarBoleto(OPagamento);
            }

            return Json(Retorno);
        }

        private string getUrlCadastro(PessoaVW OPessoaVW) {

            var urlCadastro = "";

            if (OPessoaVW.flagCategoriaPessoa == "AS") {

                urlCadastro = Url.Action(null, null, new {area = "Associados", id = OPessoaVW.idReferencia});

            } else if (OPessoaVW.flagCategoriaPessoa == "FO") {
                
                urlCadastro = Url.Action("editar", "FornecedorCadastro", new {area = "Fornecedores", id = OPessoaVW.idReferencia});


            } else if (OPessoaVW.flagCategoriaPessoa == "PA") {
                
                //urlCadastro = Url.Action("editar", "patrocinador", new {area = "Patrocinadores", id = OPessoaVW.idReferencia});

            }

            return urlCadastro;
        }
    }
}