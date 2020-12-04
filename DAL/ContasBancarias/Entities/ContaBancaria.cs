using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Bancos;
using DAL.Localizacao;
using DAL.Organizacoes;

namespace DAL.ContasBancarias {

	public class ContaBancaria {
        
		public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public int? idUnidade { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

        public string nroAgencia { get; set; }

        public string digitoAgencia { get; set; }

        public string nroConta { get; set; }

        public string digitoConta { get; set; }

        public string documentoTitular { get; set; }

        public string operacaoConta { get; set; }

	    public decimal? saldoInicial { get; set; }

        public string nomeTitular { get; set; }

        public string tipoConta { get; set; }

		public int? idBanco { get; set; }

        public virtual Banco OBanco { get; set; }

		public string cep { get; set; }

		public string logradouro { get; set; }

		public string complemento { get; set; }

		public string numero { get; set; }

		public string bairro { get; set; }

		public int? idCidade { get; set; }
		
		public virtual Cidade Cidade { get; set; }
				
		public int? idEstado { get; set; }
		
		public virtual Estado Estado { get; set; }

		public byte? qtdeDigitosNossoNumero { get; set; }

		public string codigoCarteira { get; set; }
		
		public string codigoCedente { get; set; }
		
		public string codigoConvenio { get; set; }
		
		public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }

		public bool? flagExcluido { get; set; }

		public bool? flagPrincipal { get; set; }

        public decimal? valorSaldoContaBancaria { get; set; }

        public DateTime? dtUltimaMovimentacao { get; set; }

        public List<ContaBancariaMovimentacao> listaMovimentacoesOrigem { get; set; }

        public List<ContaBancariaMovimentacao> listaMovimentacoesDestino { get; set; }

		public ContaBancaria() {

            this.listaMovimentacoesOrigem = new List<ContaBancariaMovimentacao>();

            this.listaMovimentacoesDestino = new List<ContaBancariaMovimentacao>();
		}
	}

	internal sealed class ContaBancariaMapper : EntityTypeConfiguration<ContaBancaria> {

		public ContaBancariaMapper() {

			this.ToTable("tb_conta_bancaria");
			this.HasKey(x => x.id);
            //FK
			
            //Ignorar
            this.Ignore(x => x.valorSaldoContaBancaria);
			
            this.Ignore(x => x.dtUltimaMovimentacao);
			
		    this.HasRequired(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

			this.HasOptional(x => x.OBanco).WithMany().HasForeignKey(x => x.idBanco);

            this.HasOptional(x => x.Cidade).WithMany().HasForeignKey(x => x.idCidade);
			
			this.HasOptional(x => x.Estado).WithMany().HasForeignKey(x => x.idEstado);
		}
	}
}