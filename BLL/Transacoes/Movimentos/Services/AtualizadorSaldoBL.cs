using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BLL.Associados;
using BLL.Membros;
using BLL.Services;
using DAL.Associados;
using DAL.Repository.Base;
using DAL.Transacoes;
using MoreLinq;

namespace BLL.Transacoes.Movimentos {

    public class AtualizadorSaldoBL: DefaultBL, IAtualizadorSaldoBL {
        
        //Atributos
        private IMembroSaldoConsultaBL _SaldoConsultaBL;
        private IMembroSaldoCadastroBL _SaldoCadastroBL;
        
        //Servicos
        private IMembroSaldoConsultaBL SaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();
        private IMembroSaldoCadastroBL SaldoCadastroBL => _SaldoCadastroBL = _SaldoCadastroBL ?? new MembroSaldoCadastroBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno atualizar(List<Movimento> listaMovimentos) {

            listaMovimentos = listaMovimentos.Where(x => x.idMembroDestino > 0).ToList();

            var idsDestino = listaMovimentos.Select(x => x.idMembroDestino).ToList();
            
            var listaSaldos = this.carregarSaldos(listaMovimentos);

            var listaAtualizacao = new List<MembroSaldo>();

            foreach (var idDestino in idsDestino) {

                var OSaldo = listaSaldos.FirstOrDefault(x => x.idMembro == idDestino) ?? new MembroSaldo();

                var valorCreditos = listaMovimentos.Where(x => x.idMembroDestino == idDestino && x.flagCredito == true)
                                                   .Select(x => x.valor)
                                                   .DefaultIfEmpty(0)
                                                   .Sum();

                var valorDebitos = listaMovimentos.Where(x => x.idMembroDestino == idDestino && x.flagDebito == true)
                                                   .Select(x => x.valor)
                                                   .DefaultIfEmpty(0)
                                                   .Sum();

                decimal valorNovoSaldo = decimal.Add(OSaldo.saldoAtual, valorCreditos.toDecimal());
                
                valorNovoSaldo = decimal.Subtract(valorNovoSaldo, valorDebitos.toDecimal());
                    
                var SaldoAtualizado = new MembroSaldo();

                SaldoAtualizado.id = OSaldo.id;

                SaldoAtualizado.idMembro = OSaldo.idMembro;
                
                SaldoAtualizado.saldoAtual = valorNovoSaldo;
                
                listaAtualizacao.Add(SaldoAtualizado);


            }

            var Retorno = this.salvarSaldos(listaAtualizacao);

            if (Retorno.flagError) {

                return Retorno;
            }

            this.salvarMovimentos(listaMovimentos);
            
            return Retorno;
        }

        /// <summary>
        /// 
        /// </summary>
        private UtilRetorno salvarSaldos(List<MembroSaldo> listaSaldos) {

            var Retorno = UtilRetorno.newInstance(false);
            
            using (var conex = new DataContext()) {

                try {
                    var dtAtualizacao = DateTime.Now;

                    StringBuilder command = new StringBuilder();

                    foreach (var Item in listaSaldos) {

                        command.AppendLine($"update tb_membro_saldo set saldoAtual = '{Item.saldoAtual.ToString("F4", CultureInfo.GetCultureInfo("en-GB"))}', dtAtualizacaoSaldo = '{dtAtualizacao:yyyy-MM-dd HH:mm:ss.fff}' where id = {Item.id};");

                    }

                    conex.Database.ExecuteSqlCommand(command.ToString());

                } catch (Exception ex) {

                    UtilLog.saveError(ex, "Erro ao salvar saldos");
                    
                    Retorno.flagError = true;
                    
                    Retorno.listaErros.Add("Houveram problemas ao tentar atualizar os saldos.");
                    
                    Retorno.listaErros.Add($"{ex.Message} - {ex.StackTrace}");

                }

                conex.Dispose();
            }
            
            
            return Retorno;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private UtilRetorno salvarMovimentos(List<Movimento> listaMovimentos) {

            var Retorno = UtilRetorno.newInstance(false);
            
            using (var conex = new DataContext()) {

                try {
                    var dtAtualizacao = DateTime.Now;

                    StringBuilder command = new StringBuilder();

                    foreach (var Item in listaMovimentos) {

                        command.AppendLine($"update tb_movimento set dtIntegracaoSaldo = '{dtAtualizacao:yyyy-MM-dd HH:mm:ss.fff}' where id = {Item.id};");

                    }

                    conex.Database.ExecuteSqlCommand(command.ToString());

                } catch (Exception ex) {

                    UtilLog.saveError(ex, "Erro ao registrar integracao de saldo nos movimentos.");
                    
                    Retorno.flagError = true;
                    
                    Retorno.listaErros.Add("Houveram problemas ao registrar atualização nos saldos.");
                    
                    Retorno.listaErros.Add($"{ex.Message} - {ex.StackTrace}");

                }

                conex.Dispose();
            }
            
            
            return Retorno;
        }        

        /// <summary>
        /// 
        /// </summary>
        private List<MembroSaldo> carregarSaldos(List<Movimento> listaMovimentos) {

            var idsDestinos = listaMovimentos.Select(x => x.idMembroDestino.toInt())
                                             .Where(x => x > 0)
                                             .ToList();


            var listaSaldos = SaldoConsultaBL.query(0, 1)
                                             .Where(x => idsDestinos.Contains(x.idMembro))
                                             .Select(x => new {x.id, x.idMembro, x.saldoAtual})
                                             .ToListJsonObject<MembroSaldo>();

            var idsSemSaldo = idsDestinos.Except(listaSaldos.Select(x => x.idMembro).ToList());

            var listaSemSaldo = listaMovimentos.Where(x => idsSemSaldo.Contains(x.idMembroDestino.toInt()))
                                               .DistinctBy(x => x.idMembroDestino)
                                               .ToList();

            foreach (var Membro in listaSemSaldo) {

                var NovoSaldo = new MembroSaldo();

                NovoSaldo.idMembro = Membro.idMembroDestino.toInt();
                
                NovoSaldo.idPessoa = Membro.idPessoaDestino.toInt();
                
                NovoSaldo.saldoAtual = new decimal(0);

                this.SaldoCadastroBL.salvar(NovoSaldo);
                
                listaSaldos.Add(NovoSaldo);
            }

            return listaSaldos;
        }
    }

}
