using System;
using System.Json;
using System.Linq;
using BLL.Services;
using UTIL.Resources;
using DAL.Institucionais;

namespace BLL.Institucionais {

    public class TipoConvenioBL : DefaultBL, ITipoConvenioBL {

        //Atributos

        //Propriedades

        //
        public TipoConvenioBL() {
        }

        //Carregamento de registro único pelo ID
        public TipoConvenio carregar(int id) {

            var query = from Item in db.TipoConvenio
                        where
                            Item.id == id &&
                            Item.flagExcluido == false
                        select Item;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //
        public IQueryable<TipoConvenio> listar(string valorBusca, bool? ativo) {

            var query = from C in db.TipoConvenio
                        where C.flagExcluido == false
                        select C;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string chaveUrl, int id) {

            var query = from C in db.TipoConvenio
                        where
                            C.chaveUrl == chaveUrl &&
                            C.id != id &&
                            C.flagExcluido == false
                        select C;

            query = query.condicoesSeguranca();

            return query.Any();
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        public bool salvar(TipoConvenio OTipoConvenio) {

            OTipoConvenio.chaveUrl = UtilString.cleanAccents(OTipoConvenio.descricao).ToLower().Replace(" ", "-");

            if (OTipoConvenio.id == 0) {
                return this.inserir(OTipoConvenio);
            }

            return this.atualizar(OTipoConvenio);
        }

        //Persistir e inserir um novo registro 
        //Inserir Tipo de Convenio
        private bool inserir(TipoConvenio OTipoConvenio) {

            OTipoConvenio.setDefaultInsertValues<TipoConvenio>();

            db.TipoConvenio.Add(OTipoConvenio);

            db.SaveChanges();

            return OTipoConvenio.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados do Tipo do Convenio
        private bool atualizar(TipoConvenio OTipoConvenio) {

            //Localizar existentes no banco
            TipoConvenio dbTipoConvenio = this.carregar(OTipoConvenio.id);

            //Configurar valores padrão
            OTipoConvenio.setDefaultUpdateValues();

            //Atualizacao do Tipo do Convenio
            var TipoConvenioEntry = db.Entry(dbTipoConvenio);
            TipoConvenioEntry.CurrentValues.SetValues(OTipoConvenio);
            TipoConvenioEntry.ignoreFields();

            db.SaveChanges();

            return OTipoConvenio.id > 0;
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