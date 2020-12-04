using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Profissoes {

	//
	public class Profissao {

		public int id { get; set; }
		
		public int? idOrganizacao { get; set; }
		
		public string descricao { get; set; }
		
		public DateTime dtCadastro { get; set; }
		
		public int idUsuarioCadastro { get; set; }
		
		public DateTime? dtAlteracao { get; set; }
		
		public int? idUsuarioAlteracao { get; set; }
		
		public bool? ativo { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
	}

	//
	internal sealed class ProfissaoMapper : EntityTypeConfiguration<Profissao> {

		public ProfissaoMapper() {
			this.ToTable("tb_profissao");
			this.HasKey(o => o.id);
		}
	}
}