using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Pessoas;

namespace DAL.OrganizacaoConfiguracoes {

	//
	public class OrganizacaoDadosAssociado {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

		public bool? flagCadastroAssociado { get; set; }
		
		public bool? flagSituacaoAssociado { get; set; }
		
		public bool? flagDadosEndereco { get; set; }
		
		public bool? flagDadosTelefone { get; set; }
		
		public bool? flagDadosEmail { get; set; }
		
		public bool? flagDadosProfissao { get; set; }
		
		public bool? flagDadosEmpresa { get; set; }
		
		public bool? flagDadosResponsavelEmpresa { get; set; }
		
		public bool? flagAreasAtuacao { get; set; }
		
		public bool? flagDadosFuncionario { get; set; }

		public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

		public UsuarioSistema UsuarioCadastro { get; set; }
		
        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
		
	}

	//
	internal sealed class OrganizacaoDadosAssociadoMapper : EntityTypeConfiguration<OrganizacaoDadosAssociado> {

		public OrganizacaoDadosAssociadoMapper() {

			this.ToTable("tb_organizacao_dados_associado");

		    this.HasKey(x => x.id);

			this.HasRequired(u => u.UsuarioCadastro).WithMany().HasForeignKey(u => u.idUsuarioCadastro);
			
			this.HasOptional(u => u.Organizacao).WithMany().HasForeignKey(u => u.idOrganizacao);
			
		}
	}
}