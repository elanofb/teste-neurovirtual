using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.ConfiguracoesTextos {

	//
	public class Idioma {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public string descricao { get; set; }

        public string sigla { get; set; }
       
		public int idUsuarioCadastro { get; set; }
		
		public DateTime dtCadastro { get; set; }
		
		public bool? ativo { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class IdiomaMapper : EntityTypeConfiguration<Idioma> {

		public IdiomaMapper() {

			this.ToTable("tb_idioma");

            this.HasKey(o => o.id);
			
		}
	}
}