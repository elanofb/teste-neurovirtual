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
using DAL.Entities;
using UTIL.Resources;

namespace BLL.Publicacoes {

	public class JornalBL : DefaultBL, IJornalBL {

        //Atributos
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL(); 

        //Construtor
		public JornalBL() {
        }
		
        //
	    public IQueryable<Jornal> query(int? idOrganizacaoParam = null) {

	        var query = from N in db.Jornal
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

        //
		public IQueryable<Jornal> listar(string valorBusca = "", string ativo = "S", int idTipoNoticia = 0, bool? flagImagemAtiva = false, int? idPortal = 0, int? idOrganizacaoParam = null) {
			
			var query = this.query().condicoesSeguranca();
            
            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }
             
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.titulo.Contains(valorBusca) || x.chamada.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			if (idTipoNoticia > 0) {
				query = query.Where(x => x.idTipoNoticia == idTipoNoticia);
			}

            if (flagImagemAtiva == true) {
                query = query.Where(
                    x => db.ArquivoUpload.Any(
                        ARQ => ARQ.idReferenciaEntidade == x.id &&
                        !ARQ.dtExclusao.HasValue &&
                        ARQ.ativo == "S" &&
                        ARQ.entidade == EntityTypes.JORNAL &&
                        ARQ.categoria == ArquivoUploadTypes.FOTO
                    )
                );
		    }
            
			return query;
		}

		public IQueryable<Jornal> listarComunicados(string valorBusca = "", string ativo = "S") {

            var idTipoNoticia = (int)TipoNoticiaEnum.JORNAL;
            
			var query = from N in db.Jornal.Include(x => x.TipoNoticia)
						where N.flagExcluido == "N" && N.idTipoNoticia == idTipoNoticia
						select N;

            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

		//
		public Jornal carregar(int id) {
			
			var query = db.Jornal.Where(x => x.id == id);

            query = query.condicoesSeguranca();

			return query.FirstOrDefault();
		}

		//
		public JornalDTO principalJornal(int id = 0) {
			
			var query = (from N in db.Jornal
						 from A in db.ArquivoUpload.Where(x => !x.dtExclusao.HasValue && x.ativo == "S"
						 && x.entidade == DAL.Entities.EntityTypes.JORNAL
						 && x.categoria == ArquivoUploadTypes.FOTO
						 && x.idReferenciaEntidade == N.id).Take(1).DefaultIfEmpty()
						 where N.flagExcluido == "N" && N.ativo == "S"
						 select new JornalDTO() {
							 id = N.id,
							 autor = N.autor, chamada = N.chamada, dtJornal = N.dtJornal,
							 path = A.path, pathThumb = A.pathThumb, titulo = N.titulo,
							 descricao = N.descricao
						 }
					   );

			if (id > 0) query = query.Where(x => x.id == id);

            query = query.condicoesSeguranca();

			return query.OrderByDescending(x => x.dtJornal).FirstOrDefault();
		}

        //
		public IQueryable<JornalDTO> principalJornalFotos(int id) {
			
			var query = (from A in db.ArquivoUpload
						 from N in db.Jornal.Where(x => x.flagExcluido == "N" && x.ativo == "S"
						 && A.idReferenciaEntidade == x.id && x.id == id).DefaultIfEmpty()
						 where N.flagExcluido == "N" && N.ativo == "S"
						 && A.entidade == DAL.Entities.EntityTypes.JORNAL
						 && A.categoria == DAL.Arquivos.ArquivoUploadTypes.FOTO
						 select new JornalDTO() {
							 id = N.id,
							 autor = N.autor, chamada = N.chamada, dtJornal = N.dtJornal,
							 path = A.path, pathThumb = A.pathThumb, titulo = N.titulo,
							 descricao = N.descricao
						 }
					   );

			if (id > 0) query = query.Where(x => x.id == id);

            query = query.condicoesSeguranca();

			return query.OrderByDescending(x => x.dtJornal);
		}

        //
		public IQueryable<JornalDTO> listarPortal(int id = 0) {
			
			var query = (from N in db.Jornal
						 from A in db.ArquivoUpload.Where(x => !x.dtExclusao.HasValue && x.ativo == "S"
						 && x.entidade == DAL.Entities.EntityTypes.JORNAL
						 && x.categoria == ArquivoUploadTypes.FOTO
						 && x.idReferenciaEntidade == N.id).DefaultIfEmpty()
						 where N.ativo == "S"
						 && N.flagExcluido == "N"
						 select new JornalDTO() {
							 id = N.id,
							 autor = N.autor, chamada = N.chamada, dtJornal = N.dtJornal,
							 path = A.path, pathThumb = A.pathThumb, titulo = N.titulo
						 });

            query = query.condicoesSeguranca();

			if (id > 0) query = query.Where(x => x.id != id);

			return query;
		}

		//
		public IQueryable<JornalDTO> buscar(string valorBusca) {
			var query = this.listarPortal(0);

			query = query.Where(x =>
				x.autor.Contains(valorBusca) ||
				x.chamada.Contains(valorBusca) ||
				x.descricao.Contains(valorBusca) ||
				x.titulo.Contains(valorBusca)
			);

            query = query.condicoesSeguranca();

			return query;
		}

		//
		public bool salvar(Jornal OJornal, HttpPostedFileBase[] arrayArquivos) {

            bool flagSucesso = false;

		    if(OJornal.id > 0) {
		        flagSucesso = this.atualizar(OJornal);
		    }

		    if (OJornal.id == 0) {
		        flagSucesso = this.inserir(OJornal);
		    }

            if (flagSucesso) {

                var listaThumb = new List<ThumbDTO> {
                    new ThumbDTO { folderName = "destaque", width = 250, height = 167 },
                    new ThumbDTO { folderName = "interna", width = 74, height = 50 }
                };

                foreach (var OArquivo in arrayArquivos) {

                    if (OArquivo != null) { 

                        var OArquivoUpload = new ArquivoUpload();

                        OArquivoUpload.idReferenciaEntidade = OJornal.id;

                        OArquivoUpload.entidade = EntityTypes.JORNAL;

                        this.OArquivoUploadFotoBL.salvar(OArquivoUpload, OArquivo, "", listaThumb);

                    }

                }

            }

			return (flagSucesso);
		}

        //Persistir e inserir um novo registro 
        protected virtual bool inserir(Jornal OJornal) { 
            
			OJornal.setDefaultInsertValues();
			
			db.Jornal.Add(OJornal);
			db.SaveChanges();

			return OJornal.id > 0;
		}

        //Persistir e atualizar um registro existente 
		protected virtual bool atualizar(Jornal OJornal) { 
            
			//Localizar existentes no banco
			Jornal dbJornal = this.carregar(OJornal.id);

            if (dbJornal == null) {
                return false;
            }

			//Configurar valores padrão
			OJornal.setDefaultUpdateValues();

			//Atualizacao da Instituicao
			var JornalEntry = db.Entry(dbJornal);
			JornalEntry.CurrentValues.SetValues(OJornal);
			JornalEntry.ignoreFields();

			db.SaveChanges();
			return OJornal.id > 0;
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
			db.Jornal.Where(x => ids.Contains(x.id))
			  .Update(x => new Jornal { flagExcluido = "S", dtAlteracao = DateTime.Now });

			var listaCheck = db.Jornal.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
			return (listaCheck.Count == 0);
		}

		//
		public bool existe(string descricao, int id) {
			
			var query = from C in db.Jornal
						where C.descricao == descricao && C.id != id && C.flagExcluido == "N"
						select C;

            query = query.condicoesSeguranca();

			var OBanco = query.Take(1).FirstOrDefault();
			return (OBanco == null ? false : true);
		}
	}
}