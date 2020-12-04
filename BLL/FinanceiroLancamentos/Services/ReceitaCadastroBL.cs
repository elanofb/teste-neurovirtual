using System;
using System.Data.Entity;
using System.Linq;
using DAL.Financeiro;
using BLL.Core.Events;
using BLL.FinanceiroLancamentos.Events;
using BLL.Services;
using DAL.Pessoas;

namespace BLL.FinanceiroLancamentos {

    public class ReceitaCadastroBL : DefaultBL, IReceitaCadastroBL {

        //Atributos

        //Propriedades

        //Eventos
        private readonly EventAggregator onReceitaCadastrada = OnReceitaCadastrada.getInstance;

        /// <summary>
        /// Salvar o titulo de receita avulso
        /// </summary>
        public bool salvar(TituloReceita OTituloReceita){

            OTituloReceita = this.tratarDadosPessoa(OTituloReceita);

            OTituloReceita.setDefaultInsertValues();

            if (OTituloReceita.listaTituloReceitaPagamento.Any()) {

                OTituloReceita.listaTituloReceitaPagamento.ToList().ForEach(Item => {

                    Item.setDefaultInsertValues();

                });
            }

            db.TituloReceita.Add(OTituloReceita);

            db.SaveChanges();

            bool flagSucesso = OTituloReceita.id > 0;

            if (flagSucesso) {
                this.onReceitaCadastrada.subscribe(new OnReceitaCadastradaHandler());
                this.onReceitaCadastrada.publish(OTituloReceita as object);
            }

            return flagSucesso;
        }

        /// <summary>
        /// Capturar dados adicionais do cadastro de pessoa para inserir no título
        /// </summary>
	    private TituloReceita tratarDadosPessoa(TituloReceita OTituloReceita) {

            int idPessoa = OTituloReceita.idPessoa.toInt();

            if (idPessoa == 0) {

                return OTituloReceita;
            }

            var OPessoa = this.db.Pessoa.Where(x => x.id == idPessoa)
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

                return OTituloReceita;
            }

            this.preencherRecibo(ref OTituloReceita, OPessoa);

            return OTituloReceita;
        }

        /// <summary>
        /// Preencher os dados de recibo de um titulo de receita
        /// </summary>
        private void preencherRecibo(ref TituloReceita OTituloReceita, Pessoa OPessoa) {

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