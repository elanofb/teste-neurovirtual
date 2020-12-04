using System;
using System.Data;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Migracoes {

	public class UsuarioMembro {

	    public int id { get; set; }
		
		public int flagAdmin { get; set; }
		
		public string nome { get; set; }
		
		public string email { get; set; }
		
		public string cpf { get; set; }
		
		public string celular { get; set; }
		
		public string login { get; set; }
		
		public string senha { get; set; }
		
		public string senhadecompra { get; set; }
		
		public string pais { get; set; }
		
		public string saldoRendimentos { get; set; }
		
		public string saldoIndicacoes { get; set; }
		
		public decimal? saldoBinario { get; set; }
		
		public int? planoCarreira { get; set; }
		
		public int? binario { get; set; }
						
		public int? qtdeBinario { get; set; }
		
		public int? chaveBinaria { get; set; }
		
		public int? block { get; set; }
		
		public DateTime? dtCadastro { get; set; }				
		
		public string sim { get; set; }
		
		public DateTime? dtImportacao { get; set; }

		public int? idMembro { get; set; }
		
		public decimal? valorSaldoImportado { get; set; }
		
		public DateTime? dtImportacaoSaldo { get; set; }
				
	   

	}

	internal sealed class UsuarioMembroMapper : EntityTypeConfiguration<UsuarioMembro> {

		public UsuarioMembroMapper() {

			this.ToTable("vw_usuarios");
			
			this.HasKey(x => x.id);
			
			this.Property(x => x.flagAdmin).HasColumnName("is_admin");
			
			this.Property(x => x.saldoRendimentos).HasColumnName("saldo_rendimentos");
			
			this.Property(x => x.saldoIndicacoes).HasColumnName("saldo_indicacoes");
			
			this.Property(x => x.saldoBinario).HasColumnName("saldo_binario");
			
			this.Property(x => x.planoCarreira).HasColumnName("plano_carreira");
			
			this.Property(x => x.qtdeBinario).HasColumnName("quantidade_binario");
			
			this.Property(x => x.chaveBinaria).HasColumnName("chave_binaria");
			
			this.Property(x => x.dtCadastro).HasColumnName("data_cadastro");
			
			this.Ignore(x => x.dtImportacao);
			
			this.Ignore(x => x.idMembro);
			
			this.Ignore(x => x.valorSaldoImportado);
			
			this.Ignore(x => x.dtImportacaoSaldo);

		}
	}
}