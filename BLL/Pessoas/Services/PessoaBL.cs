using System;
using System.Linq;
using DAL.Pessoas;
using EntityFramework.Extensions;
using BLL.Services;

namespace BLL.Pessoas {

    public class PessoaBL : DefaultBL, IPessoaBL {

        //
        public PessoaBL() {
        }
        
        /// <summary>
        /// Montagem de query base para consulta de registros
        /// </summary>
        public IQueryable<Pessoa> query(int? idOrganizacaoParam = null) {
            
            var query = from P in db.Pessoa
                        select P;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        /// <summary>
        /// Carregar pelo ID da pessoa. Existem regras de segurança para o sistema
        /// </summary>
		public Pessoa carregar(int id) {

            var query = from P in db.Pessoa
                        where P.id == id && P.flagExcluido == "N"
                        select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Listagem de pessoas com condicoes de segurança para o sistema
        /// </summary>
        public IQueryable<Pessoa> listar(string valorBusca, string ativo) {

            var query = from P in db.Pessoa
                        where P.flagExcluido == "N"
                        select P;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            query = query.condicoesSeguranca();

            return query;
        }

        /**
		 * Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
		 */

        public bool existe(string descricao, int id) {

            var query = from P in db.Pessoa
                        where P.nome == descricao && P.id != id && P.flagExcluido == "N"
                        select P;
            var OPessoa = query.Take(1).FirstOrDefault();
            return (OPessoa == null ? false : true);
        }

        /*
		 * Varificar existência por cpf, documento, emailPrincipal e emailSecundario
		 */

        public Pessoa existe(string valor) {

            var query = from P in db.Pessoa
                        where (P.nroDocumento == valor || P.emailPrincipal == valor || P.emailSecundario == valor) && P.flagExcluido == "N"
                        select P;

            var OPessoa = query.Take(1).FirstOrDefault();
            return OPessoa;
        }

        //
        public bool excluir(int[] ids) {
            db.Pessoa.Where(x => ids.Contains(x.id))
                .Update(x => new Pessoa { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = db.Pessoa.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
            return (listaCheck.Count == 0);
        }
    }
}