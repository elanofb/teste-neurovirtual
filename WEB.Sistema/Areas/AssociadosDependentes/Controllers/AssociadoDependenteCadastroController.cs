using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosDependentes;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosDependentes.ViewModels;

namespace WEB.Areas.AssociadosDependentes.Controllers {
    public class AssociadoDependenteCadastroController : BaseSistemaController{

        //Atributos
        private IAssociadoDependenteCadastroBL _AssociadoDependenteCadastroBL;
        private IAssociadoDependenteBL _AssociadoDependenteBL;
        private IAssociadoBL _AssociadoBL;


        //Propriedades
        private IAssociadoDependenteCadastroBL OAssociadoDependenteCadastroBL => _AssociadoDependenteCadastroBL = _AssociadoDependenteCadastroBL ?? new AssociadoDependenteCadastroBL();
        private IAssociadoDependenteBL OAssociadoDependenteBL => _AssociadoDependenteBL = _AssociadoDependenteBL ?? new AssociadoDependenteBL();
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();

        // GET: Associados/AssociadoCadastroPF
        [OrganizacaoFilter]
        public ActionResult cadastrar() {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");
            
            var ViewModel = new AssociadoDependenteCadastroForm();

            ViewModel.carregarConfiguracoes();

            ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

            ViewModel.Associado.Pessoa = new Pessoa();

            ViewModel.carregaDados();

            return View("editar", ViewModel);
        }

        // GET: Associados/AssociadoCadastroPF
        [OrganizacaoFilter, ActionName("modal-cadastrar-dependente")]
        public ActionResult modalCadastrarDependente(int? idAssociadoEstipulante){

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");
            
            var ViewModel = new AssociadoDependenteCadastroForm();           

            ViewModel.carregarConfiguracoes();

            ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

            ViewModel.carregaDados();

            var OAssociado = OAssociadoBL.listar(0, "", "", "").Where(x => x.id == idAssociadoEstipulante)
                .Select(x => new { x.id, x.Pessoa.nome }).FirstOrDefault();

            ViewModel.Associado.Pessoa = new Pessoa();

            return View("modal-cadastrar-dependente", ViewModel);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [OrganizacaoFilter]
        public ActionResult editar(int? id) {

            var ViewModel = new AssociadoDependenteCadastroForm();

            ViewModel.carregarConfiguracoes();

            ViewModel.Associado = this.OAssociadoDependenteBL.carregar(id.toInt());

            if (ViewModel.Associado == null) {
                return RedirectToAction("cadastrar");
            }

            var flagEdicao = id > 0;
            
            ViewModel.carregaDados(flagEdicao);
            
            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();

            ViewModel.Associado.Pessoa.limparListas();
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);

            return View("editar", ViewModel);
        }

        /// <summary>
        /// Processar e salvar os dados enviados pelo formulário
        /// </summary>
        [HttpPost, OrganizacaoFilter]
        public ActionResult salvarCadastroDependente(AssociadoDependenteCadastroForm ViewModel) {

            ViewModel.carregaDados();

            ViewModel.listaCampos.bind(Request.Form);

            if (!ModelState.IsValid || ViewModel.listaCampos.Any(x => x.flagValidado == false)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Algumas informações não passaram na validação, verifique abaixo."));

                return View("aba-dados-cadastrais-dependente", ViewModel);
            }

            var dbAssociado = this.OAssociadoDependenteBL.carregar(ViewModel.Associado.id);

            if (dbAssociado != null && dbAssociado.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Você não tem permissão para acessar esse cadastro."));

                return View("aba-dados-cadastrais-dependente", ViewModel);

            }


            ViewModel = ViewModel.atribuirValoresFixos(ViewModel);

            ViewModel.Associado.Pessoa.flagTipoPessoa = "F";

            ViewModel.Associado.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;

            OAssociadoDependenteCadastroBL.salvar(ViewModel.Associado);

            if (ViewModel.Associado.id > 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do membro foram validados e salvos sem erros."));

                return Json(new { error = false, message = "Os dados foram salvos com sucesso.", urlRedirecionamento = Url.Action("editar", new { ViewModel.Associado.id }) });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar os dados."));

            return View("aba-dados-cadastrais-dependente", ViewModel);
        }
    }
}