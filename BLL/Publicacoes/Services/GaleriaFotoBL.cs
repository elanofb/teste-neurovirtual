using System;
using System.Linq;
using DAL.Arquivos;
using DAL.Publicacoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using BLL.Arquivos;
using System.Collections.Generic;
using System.Data.Entity;
using BLL.Services;
using DAL.Entities;
using System.Json;
using UTIL.Resources;

namespace BLL.Publicacoes {

    public class GaleriaFotoBL : DefaultBL, IGaleriaFotoBL {

        //Atributos
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //Construtor
        public GaleriaFotoBL() {
        }

        public IQueryable<GaleriaFoto> query(int? idOrganizacaoParam = null) {

            var query = from B in db.GaleriaFoto.Include(x => x.Portal)
                        where B.flagExcluido == "N"
                        select B;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

		//
		public GaleriaFoto carregar(int id) {

			var query = this.query().condicoesSeguranca().Include(x => x.Portal);
            
			return query.FirstOrDefault(x => x.id == id);

		}

        //
        public IQueryable<GaleriaFoto> listar(string valorBusca, string ativo, int idTipoGaleria = 0, bool flagImagemAtiva = false) {
            
            var query = this.query().condicoesSeguranca().Include(x => x.Portal);
            
            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.titulo.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (idTipoGaleria > 0) {
                query = query.Where(x => x.idTipoGaleria == idTipoGaleria);
            }

            if (flagImagemAtiva == true) {
                query = query.Where(
                    x => db.ArquivoUpload.Any(
                        ARQ => ARQ.idReferenciaEntidade == x.id &&
                        !ARQ.dtExclusao.HasValue &&
                        ARQ.ativo == "S" &&
                        ARQ.entidade == EntityTypes.GALERIAFOTO &&
                        ARQ.categoria == ArquivoUploadTypes.FOTO
                    )
                );
		    }

            return query;
        }

		//Verificar se deve-se atualizar um registro existente ou criar um novo
		public bool salvar(GaleriaFoto OGaleriaFoto, List<HttpPostedFileBase> listaArquivos) {

			bool flagSucesso = false;

		    if (OGaleriaFoto.id > 0) {
		        flagSucesso = this.atualizar(OGaleriaFoto);
		    }

		    if (OGaleriaFoto.id == 0) {
		        flagSucesso = this.inserir(OGaleriaFoto);
		    }

            listaArquivos = listaArquivos.Where(x => x != null).ToList();
            
            if (flagSucesso && listaArquivos.Any()) {

                foreach (HttpPostedFileBase OFoto in listaArquivos) { 

                    var OArquivo = new ArquivoUpload();

                    OArquivo.idReferenciaEntidade = OGaleriaFoto.id;

                    OArquivo.entidade = EntityTypes.GALERIAFOTO;

					var listaThumbs = new List<ThumbDTO>();

					listaThumbs.Add(new ThumbDTO{ folderName = "home", height=178, width=0});

                    listaThumbs.Add(new ThumbDTO{ folderName = "home-370x246", height=246, width=370});

                    listaThumbs.Add(new ThumbDTO{ folderName = "sistema", height=50, width=0});

                    listaThumbs.Add(new ThumbDTO{ folderName = "interna", height=100, width=0});

                    this.OArquivoUploadFotoBL.salvar(OArquivo, OFoto, "", listaThumbs);

                }

            }

			return flagSucesso;
		}

		//Persistir o objecto e salvar na base de dados
		private bool inserir(GaleriaFoto OGaleriaFoto) { 
            
			OGaleriaFoto.setDefaultInsertValues();

            db.GaleriaFoto.Add(OGaleriaFoto);

            db.SaveChanges();

			return (OGaleriaFoto.id > 0);
		}

		//Persistir o objecto e atualizar informações
		private bool atualizar(GaleriaFoto OGaleriaFoto) {
            
            OGaleriaFoto.setDefaultUpdateValues();

			//Localizar existentes no banco
			GaleriaFoto dbGaleriaFoto = this.carregar(OGaleriaFoto.id);

            if (dbGaleriaFoto == null) {
                return false;
            }

            var GaleriaFotoEntry = db.Entry(dbGaleriaFoto);

            GaleriaFotoEntry.CurrentValues.SetValues(OGaleriaFoto);

            GaleriaFotoEntry.ignoreFields<GaleriaFoto>();

			db.SaveChanges();

            return (OGaleriaFoto.id > 0);
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
            db.GaleriaFoto.Where(x => ids.Contains(x.id))
                .Update(x => new GaleriaFoto { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = db.GaleriaFoto.
                Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();

            return (listaCheck.Count == 0);
        }
    }
}