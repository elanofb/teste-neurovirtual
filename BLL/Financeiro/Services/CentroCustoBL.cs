using System;
using System.Linq;
using DAL.Financeiro;
using EntityFramework.Extensions;
using System.Json;
using UTIL.Resources;
using System.Collections.Generic;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.Financeiro {

    public class CentroCustoBL: DefaultBL, ICentroCustoBL {

        public const string keyCache = "centro_custo";

        //
        public CentroCustoBL() {
        }

        //Carregamento de registro pelo ID
        public CentroCusto carregar(int id) {

            var query = from P in db.CentroCusto
                        where P.id == id && P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<CentroCusto> listar(string valorBusca, bool? ativo) {

            var query = from P in db.CentroCusto
                        where P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(ativo.HasValue) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao,int id) {

            var query = from P in db.CentroCusto
                        where P.descricao == descricao && P.id != id && P.flagExcluido == false
                        select P;

            query = query.condicoesSeguranca();

            var OCentroCusto = query.Take(1).FirstOrDefault();

            return (OCentroCusto != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(CentroCusto OTipoProduto) {

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(CentroCusto OCentroCusto) {

            OCentroCusto.flagSistema = false;

            OCentroCusto.setDefaultInsertValues<CentroCusto>();

            db.CentroCusto.Add(OCentroCusto);

            db.SaveChanges();

            return (OCentroCusto.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(CentroCusto OCentroCusto) {

            OCentroCusto.setDefaultUpdateValues<CentroCusto>();

            //Localizar existentes no banco
            CentroCusto dbCentroCusto = this.carregar(OCentroCusto.id);

            if (dbCentroCusto == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbCentroCusto);
            TipoEntry.CurrentValues.SetValues(OCentroCusto);
            TipoEntry.ignoreFields<CentroCusto>();

            db.SaveChanges();
            return (OCentroCusto.id > 0);
        }

        //
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);

            if (Objeto == null) {

                retorno.error = true;

                retorno.message = NotificationMessages.invalid_register_id;

            } else {

                Objeto.ativo = (Objeto.ativo != true);

                db.SaveChanges();

                retorno.active = Objeto.ativo == true? "S": "N";

                retorno.message = "Os dados foram alterados com sucesso.";

            }
            return retorno;
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            var query = db.CentroCusto.Where(x => x.id == id);

            query = query.condicoesSeguranca();

            query.Update(x => new CentroCusto { flagExcluido = true, dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;

        }


    }
}