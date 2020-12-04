using System;
using System.Data.Entity;
using System.Linq;
using BLL.Historicos.Interfaces;
using BLL.Services;
using DAL.Historicos;

namespace BLL.Historicos.Services {
    
    public class HistoricoAtualizacaoConsultaBL : DefaultBL, IHistoricoAtualizacaoConsultaBL {
        
        public IQueryable<HistoricoAtualizacao> query(int? idOrganizacaoParam = null) {

            var query = from E in db.HistoricoAtualizacao
                where !E.dtExclusao.HasValue
                select E;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        public HistoricoAtualizacao carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }

        public IQueryable<HistoricoAtualizacao> listar(int? idAssociado, int? idNaoAssociado, int? idPessoa) {
                
            var query = this.query().condicoesSeguranca();
            
            if (idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }
            
            if (idNaoAssociado > 0) {
                query = query.Where(x => x.idNaoAssociado == idNaoAssociado);
            }
            
            if (idPessoa > 0) {
                query = query.Where(x => x.idPessoa == idPessoa);
            }

            return query.AsNoTracking();
        }
                
    }
}