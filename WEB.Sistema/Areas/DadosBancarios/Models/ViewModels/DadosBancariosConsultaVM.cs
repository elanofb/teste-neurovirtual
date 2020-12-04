using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using BLL.Services;
using DAL.DadosBancarios;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.DadosBancarios.ViewModels {
        
    public class DadosBancariosConsultaVM {
        
        // Atributos        
        private IDadoBancarioConsultaBL _IDadoBancarioConsultaBL;
        
        // Serviços        
        private IDadoBancarioConsultaBL ODadoBancarioConsultaBL => _IDadoBancarioConsultaBL = _IDadoBancarioConsultaBL ?? new DadoBancarioConsultaBL();
        
        // Propriedades
        public int idPessoa { get; set; }        
        public List<DadoBancario> listaDadosBancarios{ get; set; }
        
        private IPrincipal User => HttpContextFactory.Current.User;
        
        public DadosBancariosConsultaVM(){
            this.listaDadosBancarios = new List<DadoBancario>();
        }

        public void montarLista(){
            
            var query = this.montarQuery();
            this.listaDadosBancarios = this.filtrarCampos(query);
        }
        
        private IQueryable<DadoBancario> montarQuery(){
            
            var query = this.ODadoBancarioConsultaBL.query(User.idOrganizacao()).Where(x => x.idPessoa == idPessoa);

            return query;
        }
        
        private List<DadoBancario> filtrarCampos(IQueryable<DadoBancario> query){
            
            var listaFiltrada = query.Select(x => new {
                x.id,
                x.idOrganizacao,
                x.idPessoa,
                x.ativo,
                x.idBanco,                
                Banco = new { x.Banco.descricao },
                x.flagTipoConta,
                x.nroAgencia,
                x.nroConta,
                x.digitoConta,
                x.nomeTitular,
                x.documentoTitular
                               
            }).OrderByDescending(x => x.id).ToListJsonObject<DadoBancario>();                        
            
            return listaFiltrada;
        }       
    }
}