using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class GatewayPagamentoBL : DefaultBL, IGatewayPagamentoBL {

        //
        public GatewayPagamentoBL() {
        }

        //Carregamento de registro pelo ID
        public GatewayPagamento carregar(int id) {
            
			return db.GatewayPagamento.FirstOrDefault(x => x.flagExcluido == false);
        }

        //Listagem de registros de acordo com filtros
        public IQueryable<GatewayPagamento> listar(string valorBusca, bool? ativo) {

			var query = from P in db.GatewayPagamento
                        where P.flagExcluido == false
                        select P;

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
            
			var query = from P in db.GatewayPagamento
                        where P.descricao == descricao && P.id != id && P.flagExcluido == false
                        select P;

            var OGatewayPagamento = query.FirstOrDefault();

            return (OGatewayPagamento != null);
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(GatewayPagamento OGatewayPagamento) {

            if(OGatewayPagamento.id == 0) {
                return this.inserir(OGatewayPagamento);
            }

            return this.atualizar(OGatewayPagamento);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(GatewayPagamento OGatewayPagamento) {
            
            OGatewayPagamento.setDefaultInsertValues();
            db.GatewayPagamento.Add(OGatewayPagamento);
            db.SaveChanges();

            return (OGatewayPagamento.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(GatewayPagamento OGatewayPagamento) {

            OGatewayPagamento.setDefaultUpdateValues();

            //Localizar existentes no banco
            GatewayPagamento dbGateway = this.carregar(OGatewayPagamento.id);
            
			var GatewayEntry = db.Entry(dbGateway);
            
			GatewayEntry.CurrentValues.SetValues(OGatewayPagamento);
            
			GatewayEntry.ignoreFields();

            db.SaveChanges();
            
			return (OGatewayPagamento.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.GatewayPagamento
                .Where(x => x.id == id)
                .Update(x => new GatewayPagamento { flagExcluido = true,dtAlteracao = DateTime.Now,idUsuarioAlteracao = idUsuario });

            return true;
        }
    }
}