using System;
using System.Linq;
using BLL.Services;
using DAL.Emails;

namespace BLL.Emails {
    
    public class MensagemEmailConsultaBL : DefaultBL, IMensagemEmailConsultaBL {
        
        // 
        public IQueryable<MensagemEmail> query(int? idOrganizacaoParam = null) {
            
            var query = from ME in db.MensagemEmail
                where ME.dtExclusao == null
                select ME;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }
        
        //Carregamento de registro pelo ID
        public MensagemEmail carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }
        
        //Listagem de registros de acordo com filtros
        public IQueryable<MensagemEmail> listar(string codigoIdentificacao, int? idReferencia = null) {
            
            var query = this.query().condicoesSeguranca();
            
            if (!String.IsNullOrEmpty(codigoIdentificacao)) {
                query = query.Where(x => x.codigoIdentificacao == codigoIdentificacao);
            }
            
            if (idReferencia.HasValue){
                query = query.Where(x => x.idReferencia == idReferencia);
            }
            
            query = query.Where(x => x.ativo == true);            

            return query;
        }
        
        //Verificar se já existe um registro com o codigo e organização informado
        public bool existe(string codigoIdentificacao, int idOrganizacao) {
                                                
            var query = from ME in db.MensagemEmail
                where ME.codigoIdentificacao == codigoIdentificacao && ME.idOrganizacao == idOrganizacao && ME.dtExclusao == null
                select ME;
            var OMensagemEmail = query.Take(1).FirstOrDefault();
            return (OMensagemEmail != null);
        }

    }
}