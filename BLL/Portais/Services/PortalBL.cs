using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using DAL.Organizacoes;
using EntityFramework.Extensions;
using DAL.Pessoas;
using BLL.Services;
using DAL.Entities;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;
using DAL.Portais;
using UTIL.Resources;

namespace BLL.Portais{

	public class PortalBL : DefaultBL, IPortalBL {

		// Atributos
	    private IArquivoUploadBL _ArquivoUploadBL;

        // Propriedades
	    private IArquivoUploadBL OArquivoUploadBL => _ArquivoUploadBL = _ArquivoUploadBL ?? new ArquivoUploadBL();

        //
        public PortalBL(){
		}

		//Carregar registro a partir do ID
		public Portal carregar(int id) {
			
			var query = from Unid in db.Portal
                        .Include(x => x.Organizacao)
                        where Unid.id == id && Unid.dtExclusao == null
						select Unid;

		    query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//Listagem de registro a partir de parametros
		public IQueryable<Portal> listar(string valorBusca, bool? ativo) {

            var query = from Unid in db.Portal
										.Include(x => x.Organizacao)
						where Unid.dtExclusao == null
						select Unid;

		    query = query.condicoesSeguranca();

			if (!string.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.url.Contains(valorBusca));
			}

            if (ativo.HasValue) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		/// <summary>
        /// Enviar para salvar ou atualizar um registro no banco de dados e salvar o logotipo da Portal
        /// </summary>
		public bool salvar(Portal OPortal) {

            bool flagSucesso = false;

		    flagSucesso = OPortal.id == 0 ? this.inserir(OPortal) : this.atualizar(OPortal);

            return (flagSucesso);
		}

        /// <summary>
        /// Cadastro de um novo registro no banco de dados
        /// </summary>
        private bool inserir(Portal OPortal) {

            OPortal.setDefaultInsertValues();
            
            db.Portal.Add(OPortal);

            db.SaveChanges();

            return (OPortal.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(Portal OPortal) {

            Portal dbPortal = this.carregar(OPortal.id);

            var entryPortal = db.Entry(dbPortal);
            OPortal.setDefaultUpdateValues();
            entryPortal.CurrentValues.SetValues(OPortal);
            entryPortal.State = EntityState.Modified;
            entryPortal.ignoreFields();
            
            db.SaveChanges();

            return (OPortal.id > 0);
        }
        
        /// <summary>
        /// Ativar ou desativar um registro
        /// </summary>
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo != true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Carregar registro a partir do ID
		public IQueryable<Portal> existe(string descricao, int id) {
			
			var query = from Unid in db.Portal
                        .Include(x => x.Organizacao)
                        where Unid.id != id && Unid.dtExclusao == null && Unid.descricao == descricao
						select Unid;


			return query;
		}

        /// <summary>
        ///  Remover o registro de forma lógica no sistema
        /// </summary>
        public UtilRetorno excluir(int id) {

            var idUsuarioExclusao = User.id();

            db.Portal.Where(x => x.id == id)
                        .Update(x => new Portal { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioExclusao});   

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}