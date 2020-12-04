using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WEB.Areas.Permissao.ViewModels;
using WEB.App_Infrastructure;
using PagedList;
using BLL.Permissao;
using DAL.Permissao;
using DAL.Pessoas;
using MvcFlashMessages;
using DAL.Permissao.Security.Extensions;
using BLL.UsuariosInternos;
using WEB.Helpers;
using System.Data.Entity;

namespace WEB.Areas.Permissao.Controllers{

    public class UsuarioSistemaController : BaseSistemaController{

		//Constantes

		//Atributos
        private IUsuarioSistemaBL _UsuarioInternoBL;

		//Propriedades
		private IUsuarioSistemaBL OUsuarioInternoBL => _UsuarioInternoBL = _UsuarioInternoBL ?? new UsuarioInternoBL();
			
		//Listagem de usuarios
        public ActionResult listar(){

            var ativo = UtilRequest.getString("flagAtivo");
            var nome = UtilRequest.getString("valorBusca");
            var idPerfilAcesso = UtilRequest.getInt32("idPerfilAcesso");
            var flagTipoSaida = UtilRequest.getString("flagTipoSaida");


            var listUsuarios = this.OUsuarioInternoBL.listar(UtilNumber.toInt32(idPerfilAcesso), nome, ativo)
                                         .Where(x => x.idPerfilAcesso != PerfilAcessoConst.DESENVOLVEDOR)
                                        .OrderBy(x => x.nome);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                listUsuarios = listUsuarios
                    .Include(x => x.Pessoa)
                    .Include(x => x.Pessoa.listaEnderecos)
                    .Include(x => x.Pessoa.listaEnderecos.Select(y => y.Cidade))
                    .Include(x => x.Pessoa.listaEnderecos.Select(y => y.Cidade.Estado))
                    .Include(x => x.listaUsuarioUnidade)
                    .Include(x => x.listaUsuarioUnidade.Select(y => y.Unidade))
                    .Include(x => x.listaUsuarioUnidade.Select(y => y.Unidade.Pessoa)).OrderBy(x => x.nome);

                this.baixarExcel(listUsuarios.ToList());
            }

            return View(listUsuarios.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //Listagem das unidades por ajax
        [ActionName("listar-ajax")]
        public ActionResult listarAjax() {

            var listUsuarios = this.OUsuarioInternoBL.listar(0, "", "")
                                   .Where(x => x.idPerfilAcesso != PerfilAcessoConst.DESENVOLVEDOR)
                                   .Select(x => new { value = x.id, text = x.nome })
                                   .OrderBy(x => x.text).ToList();
            
            return Json(listUsuarios, JsonRequestBehavior.AllowGet);

        }

        //Edicao de usuario
        public ActionResult editar(int? id){

            var ViewModel = new UsuarioSistemaForm();

            var OUsuarioSistema = this.OUsuarioInternoBL.carregar(UtilNumber.toInt32(id)) ?? new UsuarioSistema();
            ViewModel.UsuarioSistemaLogado = this.OUsuarioInternoBL.carregar(User.id()) ?? new UsuarioSistema();

            ViewModel.UsuarioSistema = OUsuarioSistema;

            ViewModel.UsuarioSistema.Pessoa = ViewModel.UsuarioSistema.Pessoa ?? new Pessoa();

            if (!ViewModel.UsuarioSistema.Pessoa.listaEnderecos.Any()) {
                ViewModel.UsuarioSistema.Pessoa.listaEnderecos.Add(new PessoaEndereco());
            }

            ViewModel.UsuarioSistema.idOrganizacao = UtilNumber.toInt32(OUsuarioSistema.idOrganizacao) > 0 ? OUsuarioSistema.idOrganizacao : User.idOrganizacao();

            ViewModel.flagLogAcesso = true;

            return View(ViewModel);
        }


        //POST
        [HttpPost]
        public ActionResult editar(UsuarioSistemaForm ViewModel){

            if(!ModelState.IsValid){
				return View(ViewModel);
			}

            if (User.idOrganizacao() > 0 && ViewModel.UsuarioSistema.idOrganizacao != User.idOrganizacao()){

		        this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não identificamos a associação ligada a este registro."));

                return View(ViewModel);			
		    }

            ViewModel.UsuarioSistema.idUsuarioCadastro = User.id();

            ViewModel.UsuarioSistema.idUsuarioAlteracao = User.id();

            bool flagSucesso = this.OUsuarioInternoBL.salvar(ViewModel.UsuarioSistema);

            if (flagSucesso) {
                 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do usuário foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.UsuarioSistema.id });
            } 

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
			return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OUsuarioInternoBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idUsuario in id) {

                var RetornoItem = this.OUsuarioInternoBL.excluir(idUsuario, User.id());

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }
            }

            return Json(Retorno);
        }

        private void baixarExcel(List<UsuarioSistema> listaUsuarios) {

            StringWriter sw = new StringWriter();
            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaUsuarios) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            var OResponse = System.Web.HttpContext.Current.Response;
            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-usuario-sistema-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename=" + nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();
        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Id;").Append("Nome;")
                .Append("E-mail;")
                .Append("Login;")
                .Append("Data Cadastro;")
                .Append("Dt. Degustação;")
                .Append("Unidades Acesso;")
                .Append("Perfil de Acesso;")
                .Append("Acesso a todas unidade?;")
                .Append("Somente conteúdo próprio?;")
                .Append("CPF;")
                .Append("RG;")
                .Append("Nascto.;")
                .Append("Tel. Principal;")
                .Append("Tel. Secundário;")
                .Append("Cep")
                .Append("Logradouro;")
                .Append("Numero;")
                .Append("Bairro;")
                .Append("Complemento;")
                .Append("Cidade;")
                .Append("UF");
                

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(UsuarioSistema OUsuario) {

            StringBuilder linha = new StringBuilder();

            linha.Append(OUsuario.id).Append(";");
            linha.Append(OUsuario.Pessoa.nome).Append(";");
            linha.Append(OUsuario.Pessoa.emailPrincipal()).Append(";");
            linha.Append(OUsuario.login).Append(";");
            linha.Append(OUsuario.dtCadastro).Append(";");
            linha.Append(OUsuario.dtInicioDegustacao.exibirData() + " - " + OUsuario.dtFimDegustacao.exibirData()).Append(";");
            
            var listaUnidadesAcesso = OUsuario.listaUsuarioUnidade.Where(x => x.flagExcluido == "N").Select(x => x.Unidade.Pessoa.nome).ToList();
            var unidades = (listaUnidadesAcesso.Any() ? string.Join(", ", listaUnidadesAcesso.ToList()) : "");

            linha.Append(unidades).Append(";");
            linha.Append(OUsuario.PerfilAcesso.descricao).Append(";");
            linha.Append(OUsuario.PerfilAcesso.flagTodasUnidades == true ? "Sim" : "Não").Append(";");
            linha.Append(OUsuario.PerfilAcesso.flagSomenteCadastroProprio == true ? "Sim" : "Não").Append(";");

            linha.Append(UtilString.formatCPFCNPJ(OUsuario.Pessoa.nroDocumento)).Append(";");
            linha.Append(OUsuario.Pessoa.rg).Append(";");
            linha.Append(OUsuario.Pessoa.dtNascimento.exibirData()).Append(";");
            linha.Append(UtilString.formatPhone(OUsuario.Pessoa.nroTelPrincipal)).Append(";");
            linha.Append(UtilString.formatPhone(OUsuario.Pessoa.nroTelSecundario)).Append(";");

            var endereco = OUsuario.Pessoa.listaEnderecos.FirstOrDefault(x => x.dtExclusao == null);
            if (endereco != null) {

                linha.Append(UtilString.formatCEP(endereco.cep)).Append(";");
                linha.Append(UtilString.limparParaCSV(endereco.logradouro)).Append(";");
                linha.Append(UtilString.limparParaCSV(endereco.numero)).Append(";");
                linha.Append(UtilString.limparParaCSV(endereco.bairro)).Append(";");
                linha.Append(UtilString.limparParaCSV(endereco.Cidade?.nome)).Append(";");
                linha.Append(UtilString.limparParaCSV(endereco.Cidade?.Estado?.sigla)).Append("");
            } else {
                linha.Append(";;;;;");
            }

            return linha.ToString();
        }
    }
}
