using System;
using System.Linq;
using DAL.Financeiro;
using EntityFramework.Extensions;
using System.Json;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Financeiro {

    public class TipoReceitaBL: DefaultBL, ITipoReceitaBL {

        public const string keyCache = "tipo_receita";

        //
        public TipoReceitaBL() {
        }

        //Carregamento de registro pelo ID
        public TipoReceita carregar(int id) {

            return db.TipoReceita.Find(id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<TipoReceita> listar(string valorBusca, bool? ativo) {

            var query = from P in db.TipoReceita
                        where P.flagExcluido == false
                        select P;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }


        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao,int id) {

            var query = from P in db.TipoReceita
                        where P.descricao == descricao && P.id != id && P.flagExcluido == false
                        select P;
            var OTipoReceita = query.Take(1).FirstOrDefault();
            return (OTipoReceita != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(TipoReceita OTipoProduto) {

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(TipoReceita OTipoReceita) {

            OTipoReceita.setDefaultInsertValues();
            db.TipoReceita.Add(OTipoReceita);
            db.SaveChanges();

            return (OTipoReceita.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(TipoReceita OTipoReceita) {

            OTipoReceita.setDefaultUpdateValues();

            //Localizar existentes no banco
            TipoReceita dbTipoProduto = this.carregar(OTipoReceita.id);
            var TipoEntry = db.Entry(dbTipoProduto);
            TipoEntry.CurrentValues.SetValues(OTipoReceita);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OTipoReceita.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.TipoReceita
                .Where(x => x.id == id)
                .Update(x => new TipoReceita { flagExcluido = true, dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }

        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (!Objeto.ativo);
                db.SaveChanges();
                retorno.active = (Objeto.ativo) ? "S" : "N";
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }
    }
}