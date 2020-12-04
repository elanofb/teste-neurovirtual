using System;
using System.Data.Entity.ModelConfiguration;

using DAL.Permissao;

namespace DAL.Configuracoes {

    public class ConfiguracaoPromocao {
        public int id { get; set; }

        public decimal?  valorPremioNovoMembro    { get; set; }
        public DateTime? dtInicioPremioNovoMembro { get; set; }
        public DateTime? dtFimPremioNovoMembro    { get; set; }

        public string descricao { get; set; }
        public bool   ativo     { get; set; }

        public int  idUsuarioCadastro  { get; set; }
        public int? idUsuarioAlteracao { get; set; }
        public int? idUsuarioExclusao  { get; set; }

        public DateTime? dtCadastro  { get; set; }
        public DateTime? dtAlteracao { get; set; }
        public DateTime? dtExclusao  { get; set; }

        public UsuarioSistema UsuarioCadastro  { get; set; }
        public UsuarioSistema UsuarioAlteracao { get; set; }
        public UsuarioSistema UsuarioExclusao  { get; set; }
    }

    internal sealed class ConfiguracaoPromocaoMapper : EntityTypeConfiguration<ConfiguracaoPromocao> {
        public ConfiguracaoPromocaoMapper() {
            this.ToTable("systb_configuracao_promocao");

            this.HasKey(x => x.id);

            this.Property(p => p.valorPremioNovoMembro).HasPrecision(10, 4);

            this.HasRequired(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);
            this.HasOptional(x => x.UsuarioAlteracao).WithMany().HasForeignKey(x => x.idUsuarioAlteracao);
            this.HasOptional(x => x.UsuarioExclusao).WithMany().HasForeignKey(x => x.idUsuarioExclusao);
        }
    }

}