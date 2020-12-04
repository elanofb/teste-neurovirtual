using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL;
using BLL.Services;
using DAL.Configuracoes;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesTextos.Models.Forms;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoPromocaoController : Controller {
        
        private IConfiguracaoPromocaoConsultaBL _IConfiguracaoPromocaoConsultaBL;
        private IConfiguracaoPromocaoCadastroBL _IConfiguracaoPromocaoCadastroBL;
        private IConfiguracaoPromocaoExclusaoBL _IConfiguracaoPromocaoExclusaoBL;
        
        private IConfiguracaoPromocaoConsultaBL ConsultaConfiguracaoPromocao 
            => _IConfiguracaoPromocaoConsultaBL = _IConfiguracaoPromocaoConsultaBL 
            ?? new ConfiguracaoPromocaoConsultaBL();
                
        private IConfiguracaoPromocaoCadastroBL CadastroConfiguracaoPromocao 
            => _IConfiguracaoPromocaoCadastroBL = _IConfiguracaoPromocaoCadastroBL 
            ?? new ConfiguracaoPromocaoCadastroBL();
        
        private IConfiguracaoPromocaoExclusaoBL ExclusaoConfiguracaoPromocao 
            => _IConfiguracaoPromocaoExclusaoBL = _IConfiguracaoPromocaoExclusaoBL 
            ?? new ConfiguracaoPromocaoExclusaoBL();
        
        public ActionResult index() => RedirectToAction("editar");
        
        [HttpGet]
        public ActionResult editar(int? id) {
            var Formulario = new ConfiguracaoPromocaoForm();
            
            Formulario.ConfiguracaoPromocao = ConsultaConfiguracaoPromocao.listar()
                                                                          .Select(
                                                                              E => new {
                                                                                  E.id,
                                                                                  E.descricao,
                                                                                  E.ativo,
                                                                                  E.valorPremioNovoMembro,
                                                                                  E.dtInicioPremioNovoMembro,
                                                                                  E.dtFimPremioNovoMembro,
                                                                                  E.dtCadastro,
                                                                                  E.idUsuarioCadastro,
                                                                                  UsuarioCadastro = new {
                                                                                      id   = E.idUsuarioCadastro,
                                                                                      nome = E.UsuarioCadastro.nome,
                                                                                  }
                                                                                  
                                                                              }
                                                                          ).FirstOrDefault()
                                                                          .ToJsonObject<ConfiguracaoPromocao>() ?? new ConfiguracaoPromocao();
            
            return View(Formulario);
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoPromocaoForm Form) {
            
            if (!ModelState.IsValid) {
                return View(Form);
            }
            
            if (Form.ConfiguracaoPromocao.id != 0) {
                var RetornoExclusao = ExclusaoConfiguracaoPromocao.excluir(Form.ConfiguracaoPromocao.id);
                
                if(RetornoExclusao.flagError) {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", RetornoExclusao.listaErros.FirstOrDefault()));
                
                    return View(Form);
                }
            }
            
            // esse trecho é para resetar o id para que o metodo salvar não sobrescreva o registro
            Form.ConfiguracaoPromocao.id = 0; 
            var RetornoCadastro = CadastroConfiguracaoPromocao.salvar(Form.ConfiguracaoPromocao);
            
            if(!RetornoCadastro.flagError) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", RetornoCadastro.listaErros.FirstOrDefault()));
                
                return RedirectToAction("editar", new { id = Form.ConfiguracaoPromocao.id });
            }
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
            return View(Form);
        }
    }
}