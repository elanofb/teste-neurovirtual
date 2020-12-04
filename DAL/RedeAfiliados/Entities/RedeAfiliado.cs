using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Pessoas;

namespace DAL.RedeAfiliados {

	//
	public class RedeAfiliado {

        public int id { get; set; }
		
		public int idMembro { get; set; }
		
		public Associado Membro { get; set; }
		
		public int idPessoaMembro { get; set; }
		
		public Pessoa PessoaMembro { get; set; }

		public int? idMembroAcima { get; set; }
		
		public Associado MembroAcima { get; set; }
		
		public int? idPessoaAcima { get; set; }
		
		public Pessoa PessoaAcima { get; set; }

		public int? idMembroIndicacao { get; set; }
		
		public Associado MembroIndicacao { get; set; }

		public int? idPessoaIndicacao { get; set; }
		
		public Pessoa PessoaIndicacao { get; set; }
		
		public int? nivel { get; set; }
		
		public bool flagEsquerda { get; set; }
		
		public bool flagDireita { get; set; }
		
		public DateTime dtCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }
		
	}

	//
	internal sealed class RedeAfiliadoMapper : EntityTypeConfiguration<RedeAfiliado> {

		public RedeAfiliadoMapper() {

			this.ToTable("tb_rede_afiliado");

			this.HasKey(o => o.id);

			this.HasRequired(x => x.Membro).WithMany().HasForeignKey(x => x.idMembro);

			this.HasRequired(x => x.PessoaMembro).WithMany().HasForeignKey(x => x.idPessoaMembro);

			this.HasOptional(x => x.MembroAcima).WithMany().HasForeignKey(x => x.idMembroAcima);

			this.HasOptional(x => x.PessoaAcima).WithMany().HasForeignKey(x => x.idPessoaAcima);

			this.HasOptional(x => x.MembroIndicacao).WithMany().HasForeignKey(x => x.idMembroIndicacao);

			this.HasOptional(x => x.PessoaIndicacao).WithMany().HasForeignKey(x => x.idPessoaIndicacao);

		}
	}
}

