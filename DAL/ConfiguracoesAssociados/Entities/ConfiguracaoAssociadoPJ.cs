﻿using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.ConfiguracoesAssociados {

	//
	public class ConfiguracaoAssociadoPJ {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao {get; set;}

        public bool? flagHabilitado { get; set; }

        public bool? flagAbaContato { get; set; }

        public bool? flagAbaTitulos { get; set; }

        public bool? flagAbaPedidos { get; set; }

        public bool? flagAbaContribuicoes { get; set; }

        public bool? flagAbaEventos { get; set; }

        public bool? flagAbaAnuncios { get; set; }

        public bool? flagAbaAreasAtuacao { get; set; }

        public bool? flagAbaInstituicoes { get; set; }

        public bool? flagAbaRepresentantes { get; set; }

        public bool? flagAbaListaEmails { get; set; }

        public bool? flagAbaDependentes { get; set; }

        public bool? flagAbaAbrangencia { get; set; }

        public bool? flagAbaCarteirinha { get; set; }

        public bool? flagAbaVeiculos { get; set; }

        public bool? flagPermitirCNPJVazio { get; set; }

        public bool? flagPermitirCNPJDuplicado { get; set; }

        public bool? flagRotinaInadimplencia { get; set; }

        public bool? flagTodosPagamentosAdimplencia { get; set; }

        public int? qtdeUltimosPagamentosAdimplencia { get; set; }
		
		public bool? flagDadosOutrasOrganizacoes { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class ConfiguracaoAssociadoPJMapper : EntityTypeConfiguration<ConfiguracaoAssociadoPJ> {

		public ConfiguracaoAssociadoPJMapper() {

			this.ToTable("systb_configuracao_associado_pj");

            this.HasKey(o => o.id);

		    this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}