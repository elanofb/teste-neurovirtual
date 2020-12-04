using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;
using DAL.Institucionais;
using BLL.Arquivos;
using EntityFramework.Extensions;
using System.Data.Entity;
using DAL.Permissao.Security.Extensions;

namespace BLL.Institucionais {

    public class AssociacaoHistoriaBL : DefaultBL, IAssociacaoHistoriaBL {

        //Atributos
        private IArquivoUploadBL _ArquivoUploadBL;

        //Propriedades
        private IArquivoUploadBL OArquivoUploadBL { get { return (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL()); } }

        //
        public AssociacaoHistoriaBL() {
        }

        //Carregamento de registro único pelo ID
        public AssociacaoHistoria carregar(int idOrganizacao) {

            var query = db.AssociacaoHistoria
                            .Where(x => x.dtExclusao == null).AsNoTracking();

            query = idOrganizacao > 0 ? query.Where(x => x.idOrganizacao == idOrganizacao) : query.Where(x => x.idOrganizacao == null);

            var OAssociacaoHistoria = query.OrderByDescending(x => x.id).FirstOrDefault();

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //
        public IQueryable<AssociacaoHistoria> listar(int idOrganizacao, bool? ativo) {

            var query = from C in db.AssociacaoHistoria
                        where C.dtExclusao == null
                        select C;

            query = query.condicoesSeguranca();

            if (idOrganizacao > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(AssociacaoHistoria OAssociacaoHistoria) {

            OAssociacaoHistoria.setDefaultInsertValues();

            db.AssociacaoHistoria.Add(OAssociacaoHistoria);

            db.SaveChanges();

            bool flagSucesso = OAssociacaoHistoria.id > 0;

            int? idOrganizacao = OAssociacaoHistoria.idOrganizacao;

            if (flagSucesso) {

                db.AssociacaoHistoria
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.id != OAssociacaoHistoria.id)
                    .Update(x => new AssociacaoHistoria { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id() });
            }

            return (OAssociacaoHistoria.id > 0);
        }
        
    }
}