using System;
using System.Linq;
using DAL.Organizacoes;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Organizacoes {

	public class AcessoRecursoGrupoOrganizacaoBL : DefaultBL, IAcessoRecursoGrupoOrganizacaoBL {
        
        //
        public AcessoRecursoGrupoOrganizacaoBL(){

		}

		//
		public IQueryable<AcessoRecursoGrupoOrganizacao> listar(int idOrganizacao) {
            
            var query = from Obj in db.AcessoRecursoGrupoOrganizacao
						where Obj.dtExclusao == null && Obj.idOrganizacao == idOrganizacao
                        select Obj;
            
            return query;
		}

        //
	    public bool salvar(AcessoRecursoGrupoOrganizacao OAcessoRecursoGrupoOrganizacao) {

            // Excluir registros existentes
            this.excluir(OAcessoRecursoGrupoOrganizacao.idOrganizacao, OAcessoRecursoGrupoOrganizacao.idRecursoGrupo);
            
	        OAcessoRecursoGrupoOrganizacao.dtAtivacao = DateTime.Now;

            OAcessoRecursoGrupoOrganizacao.idUsuarioAtivacao = User.id();
            
	        db.AcessoRecursoGrupoOrganizacao.Add(OAcessoRecursoGrupoOrganizacao);

	        db.SaveChanges();

	        bool flagSucesso = OAcessoRecursoGrupoOrganizacao.id > 0;
            
	        return flagSucesso;

	    }

        public void excluir(int idOrganizacao, int idRecursoGrupo) {
            
            db.AcessoRecursoGrupoOrganizacao
                .Where(x => x.idOrganizacao == idOrganizacao && x.idRecursoGrupo == idRecursoGrupo)
                .Update(x => new AcessoRecursoGrupoOrganizacao {

                    dtExclusao = DateTime.Now,
                    idUsuarioExclusao = User.id()

                });

        }

    }
}