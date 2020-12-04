using System;
using System.Data.Entity;
using System.Linq;
using DAL.Entities;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.UsuariosUnidades {

    public class UsuarioUnidadeBL : DefaultBL , IUsuarioUnidadeBL  {

		//
		public UsuarioUnidadeBL(){

		}

        public UsuarioUnidade carregar(int id) {

            var query = (from tb_usuario_unidade in db.UsuarioUnidade
                         where tb_usuario_unidade.flagExcluido == "N"
                          && tb_usuario_unidade.id == id 
                         select tb_usuario_unidade);

            return query.FirstOrDefault();
        }

		//ListaGem
		public IQueryable<UsuarioUnidade> listar(int? idUsuario, int? idUnidade, string flagExcluido = "N") {

			var query = (from MotUnid in db.UsuarioUnidade.Include(x => x.Unidade)
                                                         .Include(x => x.UsuarioSistema)
						where MotUnid.flagExcluido == flagExcluido
						select MotUnid);

            if (idUsuario > 0) {
                query = query.Where(x => x.idUsuario == idUsuario);
            }

            if (idUnidade > 0) {
                query = query.Where(x => x.idUnidade == idUnidade);
            }
            			
			return query;
		}

		/**
		*
		*/

		public void salvar(int idUsuario, int idUnidade) {
			
			UsuarioUnidade OUsuarioUnidade = this.listar(idUsuario, idUnidade).FirstOrDefault() ?? new UsuarioUnidade();

            //Não irá cadastrar novamente algo que já está cadastrado
            if(OUsuarioUnidade.id > 0) {
                return;
            }

            OUsuarioUnidade.setDefaultInsertValues();
            OUsuarioUnidade.idUnidade = idUnidade;
            OUsuarioUnidade.idUsuario = idUsuario;
            this.db.UsuarioUnidade.Add(OUsuarioUnidade);
            this.db.SaveChanges();
		}


		//
		public void excluir(int id) {

            var OUsuarioUnidade = this.carregar(id);

            if (OUsuarioUnidade == null) {
                return;
            }

            OUsuarioUnidade.idUsuarioAlteracao = User.id();
            OUsuarioUnidade.dtAlteracao = DateTime.Now;
            OUsuarioUnidade.flagExcluido = "S";
            this.db.SaveChanges();
		}
	}
}