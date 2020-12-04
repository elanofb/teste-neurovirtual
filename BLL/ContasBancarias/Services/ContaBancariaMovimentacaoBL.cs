using System;
using System.Linq;
using System.Data.Entity;
using EntityFramework.Extensions;
using System.Json;
using UTIL.Resources;
using System.Collections.Generic;
using BLL.Core.Events;
using BLL.Services;
using DAL.ContasBancarias;
using DAL.Permissao.Security.Extensions;

namespace BLL.ContasBancarias {

    public class ContaBancariaMovimentacaoBL : DefaultBL, IContaBancariaMovimentacaoBL {

        // Events
        private EventAggregator onTransferenciaRealizada => OnTransferenciaRealizada.getInstance;

        //
        public ContaBancariaMovimentacaoBL(){
        }

        //Carregamento de registro pelo ID
        public ContaBancariaMovimentacao carregar(int id) {

            var query = from P in db.ContaMovimentacao.condicoesSeguranca()
                                                      .Include(x => x.ContaBancariaOrigem)
                                                      .Include(x => x.ContaBancariaDestino)
                                where P.flagExcluido == "N"
                                select P;

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<ContaBancariaMovimentacao> listar(string valorBusca, string ativo, int idContaBancariaOrigem, int idTipoOperacao, DateTime? dtInicio, DateTime? dtFim) {

            var query = from P in db.ContaMovimentacao.condicoesSeguranca()
                                                      .Include(x => x.ContaBancariaOrigem)
                                                      .Include(x => x.ContaBancariaDestino)
                        where P.flagExcluido == "N"
                        select P;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (idContaBancariaOrigem > 0) {
                query = query.Where(x => x.idContaBancariaOrigem == idContaBancariaOrigem);
            }

            if (idTipoOperacao > 0) {
                query = query.Where(x => x.idTipoOperacao == idTipoOperacao);
            }

            if (dtInicio != null && dtFim != null) {
                query = query.Where(x => x.dtOperacao >= dtInicio && x.dtOperacao <= dtFim);
            }

            return query;
        }

        //Verificar se já existe um registro com a descrição informada, no entanto, que possua id diferente do informado
        public bool existe(ContaBancariaMovimentacao oContaMovimentacao, bool descricao) {

            var query = db.ContaMovimentacao.Select(n => n).Where(x => x.flagExcluido == "N");

            if (oContaMovimentacao.id > 0) {
                query = query.Where(x => x.id != oContaMovimentacao.id);
            }

            if (descricao) {
                query = query.Where(x => x.descricao == oContaMovimentacao.descricao);
                return query.Any();
            }

            return query.Any();
        }

        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(ContaBancariaMovimentacao oContaMovimentacao) {

            if (oContaMovimentacao.id == 0) {
                return this.inserir(oContaMovimentacao);
            }

            return this.atualizar(oContaMovimentacao);
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(ContaBancariaMovimentacao oContaMovimentacao) {
            
            oContaMovimentacao.setDefaultInsertValues();

            db.ContaMovimentacao.Add(oContaMovimentacao);

            db.SaveChanges();

            //
            this.onTransferenciaRealizada.subscribe(new OnTransferenciaRealizadaHandler());
            this.onTransferenciaRealizada.publish(oContaMovimentacao as object);

            return (oContaMovimentacao.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(ContaBancariaMovimentacao oContaMovimentacao) {

            oContaMovimentacao.setDefaultUpdateValues();

            //Localizar existentes no ContaMovimentacao
            ContaBancariaMovimentacao dbContaBancariaMovimentacao = this.carregar(oContaMovimentacao.id);

            if (dbContaBancariaMovimentacao == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbContaBancariaMovimentacao);

            TipoEntry.CurrentValues.SetValues(oContaMovimentacao);

            TipoEntry.ignoreFields();

            db.SaveChanges();

            return (oContaMovimentacao.id > 0);
        }

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {

            var idUsuario = User.id();

            db.ContaMovimentacao
                .Where(x => x.id == id)
                .Update(x => new ContaBancariaMovimentacao { flagExcluido = "S", dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario });

            return true;
        }

        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            var Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (Objeto.ativo == "S" ? "N" : "S");
                db.SaveChanges();
                retorno.active = Objeto.ativo;
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }

        public IQueryable<ContaBancariaMovimentacao> listarPorId(List<int> ids) {

            return db.ContaMovimentacao.Select(x => x).Where(x => ids.Contains(x.id));
        }

        public IQueryable<ContaBancariaMovimentacao> listarPorContaBancaria(int idContaBancaria) {

            return db.ContaMovimentacao
                .Select(x => x)
                .Where(x => x.idContaBancariaOrigem == idContaBancaria || x.idContaBancariaDestino == idContaBancaria)
                .Where(x => x.flagExcluido == "N");
        }

        //Concilia o pagamento
        public void conciliarMovimentacao(int id, bool flagConciliado) {
            db.ContaMovimentacao.Where(x => x.id == id)
                .Update(x => new ContaBancariaMovimentacao { flagConciliado = flagConciliado });
        }
    }
}