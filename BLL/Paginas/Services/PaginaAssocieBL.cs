using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Paginas;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Paginas {

    public class PaginaAssocieBL : DefaultBL, IPaginaAssocieBL {

        //
        public PaginaAssocieBL() {

        }

        //
        public PaginaAssocie carregar(int idOrganizacaoParam = 0) {

            if (User.idOrganizacao() > 0) {
                idOrganizacaoParam = User.idOrganizacao();
            }

            var query = db.PaginaAssocie
                          .Include(x => x.Organizacao).Include(x => x.UsuarioCadastro)
                          .Where(x => x.dtExclusao == null);

            query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

            var OPaginaAssocie = query.OrderByDescending(x => x.id).FirstOrDefault();

            return OPaginaAssocie;

        }

        //
        public IQueryable<PaginaAssocie> listar(int idOrganizacao) {

            var query = db.PaginaAssocie
                          .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
                          .Where(x => x.dtExclusao == null).AsNoTracking();

            if (idOrganizacao > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
            }

            return query;
        }

        /// <summary>
        /// Salvar e remover os registros anteriores.
        /// </summary>
        public bool salvar(PaginaAssocie OPaginaAssocie) {

            OPaginaAssocie.titulo = OPaginaAssocie.titulo.abreviar(255);

            OPaginaAssocie.texto = OPaginaAssocie.texto.abreviar(7000);

            OPaginaAssocie.setDefaultInsertValues();

            db.PaginaAssocie.Add(OPaginaAssocie);

            db.SaveChanges();

            bool flagSucesso = OPaginaAssocie.id > 0;

            int? idOrganizacao = OPaginaAssocie.idOrganizacao;

            if (flagSucesso) {

                db.PaginaAssocie
                  .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OPaginaAssocie.id)
                  .Update(x => new PaginaAssocie { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });
            }

            return (OPaginaAssocie.id > 0);

        }
    }
}