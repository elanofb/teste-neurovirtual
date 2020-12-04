using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Unidades;
using EntityFramework.Caching;
using EntityFramework.Extensions;
using UTIL.Resources;
using DAL.Pessoas;
using DAL.Documentos;
using DAL.Permissao.Security.Extensions;
using BLL.Services;
using DAL.Arquivos;

namespace BLL.Unidades {

	public class UnidadeConsultaBL : DefaultBL, IUnidadeConsultaBL {
		
		// Atributos        

        // Propriedades	    
		
		//
		public UnidadeConsultaBL(){
			
		}
		
		/// <summary>
		/// Montagem de query base para consulta de registros
		/// </summary>
		public IQueryable<Unidade> query(int? idOrganizacaoParam = null) {
            
			var query = from U in db.Unidade
				select U;
            
			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}
			
			return query;
		}
		
        //Verificar se já existe alguma unidade cadastrada na organização
        public bool existe(int idOrganizacaoParam) {
						            
            var query = from Ass in db.Unidade                                    
                        where                            
                            Ass.flagExcluido == "N" && 
                            Ass.Pessoa.flagExcluido == "N"
		            		&& Ass.idOrganizacao == idOrganizacaoParam
                        select Ass;			                        

            var OUnidade = query.Take(1).FirstOrDefault();

            return (OUnidade != null);
        }
       
      
	}
}