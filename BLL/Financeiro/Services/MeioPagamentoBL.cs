using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class MeioPagamentoBL: DefaultBL, IMeioPagamentoBL {

        public const string keyCache = "meio_pagamento";

        //
        public MeioPagamentoBL() {
        }

        //Carregamento de registro pelo ID
        public MeioPagamento carregar(int id) {

            return db.MeioPagamento.FirstOrDefault(x => x.id == id);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<MeioPagamento> listar(string valorBusca,string ativo) {

            var query = from P in db.MeioPagamento
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
            
			var query = from P in db.MeioPagamento
                        where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        select P;
            
			var OMeioPagamento = query.Take(1).FirstOrDefault();

            return (OMeioPagamento != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(MeioPagamento OMeioPagamento) {

            if(OMeioPagamento.id == 0) {
                return this.inserir(OMeioPagamento);
            }

            return this.atualizar(OMeioPagamento);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(MeioPagamento OMeioPagamento) {

            OMeioPagamento.setDefaultInsertValues();
            db.MeioPagamento.Add(OMeioPagamento);
            db.SaveChanges();

            return (OMeioPagamento.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(MeioPagamento OMeioPagamento) {

            OMeioPagamento.setDefaultUpdateValues();

            //Localizar existentes no banco
            MeioPagamento dbRegistro = this.carregar(OMeioPagamento.id);
            var TipoEntry = db.Entry(dbRegistro);
            TipoEntry.CurrentValues.SetValues(OMeioPagamento);
            TipoEntry.ignoreFields();

            db.SaveChanges();
            return (OMeioPagamento.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.MeioPagamento
                .Where(x => x.id == id)
                .Update(x => new MeioPagamento { flagExcluido = "S",dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }
    }
}