using System;
using System.Linq;
using BLL.Associados;
using BLL.Pedidos;
using BLL.Services;
using DAL.Associados;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras{
    
    public abstract class MediadorBase : IMediadorBase{
        
        //Atributos
        protected IAssociadoConsultaBL _AssociadoConsultaBL;         
        
        //Propriedades
        protected IAssociadoConsultaBL AssociadoConsultaBL => _AssociadoConsultaBL = _AssociadoConsultaBL ?? new AssociadoConsultaBL();        
        
        /// <summary>
        /// 
        /// </summary>
        public virtual MovimentoOperacaoDTO carregarDados(int idPessoaOrigem, int idPessoaDestino, decimal valorTransacao, int idReferencia) {
            
            var Movimento = new MovimentoOperacaoDTO();
            
            Movimento.MembroOrigem = this.carregarMembroOrigem(idPessoaOrigem);
            
            Movimento.nroContaOrigem = Movimento.MembroOrigem.nroAssociado.toInt();
            
            Movimento.MembroDestino = this.carregarMembroDestino(idPessoaDestino);
            
            Movimento.nroContaDestino = Movimento.MembroDestino.nroAssociado.toInt();                        
            
            Movimento.valorOperacao = valorTransacao;                  
            
            return Movimento;

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Associado carregarMembroOrigem(int idPessoaOrigem) {
            
            var MembroCompra = Queryable.Where<Associado>(this.AssociadoConsultaBL.queryNoFilter(1), x => x.idPessoa == idPessoaOrigem)
                .Select(x => new {
                    x.id, 
                    x.nroAssociado,
                    x.idPessoa,
                    Pessoa = new {
                        x.Pessoa.id,
                        x.Pessoa.nome,
                        x.Pessoa.nroDocumento
                    }
                }).FirstOrDefault()
                .ToJsonObject<Associado>() ?? new Associado();
            
            return MembroCompra;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual  Associado carregarMembroDestino(int idPessoaDestino) {
            
            var MembroLinkey = Queryable.Where<Associado>(this.AssociadoConsultaBL.queryNoFilter(1), x => x.idPessoa == idPessoaDestino)
                .Select(x => new {
                    x.id, 
                    x.nroAssociado,
                    x.idPessoa,
                    Pessoa = new {
                        x.Pessoa.id,
                        x.Pessoa.nome,
                        x.Pessoa.nroDocumento
                    }
                }).FirstOrDefault()
                .ToJsonObject<Associado>() ?? new Associado();

            return MembroLinkey;
            
        }
    }
}