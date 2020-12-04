using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Pessoas;

namespace DAL.Organizacoes {

	//
	public class Organizacao  {

        public int id { get; set; }

        public int? idOrganizacaoGestora { get; set; }

        public virtual Organizacao OrganizacaoGestora { get; set; }

        public int idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public byte? idStatusOrganizacao { get; set; }

        public StatusOrganizacao StatusOrganizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }

        public virtual IList<Organizacao> listaOrganizacoesVinculadas { get; set; }

        public Organizacao() {
            this.listaOrganizacoesVinculadas = new List<Organizacao>();
		}
	}

	//
	internal sealed class OrganizacaoMapper : EntityTypeConfiguration<Organizacao> {

		public OrganizacaoMapper() {

			this.ToTable("tb_organizacao");

		    this.HasKey(x => x.id);

			//FKs
			this.HasRequired(u => u.Pessoa).WithMany().HasForeignKey(u => u.idPessoa);
			this.HasOptional(u => u.OrganizacaoGestora).WithMany(x => x.listaOrganizacoesVinculadas).HasForeignKey(u => u.idOrganizacaoGestora);
			this.HasOptional(u => u.StatusOrganizacao).WithMany().HasForeignKey(u => u.idStatusOrganizacao);
		}
	}
}