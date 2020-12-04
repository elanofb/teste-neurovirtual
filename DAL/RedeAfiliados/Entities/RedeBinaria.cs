using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Pessoas;

namespace DAL.RedeAfiliados {

	//
	public class RedeBinaria {

        public int id { get; set; }
		
		public int nivel { get; set; }
		
		public int idMembro { get; set; }
		
		public Associado Membro { get; set; }

		public int nroMembro { get; set; }

		public int idPessoaMembro { get; set; }
		
		public Pessoa PessoaMembro { get; set; }

		public int? idMembroEsquerda { get; set; }
		
		public Associado MembroEsquerda { get; set; }
		
		public int? nroMembroEsquerda { get; set; }
		
		public int? idPessoaEsquerda { get; set; }
		
		public Pessoa PessoaEsquerda { get; set; }

		public DateTime? dtCadastroEsquerda { get; set; }

		public bool? flagPlanoEsquerda { get; set; }
		
		public int? idMembroDireita { get; set; }
		
		public Associado MembroDireita { get; set; }
		
		public int? nroMembroDireita { get; set; }

		public int? idPessoaDireita { get; set; }
		
		public Pessoa PessoaDireita { get; set; }
		
		public DateTime? dtCadastroDireita { get; set; }

		public bool? flagPlanoDireita { get; set; }

		public bool flagImportado { get; set; }
	}

	//
	internal sealed class RedeBinariaMapper : EntityTypeConfiguration<RedeBinaria> {

		public RedeBinariaMapper() {

			this.ToTable("tb_rede_binaria");

			this.HasKey(o => o.id);

			this.HasRequired(x => x.Membro).WithMany().HasForeignKey(x => x.idMembro);

			this.HasRequired(x => x.PessoaMembro).WithMany().HasForeignKey(x => x.idPessoaMembro);

			this.HasOptional(x => x.MembroEsquerda).WithMany().HasForeignKey(x => x.idMembroEsquerda);

			this.HasOptional(x => x.PessoaEsquerda).WithMany().HasForeignKey(x => x.idPessoaEsquerda);

			this.HasOptional(x => x.MembroDireita).WithMany().HasForeignKey(x => x.idMembroDireita);

			this.HasOptional(x => x.PessoaDireita).WithMany().HasForeignKey(x => x.idPessoaDireita);

		}
	}
}

