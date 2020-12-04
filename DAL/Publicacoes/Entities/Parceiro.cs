using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;
using System;

namespace DAL.Publicacoes {

	public class Parceiro {

        public int id { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public int? idTipoParceiro { get; set; }
		
		public virtual TipoParceiro TipoParceiro { get; set; }

		public string nome { get; set; }

		public string link { get; set; }

		public virtual ArquivoUpload Logotipo { get; set;}
	}

	internal sealed class ParceiroMapper : EntityTypeConfiguration<Parceiro> {

		public ParceiroMapper() {
			this.ToTable("tb_parceiro");

			this.HasKey(x => x.id);

			this.Ignore(x => x.Logotipo);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

            this.HasOptional(o => o.TipoParceiro).WithMany().HasForeignKey(o => o.idTipoParceiro);
		}
	}
}