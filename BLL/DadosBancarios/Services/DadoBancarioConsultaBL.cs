using System;
using System.Data.Entity;
using System.Linq;
using BLL.DadosBancarios.Interfaces;
using BLL.Services;
using DAL.DadosBancarios;

namespace BLL.DadosBancarios.Services {
    
    public class DadoBancarioConsultaBL : DefaultBL, IDadoBancarioConsultaBL {
        
        public IQueryable<DadoBancario> query(int? idOrganizacaoParam = null) {

            var query = from E in db.DadoBancario
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

        public DadoBancario carregar(int id) {

            var query = this.query().Include(x => x.Banco).condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }

        public IQueryable<DadoBancario> listar(string valorBusca, bool? ativo) {

            var query = this.query().condicoesSeguranca();

            if (!string.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nroAgencia.Contains(valorBusca) || x.nroConta.Contains(valorBusca) || x.nomeTitular.Contains(valorBusca));
            }

            if (ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query.AsNoTracking();
        }
                
    }
}