using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.FinanceiroLancamentos {

	public class DevedorVW {
		
        public string id { get; set; }

        public int? idOrganizacao { get; set; }

        public int idReferencia { get; set; }

        public int idPessoa { get; set; }

        public string flagTipoPessoa { get; set; }

        public string nome { get; set; }

        public string nroDocumento { get; set; }

        public string nroTelPrincipal { get; set; }

        public string nroTelSecundario { get; set; }

        public string emailPrincipal { get; set; }

        public string razaoSocial { get; set; }

        public string flagCategoriaPessoa { get; set; }

        public string descricaoCategoriaPessoa { get; set; }

        //Constructor
        public DevedorVW(){
			
        }
	}

	//
	internal sealed class DevedorVWMapper : EntityTypeConfiguration<DevedorVW> {

		public DevedorVWMapper() {
			
            this.ToTable("vw_devedor");
			this.HasKey(o => o.id);
		}
	}
}