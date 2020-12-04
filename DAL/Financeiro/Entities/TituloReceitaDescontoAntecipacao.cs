using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro.Entities {

    public class TituloReceitaDescontoAntecipacao {

        public int id { get; set; }
        public int idTituloReceita { get; set; }

        public virtual TituloReceita TituloReceita { get; set; }
        public DateTime? dtLimiteDesconto { get; set; }
        public decimal valor { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtExclusao { get; set; }
        public int? idUsuarioExclusao { get; set; }
        public string motivoExclusao { get; set; }
    }

	//
	internal sealed class TituloReceitaDescontoAntecipacaoMapper : EntityTypeConfiguration<TituloReceitaDescontoAntecipacao> {

		public TituloReceitaDescontoAntecipacaoMapper() {
			
            this.ToTable("tb_titulo_receita_desconto_antecipacao");

			this.HasKey(o => o.id);

			this.HasRequired(o => o.TituloReceita).WithMany(x => x.listaDescontosAntecipacao).HasForeignKey(o => o.idTituloReceita);

		}
	}
}
