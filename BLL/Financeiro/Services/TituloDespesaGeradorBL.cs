using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Pessoas;
using BLL.Services;
using DAL.Financeiro;
using DAL.Financeiro.Entities;
using DAL.Pessoas;
using DAL.Repository.Base;
using MoreLinq;

namespace BLL.Financeiro {

    public abstract class TituloDespesaGeradorBL : DefaultBL, ITituloDespesaGeradorBL {

        //Atributos
        private IPessoaBL _PessoaBL;
        
        //Propriedades
        private IPessoaBL OPessoaBL => _PessoaBL = _PessoaBL ?? new PessoaBL();

        //eventos

        //Metodo para geracao do titulo de Despesa
        public abstract UtilRetorno gerarLote(object OrigemTitulo);

        //Metodo abstrato para classes filhas
        //Cada tipo de Despesa deve gerar o titulo com suas particularidades
        public abstract UtilRetorno gerar(object OrigemTitulo);

        //Salvar uma Despesa no banco de dados
        //A operacao pode ser de atualizacao ou de insercao
        public virtual TituloDespesa salvar(TituloDespesa OTituloDespesa) {

            OTituloDespesa.Pessoa = OPessoaBL.carregar(OTituloDespesa.idPessoa.toInt()) ?? new Pessoa();
            
            //Tratar valores
            OTituloDespesa.nroTelPrincipalCredor = UtilString.onlyAlphaNumber(OTituloDespesa.Pessoa.nroTelPrincipal).abreviar(15);

            OTituloDespesa.nroTelSecundarioCredor = UtilString.onlyAlphaNumber(OTituloDespesa.Pessoa.nroTelSecundario).abreviar(15);

            OTituloDespesa.nomePessoaCredor = OTituloDespesa.Pessoa.nome.abreviar(100);

            OTituloDespesa.documentoPessoaCredor = OTituloDespesa.Pessoa.nroDocumento.abreviar(20);

            OTituloDespesa.observacao = OTituloDespesa.observacao.abreviar(1000);


            //Anular relacionamentos que nao se deseja inserções
            OTituloDespesa.idPessoa = OTituloDespesa.idPessoa == 0 ? null : OTituloDespesa.idPessoa;

            OTituloDespesa.Pessoa = null;

            OTituloDespesa.CentroCusto = null;

            OTituloDespesa.PeriodoRepeticao = null;
            
            if (OTituloDespesa.id > 0) {

                this.atualizar(OTituloDespesa);

            } else {

                this.inserir(OTituloDespesa);

            }

            return OTituloDespesa;
        }

        //Inserir os dados para um novo titulo de Despesa
        private void inserir(TituloDespesa OTituloDespesa) {

            OTituloDespesa.setDefaultInsertValues();

            OTituloDespesa.listaTituloDespesaPagamento = null;

            OTituloDespesa.Categoria = null;

            OTituloDespesa.CentroCusto = null;

            OTituloDespesa.ContaBancaria = null;

            OTituloDespesa.MacroConta = null;

            OTituloDespesa.Pessoa = null;
            
            using (var dataContext = new DataContext()) {

                dataContext.TituloDespesa.Add(OTituloDespesa);

                dataContext.SaveChanges();

            }
        }

        //Atualizar os dados de um titulo de Despesa
        private void atualizar(TituloDespesa OTituloDespesa){
            
            TituloDespesa dbTitulo = this.db.TituloDespesa.Find(OTituloDespesa.id);

            var entryTitulo = db.Entry(dbTitulo);

            entryTitulo.CurrentValues.SetValues(OTituloDespesa);

            OTituloDespesa.setDefaultUpdateValues();

            entryTitulo.State = EntityState.Modified;

            entryTitulo.ignoreFields(new[] { "idPessoa", "idOrganizacao", "idUnidade", "idCentroCusto", "idDespesa" });

            db.SaveChanges();
        }

        /// <summary>
        /// Preencher os dados de recibo de um titulo de Despesa
        /// </summary>
        protected void preencherRecibo(ref TituloDespesa OTituloDespesa, Pessoa OPessoa) {

            OTituloDespesa.nomePessoaCredor = OPessoa.nome;

            OTituloDespesa.documentoPessoaCredor = OPessoa.nroDocumento;

            OTituloDespesa.nroTelPrincipalCredor = string.Concat(OPessoa.dddTelPrincipal, OPessoa.nroTelPrincipal);

            //Carregar E-mails da Pessoa 
            var listaEmails = OPessoa.listaEmails.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.email)).ToList();

            if (listaEmails.Any()) {

                OTituloDespesa.emailPrincipalCredor = listaEmails.Select(x => x.email).FirstOrDefault();

            }

            //Carregar E-mails da Pessoa 
            var listaTelefones = OPessoa.listaTelefones.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.nroTelefone)).ToList();

            if (listaTelefones.Any()) {

                OTituloDespesa.nroTelPrincipalCredor = listaTelefones.Select(x => x.nroTelefone).FirstOrDefault();

            }

            //Carregar Endereco da Pessoa 
            var listaEnderecos = OPessoa.listaEnderecos.Where(x => x.dtExclusao == null).ToList();

            var OEndereco = listaEnderecos.FirstOrDefault(x => !string.IsNullOrEmpty(x.cep)) ?? new PessoaEndereco();

//            OTituloDespesa.cepRecibo = OEndereco.cep;
//
//            OTituloDespesa.logradouroRecibo = OEndereco.logradouro;
//
//            OTituloDespesa.complementoRecibo = OEndereco.complemento;
//
//            OTituloDespesa.numeroRecibo = OEndereco.numero;
//
//            OTituloDespesa.bairroRecibo = OEndereco.bairro;
//
//            OTituloDespesa.idCidadeRecibo = OEndereco.idCidade;

        }
    }
}