using DAL.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.CuponsDesconto {

	//
	[Serializable]
	public class CupomDesconto : DefaultEntity {

        public int idOrganizacao { get; set; }

        public string codigo { get; set; }

        public string nome { get; set; }

        public string emailPrincipal { get; set; }

        public string emailSecundario { get; set; }

        public DateTime? dtVencimento { get; set; }

        public decimal valorDesconto { get; set; }

        public int? qtdeUsos { get; set; }

        public bool flagPedido { get; set; }

        public bool flagEvento { get; set; }

        public bool flagContribuicao { get; set; }

        public bool flagUtilizado { get; set; }

        public DateTime? dtUso { get; set; }

        //Ignore
        public int? qtdeUsados { get; set; }

		public CupomDesconto() {
		}
	}

	//
	internal sealed class CupomDescontoMapper : EntityTypeConfiguration<CupomDesconto> {

		public CupomDescontoMapper() {

            this.ToTable("tb_cupom_desconto");

            this.HasKey(o => o.id);

		    this.Ignore(x => x.qtdeUsados);
		}
	}
}