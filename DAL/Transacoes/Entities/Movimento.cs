using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Pessoas;

namespace DAL.Transacoes {

    public class Movimento {

        public long id { get; set; }

        public int idOrganizacao { get; set; }

        public long? codigo { get; set; }

        public byte idTipoTransacao { get; set; }

        public TipoTransacao TipoTransacao { get; set; }
        
        public bool flagDebito { get; set; }
        
        public bool flagCredito { get; set; }

        public int? idMembroOrigem { get; set; }

        public int? idPessoaOrigem { get; set; }

        public Pessoa PessoaOrigem { get; set; }

        public int? idMembroDestino { get; set; }

        public int? idPessoaDestino { get; set; }

        public Pessoa PessoaDestino { get; set; }

        public decimal? valor { get; set; }

        public long? idMovimentoPrincipal { get; set; }

        public Movimento MovimentoPrincipal { get; set; }

        public decimal? valorMovimentoPrincipal { get; set; }

        public decimal? percentualMovimentoPrincipal { get; set; }
        
        public int? idPedidoProduto { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public bool? flagSaldoInicial { get; set; }
        
        public int? idImportacao { get; set; }
        
        public string observacao { get; set; }
        
        public string ip { get; set; }

        public string browser { get; set; }

        public string so { get; set; }

        public byte? idOrigem { get; set; }

        public DateTime? dtIntegracaoSaldo { get; set; }
        
        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }
        
        public bool flagMovimentoOrigem { get; set; }
        
        public bool flagMovimentoDestino { get; set; }
    }

    //
    internal sealed class MovimentoMapper : EntityTypeConfiguration<Movimento> {

        public MovimentoMapper() {

            this.ToTable("tb_movimento");

            this.HasKey(o => o.id);
            
            this.Property(x => x.valor).HasPrecision(18, 4);
            
            this.Property(x => x.valorMovimentoPrincipal).HasPrecision(18, 4);


            this.HasRequired(x => x.TipoTransacao)
                .WithMany()
                .HasForeignKey(x => x.idTipoTransacao);

            this.HasRequired(x => x.PessoaOrigem).WithMany().HasForeignKey(x => x.idPessoaOrigem);

            this.HasRequired(x => x.PessoaDestino).WithMany().HasForeignKey(x => x.idPessoaDestino);

            this.HasOptional(x => x.MovimentoPrincipal).WithMany().HasForeignKey(x => x.idMovimentoPrincipal);

            this.Ignore(x => x.flagMovimentoOrigem);
            
            this.Ignore(x => x.flagMovimentoDestino);
        }
    }

}
