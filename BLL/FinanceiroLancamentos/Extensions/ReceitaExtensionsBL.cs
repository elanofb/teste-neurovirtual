using System;
using System.Data.Entity;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;
using DAL.Pessoas;
using DAL.Repository.Base;

namespace BLL.FinanceiroLancamentos {

    public static class ReceitaExtensionsBL {

        //Atributos
        private static DataContext _DataContext;

        //Propriedades
        private static DataContext db => _DataContext = _DataContext ?? new DataContext();
        
        /// <summary>
        /// Capturar dados adicionais do cadastro de pessoa para inserir no título
        /// </summary>
	    public static void tratarDadosPessoa(this TituloReceita OTituloReceita) {

            int idPessoa = OTituloReceita.idPessoa.toInt();

            if (idPessoa == 0) {

                return;
            }

            var OPessoa = db.Pessoa.Where(x => x.id == idPessoa)
                            .Select(x => new {
                                x.id,
                                x.nroDocumento,
                                x.nome,
                                x.dddTelPrincipal,
                                x.nroTelPrincipal,
                                x.dddTelSecundario,
                                x.nroTelSecundario,
                                listaEmails = x.listaEmails.Select(e => new { e.id, e.email, e.idTipoEmail, e.dtExclusao }).ToList(),
                                listaEnderecos = x.listaEnderecos.Select(e => new { e.id, e.idTipoEndereco, e.cep, e.logradouro, e.numero, e.complemento, e.bairro, e.nomeCidade, e.idCidade, e.dtExclusao }).ToList(),
                                listaTelefones = x.listaTelefones.Select(t => new { t.id, t.idTipoTelefone, t.nroTelefone, t.dtExclusao }).ToList()
                            }).AsNoTracking().FirstOrDefault().ToJsonObject<Pessoa>();

            if (OPessoa == null) {
                return;
            }

            OTituloReceita.preencherRecibo(OPessoa);
        }

        /// <summary>
        /// Preencher os dados de recibo de um titulo de receita
        /// </summary>
        public static void preencherRecibo(this TituloReceita OTituloReceita, Pessoa OPessoa) {

            OTituloReceita.nomePessoa = OPessoa.nome;

            OTituloReceita.documentoPessoa = OPessoa.nroDocumento;

            OTituloReceita.nroTelPrincipal = string.Concat(OPessoa.dddTelPrincipal, OPessoa.nroTelPrincipal);

            OTituloReceita.nomeRecibo = OPessoa.nome;

            OTituloReceita.documentoRecibo = OPessoa.nroDocumento;

            OTituloReceita.nroTelPrincipal = OPessoa.nroTelPrincipal;

            OTituloReceita.nroTelSecundario = OPessoa.nroTelSecundario;

            //Carregar E-mails da Pessoa 
            var listaEmails = OPessoa.retornarListaEmails();

            if (listaEmails.Any()) {

                OTituloReceita.emailPrincipal = listaEmails.Select(x => x.email).FirstOrDefault();

            }

            //Carregar E-mails da Pessoa 
            var listaTelefones = OPessoa.retornarListaTelefones();

            if (listaTelefones.Any()) {

                OTituloReceita.nroTelPrincipal = listaTelefones.Select(x => x.nroTelefone).FirstOrDefault();
            }

            //Carregar Endereco da Pessoa 
            var listaEnderecos = OPessoa.retornarListaEnderecos();

            var OEndereco = listaEnderecos.FirstOrDefault(x => !string.IsNullOrEmpty(x.cep)) ?? new PessoaEndereco();

            OTituloReceita.cepRecibo = OEndereco.cep;

            OTituloReceita.logradouroRecibo = OEndereco.logradouro;

            OTituloReceita.complementoRecibo = OEndereco.complemento;

            OTituloReceita.numeroRecibo = OEndereco.numero;

            OTituloReceita.bairroRecibo = OEndereco.bairro;

            OTituloReceita.idCidadeRecibo = OEndereco.idCidade;

        }
    }
}