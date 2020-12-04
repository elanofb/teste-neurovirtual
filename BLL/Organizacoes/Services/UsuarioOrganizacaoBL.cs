using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Organizacoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Organizacoes {

    public class UsuarioOrganizacaoBL : DefaultBL , IUsuarioOrganizacaoBL  {

		//
		public UsuarioOrganizacaoBL(){

		}

        public UsuarioOrganizacao carregar(int id) {

            var query = (from tb_usuario_unidade in db.UsuarioOrganizacao
                         where tb_usuario_unidade.flagExcluido == "N"
                          && tb_usuario_unidade.id == id 
                         select tb_usuario_unidade);

            return query.FirstOrDefault();
        }

		//ListaGem
		public IQueryable<UsuarioOrganizacao> listar(int? idUsuario, int? idUnidade, string flagExcluido = "N") {

			var query = (from MotUnid in db.UsuarioOrganizacao.Include(x => x.Organizacao)
                                                         .Include(x => x.UsuarioSistema)
						where MotUnid.flagExcluido == flagExcluido
						select MotUnid);

            if (idUsuario > 0) {
                query = query.Where(x => x.idUsuario == idUsuario);
            }

            if (idUnidade > 0) {
                query = query.Where(x => x.idOrganizacao == idUnidade);
            }
            			
			return query;
		}

		/**
		*
		*/

		public void salvar(int idUsuario, int idOrganizacaoParam) {
			
			UsuarioOrganizacao OUsuarioOrganizacao = this.listar(idUsuario, idOrganizacaoParam).FirstOrDefault() ?? new UsuarioOrganizacao();

            //Não irá cadastrar novamente algo que já está cadastrado
            if(OUsuarioOrganizacao.id > 0) {
                return;
            }

            OUsuarioOrganizacao.setDefaultInsertValues();
            OUsuarioOrganizacao.idOrganizacao = idOrganizacaoParam;
            OUsuarioOrganizacao.idUsuario = idUsuario;
            this.db.UsuarioOrganizacao.Add(OUsuarioOrganizacao);
            this.db.SaveChanges();
		}


		//
		public void excluir(int id) {

            var OUsuarioOrganizacao = this.carregar(id);

            if (OUsuarioOrganizacao == null) {
                return;
            }

            OUsuarioOrganizacao.idUsuarioAlteracao = User.id();
            OUsuarioOrganizacao.dtAlteracao = DateTime.Now;
            OUsuarioOrganizacao.flagExcluido = "S";
            this.db.SaveChanges();
		}
	}
}