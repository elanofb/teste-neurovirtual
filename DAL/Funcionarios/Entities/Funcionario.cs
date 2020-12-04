using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Cargos;
using DAL.Pessoas;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Funcionarios {

	//
	public class Funcionario : Geral {

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public int? idCargo { get; set; }

		public virtual Cargo Cargo { get; set; }

		public int? idDepartamento { get; set; }

		//public virtual Departamento Departamento { get; set; }

		public Decimal? salarioAtual { get; set; }

		public DateTime? dtAdmissao { get; set; }

		public DateTime? dtDemissao { get; set; }

		public string motivoDemissao { get; set; }
		
		public string matricula { get; set; }
		
		public int? idModeloContratacao { get; set; }
	    
		public string observacao { get; set; }

	}

	/**
	*
	*/

	internal sealed class FuncionarioMapper : EntityTypeConfiguration<Funcionario> {

		public FuncionarioMapper() {

			this.ToTable("tb_funcionario");
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasRequired(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);

			this.HasOptional(x => x.Cargo).WithMany().HasForeignKey(x => x.idCargo);

			//this.HasOptional(x => x.Departamento).WithMany().HasForeignKey(x => x.idDepartamento);

		}
	}
}