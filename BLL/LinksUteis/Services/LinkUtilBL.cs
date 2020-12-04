using System;
using System.Json;
using System.Linq;
using System.Data.Entity;
using EntityFramework.Extensions;
using UTIL.Resources;
using BLL.Services;
using EntityFramework.Caching;
using DAL.Permissao.Security.Extensions;
using System.Web;
using System.Collections.Generic;
using DAL.Arquivos;
using BLL.Arquivos;
using DAL.Entities;
using DAL.LinksUteis;

namespace BLL.LinksUteis {

    public class LinkUtilBL : DefaultBL, ILinkUtilBL {

        //Atributos
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //
        public LinkUtilBL() {

        }

        //
        public IQueryable<LinkUtil> query(int? idOrganizacaoParam = null) {

            var query = from L in db.LinkUtil
                        where L.flagExcluido == false
                        select L;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregar registro a partir do ID
        public LinkUtil carregar(int id, bool flagCache = false) {

            var query = this.query().condicoesSeguranca();
            
            if (flagCache) {
                return query.FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromHours(1))).FirstOrDefault();
            }

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registro a partir de parametros
        public IQueryable<LinkUtil> listar(string valorBusca, bool? ativo, int? idPortal = 0) {

            var query = this.query().condicoesSeguranca();
            
            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (idPortal > 0) {
                query = query.Where(x => x.idPortal == idPortal);
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;

        }

        //Salvar dados e imagem (se for enviado)
        public bool salvar(LinkUtil OLinkUtil, HttpPostedFileBase OFoto) {

            bool flagSucesso = false;
            
            if (OLinkUtil.id > 0) {
                flagSucesso = this.atualizar(OLinkUtil);
            }

            if (OLinkUtil.id == 0) {
                flagSucesso = this.inserir(OLinkUtil);
            }

            // Salvar Fotos
            if (flagSucesso && OFoto != null) {

                var listaThumb = new List<ThumbDTO> { new ThumbDTO { folderName = "linkUtil", height = 68, width = 0 } };
                
                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = OLinkUtil.id;

                OArquivo.entidade = EntityTypes.LINK_UTIL;

                this.OArquivoUploadFotoBL.salvar(OArquivo, OFoto, "", listaThumb);

            }

            return (flagSucesso);
        }

        private bool inserir(LinkUtil OLinkUtil) {

            OLinkUtil.setDefaultInsertValues();

            db.LinkUtil.Add(OLinkUtil);

            db.SaveChanges();

            return (OLinkUtil.id > 0);
        }

        //Atualizar os dados de um associado e os objetos relacionados
        private bool atualizar(LinkUtil OLinkUtil) {

            LinkUtil dbLinkUtil = this.carregar(OLinkUtil.id);

            if (dbLinkUtil == null) {
                return false;
            }

            var entryLinkUtil = db.Entry(dbLinkUtil);

            OLinkUtil.setDefaultUpdateValues();

            entryLinkUtil.CurrentValues.SetValues(OLinkUtil);

            entryLinkUtil.ignoreFields();

            db.SaveChanges();

            return (OLinkUtil.id > 0);
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            LinkUtil item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo == true ? false : true);
                db.SaveChanges();
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Excluir um registro a partir de um array  de ids
        public JsonMessage delete(int[] id) {

            int idUsuarioAlteracao = User.id();

            this.db.LinkUtil.Where(x => (id.Contains(x.id))).Update(x => new LinkUtil { flagExcluido = true, dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuarioAlteracao });

            return new JsonMessage { error = false, message = NotificationMessages.delete_success };
            
        }

    }

}
