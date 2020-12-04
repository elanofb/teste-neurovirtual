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

    public class NoticiaBL : DefaultBL, INoticiaBL {

        //Atributos
        private IArquivoUploadBL _ArquivoUploadBL;
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadBL OArquivoUploadBL => _ArquivoUploadBL = _ArquivoUploadBL ?? new ArquivoUploadBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //Construtor
        public NoticiaBL() {
        }

        //
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

        //
        public IQueryable<Noticia> listar(string valorBusca = "", string ativo = "S", int tipoNoticia = 0, bool? flagImagemAtiva = false, int? idPortal = 0) {

            var query = this.query().condicoesSeguranca().Include(x => x.Portal)
                                                         .Include(x => x.CategoriaNoticia);

            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca) || x.titulo.Contains(valorBusca) || x.chamada.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (tipoNoticia > 0) {
                query = query.Where(x => x.idTipoNoticia == tipoNoticia);
            }

            if (flagImagemAtiva == true) {
                query = query.Where(
                    x => db.ArquivoUpload.Any(
                        ARQ => ARQ.idReferenciaEntidade == x.id &&
                        !ARQ.dtExclusao.HasValue &&
                        ARQ.ativo == "S" &&
                        ARQ.entidade == EntityTypes.NOTICIA &&
                        ARQ.categoria == ArquivoUploadTypes.FOTO
                    )
                );
            }
            
            return query;
        }

        public IQueryable<Noticia> listarComunicados(string valorBusca = "", string ativo = "S") {
            var idTipoNoticia = (int)TipoNoticiaEnum.COMUNICADO;

            var query = from N in db.Noticia.Include(x => x.TipoNoticia).Include(x => x.CategoriaNoticia)
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
        public Noticia carregar(int id) {

            var query = db.Noticia.Where(x => x.id == id).Include(x => x.Portal).Include(x => x.CategoriaNoticia);

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //
        public NoticiaDTO principalNoticia(int id = 0) {

            var query = (from N in db.Noticia
                         from A in db.ArquivoUpload.Where(x => !x.dtExclusao.HasValue && x.ativo == "S"
                         && x.entidade == DAL.Entities.EntityTypes.NOTICIA
                         && x.categoria == ArquivoUploadTypes.FOTO
                         && x.idReferenciaEntidade == N.id).Take(1).DefaultIfEmpty()
                         where N.flagExcluido == "N" && N.ativo == "S"
                         select new NoticiaDTO() {
                             id = N.id,
                             autor = N.autor,
                             chamada = N.chamada,
                             dtNoticia = N.dtNoticia,
                             path = A.path,
                             pathThumb = A.pathThumb,
                             titulo = N.titulo,
                             descricao = N.descricao
                         }
                       );

            if (id > 0)
                query = query.Where(x => x.id == id);

            query = query.condicoesSeguranca();

            return query.OrderByDescending(x => x.dtNoticia).FirstOrDefault();
        }

        //
        public IQueryable<NoticiaDTO> principalNoticiaFotos(int id) {

            var query = (from A in db.ArquivoUpload
                         from N in db.Noticia.Where(x => x.flagExcluido == "N" && x.ativo == "S"
                         && A.idReferenciaEntidade == x.id && x.id == id).DefaultIfEmpty()
                         where N.flagExcluido == "N" && N.ativo == "S"
                         && A.entidade == DAL.Entities.EntityTypes.NOTICIA
                         && A.categoria == DAL.Arquivos.ArquivoUploadTypes.FOTO
                         select new NoticiaDTO() {
                             id = N.id,
                             autor = N.autor,
                             chamada = N.chamada,
                             dtNoticia = N.dtNoticia,
                             path = A.path,
                             pathThumb = A.pathThumb,
                             titulo = N.titulo,
                             descricao = N.descricao
                         }
                       );

            if (id > 0)
                query = query.Where(x => x.id == id);

            query = query.condicoesSeguranca();

            return query.OrderByDescending(x => x.dtNoticia);
        }

        //
        public IQueryable<NoticiaDTO> listarPortal(int id = 0) {

            var query = (from N in db.Noticia
                         from A in db.ArquivoUpload.Where(x => !x.dtExclusao.HasValue && x.ativo == "S"
                         && x.entidade == DAL.Entities.EntityTypes.NOTICIA
                         && x.categoria == ArquivoUploadTypes.FOTO
                         && x.idReferenciaEntidade == N.id).DefaultIfEmpty()
                         where N.ativo == "S"
                         && N.flagExcluido == "N"
                         select new NoticiaDTO() {
                             id = N.id,
                             autor = N.autor,
                             chamada = N.chamada,
                             dtNoticia = N.dtNoticia,
                             path = A.path,
                             pathThumb = A.pathThumb,
                             titulo = N.titulo
                         });

            if (id > 0)
                query = query.Where(x => x.id != id);

            query = query.condicoesSeguranca();

            return query;
        }

        //
        public IQueryable<NoticiaDTO> buscar(string valorBusca) {
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
        public bool salvar(Noticia ONoticia, HttpPostedFileBase OFoto, HttpPostedFileBase OArquivoPDF = null) {

            bool flagSucesso = false;

            ONoticia.chaveUrl = UtilString.cleanStringToURL(ONoticia.titulo).ToLower();

            if (ONoticia.id > 0) {
                flagSucesso = this.atualizar(ONoticia);
            }

            if (ONoticia.id == 0) {
                flagSucesso = this.inserir(ONoticia);
            }
            
            
            if (flagSucesso && OFoto != null) {

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = ONoticia.id;

                OArquivo.entidade = EntityTypes.NOTICIA;

                var listaThumb = new List<ThumbDTO> {

                    new ThumbDTO { folderName  = "destaque", width = 105, height = 70 },

                    new ThumbDTO { folderName  = "listagem", width = 250, height = 167 },

                    new ThumbDTO { folderName  = "interna", width = 74, height = 50 },

                };

                this.OArquivoUploadFotoBL.salvar(OArquivo, OFoto, "", listaThumb);

            }

            if (flagSucesso && OArquivoPDF != null) {

                this.OArquivoUploadBL.salvarDocumento(ONoticia.id, EntityTypes.NOTICIA, "", OArquivoPDF, 0);

            }

            return true;
        }

        //Persistir e inserir um novo registro 
        protected virtual bool inserir(Noticia ONoticia) {

            ONoticia.setDefaultInsertValues();

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

            db.Noticia.Where(x => ids.Contains(x.id))
                .Update(x => new Noticia { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = db.Noticia.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();

            return (listaCheck.Count == 0);
        }

        //
        public bool existe(string descricao, int id) {

            var query = from C in db.Noticia
                        where C.descricao == descricao && C.id != id && C.flagExcluido == "N"
                        select C;

            query = query.condicoesSeguranca();

            var OBanco = query.Take(1).FirstOrDefault();

            return (OBanco == null ? false : true);
        }

        //
        public bool existeUrl(string titulo, int id, int idTipoNoticia) {

            var query = from C in db.Noticia
                        where C.chaveUrl == titulo && C.id != id && C.flagExcluido == "N" && C.idTipoNoticia == idTipoNoticia
                        select C;

            query = query.condicoesSeguranca();

            var OBanco = query.FirstOrDefault() ?? new Noticia();

            return (OBanco.id > 0);
        }
    }
}