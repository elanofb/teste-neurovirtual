using System.Linq;
using BLL.Associados;
using BLL.Services;
using DAL.Associados;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public class CarregadorDadosOperacao : ICarregadorDadosOperacao {
        
        //Atributos
        private IAssociadoConsultaBL _MembroConsultaBL;
        
        //Servicos
        private IAssociadoConsultaBL MembroConsultaBL => _MembroConsultaBL = _MembroConsultaBL ?? new AssociadoConsultaBL();

        /// <summary>
        /// 
        /// </summary>
        public MovimentoOperacaoDTO carregar(MovimentoOperacaoDTO Transacao) {

            var listaMembros = MembroConsultaBL.queryNoFilter(1)
                                               .Where(x => x.nroAssociado == Transacao.nroContaOrigem ||
                                                           x.nroAssociado == Transacao.nroContaDestino
                                                     )
                                               .Select(x => new {
                                                                    x.id,
                                                                    x.idPessoa,
                                                                    x.nroAssociado,
                                                                    x.idTipoAssociado,
                                                                    x.idTipoCadastro,
                                                                    x.percentualDesconto,
                                                                    x.idIndicador,
                                                                    x.idIndicadorSegundoNivel,
                                                                    x.idIndicadorTerceiroNivel,
                                                                    x.senhaTransacao,
                                                                    Pessoa = new {
                                                                                     x.Pessoa.nome,
                                                                                     x.Pessoa.nroDocumento
                                                                                 },
                                                                    x.ativo
                                                                })
                                               .ToListJsonObject<Associado>();


            Transacao.MembroOrigem = listaMembros.FirstOrDefault(x => x.nroAssociado == Transacao.nroContaOrigem);
            
            Transacao.MembroDestino = listaMembros.FirstOrDefault(x => x.nroAssociado == Transacao.nroContaDestino);

            return Transacao;
        }
        
    }

}
