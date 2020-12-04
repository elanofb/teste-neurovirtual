using System;
using System.Linq;
using BLL.Services;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public class PessoaVWBL : DefaultBL, IPessoaVWBL {

        // 
        public IQueryable<PessoaVW> query(int? idOrganizacaoParam = null) {

            var query = from Pes in db.PessoaVW select Pes;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //
        public PessoaVW carregar(int idPessoa) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.idPessoa == idPessoa);

        }

        //
        public IQueryable<PessoaVW> listar(string valorBusca, int? idOrganizacaoInf = null) {

            var query = this.query().condicoesSeguranca();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca));
            }

            if (idOrganizacaoInf.toInt() == 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }

            return query;
        }
    }
}