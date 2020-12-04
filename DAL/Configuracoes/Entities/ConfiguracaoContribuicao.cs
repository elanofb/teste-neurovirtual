using System;
using System.Data.Entity.ModelConfiguration;
using DAL.ContasBancarias;
using DAL.Financeiro;
using DAL.Organizacoes;

namespace DAL.Configuracoes {

    //
    public class ConfiguracaoContribuicao {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

        public int? idContaBancariaPadrao { get; set; }

        public ContaBancaria ContaBancariaPadrao { get; set; }

        public int? idCentroCustoPadrao { get; set; }

        public CentroCusto CentroCustoPadrao { get; set; }

        public int? idMacroContaPadrao { get; set; }

        public MacroConta MacroContaPadrao { get; set; }

        public byte? limiteParcelamento { get; set; }

        public byte? idadeIsencao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public bool? flagExcluido { get; set; }

    }

    //
    internal sealed class ConfiguracaoContribuicaoMapper : EntityTypeConfiguration<ConfiguracaoContribuicao> {

        public ConfiguracaoContribuicaoMapper() {
            this.ToTable("systb_configuracao_contribuicao");
            this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasOptional(x => x.ContaBancariaPadrao).WithMany().HasForeignKey(x => x.idContaBancariaPadrao);

            this.HasOptional(x => x.CentroCustoPadrao).WithMany().HasForeignKey(x => x.idCentroCustoPadrao);

            this.HasOptional(x => x.MacroContaPadrao).WithMany().HasForeignKey(x => x.idMacroContaPadrao);
        }

    }
}