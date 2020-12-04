using System;
using System.Data.Entity;
using System.Linq;
using DAL.Arquivos;
using DAL.Publicacoes;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using BLL.Arquivos;
using System.Collections.Generic;
using System.Json;
using BLL.Services;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public abstract class PublicacaoBL : DefaultBL, IPublicacaoBL {

        //Atributos
        protected IArquivoUploadFotoBL _IArquivoUploadFotoBL;

        //Propriedades
        protected IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL(); 

        //Construtor
		public PublicacaoBL() {
        }

	    public IQueryable<Noticia> query(int? idOrganizacaoParam = null) {

	        var query = from N in db.Noticia
	                    where N.flagExcluido == "N"
	                    select N;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

		//Carregar registro a partir do ID
		public Noticia carregar(int id) {

			var query = this.query().condicoesSeguranca();
            
			return query.FirstOrDefault(x => x.id == id);
		}
		
		//Listagem de registros conforme filtros
		public abstract IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0);


        //Listagem de registros conforme filtros
		protected virtual IQueryable<Noticia> listar(int idTipoNoticia, string valorBusca, string ativo = "S", int? idPortal = 0) {
			
			var query = this.query().condicoesSeguranca();
            
            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }

			if (idTipoNoticia > 0) {
				query = query.Where(x => x.idTipoNoticia == idTipoNoticia);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.titulo.Contains(valorBusca) || x.chamada.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			return query;
		}

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		public abstract bool salvar(Noticia OPublicacao, HttpPostedFileBase OArquivo);

		//Realizar os tratamentos necessários
		//Salvar um novo registro
		protected virtual bool salvar(Noticia ONoticia) {

			bool flagSucesso = false;

			if(ONoticia.id == 0){	
				flagSucesso = this.inserir(ONoticia);
			}

			flagSucesso = this.atualizar(ONoticia);

			//if (OArquivo != null && ONoticia.id > 0) {
			//	this.OArquivoUploadBL.salvarFoto(ONoticia.id, DAL.Entities.EntityTypes.NOTICIA, OArquivo, listaThumb);
			//}

			return flagSucesso;
		}

        //Persistir e inserir um novo registro 
        protected virtual bool inserir(Noticia ONoticia) { 

			ONoticia.setDefaultInsertValues<Noticia>();
			
			db.Noticia.Add(ONoticia);
			db.SaveChanges();

			return ONoticia.id > 0;
		}

        //Persistir e atualizar um registro existente 
		protected virtual bool atualizar(Noticia ONoticia) { 

			//Localizar existentes no banco
			Noticia dbNoticia = this.carregar(ONoticia.id);

            if (dbNoticia == null) {
                return false;
            }

			//Configurar valores padrão
			ONoticia.setDefaultUpdateValues();

			//Atualizacao da Instituicao
			var NoticiaEntry = db.Entry(dbNoticia);
			NoticiaEntry.CurrentValues.SetValues(ONoticia);
			NoticiaEntry.ignoreFields();

			db.SaveChanges();
			return ONoticia.id > 0;
		}

		//Verificar se um registro existe para evitar duplicidade
		public bool existe(Noticia ONoticia) {

			var query = from C in db.Noticia
						where 
							C.titulo == ONoticia.titulo && 
							C.id != ONoticia.id && 
							C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

			return (query.Count() == 0 ? false : true);
		}

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

		//Exclusao Logica
		public UtilRetorno excluir(int id) {

			Noticia ONoticia = this.carregar(id);

			if (ONoticia == null) { 
				return UtilRetorno.newInstance(true, "O registro não pode ser localizado.");
			}

			ONoticia.flagExcluido = "S";
			ONoticia.dtAlteracao = DateTime.Now; 

            this.db.SaveChanges();

			return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
		}
	}
}