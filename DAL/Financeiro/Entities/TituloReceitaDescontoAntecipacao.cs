﻿using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro.Entities {

    public class TituloReceitaDescontoAntecipacao {




        public virtual TituloReceita TituloReceita { get; set; }






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