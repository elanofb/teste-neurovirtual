using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using DAL.Organizacoes;
using DAL.Portais;

namespace DAL.Publicacoes {
    
	public class Conteudo {

        public int id { get; set; }
		
		public int? idOrganizacao { get; set; }
		
		public Organizacao Organizacao { get; set; }

        public string idInterno { get; set; }
		
		public string titulo { get; set; }
		
		public string conteudo { get; set; }

        public DateTime? dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }
		
        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public bool ativo { get; set; }

        public DateTime? dtExclusao { get; set; }
		
		public int idUsuarioExclusao { get; set; }
        
		public Conteudo() { 
			
		}
	}

	//
	internal sealed class ConteudoMapper : EntityTypeConfiguration<Conteudo> {

		public ConteudoMapper() {

			this.ToTable("tb_conteudo");

			this.HasKey(x => x.id);
			
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
			
        }
	}
}