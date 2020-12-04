using System;
using System.Data.Entity.ModelConfiguration;
using DAL.ContasBancarias;
using DAL.Organizacoes;

namespace DAL.Configuracoes
{

    //
    public class ConfiguracaoFinanceiro{

        public int id { get; set; }
        
        public int? idOrganizacao { get; set; }

        public bool? flagAprovarContas { get; set; }

        public int? idUsuarioAprovacaoContas { get; set; }

        public Organizacao Organizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public bool? flagExcluido { get; set; }
    }

    //
    internal sealed class ConfiguracaoFinanceiroMapper : EntityTypeConfiguration<ConfiguracaoFinanceiro>{

        public ConfiguracaoFinanceiroMapper(){
            this.ToTable("systb_configuracao_financeiro");
            this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }
    }
}