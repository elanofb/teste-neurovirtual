using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Documentos;

namespace DAL.Pessoas {

    public static class PessoaExtensions {

        // Capturar a string de e-mails e joga-la dentro de uma lista
        public static List<string> ToEmailList(this Pessoa OPessoa) {

            List<string> lista = new List<string>();

            if (OPessoa == null) {
                return lista;
            }

            var listaEmails = OPessoa.listaEmails.Where(x => x.dtExclusao == null && UtilValidation.isEmail(x.email))
                                     .Select(x => x.email).Distinct().ToList();

            return listaEmails;
        }

        // Capturar a string de e-mails e joga dentro de uma lista
        public static List<string> ToEmailsPessoa(this Pessoa OPessoa) {

            List<string> lista = new List<string>();

            if (OPessoa == null) {
                return lista;
            }

            if (UtilValidation.isEmail(OPessoa.emailPrincipal)) {
                lista.Add(OPessoa.emailPrincipal);
            }

            if (UtilValidation.isEmail(OPessoa.emailSecundario)) {
                lista.Add(OPessoa.emailSecundario);
            }

            return lista;
        }

        public static string formatarDocumento(this Pessoa OPessoa) {
            string documento = OPessoa.nroDocumento;

            if (OPessoa.idTipoDocumento == TipoDocumentoConst.CPF || OPessoa.idTipoDocumento == TipoDocumentoConst.CNPJ) {
                return UtilString.formatCPFCNPJ(documento);
            }
            return documento;
        }

        //Formatar número de telefone principal
        public static string formatarTelPrincipal(this Pessoa OPessoa, bool flagDDI = false) {

            string telFormatado = "";

            var OPessoaTelefone = OPessoa?.listaTelefones?.FirstOrDefault(x => x.dtExclusao == null) ?? new PessoaTelefone();

            if (OPessoaTelefone.id > 0) {
                if (OPessoaTelefone.ddi > 0) {
                    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
                }

                telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone));
            }

            return telFormatado;
        }

        //Formatar número de telefone secundario
        public static string formatarTelSecundario(this Pessoa OPessoa, bool flagDDI = false) {

            string telFormatado = "";

            var listaTelefones = OPessoa?.listaTelefones?.Where(x => x.dtExclusao == null).Take(2).ToList() ?? new List<PessoaTelefone>();
            var OPessoaTelefone = (listaTelefones.Count == 2) ? listaTelefones.LastOrDefault(x => x.dtExclusao == null) : null;

            if (UtilNumber.toInt32(OPessoaTelefone?.id) > 0) {
                if (OPessoaTelefone.ddi > 0) {
                    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
                }

                telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone));
            }

            return telFormatado;
        }

        //Formatar número de telefone secundario
        public static string formatarTelTerciario(this Pessoa OPessoa, bool flagDDI = false) {

            string telFormatado = "";

            var listaTelefones = OPessoa?.listaTelefones?.Where(x => x.dtExclusao == null).Take(3).ToList() ?? new List<PessoaTelefone>();
            var OPessoaTelefone = (listaTelefones.Count == 3) ? listaTelefones.LastOrDefault(x => x.dtExclusao == null) : null;

            if (UtilNumber.toInt32(OPessoaTelefone?.id) > 0) {
                if (OPessoaTelefone.ddi > 0) {
                    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
                }

                telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone));
            }

            return telFormatado;
        }

        //Retorna o primeiro e-mail cadastrado.
        public static string emailPrincipal(this Pessoa OPessoa) {

            string email = "";

            var listaEmail = OPessoa.ToEmailList();

            if (listaEmail.Count > 0) {
                email = listaEmail[0];
            }

            return email;
        }

        //Retorna o segundo e-mail cadastrado.
        public static string emailSecundario(this Pessoa OPessoa) {

            string email = "";

            var listaEmail = OPessoa.ToEmailList();

            if (listaEmail.Count > 1) {
                email = listaEmail[1];
            }

            return email;
        }

        //Retorna o primeiro telefone cadastrado.
        public static string telefonePrincipal(this Pessoa OPessoa) {

            string telefone = "";
            
            if (OPessoa == null){
                
                return telefone;
            }

            var listaTelefones = OPessoa.listaTelefones.Where(x => x.dtExclusao == null).ToList();

            if (listaTelefones.Count > 0) {
                telefone = listaTelefones[0].nroTelefone;
            }

            return telefone;
        }

        //Retorna o segundo telefone cadastrado.
        public static string telefoneSecundario(this Pessoa OPessoa) {

            string telefone = "";

            var listaTelefones = OPessoa.listaTelefones.Where(x => x.dtExclusao == null).ToList();

            if (listaTelefones.Count > 1) {
                telefone = listaTelefones[1].nroTelefone;
            }

            return telefone;
        }

        //Retorna o primeiro endereco cadastrado.
        public static PessoaEndereco enderecoPrincipal(this Pessoa OPessoa) {

            var OPessoaEndereco = new PessoaEndereco();

            var listaEnderecos = OPessoa.retornarListaEnderecos();

            if (listaEnderecos.Count > 0) {
                OPessoaEndereco = listaEnderecos[0];
            }

            return OPessoaEndereco;
        }

        //Retorna o segundo endereco cadastrado.
        public static PessoaEndereco enderecoSecundario(this Pessoa OPessoa) {

            var OPessoaEndereco = new PessoaEndereco();

            var listaEnderecos = OPessoa.retornarListaEnderecos();

            if (listaEnderecos.Count > 1) {
                OPessoaEndereco = listaEnderecos[1];
            }

            return OPessoaEndereco;
        }

        /// <summary>
        /// Retornar a string de pessoa física ou jurídica
        /// </summary>
        public static string retornarTipoPessoa(this Pessoa OPessoa) {

            if (OPessoa == null) {
                return "";
            }

            if (OPessoa.flagTipoPessoa == "J") {
                return "Pessoa Jurídica";
            }

            if (OPessoa.flagTipoPessoa == "F") {
                return "Pessoa Física";
            }

            return "";
        }

        /// <summary>
        /// Retornar a string de pessoa física ou jurídica
        /// </summary>
        public static string retornarSexo(this Pessoa OPessoa) {

            if (OPessoa == null) {
                return "";
            }

            if (OPessoa.flagSexo == "F") {
                return "Feminino";
            }

            if (OPessoa.flagSexo == "M") {
                return "Masculino";
            }

            return "";
        }

        // Limpar atributos e inserir valor nulo para objetos relacionados
        public static Pessoa limparAtributos(this Pessoa OPessoa, bool limparObjetosRelacionados  = true) {
            
            OPessoa.nroDocumento = UtilString.onlyAlphaNumber(OPessoa.nroDocumento);

            OPessoa.inscricaoEstadual = UtilString.onlyAlphaNumber(OPessoa.inscricaoEstadual);

            OPessoa.inscricaoMunicipal = UtilString.onlyAlphaNumber(OPessoa.inscricaoMunicipal);

            OPessoa.rg = UtilString.onlyAlphaNumber(OPessoa.rg);

            OPessoa.nroCNH = UtilString.onlyAlphaNumber(OPessoa.nroCNH);

            OPessoa.nome = OPessoa.nome.toUppercaseWords().abreviar(100);

            OPessoa.razaoSocial = OPessoa.razaoSocial.toUppercaseWords().abreviar(100);

            OPessoa.rg = OPessoa.rg.abreviar(50);

            OPessoa.nroCNH = OPessoa.nroCNH.abreviar(20);

            OPessoa.categoriaCNH = OPessoa.categoriaCNH.abreviar(1);

            OPessoa.cnaeAtividade = OPessoa.cnaeAtividade.abreviar(10);

            OPessoa.inscricaoEstadual = OPessoa.inscricaoEstadual.abreviar(50);

            OPessoa.inscricaoMunicipal = OPessoa.inscricaoMunicipal.abreviar(50);

            OPessoa.nomeCidadeOrigem = OPessoa.nomeCidadeOrigem.abreviar(80);

            OPessoa.nomePai = OPessoa.nomePai.toUppercaseWords().abreviar(100);

            OPessoa.nomeMae = OPessoa.nomeMae.toUppercaseWords().abreviar(100);

            OPessoa.profissao = OPessoa.profissao.toUppercaseWords().abreviar(50);

            OPessoa.localTrabalho = OPessoa.localTrabalho.toUppercaseWords().abreviar(50);

            OPessoa.login = OPessoa.login.abreviar(50).stringOrEmptyLower();

            OPessoa.observacoes = OPessoa.observacoes.abreviar(1000);

            OPessoa.idPaisOrigem = OPessoa.idPaisOrigem.isEmpty() ? "BRA" : OPessoa.idPaisOrigem;

            OPessoa.nomeResponsavelCadastro = OPessoa.nomeResponsavelCadastro.toUppercaseWords().abreviar(100);

            OPessoa.documentoResponsavelCadastro = OPessoa.documentoResponsavelCadastro.onlyNumber();

            OPessoa.obsResponsavelCadastro = OPessoa.obsResponsavelCadastro.abreviar(500);

            if (limparObjetosRelacionados){

                //Anular relacionamentos que nao se deseja inserções
                OPessoa.CidadeOrigem = null;
                OPessoa.PaisOrigem = null;
                OPessoa.TipoDocumento = null;
                OPessoa.EmpresaPorte = null;
                OPessoa.SetorAtuacao = null;
                OPessoa.OrgaoClasse = null;
                OPessoa.NivelEscolar = null;
            }
            
            OPessoa.listaEnderecos?.ForEach(e => {

                if (limparObjetosRelacionados){
                    
                    e.TipoEndereco = null;

                    e.Pais = null;

                    e.Cidade = null;

                    e.Estado = null;    
                }
                                
                e.cep = e.cep.onlyNumber().abreviar(8);

                e.logradouro = e.logradouro.toUppercaseWords().abreviar(100);

                e.numero = e.numero.abreviar(20);

                e.complemento = e.complemento.abreviar(50);

                e.bairro = e.bairro.toUppercaseWords().abreviar(80);

                e.zona = e.zona.abreviar(2);

                e.observacoes = e.observacoes.abreviar(100);

                e.idPais = e.idPais.isEmpty() ? "BRA" : e.idPais;

            });

            OPessoa.listaEmails?.ForEach(e => {
                
                if (limparObjetosRelacionados){
                    
                    e.TipoEmail = null;    
                }
                
                e.email = e.email.stringOrEmptyLower().abreviar(100);

            });
            
            OPessoa.listaTelefones?.ForEach(e => {
                
                if (limparObjetosRelacionados){
                    
                    e.TipoTelefone = null;    
                }                                                                

                e.ddi = e.ddi.isEmpty() ? 55 : e.ddi;

                e.nroTelefone = e.nroTelefone.onlyNumber().abreviar(15);

            });
                                   

            return OPessoa;
        }

        /// <summary>
        /// Filtrar somente e-mails, telefones e endereços não excluídos.
        /// </summary>
	    public static Pessoa limparListas(this Pessoa OPessoa) {

            OPessoa.listaTelefones = OPessoa.listaTelefones?.Where(x => x.dtExclusao == null).ToList();

            OPessoa.listaEmails = OPessoa.listaEmails?.Where(x => x.dtExclusao == null).ToList();

            OPessoa.listaEnderecos = OPessoa.listaEnderecos?.Where(x => x.dtExclusao == null).ToList();
            
            return OPessoa;
        }
    }
}
