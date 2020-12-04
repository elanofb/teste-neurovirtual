using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System.Collections.Generic;
using DAL.Organizacoes;

namespace DAL.MateriaisApoio {

	public class MaterialApoio {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public byte? idTipoMaterialApoio { get; set; }

        public TipoMaterialApoio TipoMaterialApoio { get; set; }

		public string titulo { get; set; }

		public string descricao { get; set; }

        public string flagDisponibilidadeAssociado { get; set; }

		public DateTime? dtInicioDisponivel { get; set; }

		public DateTime? dtFinalDisponivel { get; set; }

        public List<MaterialApoioPessoa> listaPessoasPermitidas { get; set; }

        public MaterialApoio() { 
            this.listaPessoasPermitidas = new List<MaterialApoioPessoa>();
        }

	}

	public class MaterialApoioMapper : EntityTypeConfiguration<MaterialApoio> {

		public MaterialApoioMapper() {
			
			this.ToTable("tb_material_apoio");
			
			this.HasKey(x => x.id);
			
            this.HasOptional(x => x.TipoMaterialApoio).WithMany().HasForeignKey(x => x.idTipoMaterialApoio);
			
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
		}
	}
}