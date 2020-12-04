using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Configuracoes;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAssociados {

	//
    public class ConfiguracaoAssociadoCampo {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int idTipoCampoCadastro { get; set; }
        
        public string label { get; set; }
		
        public int idAssociadoCampoGrupo { get; set; }
		
        public virtual ConfiguracaoAssociadoCampoGrupo AssociadoCampoGrupo { get; set; }
		
        public int idTipoCampo { get; set; }

        public virtual ConfiguracaoTipoCampo TipoCampo { get; set; }
		
        public short? idFuncaoFiltro { get; set; }
		
        public virtual FuncaoFiltro FuncaoFiltro { get; set; }
		
        public string nameHelper { get; set; }

        public string methodHelper { get; set; }

        public string parametrosHelper { get; set; }

        public string name { get; set; }

        public string nameDescription { get; set; }

        public string idDOM { get; set; }

        public bool? flagAreaAssociado { get; set; }

        public bool? flagAreaAdm { get; set; }

        public bool? flagCadastro { get; set; }

        public bool? flagEdicao { get; set; }

        public bool? flagAssociadoPodeEditar { get; set; }

        public bool? flagDependente { get; set; }

        public bool? flagExibir { get; set; }

        public bool? flagObrigatorio { get; set; }

        public bool? flagExibirOptionVazio { get; set; }
		
        public string valorFixo { get; set; }

        public string valorPadrao { get; set; }
		
        public int? minlength { get; set; }
	
        public int? maxlength { get; set; }
	    
        public string mask { get; set; }
		
        public string cssClassBox { get; set; }

        public string cssClassCampo { get; set; }

        public string textoInstrucoes { get; set; }

        public string mensagemErro { get; set; }

        public string htmlAfterBox { get; set; }

        public string htmlAposCampo { get; set; }

        public int? ordemExibicao { get; set; }

        public bool? ativo { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual List<ConfiguracaoAssociadoCampoPropriedade> listaCampoPropriedades { get; set; }

        public virtual List<ConfiguracaoAssociadoCampoOpcao> listaCampoOpcoes { get; set; }

        public virtual List<ConfiguracaoAssociadoCampoTipoAssociado> listaTipoAssociado { get; set; }

        public bool? flagMultiSelect { get; set; }

        public bool flagValidado { get; set; }

        public string valorAtual { get; set; }

        public List<int> idsTipoAssociado { get; set; }
	    
	    public bool? flagReadOnly { get; set; }

        public ConfiguracaoAssociadoCampo() {

            this.listaCampoPropriedades = new List<ConfiguracaoAssociadoCampoPropriedade>();

            this.listaCampoOpcoes = new List<ConfiguracaoAssociadoCampoOpcao>();

            this.listaTipoAssociado = new List<ConfiguracaoAssociadoCampoTipoAssociado>();

            this.idsTipoAssociado = new List<int>();
        }
    }

    //
	internal sealed class ConfiguracaoAssociadoCampoMapper : EntityTypeConfiguration<ConfiguracaoAssociadoCampo> {

		public ConfiguracaoAssociadoCampoMapper() {

			this.ToTable("systb_configuracao_associado_campo");

            this.HasKey(o => o.id);

		    this.HasRequired(x => x.AssociadoCampoGrupo).WithMany(x => x.listaConfiguracaoAssociadoCampos).HasForeignKey(x => x.idAssociadoCampoGrupo);

		    this.HasRequired(x => x.TipoCampo).WithMany().HasForeignKey(x => x.idTipoCampo);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.FuncaoFiltro).WithMany().HasForeignKey(x => x.idFuncaoFiltro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		    this.Ignore(x => x.flagValidado);

            this.Ignore(x => x.valorAtual);

            this.Ignore(x => x.idsTipoAssociado);
			
			this.Ignore(x => x.flagReadOnly);
		}
	}
}