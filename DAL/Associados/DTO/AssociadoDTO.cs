using System;
using System.Collections.Generic;
using DAL.Arquivos;
using DAL.Contribuicoes;
using DAL.Localizacao;
using DAL.Mailings;
using DAL.MeiosDivulgacao;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Pessoas;
using DAL.Tipos;
using DAL.Unidades;

namespace DAL.Associados {

	public class AssociadoDTO {

		public int id { get; set; }

        public int? idOrigem { get; set; }

        public int idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

        public int? idUnidade { get; set; }

        public Unidade Unidade { get; set; }

		public int idPessoa { get; set; }

		public PessoaDTO Pessoa { get; set; }

		public int idTipoAssociado { get; set; }

		public TipoAssociado TipoAssociado { get; set; }

		public byte idTipoCadastro { get; set; }

        public int? nroAssociado { get; set; }
		
		public string titulacao { get; set; }

		public string nroPrimeiraCOCEP { get; set; }

		public string nroSegundaCOCEP { get; set; }

		public int? idEstadoPrimeiraCOCEP { get; set; }

		public Estado EstadoPrimeiraCOCEP { get; set; }

		public int? idEstadoSegundaCOCEP { get; set; }

		public Estado EstadoSegundaCOCEP { get; set; }

		public string nroMatriculaEstudante { get; set; }

		public string nomeUniversidadeFormacao { get; set; }

		public int? anoFormacao { get; set; }

		public string nomeInstituicao { get; set; }	

		public string flagInformativosOnline { get; set; }

		public DateTime? dtAdmissao { get; set; }

		public int? idUsuarioAdmissao { get; set; }
        
        public UsuarioSistema UsuarioAdmissao { get; set; }

		public string observacoes { get; set; }

		public int? idEmpresaEstipulante { get; set; }

		public int? idAssociadoEstipulante { get; set; }

        public virtual Associado AssociadoEstipulante { get; set; }

		public int? idUltimaContribuicao { get; set; }

        public int? idContribuicaoPadrao { get; set; }

        public Contribuicao ContribuicaoPadrao  { get; set; }

        public DateTime? dtUltimoPagamentoContribuicao { get; set; }

        public DateTime? dtProximoVencimento { get; set; }

		public string dadoCustomizado01 { get; set;  }

		public string dadoCustomizado02 { get; set;  }

		public string dadoCustomizado03 { get; set;  }

        public DateTime? dtImportacao { get; set; }

        public int? idMeioDivulgacao { get; set;  }

        public MeioDivulgacao MeioDivulgacao { get; set;  }

        public int? idTipoDependente { get; set; }

        public virtual TipoDependente TipoDependente { get; set; }

        public int? idRepresentante { get; set; } 

        public UsuarioSistema Representante { get; set; }

		public string ativo { get; set; }

		public DateTime dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }
        
        public UsuarioSistema UsuarioCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public DateTime? dtDesativacao { get; set; }

		public int? idUsuarioDesativacao { get; set; }

        public int? idMotivoDesativacao { get; set; }
               
        public MotivoDesativacao MotivoDesativacao { get; set; }

        public string observacaoDesativacao { get; set; }

		public DateTime? dtReativacao { get; set; }

		public int? idUsuarioReativacao { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public int? idMotivoDesligamento { get; set; }

        public virtual MotivoDesligamento MotivoDesligamento { get; set; }

		public string observacaoDesligamento { get; set; }

		public List<AssociadoAreaAtuacao> listaAreaAtuacao { get; set; }

		public List<AssociadoTitulo> listaTitulo { get; set; }

		public List<AssociadoRepresentante> listaRepresentante { get; set; }

		public List<AssociadoAbrangencia> listaAbrangencia { get; set; }

        public List<Mailing> listaMailing { get; set; }

        //Ignorados
		public ArquivoUpload Foto { get; set;  }

        public bool? flagGerarCobrancaAposCadastro { get; set;  }

        //Construtor
		public AssociadoDTO() {

			this.listaAreaAtuacao = new List<AssociadoAreaAtuacao>();

            this.listaTitulo = new List<AssociadoTitulo>();

            this.listaRepresentante = new List<AssociadoRepresentante>();

            this.listaAbrangencia = new List<AssociadoAbrangencia>();

            this.listaMailing = new List<Mailing>();
		}
	}
}