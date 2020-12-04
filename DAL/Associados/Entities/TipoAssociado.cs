using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.ContasBancarias;
using DAL.Contribuicoes;
using DAL.Entities;
using DAL.Financeiro;

namespace DAL.Associados {

    [Serializable]
    public class TipoAssociado {

        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public int? idOrganizacao { get; set; }

        public string nomeDisplay { get; set; }
        
        public int? idCategoria { get; set; }

        public virtual CategoriaTipoAssociado Categoria { get; set; }

        public bool flagPessoaFisica { get; set; }

        public bool flagPessoaJuridica { get; set; }

        public bool flagEstudante { get; set; }

        public decimal? valorTaxaInscricao { get; set; }

        public byte? diasPrazoTaxaInscricao { get; set; }

        public int? idCentroCustoInscricao { get; set; }

        public virtual CentroCusto CentroCustoInscricao { get; set; }

        public int? idMacroContaInscricao { get; set; }

        public virtual MacroConta MacroContaInscricao { get; set; }

        public int? idContaBancariaInscricao { get; set; }

        public virtual ContaBancaria ContaBancariaInscricao { get; set; }
        
        public bool? flagGerarCobrancaPosCadastro { get; set; }
        
        public byte? diasPrazoPrimeiraCobranca { get; set; }
        
        public int? idContribuicaoPadrao { get; set; }

        public virtual Contribuicao ContribuicaoPadrao { get; set; }

        public short? ordem { get; set; }
        
        public bool? flagIsento { get; set; }

        public bool? flagAreaAssociado { get; set; }

        public bool? flagNaoAssociado { get; set; }

        public bool? flagDependente { get; set; }

        public bool? flagProcessoAdmissao { get; set; }

        public string observacoes { get; set; }

        public string flagSistema { get; set; }

    }

    internal sealed class TipoAssociadoMapper : EntityTypeConfiguration<TipoAssociado> {

        public TipoAssociadoMapper() {

            this.ToTable("tb_tipo_associado");

            this.HasKey(o => o.id);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasOptional(x => x.Categoria).WithMany().HasForeignKey(x => x.idCategoria);

            this.HasOptional(x => x.ContribuicaoPadrao).WithMany().HasForeignKey(x => x.idContribuicaoPadrao);

            this.HasOptional(x => x.CentroCustoInscricao).WithMany().HasForeignKey(x => x.idCentroCustoInscricao);

            this.HasOptional(x => x.MacroContaInscricao).WithMany().HasForeignKey(x => x.idMacroContaInscricao);

            this.HasOptional(x => x.ContaBancariaInscricao).WithMany().HasForeignKey(x => x.idContaBancariaInscricao);
        }
    }
}