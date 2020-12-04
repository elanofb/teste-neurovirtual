using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Planos {

	//
	public class Plano : DefaultEntity {

        public int? idOrganizacao { get; set; }

		public string descricao { get; set; }

		public string nome { get; set; }

		public string codigo { get; set; }

		public Decimal valor { get; set; }

		public int qtdeMesVigencia { get; set; }

        public bool? flagExibirPortal { get; set; }
	}

	//
	internal sealed class PlanoMapper : EntityTypeConfiguration<Plano> {

		public PlanoMapper() {
			this.ToTable("tb_plano");
			this.HasKey(o => o.id);
		}
	}
}