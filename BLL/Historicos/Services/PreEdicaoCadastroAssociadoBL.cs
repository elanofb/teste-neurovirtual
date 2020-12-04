using System;
using System.Security.Principal;
using System.Web;
using BLL.Historicos.Interfaces;
using BLL.Services;
using DAL.Associados;
using DAL.Historicos;
using DAL.Permissao.Security.Extensions;
using Newtonsoft.Json;

namespace BLL.Historicos.Services {
    
    public class PreEdicaoCadastroAssociadoBL : IPreEdicaoCadastroBL {
        
        //Atributos
        private IHistoricoAtualizacaoCadastroBL _HistoricoAtualizacaoCadastroBL;        
        
        //Propriedades
        private IHistoricoAtualizacaoCadastroBL OHistoricoAtualizacaoCadastroBL => _HistoricoAtualizacaoCadastroBL = _HistoricoAtualizacaoCadastroBL ?? new HistoricoAtualizacaoCadastroBL();        
        
        private IPrincipal User => HttpContextFactory.Current.User;
        
        public UtilRetorno salvar(object Origem){

            UtilRetorno ORetorno = new UtilRetorno();

            Associado OAssociado = Origem.ToJsonObject<Associado>();
            
            if (OAssociado.id == 0){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("O associado informado não foi localizado!");
                return ORetorno;
                
            }
            
            HttpRequestBase req = HttpContextFactory.Current.Request;
            string browserName = req.Browser.Browser;
            
            string infoCadastro = JsonConvert.SerializeObject(Origem, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            
            HistoricoAtualizacao OHistoricoAtualizacao = new HistoricoAtualizacao();
            
            OHistoricoAtualizacao.idOrganizacao = OAssociado.idOrganizacao;
            OHistoricoAtualizacao.idAssociado = OAssociado.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR ? OAssociado.id : (int?) null;
            OHistoricoAtualizacao.idPessoa = OAssociado.idPessoa;
            OHistoricoAtualizacao.idNaoAssociado = OAssociado.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE ? OAssociado.id : (int?) null;
            OHistoricoAtualizacao.dtAtualizacao = DateTime.Now;
            OHistoricoAtualizacao.emailOrigem = User.email();
            OHistoricoAtualizacao.informacoes = infoCadastro;
            OHistoricoAtualizacao.browser = browserName;
            
            bool flagSucesso = this.OHistoricoAtualizacaoCadastroBL.salvar(OHistoricoAtualizacao);
               
            if (!flagSucesso){
                
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não foi possível realizar a alteração do cadastro, tente novamente!");
                return ORetorno;
                
            }
            
            ORetorno.flagError = false;            
            return ORetorno;
            
        }
        
    }
} 