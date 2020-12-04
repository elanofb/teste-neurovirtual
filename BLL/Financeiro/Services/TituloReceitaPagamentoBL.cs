using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DAL.Financeiro;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoBL : DefaultBL, ITituloReceitaPagamentoBL {

        //Atributos

        //Propriedades

        //
        public IQueryable<TituloReceitaPagamento> query(int? idOrganizacaoParam = null) {

            var query = from Tit in this.db.TituloReceitaPagamento
                        where !Tit.dtExclusao.HasValue 
                        select Tit;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }
        
        //Carregamento do registro
        public TituloReceitaPagamento carregar(int id, bool? flagExcluido = false) {

            var query = db.TituloReceitaPagamento
                            .Include(x => x.TituloReceita)
                            .Include(x => x.FormaPagamento);

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true){
                query = query.Where(x => x.dtExclusao != null);
            }

            return query.FirstOrDefault(x => x.id == id);
        }


        //Listagem de opcoes de pagamento realizadas
        public IQueryable<TituloReceitaPagamento> listar(int idTituloReceita, bool? flagExcluido = false) {

            var query = from Tit in this.db.TituloReceitaPagamento.Include(x => x.TituloReceita) select Tit;

            query = query.condicoesSeguranca();

            if (flagExcluido == false) {
                query = query.Where(x => x.dtExclusao == null);
            }

            if (flagExcluido == true){
                query = query.Where(x => x.dtExclusao != null);
            }

            if (idTituloReceita > 0) {
                query = query.Where(x => x.idTituloReceita == idTituloReceita);
            }

            return query;
        }

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public TituloReceitaPagamento salvar(TituloReceitaPagamento OTituloReceitaPagamento) {

            OTituloReceitaPagamento.TituloReceita = null;

            OTituloReceitaPagamento.MeioPagamento = null;

            OTituloReceitaPagamento.FormaPagamento = null;
            
            OTituloReceitaPagamento.Categoria = null;
            
            OTituloReceitaPagamento.CentroCusto = null;
            
            OTituloReceitaPagamento.MacroConta = null;
            
            OTituloReceitaPagamento.ContaBancaria = null;
            
            OTituloReceitaPagamento.CidadeRecibo = null;
            
            OTituloReceitaPagamento.CupomDesconto = null;
            
            OTituloReceitaPagamento.GatewayPagamento = null;
            
            OTituloReceitaPagamento.Organizacao = null;
            
            OTituloReceitaPagamento.StatusPagamento = null;
            
            OTituloReceitaPagamento.UsuarioBaixa = null;
            
            OTituloReceitaPagamento.UsuarioExclusao = null;
            
            OTituloReceitaPagamento.ativo = true;

            if (OTituloReceitaPagamento.id == 0) {
                return this.inserir(OTituloReceitaPagamento);
            }

            return this.atualizar(OTituloReceitaPagamento);
        }

        //Persistir e inserir um novo registro 
        private TituloReceitaPagamento inserir(TituloReceitaPagamento OTituloReceitaPagamento) {

            OTituloReceitaPagamento.setDefaultInsertValues();

            using (var dataContext = new DataContext()) {

                dataContext.Configuration.AutoDetectChangesEnabled = false;

                dataContext.Configuration.ValidateOnSaveEnabled = false;

                dataContext.TituloReceitaPagamento.Add(OTituloReceitaPagamento);

                dataContext.SaveChanges();

            }

            return OTituloReceitaPagamento;
        }

        //Persistir e atualizar um registro existente 
        private TituloReceitaPagamento atualizar(TituloReceitaPagamento OTituloReceitaPagamento) {

            //Localizar existentes no banco
            //Nao aplicado condicoes de seguranca pois a atualização é usada no checkout 
            TituloReceitaPagamento dbPagamento = this.db.TituloReceitaPagamento.Find(OTituloReceitaPagamento.id);

            //Configurar valores padrão
            OTituloReceitaPagamento.setDefaultUpdateValues();

            //Atualizacao do pagamento
            var PagamentoEntry = db.Entry(dbPagamento);
            PagamentoEntry.CurrentValues.SetValues(OTituloReceitaPagamento);
            PagamentoEntry.ignoreFields();

            db.SaveChanges();

            return OTituloReceitaPagamento;
        }
        
        //Concilia o pagamento
        public void conciliarPagamentos(int[] ids, bool flagConciliado) {
            
            if (flagConciliado) {
                db.TituloReceitaPagamento.Where(x => ids.Contains(x.id))
                    .Update(x => new TituloReceitaPagamento { dtConciliacao = DateTime.Now, idUsuarioConciliacao = User.id() });
            }

            if (!flagConciliado) {
                db.TituloReceitaPagamento.Where(x => ids.Contains(x.id))
                    .Update(x => new TituloReceitaPagamento { dtConciliacao = null, idUsuarioConciliacao = null });
            }
            
        }

        public UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloReceitaPagamento.Where(x => ids.Contains(x.id))
                .Update(x => new TituloReceitaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idMacroConta = idNovaMacroConta });

            return UtilRetorno.newInstance(false);
        }

        public UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta) {

            var idUsuario = User.id();

            if (ids == null) {
                return UtilRetorno.newInstance(true, "Registros não localizado");
            }

            db.TituloReceitaPagamento.Where(x => ids.Contains(x.id))
                .Update(x => new TituloReceitaPagamento { dtAlteracao = DateTime.Now, idUsuarioAlteracao = idUsuario, idCategoria = idNovaCategoria, idMacroConta = idNovaMacroConta });

            return UtilRetorno.newInstance(false);
        }


    }
}
