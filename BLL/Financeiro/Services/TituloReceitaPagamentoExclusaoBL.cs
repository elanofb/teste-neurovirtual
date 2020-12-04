using System;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

    public class TituloReceitaPagamentoExclusaoBL : DefaultBL, ITituloReceitaPagamentoExclusaoBL {

        //Atributos

        //Propriedades

        //Excluir o registro de um pagamento e salvar os dados de log
        //Chamar rotina para reordenar descrição das parcelas
        public UtilRetorno excluirPagamento(int idTituloReceitaPagamento, string motivoExclusao, bool? flagAtualizarParcelas = true, string flagOutros = "") {
            using (var Context = new DataContext()){
                try{
                    var dbPagamento = db.TituloReceitaPagamento.condicoesSeguranca().FirstOrDefault(x => x.id == idTituloReceitaPagamento && x.dtExclusao == null);

                    if (dbPagamento == null) {
                        return UtilRetorno.newInstance(true, "Pagamento não localizado no sistema");
                    }

                    if (dbPagamento.dtPagamento.HasValue) {
                        return UtilRetorno.newInstance(true, "Não é possível remover com o pagamento já realizado");
                    }

                    dbPagamento.motivoExclusao = motivoExclusao;

                    dbPagamento.dtExclusao = DateTime.Now;

                    dbPagamento.idUsuarioExclusao = User.id();

                    dbPagamento.idUsuarioAlteracao = User.id();

                    db.SaveChanges();

                    if (flagOutros == "NEXT") {
                        Context.TituloReceitaPagamento
                            .Where(x => x.dtExclusao == null && x.idTituloReceita == dbPagamento.idTituloReceita && x.dtPagamento == null && (x.dtVencimento > dbPagamento.dtVencimento || (x.dtVencimento == dbPagamento.dtVencimento && x.id > dbPagamento.id)))
                            .Update(x => new TituloReceitaPagamento() {
                                motivoExclusao = dbPagamento.motivoExclusao,
                                dtExclusao = DateTime.Now,
                                idUsuarioExclusao = dbPagamento.idUsuarioExclusao
                            });
                    }

                    if (flagOutros == "ALL") {
                        db.TituloReceitaPagamento
                            .Where(x => x.dtExclusao == null && x.idTituloReceita == dbPagamento.idTituloReceita && x.dtPagamento == null)
                            .Update(x => new TituloReceitaPagamento() {
                                motivoExclusao = dbPagamento.motivoExclusao,
                                dtExclusao = DateTime.Now,
                                idUsuarioExclusao = dbPagamento.idUsuarioExclusao
                            });
                    }

                    if (flagAtualizarParcelas == true) {
                        this.atualizarDescricaoParcelas(dbPagamento.idTituloReceita);
                    }
                    
                    return UtilRetorno.newInstance(false, "Exclusão do(s) pagamento(s) realizado com sucesso.");
                }
                catch (Exception ex){
                    UtilLog.saveError(ex, $"Erro ao excluir manualmente o pagamento da receita {idTituloReceitaPagamento}.");
                }
            }
            return UtilRetorno.newInstance(true, "Não foi possível realizar a exclusão do(s) pagamento(s).");
        }

        /// <summary>
        /// Atualizar as descricoes de acordo a nova quantidade de parcelas
        /// </summary>
        public void atualizarDescricaoParcelas(int idTitulo) {

            var listaPagamentos = db.TituloReceitaPagamento
                                        .Where(x => x.idTituloReceita == idTitulo && x.dtExclusao == null)
                                        .OrderBy(x => x.dtVencimento)
                                        .ToList();
            int cont = 1;
            foreach (var OPagamento in listaPagamentos) {

                var textoParcela = $"Parcela {cont}";

                db.TituloReceitaPagamento.Where(x => x.id == OPagamento.id)
                                         .Update(x => new TituloReceitaPagamento { descricaoParcela = textoParcela });

                cont++;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno excluirParcelaVinculadas(int idTituloReceitaPagamento, string motivoExclusaoParam) {
            
            db.TituloReceitaPagamento
                .Where(x => x.idParcelaPrincipal == idTituloReceitaPagamento && x.dtExclusao == null)
                .Update(x => new TituloReceitaPagamento{ dtExclusao = DateTime.Now, motivoExclusao =  motivoExclusaoParam});
            
            
            return UtilRetorno.newInstance(false, "");
        }
    }
}
