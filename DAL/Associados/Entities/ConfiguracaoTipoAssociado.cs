using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Associados {

	//
	public class ConfiguracaoTipoAssociado
    {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

        public int? idTipoAssociado { get; set; }

        public string htmlCarteirinha { get; set; }

        public int? qtdeMesesValidade { get; set; }

        public DateTime? dtValidadeFixa { get; set; }

        public DateTime? dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public bool? ativo { get; set; }
    }

    //
    internal sealed class ConfiguracaoTipoAssociadoMapper : EntityTypeConfiguration<ConfiguracaoTipoAssociado> {

		public ConfiguracaoTipoAssociadoMapper() {

			this.ToTable("tb_tipo_associado_configuracao");
            this.HasKey(o => o.id);

            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }
    }
}