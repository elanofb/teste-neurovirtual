using System;
using System.Collections.Generic;
using System.Security.Principal;
using Newtonsoft.Json;

namespace WEB.Areas.Pessoas.ViewModels {

    public class PessoaContaBancariaConsultaVM  {
        
        // Propriedades
        public ContaBancariaListaRetorno RetornoAPI { get; set; }        
        
        // Constantes
        private string urlServico => "Pessoas/ContaBancariaLista";
        private IPrincipal User => HttpContextFactory.Current.User;
        
        // Construtor
        public PessoaContaBancariaConsultaVM(int? idOrganizacaoInformado = null)  {
            
        }
        
        //
        public void carregarContas(int idPessoa) {
            
            try{
                
                var dados = $"idPessoa={idPessoa}";

                string jsonRetorno = "";// this.OApiConex.get(urlServico, dados);
                                    
                RetornoAPI = JsonConvert.DeserializeObject<ContaBancariaListaRetorno>(jsonRetorno);
                
            } catch (Exception ex) {
                
                UtilLog.saveError(ex, "Erro ao consumir serviço listagem de contas bancárias");
                
                
            }
               
            RetornoAPI = RetornoAPI ?? new ContaBancariaListaRetorno();            

        }
        
    }
}