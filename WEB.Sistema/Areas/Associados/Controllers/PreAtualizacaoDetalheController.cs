using System;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using DAL.Pessoas;
using WEB.Areas.Associados.ViewModels;
using WEB.Areas.Associados.Models.ViewModels;

namespace WEB.Areas.Associados.Controllers {
    [OrganizacaoFilter]
    public class PreAtualizacaoDetalheController : Controller{

        //Atributos
        private IAssociadoCadastroBL _AssociadoCadastroBL;
        private IAssociadoBL _AssociadoBL;

        //Propriedades
        private IAssociadoCadastroBL OAssociadoCadastroBL => _AssociadoCadastroBL = _AssociadoCadastroBL ?? new AssociadoCadastroBL();
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();

        // GET: Associados/AssociadoCadastroPF
        public ActionResult index(int? id){

            if (id.toInt() == 0){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }

            var ViewModel = new PreAtualizacaoDetalheVM();

            ViewModel.carregarHistorico(id);

            if (ViewModel.OHistoricoAtualizacao.id == 0){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }                       
            
            ViewModel.carregarDadosAssociado();

            if (ViewModel.OAssociado.id == 0){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }
            
            if (ViewModel.OAssociado.Pessoa.flagTipoPessoa == "J"){
                return RedirectToAction("editar-cadastro-pj", new{id = ViewModel.OHistoricoAtualizacao.id});
            }
            
            return RedirectToAction("editar-cadastro-pf", new{id = ViewModel.OHistoricoAtualizacao.id});

        }
        
        [HttpGet, ActionName("editar-cadastro-pf")]
        public ActionResult editarCadastroPF(int? id){
            
            var VMPreAtualizacaoDetalhe = new PreAtualizacaoDetalheVM();
            
            VMPreAtualizacaoDetalhe.carregarHistorico(id);
            
            if (VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.id == 0){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }
            
            VMPreAtualizacaoDetalhe.carregarDadosAssociado();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF = new AssociadoPreAtualizacaoCadastroPFForm();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.Associado = VMPreAtualizacaoDetalhe.OAssociado;
            
            if (VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.dtAnalise == null){
                VMPreAtualizacaoDetalhe.FormAssociadoPF.carregarInformacoesAssociado(VMPreAtualizacaoDetalhe.OAssociado.id);    
            }else{
                VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual = VMPreAtualizacaoDetalhe.retonarDadosAlterados();
            }
            
            //Salvar informações atuais do associado para guardar histórico das alterações
            VMPreAtualizacaoDetalhe.salvarHistoricoAssociado(VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual);                           
            
            if (VMPreAtualizacaoDetalhe.FormAssociadoPF.Associado == null){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }
            
            if (VMPreAtualizacaoDetalhe.FormAssociadoPF.Associado.Pessoa.flagTipoPessoa == "J"){
                return RedirectToAction("editar-cadastro-pj", new{id = VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.id});
            }
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.carregarConfiguracoes();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.carregaDados(true);
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual.Pessoa = VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual.Pessoa ?? new Pessoa();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual.Pessoa.limparListas();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.AssociadoAtual.limparListas();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPF.carregarValorCampos(VMPreAtualizacaoDetalhe.FormAssociadoPF);
            
            return View("editar-cadastro-pf", VMPreAtualizacaoDetalhe);

        }
        
        [HttpGet, ActionName("editar-cadastro-pj")]
        public ActionResult editarCadastroPJ(int? id){
            
            var VMPreAtualizacaoDetalhe = new PreAtualizacaoDetalheVM();
            
            VMPreAtualizacaoDetalhe.carregarHistorico(id);
            
            if (VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.id == 0){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }
            
            VMPreAtualizacaoDetalhe.carregarDadosAssociado();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ = new AssociadoPreAtualizacaoCadastroPJForm();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.Associado = VMPreAtualizacaoDetalhe.OAssociado;
            
            if (VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.dtAnalise == null){
                VMPreAtualizacaoDetalhe.FormAssociadoPJ.carregarInformacoesAssociado(VMPreAtualizacaoDetalhe.OAssociado.id);    
            }else{
                VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual = VMPreAtualizacaoDetalhe.retonarDadosAlterados();
            }
            
            //Salvar informações atuais do associado para guardar histórico das alterações
            VMPreAtualizacaoDetalhe.salvarHistoricoAssociado(VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual);                           
            
            if (VMPreAtualizacaoDetalhe.FormAssociadoPJ.Associado == null){
                return RedirectToAction("index", "PreAtualizacaoConsulta");
            }
            
            if (VMPreAtualizacaoDetalhe.FormAssociadoPJ.Associado.Pessoa.flagTipoPessoa == "J"){
                return RedirectToAction("editar-cadastro-pf", new{id = VMPreAtualizacaoDetalhe.OHistoricoAtualizacao.id});
            }
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.carregarConfiguracoes();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.carregaDados(true);
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual.Pessoa = VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual.Pessoa ?? new Pessoa();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual.Pessoa.limparListas();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.AssociadoAtual.limparListas();
            
            VMPreAtualizacaoDetalhe.FormAssociadoPJ.carregarValorCampos(VMPreAtualizacaoDetalhe.FormAssociadoPJ);
            
            return View("editar-cadastro-pj", VMPreAtualizacaoDetalhe);

        }
    }
}