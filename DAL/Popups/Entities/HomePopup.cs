using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Popups {


	public class HomePopup {
        
        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idPortal { get; set; }

        public virtual Portal Portal { get; set; }

        public string titulo { get; set; }

		public string conteudo { get; set; }

		public DateTime? dtInicioExibicao { get; set; }

        public DateTime? dtFimExibicao { get; set; }

        public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }


		public HomePopup() { 
			
		}

	}

	//
	internal sealed class HomePopupMapper : EntityTypeConfiguration<HomePopup> {

		public HomePopupMapper() {

			this.ToTable("tb_home_popup");

			this.HasKey(x => x.id);
            
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.Portal).WithMany().HasForeignKey(x => x.idPortal);

        }
	}
}