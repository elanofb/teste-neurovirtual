using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using DAL.Enderecos;
using DAL.Localizacao;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class PessoaEndereco  {

		public int id { get; set; }
		
		public int? idOrganizacao { get; set; }

		[MaxLength(10)]
		public string cep { get; set; }

		[MaxLength(100)]
		public string logradouro { get; set; }

		[MaxLength(50)]
		public string complemento { get; set; }

		[MaxLength(20)]
		public string numero { get; set; }

		[MaxLength(80)]
		public string bairro { get; set; }

        public bool? flagEnviarCorrespondencia { get; set; }

		[MaxLength(2)]
        public string zona { get; set; }

		public int? idCidade { get; set; }

		public virtual Cidade Cidade { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		[MaxLength(3)]
		public string idPais { get; set; }

		public virtual Pais Pais { get; set; }

		[MaxLength(80)]
		public string nomeCidade { get; set; }

		public int? idEstado { get; set; }

		public virtual Estado Estado { get; set; }

		[MaxLength(2)]
		public string uf { get; set; }

		public byte? idTipoEndereco { get; set; }

		public virtual TipoEndereco TipoEndereco { get; set; }

        public bool? flagEntrega { get; set; }

        public string observacoes { get; set; }

        public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }
	}

	//
	internal sealed class PessoaEnderecoMapper : EntityTypeConfiguration<PessoaEndereco> {

		public PessoaEnderecoMapper() {
			this.ToTable("tb_pessoa_endereco");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Pais).WithMany().HasForeignKey(o => o.idPais);
			this.HasRequired(o => o.Pessoa).WithMany(p => p.listaEnderecos).HasForeignKey(o => o.idPessoa);
			this.HasOptional(o => o.TipoEndereco).WithMany().HasForeignKey(o => o.idTipoEndereco);
			this.HasOptional(o => o.Estado).WithMany().HasForeignKey(o => o.idEstado);
			this.HasOptional(o => o.Cidade).WithMany().HasForeignKey(o => o.idCidade);
		}
	}
}