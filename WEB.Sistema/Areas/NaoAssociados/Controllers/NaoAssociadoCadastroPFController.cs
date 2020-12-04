using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Associados;
using DAL.ConfiguracoesAssociados;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using MvcFlashMessages;
using System.Data.Entity;
using BLL.Associados;
using BLL.NaoAssociados;
using BLL.Pessoas;
using BLL.Services;
using WEB.Areas.NaoAssociados.ViewModels;

namespace WEB.Areas.NaoAssociados.Controllers {
    public class NaoAssociadoCadastroPFController : Controller {

        //Atributos
        private INaoAssociadoCadastroBL _NaoAssociadoCadastroBL;
        private INaoAssociadoBL _NaoAssociadoBL;
        private IPessoaEmailConsultaBL _PessoaEmailConsultaBL;
        private IPessoaTelefoneConsultaBL _PessoaTelefoneConsultaBL;
        private IPessoaEnderecoConsultaBL _PessoaEnderecoConsultaBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;
        private IValidadorPessoaEmail _ValidadorEmailBL;


        //Propriedades
        private INaoAssociadoCadastroBL ONaoAssociadoCadastroBL => _NaoAssociadoCadastroBL = _NaoAssociadoCadastroBL ?? new NaoAssociadoCadastroBL();
        private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();
        private IPessoaEmailConsultaBL OPessoaEmailConsultaBL => _PessoaEmailConsultaBL = _PessoaEmailConsultaBL ?? new PessoaEmailConsultaBL();
        private IPessoaTelefoneConsultaBL OPessoaTelefoneConsultaBL => _PessoaTelefoneConsultaBL = _PessoaTelefoneConsultaBL ?? new PessoaTelefoneConsultaBL();
        private IPessoaEnderecoConsultaBL OPessoaEnderecoConsultaBL => _PessoaEnderecoConsultaBL = _PessoaEnderecoConsultaBL ?? new PessoaEnderecoConsultaBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();
        private IValidadorPessoaEmail ValidadorEmailBL => _ValidadorEmailBL = _ValidadorEmailBL ?? new ValidadorPessoaEmail();

        // GET: Associados/AssociadoCadastroPF
        [OrganizacaoFilter]
        public ActionResult cadastrar() {

            var idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");

            var ViewModel = new NaoAssociadoCadastroPFForm();
            
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
            
            var ViewModel = new NaoAssociadoCadastroPFForm();
            
            ViewModel.Associado = this.ONaoAssociadoBL.listar("", "")
                                                .Where(x => x.id == id)
                                                .Include(x => x.Indicador)
                                                .Include(x => x.UsuarioCadastro)
                                                .Include(x => x.Unidade)
                                                .Include(x => x.TipoAssociado)
                                                .Include(x => x.Pessoa.CidadeOrigem).FirstOrDefault();                        
            
            ViewModel.Saldo = this.OSaldoConsultaBL.query(id.toInt()).Where(x => x.idMembro > 0)
                                  .Select(x => new { x.id, x.saldoAtual, x.dtAtualizacaoSaldo})
                                  .FirstOrDefault()
                                  .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();
            
            if (ViewModel.Associado == null) {
                return RedirectToAction("cadastrar");
            }
            
            if (ViewModel.Associado.Pessoa.flagTipoPessoa == "J") {
                return RedirectToAction("editar", "NaoAssociadoCadastroPJ", new { ViewModel.Associado.id });
            }
            
            ViewModel.carregarConfiguracoes();
            
            ViewModel.carregaDados(true);
            
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

            var ViewModel = new NaoAssociadoCadastroPFForm();

            if (id > 0) {
                ViewModel.Associado = this.ONaoAssociadoBL.listar("", "").Where(x => x.id == id)
                    .Include(x => x.UsuarioCadastro)
                    .Include(x => x.Unidade)
                    .Include(x => x.Pessoa.CidadeOrigem).FirstOrDefault();

                if (ViewModel.Associado == null){
                    return RedirectToAction("cadastrar");
                }

                if (ViewModel.Associado.Pessoa.flagTipoPessoa == "J"){
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
        public ActionResult salvarCadastro(NaoAssociadoCadastroPFForm ViewModel) {

            var flagEdicao = ViewModel.Associado.id > 0;

            ViewModel.carregaDados(flagEdicao);

            ViewModel.listaCampos.bind(Request.Form);

            if (!ModelState.IsValid || ViewModel.listaCampos.Any(x => x.flagValidado == false)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Algumas informações não passaram na validação, verifique abaixo."));

                return View("aba-dados-cadastrais", ViewModel);
            }

            var listaEmails = ViewModel.Associado.Pessoa.listaEmails.ToList();
            
            listaEmails.ForEach(x => { x.idPessoa = ViewModel.Associado.idPessoa; });

            var ValidacaoEmail = this.ValidadorEmailBL.validar(listaEmails, false);

            if (ValidacaoEmail.flagError) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", string.Join("<br />", ValidacaoEmail.listaErros)));

                return View("aba-dados-cadastrais", ViewModel);
                
            }

            var dbAssociado = this.ONaoAssociadoBL.carregar(ViewModel.Associado.id).condicoesSeguranca().FirstOrDefault();

            if (dbAssociado != null && dbAssociado.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Você não tem permissão para acessar esse cadastro."));

                return View("aba-dados-cadastrais", ViewModel);

            }

            ViewModel = ViewModel.atribuirValoresFixos(ViewModel);

            ViewModel.Associado.Pessoa.flagTipoPessoa = "F";

            ViewModel.Associado.Pessoa.idTipoDocumento = TipoDocumentoConst.CPF;

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