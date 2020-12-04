using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;
using System.Data.Entity;
using BLL.Services;
using DAL.Entities;
using WEB.Extensions;

namespace WEB.Areas.Associados.Controllers {
    [OrganizacaoFilter]
    public class AssociadoCadastroPFController : Controller {

        //Atributos
        private IAssociadoCadastroBL _AssociadoCadastroBL;
        private IAssociadoBL _AssociadoBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;
        

        //Propriedades
        private IAssociadoCadastroBL OAssociadoCadastroBL => _AssociadoCadastroBL = _AssociadoCadastroBL ?? new AssociadoCadastroBL();
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();

        // GET: Associados/AssociadoCadastroPF
        public ActionResult cadastrar() {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new AssociadoCadastroPFForm();

            ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

            ViewModel.Associado.Pessoa = new Pessoa();
            
            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados();
            
            return View("editar", ViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionResult editar(int? id) {

            var ViewModel = new AssociadoCadastroPFForm();
            
            ViewModel.Associado = this.OAssociadoBL.carregar(id.toInt());
            
            ViewModel.Saldo = this.OSaldoConsultaBL.query(id.toInt())
                                  .Select(x => new { x.id, x.saldoAtual, x.dtAtualizacaoSaldo})
                                  .FirstOrDefault()
                                    .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();

            if (ViewModel.Associado == null) {
                return RedirectToAction("cadastrar");
            }

            if (ViewModel.Associado.Pessoa.flagTipoPessoa == "J") {
                return RedirectToAction("editar", "AssociadoCadastroPJ", new { ViewModel.Associado.id });
            }

            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados(true);

            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();

            ViewModel.Associado.Pessoa.limparListas();
            
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);
            
            ViewModel.carregarConfiguracaoMembro();

            return View("editar", ViewModel);
        }

        [ActionName("aba-dados-cadastrais")]
        public ActionResult abaDadosCadastrais(int? id) {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new AssociadoCadastroPFForm();

            if (id > 0) {
                ViewModel.Associado = this.OAssociadoBL.listar(0, "", "", "")
                                            .Where(x => x.id == id)
                                            .Include(x => x.UsuarioCadastro)
                                            .Include(x => x.Unidade)
                                            .Include(x => x.Pessoa.CidadeOrigem)
                                          .FirstOrDefault();

                if (ViewModel.Associado == null){
                    return RedirectToAction("cadastrar");
                }

                if (ViewModel.Associado.Pessoa.flagTipoPessoa == "J"){
                    return RedirectToAction("editar", "AssociadoCadastroPJ", new { ViewModel.Associado.id });
                }
            } else {
                ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

                ViewModel.Associado.Pessoa = new Pessoa();
            }

            ViewModel.carregarConfiguracoes();

            var flagEdicao = id > 0;

            ViewModel.carregaDados(flagEdicao);

            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();

            ViewModel.Associado.Pessoa.limparListas();
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);

            return View("aba-dados-cadastrais", ViewModel);
        }

        /// <summary>
        /// Processar e salvar os dados enviados pelo formulário
        /// </summary>
        [HttpPost]
        public ActionResult salvarCadastro(AssociadoCadastroPFForm ViewModel) {
            
            var flagEdicao = ViewModel.Associado.id > 0;

            if (ViewModel.Associado.id == 1) {
                
                if (ViewModel.flagRetornoAjax == true){
                    
                    return Json(new { error = true, message = "Os dados da conta principal do sistema não podem ser editados." });
                }
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Os dados da conta principal do sistema não podem ser editados."));

                return View("aba-dados-cadastrais", ViewModel);                
            }

            ViewModel.carregaDados(flagEdicao);
    
            ViewModel.listaCampos.bind(Request.Form);

            if (!ModelState.IsValid || ViewModel.listaCampos.Any(x => x.flagValidado == false)){
                
                if (ViewModel.flagRetornoAjax == true){
                    
                    string errosValidacao = ModelState.retornarErros();

                    string errosCampos = string.Join("<br>", ViewModel.listaCampos.Select(x => x.mensagemErro).Where(x => !x.isEmpty()));

                    string errosConcatenados = String.Join(errosValidacao.Trim(), errosCampos.Trim());  
                    
                    return Json(new { error = true, message = errosConcatenados });
                }

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Algumas informações não passaram na validação, verifique abaixo."));

                return View("aba-dados-cadastrais", ViewModel);
            }

            var dbAssociado = this.OAssociadoBL.carregar(ViewModel.Associado.id);

            if (dbAssociado != null && dbAssociado.idOrganizacao != User.idOrganizacao()) {
                
                if (ViewModel.flagRetornoAjax == true){
                    
                    return Json(new { error = true, message = "Você não tem permissão para acessar esse cadastro." });
                }
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Você não tem permissão para acessar esse cadastro."));

                return View("aba-dados-cadastrais", ViewModel);

            }

            ViewModel = ViewModel.atribuirValoresFixos(ViewModel);

            ViewModel.Associado.Pessoa.flagTipoPessoa = "F";

            ViewModel.Associado.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;

            ViewModel.Associado.ativo = "E"; //Em admissão

            ViewModel.Associado.idOrigem = OrigemCadastroConst.SISTEMA;

            OAssociadoCadastroBL.salvar(ViewModel.Associado);

            if (ViewModel.Associado.id > 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do membro foram validados e salvos sem erros."));

                return Json(new { error = false, message = "Os dados foram salvos com sucesso.", id = ViewModel.Associado.id, urlRedirecionamento = Url.Action("editar", new { ViewModel.Associado.id }) });
            }
            
            if (ViewModel.flagRetornoAjax == true){
                    
                return Json(new { error = true, message = "Não foi possível salvar os dados." });
            }
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar os dados."));

            return View("aba-dados-cadastrais", ViewModel);
        }
    }
}