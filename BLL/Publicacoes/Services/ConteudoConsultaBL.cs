using System;
using System.Linq;
using BLL.Services;
using DAL.Publicacoes;

namespace BLL.Publicacoes {

	public class ConteudoConsultaBL : DefaultBL , IConteudoConsultaBL {

		//
		public ConteudoConsultaBL() {
		}

        // 
        public IQueryable<Conteudo> query(int? idOrganizacaoParam = null) {

            var query = from C in db.Conteudo
                        where C.dtExclusao == null
                        select C;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro pelo ID
        public Conteudo carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<Conteudo> listar(string valorBusca, bool? ativo = true) {
                
            var query = this.query().condicoesSeguranca();
            
            if (!valorBusca.isEmpty()) {
                query = query.Where(x => x.titulo.Contains(valorBusca));
            }
            
            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }
	    
	    //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
	    public bool existe(string idInterno, int id, int idOrganizacao) {
            		
	        var query = from CON in db.Conteudo
	            where CON.idInterno == idInterno && CON.id != id && CON.idOrganizacao == idOrganizacao && CON.dtExclusao == null 
	            select CON;
            
	        var Conteudo = query.Take(1).FirstOrDefault();
            
	        return (Conteudo != null);
	    }
       
       
	}
}