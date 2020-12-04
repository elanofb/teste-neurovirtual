using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.LinksUteis {

	public class LinkUtil {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public string link { get; set; }

		public string flagBlank { get; set; }

	}

	internal sealed class LinkUtilMapper : EntityTypeConfiguration<LinkUtil> {

		public LinkUtilMapper() {
			this.ToTable("tb_link_util");
			this.HasKey(x => x.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
    }
}