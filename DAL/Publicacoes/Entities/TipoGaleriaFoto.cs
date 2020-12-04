using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {

	//
	public class TipoGaleriaFoto {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

		public bool flagSistema { get; set; }
	}

	//
	internal sealed class TipoGaleriaFotoMapper : EntityTypeConfiguration<TipoGaleriaFoto> {

		public TipoGaleriaFotoMapper() {

			this.ToTable("tb_tipo_galeria_foto");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
	}
}