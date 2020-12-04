using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using DAL.Documentos;
using DAL.Empresas;
using DAL.Escolaridades;
using DAL.Localizacao;
using DAL.OrgaosClasses;
using DAL.RamosAtividade;
using DAL.SegmentosAtuacao;
using DAL.Unidades;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class Pessoa {

		public int id { get; set; }
		
		public int? idOrganizacao { get; set; }

		public int? idUnidade { get; set; }
		
		public virtual Unidade Unidade { get; set; }
		
		[MaxLength(1)]
		public string flagTipoPessoa { get; set; }
		
		public bool? flagEstrangeiro { get; set; }

		public int? idTipoDocumento { get; set; }

		public virtual TipoDocumento TipoDocumento { get; set; }

		[MaxLength(30)]
		public string nroDocumento { get; set; } //CPF, CNPJ, PASSAPORTE, etc

		[MaxLength(20)]
        public string passaporte { get; set; }

		[MaxLength(20)]
		public string rne { get; set; }
		
		[MaxLength(100)]
		public string nome { get; set; } //Quando for pessoa jurídica, usar para o Nome Fantasia da empresa

		[MaxLength(100)]
		public string razaoSocial { get; set; } //Pessoa Jurídica

		[MaxLength(50)]
		public string rg { get; set; } //Pessoa Física

		[MaxLength(10)]
		public string orgaoEmissorRg { get; set; } //Pessoa Física

        public int? idEstadoEmissaoRg { get; set; }
		
		[MaxLength(20)]
        public string nroCTPS { get; set; }
		
		[MaxLength(5)]
        public string serieCTPS { get; set; }
		
        public DateTime? dtEmissaoCTPS { get; set; }
		
		[MaxLength(20)]
        public string nroPIS { get; set; }
		
		[MaxLength(20)]
        public string nroTituloEleitor { get; set; }
		
		[MaxLength(5)]
        public string zonaEleitoral { get; set; }
		
		[MaxLength(5)]
        public string sessaoEleitoral { get; set; }
		
		[MaxLength(15)]
        public string nroReservista { get; set; }
		
		[MaxLength(5)]
        public string serieReservista { get; set; }
		
		[MaxLength(20)]
        public string nroCNH { get; set; } //Pessoa Física

		[MaxLength(1)]
        public string categoriaCNH { get; set; } //Pessoa Física
		
        public DateTime? dtValidadeCNH { get; set; } //Pessoa Física

		[MaxLength(20)]
		public string inscricaoEstadual { get; set; } //Pessoa Jurídica

		[MaxLength(20)]
		public string inscricaoMunicipal { get; set; } //Pessoa Jurídica

		[MaxLength(10)]
		public string cnaeAtividade { get; set; } //Pessoa Jurídica

        public bool? flagOptanteSimplesNacional { get; set; }

        public bool? flagFinsLucrativos { get; set; }

        public int? qtdeEmpregadosCLT { get; set; }

        public int? qtdeEmpregadosTerceiros { get; set; }

        public int? qtdeEstagiarios { get; set; }

        public int? qtdeMenorAprendiz { get; set; }

        public int? idEmpresaPorte { get; set; }

        public virtual EmpresaPorte EmpresaPorte { get; set; }

		public int? idOrgaoClasse { get; set; }

		public virtual OrgaoClasse OrgaoClasse { get; set; }

		[MaxLength(20)]
        public string nroRegistroOrgaoClasse { get; set; }

        public int? idEstadoOrgaoClasse { get; set; }

        public Estado EstadoOrgaoClasse { get; set; }

		public int? idSetorAtuacao { get; set; }

		public SetorAtuacao SetorAtuacao { get; set; }

		public int? idSegmento { get; set; }

		public SegmentoAtuacao Segmento { get; set; }

		[MaxLength(3)]
		public string idPaisOrigem { get; set; }

		public virtual Pais PaisOrigem { get; set; }

		public int? idCidadeOrigem { get; set; }

		public virtual Cidade CidadeOrigem { get; set; }

		public string nomeCidadeOrigem { get; set; }

		public DateTime? dtNascimento { get; set; }

		[MaxLength(1)]
		public string flagSexo { get; set; }

		[MaxLength(100)]
		public string nomePai { get; set; }

		[MaxLength(100)]
		public string nomeMae { get; set; }

		[MaxLength(3)]
		public string ddiTelPrincipal { get; set; }

		[MaxLength(3)]
		public string dddTelPrincipal { get; set; }

		[MaxLength(20)]
		public string nroTelPrincipal { get; set; }

		[MaxLength(3)]
		public string ddiTelSecundario { get; set; }

		[MaxLength(3)]
		public string dddTelSecundario { get; set; }

		[MaxLength(20)]
		public string nroTelSecundario { get; set; }

		[MaxLength(3)]
		public string ddiTelTerciario { get; set; }

		[MaxLength(3)]
		public string dddTelTerciario { get; set; }

		[MaxLength(20)]
		public string nroTelTerciario { get; set; }

		[MaxLength(150)]
		public string emailPrincipal { get; set; }

		[MaxLength(150)]
		public string emailSecundario { get; set; }

		[MaxLength(150)]
        public string enderecoWeb { get; set; }

		public int? idTipoEnderecoCorrespondencia { get; set; }

		public int? idNivelEscolar { get; set; }

		public virtual NivelEscolar NivelEscolar { get; set; }

        public string instituicaoFormacao { get; set; }

        public int? anoFormacao { get; set; }

        public DateTime? dtFormacao { get; set; }

        public string nroMatriculaEstudante { get; set; }

        public string instituicaoEstudante { get; set; }

        public string profissao { get; set; }

        public string localTrabalho { get; set; }

		[MaxLength(50)]
		public string login { get; set; }

		[MaxLength(128)]
		public string senha { get; set; }

		[MaxLength(1000)]
        public string observacoes { get; set; }

		[MaxLength(100)]
        public string nomeResponsavelCadastro { get; set; }

		[MaxLength(30)]
        public string documentoResponsavelCadastro { get; set; }

		[MaxLength(500)]
        public string obsResponsavelCadastro { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public List<PessoaEndereco> listaEnderecos { get; set; }

        public List<PessoaEmail> listaEmails { get; set; }

        public List<PessoaTelefone> listaTelefones { get; set; }

	    public string senhaConfirmacao { get; set; }

        public Pessoa() {

            this.listaEnderecos = new List<PessoaEndereco>();

            this.listaEmails = new List<PessoaEmail>();

            this.listaTelefones = new List<PessoaTelefone>();

		}

		//Calcular a idade
		public byte calcularIdade() {
			byte idade = 0;

			if (this.dtNascimento == null) {
				return idade;
			}

			idade = (byte)(DateTime.Now.Year - this.dtNascimento.Value.Year);

			if ((DateTime.Now.Month < this.dtNascimento.Value.Month) || (DateTime.Now.Month == this.dtNascimento.Value.Month && DateTime.Now.Day < this.dtNascimento.Value.Day)) {
				idade--;
			}
			return idade;
		}

        /// <summary>
        /// Retornar a lista de endereços removendo os itens excluidos
        /// </summary>
	    public List<PessoaEndereco> retornarListaEnderecos(){

	        return this.listaEnderecos.Where(x => x.dtExclusao == null).ToList();
	    }

        /// <summary>
        /// Retornar a lista de emails removendo os itens excluidos
        /// </summary>
	    public List<PessoaEmail> retornarListaEmails(){

	        return this.listaEmails.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.email)).ToList();
	    }

        /// <summary>
        /// Retornar a lista de endereços removendo os itens excluidos
        /// </summary>
	    public List<PessoaTelefone> retornarListaTelefones(){

	        return this.listaTelefones.Where(x => x.dtExclusao == null  && !string.IsNullOrEmpty(x.nroTelefone)).ToList();
	    }

	}

	//
	internal sealed class PessoaMapper : EntityTypeConfiguration<Pessoa> {

		public PessoaMapper() {

			this.ToTable("tb_pessoa");

			this.HasKey(o => o.id);

		    this.Ignore(x => x.senhaConfirmacao);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.HasRequired(o => o.PaisOrigem).WithMany().HasForeignKey(o => o.idPaisOrigem);
			
			this.HasOptional(o => o.Unidade).WithMany().HasForeignKey(o => o.idUnidade);

			this.HasOptional(o => o.TipoDocumento).WithMany().HasForeignKey(o => o.idTipoDocumento);

			this.HasOptional(o => o.EmpresaPorte).WithMany().HasForeignKey(o => o.idEmpresaPorte);

            this.HasOptional(o => o.EstadoOrgaoClasse).WithMany().HasForeignKey(o => o.idEstadoOrgaoClasse);

            this.HasOptional(o => o.CidadeOrigem).WithMany().HasForeignKey(o => o.idCidadeOrigem);

            this.HasOptional(o => o.NivelEscolar).WithMany().HasForeignKey(o => o.idNivelEscolar);

            this.HasOptional(o => o.OrgaoClasse).WithMany().HasForeignKey(o => o.idOrgaoClasse);

			this.HasOptional(o => o.SetorAtuacao).WithMany().HasForeignKey(o => o.idSetorAtuacao);

			this.HasOptional(o => o.Segmento).WithMany().HasForeignKey(o => o.idSegmento);
		}
	}
}