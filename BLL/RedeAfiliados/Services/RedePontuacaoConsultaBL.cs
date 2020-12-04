using System;
using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;
using DAL.RedeAfiliados.DTO;

namespace BLL.RedeAfiliados.Services {


    public class RedePontuacaoConsultaBL: DefaultBL, IRedePontuacaoConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public RedePontuacaoConsultaBL() {
            
        }
        
        //
        public IQueryable<RedePontuacao> query() {

            var query = from Rede in db.RedePontuacao
                        where Rede.dtExclusao == null
                        select Rede;

            return query;

        }        

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RedePontuacao> listar(int? idMembro, bool? flagPago) {

            var query = this.query();

            if (idMembro.HasValue) {
                
                query = query.Where(x => x.idMembro == idMembro);
            }

            if (flagPago.HasValue) {
                
                query = query.Where(x => x.flagPago == flagPago);
            }

            return query;

        }          
        
        /// <summary>
        /// 
        /// </summary>
        public PontuacaoMembroDTO carregarPorMembro(int idMembro) {

            var PontoCarreira = this.carregarPontoCarreiraMembro(idMembro);
            
            var PontoPendente = this.carregarPontoPendenteMembro(idMembro);
            
            var Pontuacao = new PontuacaoMembroDTO();

            Pontuacao.qtdeCarreiraLE = PontoCarreira.qtdeCarreiraLE;

            Pontuacao.qtdeCarreiraLD = PontoCarreira.qtdeCarreiraLD;

            Pontuacao.qtdeCarreiraTotal = PontoCarreira.qtdeCarreiraTotal;

            Pontuacao.qtdePendenteLE = PontoPendente.qtdePendenteLE;
            
            Pontuacao.qtdePendenteLD = PontoPendente.qtdePendenteLD;
            
            Pontuacao.qtdePendenteTotal = PontoPendente.qtdePendenteTotal;
            
            Pontuacao.qtdeDiaLE = PontoPendente.qtdeDiaLE;

            Pontuacao.qtdeDiaLD = PontoPendente.qtdeDiaLD;

            Pontuacao.qtdeDiaTotal = PontoPendente.qtdeDiaTotal;
            
            return Pontuacao;

        }        
        
        /// <summary>
        /// 
        /// </summary>
        public PontuacaoMembroDTO carregarPontoCarreiraMembro(int idMembro) {

            var Pontuacao = new PontuacaoMembroDTO();

            var listaPagos = this.listar(idMembro, true)
                                  .Select(x => new {x.id, x.qtdePontos, x.flagLadoEsquerdo, x.flagLadoDireito})
                                  .ToListJsonObject<RedePontuacao>();


            Pontuacao.qtdeCarreiraLE = listaPagos.Where(x => x.flagLadoEsquerdo == true)
                                                 .Select(x => x.qtdePontos)
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            Pontuacao.qtdeCarreiraLD = listaPagos.Where(x => x.flagLadoEsquerdo != true)
                                                 .Select(x => x.qtdePontos)
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            Pontuacao.qtdeCarreiraTotal = decimal.Add(Pontuacao.qtdeCarreiraLE, Pontuacao.qtdeCarreiraLD);

            
            return Pontuacao;

        }         
        
        /// <summary>
        /// 
        /// </summary>
        public PontuacaoMembroDTO carregarPontoPendenteMembro(int idMembro) {

            var Pontuacao = new PontuacaoMembroDTO();

            var listaPendente = this.listar(idMembro, false)
                                     .Select(x => new {x.id, x.qtdePontos, x.flagLadoEsquerdo, x.flagLadoDireito})
                                     .ToListJsonObject<RedePontuacao>();

            Pontuacao.qtdePendenteLE = listaPendente.Where(x => x.flagLadoEsquerdo == true)
                                                 .Select(x => x.qtdePontos)
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            Pontuacao.qtdePendenteLD = listaPendente.Where(x => x.flagLadoEsquerdo != true)
                                                 .Select(x => x.qtdePontos)
                                                 .DefaultIfEmpty(0)
                                                 .Sum();

            Pontuacao.qtdePendenteTotal = Pontuacao.qtdePendenteLE > Pontuacao.qtdePendenteLD? Pontuacao.qtdePendenteLE:  Pontuacao.qtdePendenteLD;


            var dtFiltroIni = DateTime.Today;
            
            var dtFiltroFim = DateTime.Today.AddDays(1);
            
            var listaDia = listaPendente.Where(x => x.dtCadastro >= dtFiltroIni && x.dtCadastro < dtFiltroFim).ToList();

            Pontuacao.qtdeDiaLE = listaDia.Where(x => x.flagLadoEsquerdo == true)
                                                    .Select(x => x.qtdePontos)
                                                    .DefaultIfEmpty(0)
                                                    .Sum();

            Pontuacao.qtdeDiaLD = listaDia.Where(x => x.flagLadoEsquerdo != true)
                                                    .Select(x => x.qtdePontos)
                                                    .DefaultIfEmpty(0)
                                                    .Sum();

            Pontuacao.qtdeDiaTotal = Pontuacao.qtdeDiaLE > Pontuacao.qtdeDiaLD? Pontuacao.qtdeDiaLE:  Pontuacao.qtdeDiaLD;
            
            return Pontuacao;


        }           
    }

}

