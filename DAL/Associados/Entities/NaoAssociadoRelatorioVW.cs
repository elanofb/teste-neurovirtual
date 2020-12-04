﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Arquivos;
using DAL.Pessoas;

namespace DAL.Associados {

	//
	[Serializable]
	public class NaoAssociadoRelatorioVW {

		public int id { get; set; }
		
        public int? idOrigem { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

		public int idPessoa { get; set; }

		public int? idTipoAssociado { get; set; }

		public byte idTipoCadastro { get; set; }

        public int? nroAssociado { get; set; }		

		public string flagInformativosOnline { get; set; }

		public DateTime? dtAdmissao { get; set; }

		public int? idUsuarioAdmissao { get; set; }       

		public DateTime? dtDesativacao { get; set; }

		public int? idUsuarioDesativacao { get; set; }

		public DateTime? dtReativacao { get; set; }

		public int? idUsuarioReativacao { get; set; }

		public string observacoes { get; set; }

		public int? idEmpresaEstipulante { get; set; }

		public int? idAssociadoEstipulante { get; set; }

		public int? idUltimaContribuicao { get; set; }

        public int? idContribuicaoPadrao { get; set; }

        public DateTime? dtUltimoPagamentoContribuicao { get; set; }

        public DateTime? dtProximoVencimento { get; set; }

        public DateTime? dtImportacao { get; set; }

        public string ativo { get; set; }
		
        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public string nomeUsuarioCadastro { get; set; }

        public DateTime? dtDesligamento { get; set; }

        public int? idUsuarioDesligamento { get; set; }

        public string observacaoDesligamento { get; set; }

        public int? idMotivoDesligamento { get; set; }


        public string flagTipoPessoa { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public int? idTipoDocumento { get; set; }

        public string nroDocumento { get; set; }

        public string rg { get; set; }

        public string inscricaoEstadual { get; set; }

        public string inscricaoMunicipal { get; set; }

        public DateTime? dtNascimento { get; set; }

        public string flagSexo { get; set; }

        public string nomePai { get; set; }

        public string nomeMae { get; set; }

        public string login { get; set; }
        public string descricaoTipoAssociado { get; set; }

        public string siglaEstadoPrimeiroCOCEP { get; set; }

        public bool flagEstudante { get; set; }

        public string emails { get; set; }

        public string telefones { get; set; }

        //Ignorados
        public ArquivoUpload Foto { get; set;  }

        public bool? flagGerarCobrancaAposCadastro { get; set; }

        public List<PessoaEndereco> listaEnderecos { get; set; } 

        //Construtor
		public NaoAssociadoRelatorioVW() {
            this.listaEnderecos = new List<PessoaEndereco>();
		}
	}

	//
	internal sealed class NaoAssociadoRelatorioVWMapper : EntityTypeConfiguration<NaoAssociadoRelatorioVW> {

		public NaoAssociadoRelatorioVWMapper() {

			this.ToTable("vw_nao_associado_relatorio");

			this.HasKey(o => o.id);

			this.Ignore(x => x.Foto);

            this.Ignore(x => x.flagGerarCobrancaAposCadastro);

		    this.Ignore(x => x.listaEnderecos);
		}
	}
}