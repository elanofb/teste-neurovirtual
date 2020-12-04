using System.Data.Entity.ModelConfiguration;
using System;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Paginas {

    //
	public class PaginaEstatuto {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public string titulo { get; set; }

        public string texto { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioExclusao { get; set; }
        

		public PaginaEstatuto() { 

		}

	}

	//
	internal sealed class PaginaEstatutoMapper : EntityTypeConfiguration<PaginaEstatuto> {

		public PaginaEstatutoMapper() {

			this.ToTable("tb_pagina_estatuto");

			this.HasKey(x => x.id);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

            this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);

            this.HasOptional(o => o.UsuarioExclusao).WithMany().HasForeignKey(o => o.idUsuarioExclusao);
        }
	}
}