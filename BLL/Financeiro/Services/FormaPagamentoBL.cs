using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using EntityFramework.Extensions;
using DAL.Permissao.Security.Extensions;

namespace BLL.Financeiro {

    public class FormaPagamentoBL: DefaultBL, IFormaPagamentoBL {

        //
        public FormaPagamentoBL() {
        }

        //Carregamento de registro pelo ID
        public FormaPagamento carregar(int id) {
 
            return db.FormaPagamento.Find(id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<FormaPagamento> listar(string valorBusca,string ativo) {

            var query = from P in db.FormaPagamento
                        where P.flagExcluido == "N"
                        select P;

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(string descricao,int id) {

            var query = from P in db.FormaPagamento
                        where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        select P;
            var OFormaPagamento = query.Take(1).FirstOrDefault();
            return (OFormaPagamento != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(FormaPagamento OTipoProduto) {

            if(OTipoProduto.id == 0) {
                return this.inserir(OTipoProduto);
            }

            return this.atualizar(OTipoProduto);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(FormaPagamento OFormaPagamento) {


            OFormaPagamento.setDefaultInsertValues<FormaPagamento>();
            db.FormaPagamento.Add(OFormaPagamento);
            db.SaveChanges();

            return (OFormaPagamento.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(FormaPagamento OFormaPagamento) {

            OFormaPagamento.setDefaultUpdateValues<FormaPagamento>();

            //Localizar existentes no banco
            FormaPagamento dbTipoProduto = this.carregar(OFormaPagamento.id);
            var TipoEntry = db.Entry(dbTipoProduto);
            TipoEntry.CurrentValues.SetValues(OFormaPagamento);
            TipoEntry.ignoreFields<FormaPagamento>();

            db.SaveChanges();
            return (OFormaPagamento.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.FormaPagamento
                .Where(x => x.id == id)
                .Update(x => new FormaPagamento { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }
    }
}