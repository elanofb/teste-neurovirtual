using System;
using System.Linq;
using BLL.Services;
using DAL.Organizacoes;

namespace BLL.Organizacoes {

    public class StatusOrganizacaoBL: DefaultBL, IStatusOrganizacaoBL {


        //Carregamento de registro pelo ID
        public StatusOrganizacao carregar(int id) {

            var query = from P in db.StatusOrganizacao
                where P.flagExcluido == false && P.id == id
                select P;

            return query.FirstOrDefault();
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<StatusOrganizacao> listar(string valorBusca, bool? ativo) {

            var query = from P in db.StatusOrganizacao
                        where P.flagExcluido == false
                        select P;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }
    }
}