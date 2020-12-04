using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;
using DAL.Institucionais;
using System.Web;
using System.Collections.Generic;
using DAL.Arquivos;
using BLL.Arquivos;
using DAL.Entities;

namespace BLL.Institucionais {

    public class ConvenioBL : DefaultBL, IConvenioBL {

        //Atributos
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //
        public ConvenioBL() {
        }

        //
        public IQueryable<Convenio> query(int? idOrganizacaoParam = null) {

            var query = from Item in db.Convenio
                        where Item.flagExcluido == false
                        select Item;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro único pelo ID
        public Convenio carregar(int id) {

            var query = this.query().condicoesSeguranca();
                
            return query.FirstOrDefault(x => x.id == id);

        }

        //
        public IQueryable<Convenio> listar(string valorBusca, bool? ativo, int? idTipoConvenio = 0) {

            var query = this.query().condicoesSeguranca();
            
            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (idTipoConvenio > 0) {
                query = query.Where(x => x.idTipoConvenio == idTipoConvenio);
            }

            return query;

        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string titulo, string chaveUrl, int id) {

            var query = from C in db.Convenio
                        where
                            (C.titulo == titulo || C.chaveUrl == chaveUrl) &&
                            C.id != id &&
                            C.flagExcluido == false
                        select C;

            query = query.condicoesSeguranca();

            return query.Any();
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(Convenio OConvenio, HttpPostedFileBase OFoto) {
            
            OConvenio.chaveUrl = UtilString.cleanStringToURL(OConvenio.titulo).ToLower();

            var flagSucesso = false;

            if (OConvenio.id > 0) {
                flagSucesso = this.atualizar(OConvenio);
            }

            if (OConvenio.id == 0) {
                flagSucesso = this.inserir(OConvenio);
            }
            
            if (flagSucesso && OFoto != null) {
                
                var listaThumbs = new List<ThumbDTO>();

                listaThumbs.Add(new ThumbDTO { folderName = "sistema", height = 50, width = 0 });

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = OConvenio.id;

                OArquivo.entidade = EntityTypes.CONVENIO;
                
                this.OArquivoUploadFotoBL.salvar(OArquivo, OFoto, "", listaThumbs);

            }

            return flagSucesso;
        }

        //Persistir e inserir um novo registro 
        //Inserir Convenio
        private bool inserir(Convenio OConvenio) {

            OConvenio.setDefaultInsertValues<Convenio>();

            db.Convenio.Add(OConvenio);

            db.SaveChanges();

            return OConvenio.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados do Convenio
        private bool atualizar(Convenio OConvenio) {

            //Localizar existentes no banco
            Convenio dbConvenio = this.carregar(OConvenio.id);

            //Configurar valores padrão
            OConvenio.setDefaultUpdateValues();

            //Atualizacao do Convenio
            var ConvenioEntry = db.Entry(dbConvenio);
            ConvenioEntry.CurrentValues.SetValues(OConvenio);
            ConvenioEntry.ignoreFields();

            db.SaveChanges();

            return OConvenio.id > 0;
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = this.carregar(id);

            if (item == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                item.ativo = (item.ativo == true ? false : true);
                db.SaveChanges();
                retorno.active = (item.ativo == true ? "S" : "N");
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        // Excluir Registro
        public UtilRetorno excluir(int id, int idUsuarioExclusao) {

            var ORegistro = this.carregar(id);

            if (ORegistro == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pôde ser localizado.");
            }

            ORegistro.flagExcluido = true;

            ORegistro.idUsuarioAlteracao = idUsuarioExclusao;

            ORegistro.dtAlteracao = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "Os dados foram atualizados com sucesso.");
        }
    }
}