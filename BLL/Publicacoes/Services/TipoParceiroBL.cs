using System;
using System.Json;
using System.Linq;
using DAL.Publicacoes;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Publicacoes {

    public class TipoParceiroBL : DefaultBL, ITipoParceiroBL {

        //
        public IQueryable<TipoParceiro> query(int? idOrganizacaoParam = null) {

            var query = from Obj in db.TipoParceiro
                        where Obj.flagExcluido == false
                        select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }
        
        //Carregar pelo ID
        public TipoParceiro carregar(int id) {

            var query = from Parc in db.TipoParceiro
                        where
                            Parc.id == id &&
                            Parc.flagExcluido == false
                        select Parc;

            return query.FirstOrDefault();
        }

        //Listagem 
        public IQueryable<TipoParceiro> listar(int idOrganizacaoParam, string valorBusca, bool? ativo) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = User.idOrganizacao();
            }

            var query = from Parc in db.TipoParceiro.AsNoTracking()
                        where
                            Parc.flagExcluido == false
                        select Parc;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (idOrganizacaoParam > 0){

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Realizar os tratamentos necessários
        //Salvar um novo registro
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(TipoParceiro OTipoParceiro) {
            bool flagSucesso = false;

            if (OTipoParceiro.id == 0) {
                flagSucesso = this.inserir(OTipoParceiro);
            } else {
                flagSucesso = this.atualizar(OTipoParceiro);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoParceiro OTipoParceiro) {

            OTipoParceiro.setDefaultInsertValues<TipoParceiro>();
            db.TipoParceiro.Add(OTipoParceiro);
            db.SaveChanges();

            return (OTipoParceiro.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoParceiro OTipoParceiro) {
            OTipoParceiro.setDefaultUpdateValues<TipoParceiro>();

            //Localizar existentes no banco
            TipoParceiro dbTipoParceiro = this.carregar(OTipoParceiro.id);

            if (dbTipoParceiro == null) {
                return false;
            }

            var TipoParceiroEntry = db.Entry(dbTipoParceiro);
            TipoParceiroEntry.CurrentValues.SetValues(OTipoParceiro);
            TipoParceiroEntry.ignoreFields<TipoParceiro>();

            db.SaveChanges();
            return (OTipoParceiro.id > 0);
        }

        // Verificar se já existe um registro com o mesmo nome, no entanto, que possua id diferente do informado
        public bool existe(string descricao, int id, int idOrganizacao) {

            var query = from Parc in db.TipoParceiro
                        where
                Parc.descricao == descricao &&
                Parc.flagExcluido == false &&
                Parc.idOrganizacao == idOrganizacao &&
                Parc.id != id
                        select Parc;

            query = query.condicoesSeguranca();

            var OItem = query.Take(1).FirstOrDefault();
            return (OItem != null);
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
                retorno.active = item.ativo == true ? "S" : "N";
                retorno.message = NotificationMessages.updateSuccess;
            }
            return retorno;
        }

        //Exclusao logica de registros
        public UtilRetorno excluir(int id) {

            TipoParceiro OTipoParceiro = this.carregar(id);

            if (OTipoParceiro == null) {
                return UtilRetorno.newInstance(true, "O registro não foi localizado.");
            }

            OTipoParceiro.flagExcluido = true;
            OTipoParceiro.idUsuarioAlteracao = User.id();
            OTipoParceiro.dtAlteracao = DateTime.Now;
            db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
    }
}