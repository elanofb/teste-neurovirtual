using System;
using System.Linq;
using BLL.Services;
using DAL.Profissoes;

namespace BLL.Profissoes {

    public class ProfissaoConsultaBL : DefaultBL, IProfissaoConsultaBL {

        //
        public ProfissaoConsultaBL(){

        }

        //
        public IQueryable<Profissao> query(int? idOrganizacaoParam = null) {

            var query = from OEmpresa in db.Profissao
                        where OEmpresa.dtExclusao == null
                        select OEmpresa;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregar empresa pelo CNPJ
        public Profissao carregar(int id) {

            var query = this.query().condicoesSeguranca();
            
            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem das empresas com possibilidade de busca pelos filtros
        public IQueryable<Profissao> listar(string valorBusca, bool? ativo) {

            var query = this.query().condicoesSeguranca();
            
            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(!ativo.isEmpty()) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar existencia de dados para evitar duplicidades
        public bool existe(string descricao, int idDesconsiderado) {

            var query = from Emp in db.Profissao.AsNoTracking()
                where
                    Emp.id != idDesconsiderado &&
                    Emp.dtExclusao == null
                select
                    Emp;

            query = query.condicoesSeguranca();

            if(!String.IsNullOrEmpty(descricao)) {
                query = query.Where(x => x.descricao == descricao);
            }

            bool flagExiste = (query.Any());
            return flagExiste;
        }

    }
}