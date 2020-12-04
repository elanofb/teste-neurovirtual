using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.Documentos;
using DAL.Empresas;
using DAL.Escolaridades;
using DAL.Localizacao;
using DAL.OrgaosClasses;
using DAL.RamosAtividade;

namespace DAL.Pessoas {

	//
    [Serializable]
    public class PessoaDTO {

        public int id { get; set; }

        public string flagTipoPessoa { get; set; }

        public int? idTipoDocumento { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }

        public string nroDocumento { get; set; } //CPF, CNPJ, PASSAPORTE, etc

        public string passaporte { get; set; }

        public string nome { get; set; } //Quando for pessoa jurídica, usar para o Nome Fantasia da empresa

        public string razaoSocial { get; set; } //Pessoa Jurídica

        public string rg { get; set; } //Pessoa Física

        public string orgaoEmissorRg { get; set; } //Pessoa Física

        public int? idEstadoEmissaoRg { get; set; }

        public string nroCNH { get; set; } //Pessoa Física

        public string categoriaCNH { get; set; } //Pessoa Física

        public string inscricaoEstadual { get; set; } //Pessoa Jurídica

        public string inscricaoMunicipal { get; set; } //Pessoa Jurídica

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

        public string nroRegistroOrgaoClasse { get; set; }

        public int? idSetorAtuacao { get; set; }

        public virtual SetorAtuacao SetorAtuacao { get; set; }

        public string idPaisOrigem { get; set; }

        public virtual Pais PaisOrigem { get; set; }

        public int? idCidadeOrigem { get; set; }

        public virtual Cidade CidadeOrigem { get; set; }

        public string nomeCidadeOrigem { get; set; }

        public DateTime? dtNascimento { get; set; }

        public string flagSexo { get; set; }

        public string nomePai { get; set; }

        public string nomeMae { get; set; }

        public string ddiTelPrincipal { get; set; }

        public string dddTelPrincipal { get; set; }

        public string nroTelPrincipal { get; set; }

        public string ddiTelSecundario { get; set; }

        public string dddTelSecundario { get; set; }

        public string nroTelSecundario { get; set; }

        public string ddiTelTerciario { get; set; }

        public string dddTelTerciario { get; set; }

        public string nroTelTerciario { get; set; }

        public string emailPrincipal { get; set; }

        public string emailSecundario { get; set; }

        public string enderecoWeb { get; set; }

        public int? idTipoEnderecoCorrespondencia { get; set; }

        public int? idNivelEscolar { get; set; }

        public virtual NivelEscolar NivelEscolar { get; set; }

        public string instituicaoFormacao { get; set; }

        public int? anoFormacao { get; set; }

        public string nroMatriculaEstudante { get; set; }

        public string profissao { get; set; }

        public string login { get; set; }

        public string senha { get; set; }

        public string observacoes { get; set; }

        public string nomeResponsavelCadastro { get; set; }

        public string documentoResponsavelCadastro { get; set; }

        public string obsResponsavelCadastro { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public virtual IList<PessoaEndereco> listaEnderecos { get; set; }

        public virtual IEnumerable<PessoaEmailDTO> listaEmails { get; set; }

        public virtual IList<PessoaTelefone> listaTelefones { get; set; }

        //public virtual IList<PessoaDocumento> listaPessoaDocumento { get; set; }

        public virtual IList<PessoaContato> listaPessoaContato { get; set; }

        public virtual IList<PessoaRelacionamento> listaPessoaRelacionamento { get; set; }

        public PessoaDTO() {

            this.listaEnderecos = new List<PessoaEndereco>();

            this.listaEmails = new List<PessoaEmailDTO>();

            this.listaTelefones = new List<PessoaTelefone>();

            //this.listaPessoaDocumento = new List<PessoaDocumento>();

            this.listaPessoaContato = new List<PessoaContato>();

            this.listaPessoaRelacionamento = new List<PessoaRelacionamento>();
        }

    }
}