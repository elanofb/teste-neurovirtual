using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Json;
using System.Linq;
using DAL.Arquivos;
using DAL.Publicacoes;
using DAL.Entities;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using BLL.Arquivos;
using System.Web;
using BLL.Services;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class BannerBL : DefaultBL, IBannerBL {

		//Atributos
		private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

		//Propriedades
		private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();


		//Construtor
		public BannerBL(){
		}

		//Carregar um registro pelo ID
		public Banner carregar(int id) { 
			
			var query = db.Banner.Where(x => x.id == id && x.flagExcluido == "N").Include(x => x.Portal);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}


		//Listagem dos banners com a relação de imagens que os compõe
		public IQueryable<Banner> listar(string posicao, string valorBusca, string ativo) {

            var query = from Ban in db.Banner.Include(x => x.Portal)
                        where 
							Ban.flagExcluido == "N"
						select Ban;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(posicao)) { 
				query = query.Where(x => x.posicao == posicao);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) { 
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(Banner OBanner, HttpPostedFileBase OImagem) {

			bool flagSucesso = false;

		    if (OBanner.id > 0) {
		        flagSucesso = this.atualizar(OBanner);
            }

			if (OBanner.id == 0) { 
				flagSucesso =  this.inserir(OBanner);
			}

			if (flagSucesso && OImagem != null) { 

                var OArquivo = new ArquivoUpload();

			    OArquivo.idReferenciaEntidade = OBanner.id;

			    OArquivo.entidade = EntityTypes.BANNER;

				var listaThumbs = new List<ThumbDTO>();

				listaThumbs.Add(new ThumbDTO { folderName = "sistema", height = 50, width = 0 });

				this.OArquivoUploadFotoBL.salvar(OArquivo, OImagem, "", listaThumbs);

			}

			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(Banner OBanner) { 
			
			OBanner.setDefaultInsertValues<Banner>();
			db.Banner.Add(OBanner);
			db.SaveChanges();

			return (OBanner.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(Banner OBanner) { 

			OBanner.setDefaultUpdateValues<Banner>();

			//Localizar existentes no banco
			Banner dbBanner = this.carregar(OBanner.id);		

            if (dbBanner == null) {
                return false;
            }

			var BannerEntry = db.Entry(dbBanner);
			BannerEntry.CurrentValues.SetValues(OBanner);
			BannerEntry.ignoreFields<Banner>();

			db.SaveChanges();

			return true;
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

		//Excluir o registro e os vínculos de imagens
		public bool excluir(int id) {
            
			db.Banner
				.Where(x => x.id == id)
				.Update(x => new Banner { flagExcluido = "S", dtAlteracao = DateTime.Now });

			return true;
		}
	}
}