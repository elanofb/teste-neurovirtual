using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using MvcFlashMessages;
using BLL.NaoAssociados;
using BLL.Pessoas;
using BLL.Services;
using WEB.Areas.NaoAssociados.ViewModels;

namespace WEB.Areas.NaoAssociados.Controllers {

    public class NaoAssociadoCadastroPJController : Controller {

        //Atributos
        private INaoAssociadoCadastroBL _NaoAssociadoCadastroBL;
        private INaoAssociadoBL _NaoAssociadoBL;
        private IPessoaEmailConsultaBL _PessoaEmailConsultaBL;
        private IPessoaTelefoneConsultaBL _PessoaTelefoneConsultaBL;
        private IPessoaEnderecoConsultaBL _PessoaEnderecoConsultaBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;

        //Propriedades
        private INaoAssociadoCadastroBL ONaoAssociadoCadastroBL => _NaoAssociadoCadastroBL = _NaoAssociadoCadastroBL ?? new NaoAssociadoCadastroBL();
        private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();
        private IPessoaEmailConsultaBL OPessoaEmailConsultaBL => _PessoaEmailConsultaBL = _PessoaEmailConsultaBL ?? new PessoaEmailConsultaBL();
        private IPessoaTelefoneConsultaBL OPessoaTelefoneConsultaBL => _PessoaTelefoneConsultaBL = _PessoaTelefoneConsultaBL ?? new PessoaTelefoneConsultaBL();
        private IPessoaEnderecoConsultaBL OPessoaEnderecoConsultaBL => _PessoaEnderecoConsultaBL = _PessoaEnderecoConsultaBL ?? new PessoaEnderecoConsultaBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();

        // GET: Associados/AssociadoCadastroPJ
        [OrganizacaoFilter]
        public ActionResult cadastrar() {
            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new NaoAssociadoCadastroPJForm();

            ViewModel.Associado = new Associado() { idTipoAssociado = idTipoAssociado };

            ViewModel.Associado.Pessoa = new Pessoa();

            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados();

            return View("editar", ViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        [OrganizacaoFilter]
        public ActionResult editar(int? id) {

            var ViewModel = new NaoAssociadoCadastroPJForm();
            
            ViewModel.Associado = this.ONaoAssociadoBL.listar("", "").Where(x => x.id == id)
                .Include(x => x.Indicador)
                .Include(x => x.UsuarioCadastro)
                .Include(x => x.Unidade)
                .Include(x => x.Pessoa.CidadeOrigem).FirstOrDefault();

            ViewModel.Saldo = this.OSaldoConsultaBL.query(id.toInt()).Where(x => x.idMembro > 0)
                                  .Select(x => new { x.id, x.saldoAtual, x.dtAtualizacaoSaldo})
                                  .FirstOrDefault()
                                  .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();
            
            if (ViewModel.Associado == null){
                return RedirectToAction("cadastrar");
            }

            if (ViewModel.Associado.Pessoa.flagTipoPessoa == "F") {
                return RedirectToAction("editar", "NaoAssociadoCadastroPF", new { ViewModel.Associado.id });
            }

            ViewModel.carregarConfiguracoes();

            ViewModel.carregaDados(true);

            ViewModel.Associado = this.ONaoAssociadoBL.carregar(id.toInt()).condicoesSeguranca().FirstOrDefault();
            ViewModel.Associado.Pessoa = ViewModel.Associado.Pessoa ?? new Pessoa();
            
            if (ViewModel.Associado.Pessoa.id > 0){

                ViewModel.Associado.Pessoa.listaEmails = this.OPessoaEmailConsultaBL.listar(ViewModel.Associado.Pessoa.id).ToList();
                ViewModel.Associado.Pessoa.listaTelefones = this.OPessoaTelefoneConsultaBL.listar(ViewModel.Associado.Pessoa.id).ToList();
                ViewModel.Associado.Pessoa.listaEnderecos = this.OPessoaEnderecoConsultaBL.listar(ViewModel.Associado.Pessoa.id).ToList();
                
            }

            ViewModel.Associado.Pessoa.limparListas();
            ViewModel.Associado.limparListas();

            ViewModel.carregarValorCampos(ViewModel);

            return View("editar", ViewModel);
        }

        [ActionName("aba-dados-cadastrais")]
        public ActionResult abaDadosCadastrais(int? id) {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new NaoAssociadoCadastroPJForm();

            if (id > 0) {
                ViewModel.Associado = this.ONaoAssociadoBL.listar("", "").Where(x => x.id == id)
                    .Include(x => x.UsuarioCadastro)
                    .Include(x => x.Unidade)
                    .Include(x => x.Pessoa.CidadeOrigem).FirstOrDefault();

                if (ViewModel.Associado == null){
                    return RedirectToAction("cadastrar");
                }

                if (ViewModel.Associado.Pessoa.flagTipoPessoa == "F"){
                    return RedirectToAction("editar", "NaoAssociadoCadastroPJ", new { ViewModel.Associado.id });
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
        [HttpPost, OrganizacaoFilter]
        public ActionResult salvarCadastro(NaoAssociadoCadastroPJForm ViewModel) {

            var flagEdicao = ViewModel.Associado.id > 0;

            ViewModel.carregaDados(flagEdicao);

            ViewModel.listaCampos.bind(Request.Form);

            if (!ModelState.IsValid || ViewModel.listaCampos.Any(x => x.flagValidado == false)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Algumas informações não passaram na validação, verifique abaixo."));

                return View("aba-dados-cadastrais", ViewModel);
            }

            var dbAssociado = this.ONaoAssociadoBL.carregar(ViewModel.Associado.id).condicoesSeguranca().FirstOrDefault();

            if (dbAssociado != null && dbAssociado.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Você não tem permissão para acessar esse cadastro."));

                return View("aba-dados-cadastrais", ViewModel);

            }

            ViewModel = ViewModel.atribuirValoresFixos(ViewModel);

            ViewModel.Associado.Pessoa.flagTipoPessoa = "J";

            ViewModel.Associado.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;

            ViewModel.Associado.ativo = "S"; 

            ONaoAssociadoCadastroBL.salvar(ViewModel.Associado);

            if (ViewModel.Associado.id > 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do membro foram validados e salvos sem erros."));

                return Json(new { error = false, message = "Os dados foram salvos com sucesso.", urlRedirecionamento = Url.Action("editar", new { ViewModel.Associado.id }) });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar os dados."));

            return View("aba-dados-cadastrais", ViewModel);
        }
    }
}