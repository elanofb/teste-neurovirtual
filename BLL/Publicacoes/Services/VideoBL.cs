using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web;
using BLL.Services;
using DAL.Publicacoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class VideoBL : DefaultBL, IVideoBL {

		//
		public VideoBL() {

		}

		//
		public IQueryable<Video> listar(string valorBusca, string ativo, int? idOrganizacaoParam = null) {
			
			var query = from B in db.Video.Include(x => x.Portal)
                        where B.flagExcluido == "N"
						select B;
            
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

			return query;
		}

		//
		public Video carregar(int id) {
			
			var query = db.Video.Where(x => x.id == id).Include(x => x.Portal);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

        //
		public bool existe(string descricao, int id) {
            
            var query = from C in db.Video
						where C.descricao == descricao && C.id != id && C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

            var OItem = query.Take(1).FirstOrDefault();

            return (OItem == null ? false : true);
		}


		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(Video OVideo, HttpPostedFileBase OImagem) {

            bool flagSucesso;

			if (OVideo.id == 0) {

                flagSucesso = this.inserir(OVideo);

                return flagSucesso;
			}

			flagSucesso = this.atualizar(OVideo);

			//if (OImagem != null && dbVideo.id > 0) { 

				//var listaThumbs = new List<ThumbDTO>();
				//listaThumbs.Add(new ThumbDTO{ folderName="sistema", height = 50, width = 0});
				//this.OArquivoUploadBL.salvarFoto(dbVideo.id, EntityTypes.V, OImagem);
			//}

			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(Video OVideo) { 
			
			OVideo.setDefaultInsertValues();

			db.Video.Add(OVideo);

            db.SaveChanges();

			return (OVideo.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(Video OVideo) {
            
			OVideo.setDefaultUpdateValues();

			//Localizar existentes no banco
			Video dbVideo = this.carregar(OVideo.id);		

            if (dbVideo == null) {
                return false;
            }

			var VideoEntry = db.Entry(dbVideo);

            VideoEntry.CurrentValues.SetValues(OVideo);

            VideoEntry.ignoreFields();

			db.SaveChanges();

            return (OVideo.id > 0);
		}

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
	        var retorno = new JsonMessageStatus();

	        var item = this.carregar(id);

	        if (item == null) {
		        retorno.error = true;
		        retorno.message = NotificationMessages.invalid_register_id;
	        } else {
		        item.ativo = (item.ativo == "S" ? "N" : "S");
		        db.SaveChanges();
		        retorno.active = item.ativo;
		        retorno.message = NotificationMessages.updateSuccess;
	        }
	        return retorno;
        }

		//
		public bool excluir(int[] ids) {
			db.Video.Where(x => ids.Contains(x.id))
				.Update(x => new Video { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = db.Video.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}
	}
}