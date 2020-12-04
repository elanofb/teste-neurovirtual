using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Financeiro;
using DAL.Fornecedores;
using DAL.Pessoas;

namespace DAL.Contratos {

	//
	public class Contrato : DefaultEntity {

		public int idTipoContrato { get; set; }

		public virtual TipoContrato TipoContrato { get; set; }

		public int? idContratoVinculado { get; set; }

		public virtual Contrato ContratoVinculado { get; set; }

		public int idStatusContrato { get; set; }

		public virtual StatusContrato StatusContrato { get; set; }

		public int? idFornecedor { get; set; }

		public virtual Fornecedor Fornecedor { get; set; }

		public int? idPessoaContratante { get; set; }

	    public virtual Associado PessoaContratante { get; set; }

        public int? idMacroConta { get; set; }

        public int? idSubConta { get; set; }

        public int? idCentroCusto { get; set; }

		public virtual CentroCusto CentroCusto { get; set; }

		public string titulo { get; set; }

		public string objetoContrato { get; set; }

		public string nroContrato { get; set; }

		public DateTime? dtInicioVigencia { get; set; }

		public DateTime? dtFimVigencia { get; set; }

		public decimal? valorTotal { get; set; }

	    public decimal? icms { get; set; }

	    public decimal? pis { get; set; }

	    public decimal? cofins { get; set; }

	    public decimal? iss { get; set; }

	    public bool? flagInserirImpostos { get; set; }

        public string flagOperacaoFinanceira { get; set; }

		public string flagRenovado { get; set; }
	}

	//
	internal sealed class ContratoMapper : EntityTypeConfiguration<Contrato> {

		public ContratoMapper() {

			this.ToTable("tb_contrato");

            this.HasKey(o => o.id);

            this.HasOptional(o => o.ContratoVinculado).WithMany().HasForeignKey(o => o.idContratoVinculado);

            this.HasOptional(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);

            this.HasOptional(o => o.Fornecedor).WithMany().HasForeignKey(o => o.idFornecedor);

            this.HasOptional(o => o.PessoaContratante).WithMany().HasForeignKey(o => o.idPessoaContratante);

            this.HasRequired(o => o.StatusContrato).WithMany().HasForeignKey(o => o.idStatusContrato);

            this.HasRequired(o => o.TipoContrato).WithMany().HasForeignKey(o => o.idTipoContrato);
		}
	}
}