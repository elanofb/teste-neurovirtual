using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAssociados {

	//
	public class ConfiguracaoAssociadoCampoGrupo {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}

        public int idTipoCampoCadastro { get; set; }

        public string descricao { get; set; }

        public string cssBoxGrupo { get; set; }

        public string htmlAposBox { get; set; }

        public bool? ativo { get; set; }
        
        public byte? ordemExibicao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual List<ConfiguracaoAssociadoCampo> listaConfiguracaoAssociadoCampos { get; set; }

	}

	//
	internal sealed class ConfiguracaoAssociadoCampoGrupoMapper : EntityTypeConfiguration<ConfiguracaoAssociadoCampoGrupo> {

		public ConfiguracaoAssociadoCampoGrupoMapper() {

			this.ToTable("systb_configuracao_associado_campo_grupo");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}