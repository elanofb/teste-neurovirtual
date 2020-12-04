using System;
using System.Linq;
using BLL.Associados;
using BLL.Services;
using DAL.Associados;
using DAL.Pedidos;
using DAL.RedeAfiliados;
using DAL.RedeAfiliados.DTO;
using DAL.RedeAfiliados.Extensions;

namespace BLL.RedeAfiliados.Services {

    public class PagadorRedeFacadeBL {
        
        
        //Atributos
        private IConfiguracaoMembroConsultaBL _ConfigConsultaBL;
        private IAssociadoConsultaBL _AssociadoConsultaBL;
        private IRedeEsquerdaConsultaBL _EsquerdaConsultaBL;
        private IRedeDireitaConsultaBL _DireitaConsultaBL;
        private IRedePontuacaoConsultaBL _PontosConsultaBL;
        private IRedeBinariaCadastroBL _RedeCadastroBL;
        private IGeradorPontuacaoRede _GeradorPontuacao;

        //Services
        private IAssociadoConsultaBL AssociadoConsultaBL => _AssociadoConsultaBL = _AssociadoConsultaBL ?? new AssociadoConsultaBL();
        private IConfiguracaoMembroConsultaBL ConfigConsultaBL => _ConfigConsultaBL = _ConfigConsultaBL ?? new ConfiguracaoMembroConsultaBL();
        private IRedeEsquerdaConsultaBL EsquerdaConsultaBL => _EsquerdaConsultaBL = _EsquerdaConsultaBL ?? new RedeEsquerdaConsultaBL();
        private IRedeDireitaConsultaBL DireitaConsultaBL => _DireitaConsultaBL = _DireitaConsultaBL ?? new RedeDireitaConsultaBL();
        private IRedePontuacaoConsultaBL PontosConsultaBL => _PontosConsultaBL = _PontosConsultaBL ?? new RedePontuacaoConsultaBL();
        private IRedeBinariaCadastroBL RedeCadastroBL => _RedeCadastroBL = _RedeCadastroBL ?? new RedeBinariaCadastroBL();
        private IGeradorPontuacaoRede GeradorPontuacao => _GeradorPontuacao = _GeradorPontuacao ?? new GeradorPontuacaoRede();
        
        
        /// <summary>
        /// 
        /// </summary>
        public PagadorRedeFacadeBL() {
        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno salvarRedeEPontucao(PedidoProduto Item) {

            int idMembro = Item.Pedido.idAssociado.toInt();

            var Membro = this.carregarMembro(idMembro);

            if (Membro == null) {
                return UtilRetorno.newInstance(true, "Os dados do membro não foram encontrados.");
            }

            var RetornoRede = this.salvarNovoMembroRede(Membro);

            if (RetornoRede.flagError) {
                
                return RetornoRede;
            }

            var NovoItemRede = RetornoRede.info as RedeBinaria;

            if (NovoItemRede != null) {

                this.GeradorPontuacao.gerarPontos(Item, NovoItemRede.flagPlanoEsquerda == true ? ChaveBinariaConst.ESQUERDA : ChaveBinariaConst.DIREITA, NovoItemRede.idMembro);
            }

            return UtilRetorno.newInstance(false);

        }

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno salvarNovoMembroRede(Associado Membro) {

            int idIndicador = Membro.idIndicador.toInt();

            var ConfiguracaoIndicador = this.carregarConfiguracaoIndicador(idIndicador);

            var NovoNivel = carregarNivelPai(ConfiguracaoIndicador, idIndicador);

            NovoNivel.idMembro = Membro.id;
            
            var NovoItemRede = this.RedeCadastroBL.salvar(NovoNivel);
            
            return UtilRetorno.newInstance(false, "", NovoItemRede);
        }

        /// <summary>
        /// 
        /// </summary>
        private NovoMembroRede carregarNivelPai(ConfiguracaoMembro ConfiguracaoIndicador, int idIndicador) {

            var NivelPai = new NovoMembroRede();

            RedeBinariaBase RedeIndicador = carregarRedeBinaria(ConfiguracaoIndicador.idChaveBinaria.toByte(), idIndicador);
            
            if (RedeIndicador == null) {

                NivelPai.idMembroPai = idIndicador;

                NivelPai.flagDireita = ConfiguracaoIndicador.idChaveBinaria == ChaveBinariaConst.DIREITA;
                
                NivelPai.flagEsquerda = ConfiguracaoIndicador.idChaveBinaria == ChaveBinariaConst.ESQUERDA;

                return NivelPai;
            }

            bool flagTemTodos = RedeIndicador.flagTemTodos();

            if (flagTemTodos) {

                int idUltimoNivel = RedeIndicador.idMembroNivel10.toInt();

                return carregarNivelPai(ConfiguracaoIndicador, idUltimoNivel);

            }

            var listaMembrosRede = RedeIndicador.toListaMembros();
            
            var MembroPai = listaMembrosRede.proximoSemFilho();

            NivelPai.idMembroPai = MembroPai.id;

            NivelPai.flagDireita = RedeIndicador.flagDireita;
                
            NivelPai.flagEsquerda = RedeIndicador.flagEsquerda;

            return NivelPai;
            
        }

        /// <summary>
        /// 
        /// </summary>
        private RedeBinariaBase carregarRedeBinaria(byte idChaveBinaria, int idIndicador) {

            RedeBinariaBase RedeIndicador;

            if (idChaveBinaria == ChaveBinariaConst.ESQUERDA) {

                RedeIndicador = this.EsquerdaConsultaBL.query().FirstOrDefault(x => x.idMembro == idIndicador) ?? new RedeBinariaEsquerdaVW();

                return RedeIndicador;

            } 
            
            if (idChaveBinaria == ChaveBinariaConst.DIREITA) {

                RedeIndicador = this.DireitaConsultaBL.query().FirstOrDefault(x => x.idMembro == idIndicador) ?? new RedeBinariaDireitaVW();
                
                return RedeIndicador;
            }

            var Pontuacao = this.PontosConsultaBL.carregarPontoPendenteMembro(idIndicador);

            idChaveBinaria = Pontuacao.qtdePendenteLE > Pontuacao.qtdePendenteLD ? ChaveBinariaConst.ESQUERDA : ChaveBinariaConst.DIREITA;

            return this.carregarRedeBinaria(idChaveBinaria, idIndicador);

        }

        /// <summary>
        /// 
        /// </summary>
        private Associado carregarMembro(int idMembro) {
            
            var OMembro = AssociadoConsultaBL.query(1)
                                             .Where(x => x.id == idMembro)
                                             .Select(x => new {x.id, x.nroAssociado, x.idIndicador})
                                             .FirstOrDefault()
                                             .ToJsonObject<Associado>();

            return OMembro;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private ConfiguracaoMembro carregarConfiguracaoIndicador(int idMembro) {
            
            var OMembro = ConfigConsultaBL.query(1)
                                             .Where(x => x.id == idMembro)
                                             .Select(x => new {x.id, x.idMembro, x.idChaveBinaria})
                                             .FirstOrDefault()
                                             .ToJsonObject<ConfiguracaoMembro>() ?? new ConfiguracaoMembro();

            return OMembro;
        }        
    }

}

