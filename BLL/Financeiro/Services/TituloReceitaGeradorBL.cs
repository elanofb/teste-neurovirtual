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

    public abstract class TituloReceitaGeradorBL : DefaultBL, ITituloReceitaGeradorBL {

        //Atributos

        //Propriedades

        //eventos

        //Metodo para geracao do titulo de receita
        public abstract UtilRetorno gerarLote(object OrigemTitulo);

        //Metodo abstrato para classes filhas
        //Cada tipo de receita deve gerar o titulo com suas particularidades
        public abstract UtilRetorno gerar(object OrigemTitulo);

        //Salvar uma receita no banco de dados
        //A operacao pode ser de atualizacao ou de insercao
        public virtual TituloReceita salvar(TituloReceita OTituloReceita) {

            //Tratar valores
            OTituloReceita.nroTelPrincipal = UtilString.onlyAlphaNumber(OTituloReceita.nroTelPrincipal).abreviar(15);

            OTituloReceita.nroTelSecundario = UtilString.onlyAlphaNumber(OTituloReceita.nroTelSecundario).abreviar(15);

            OTituloReceita.nomePessoa = OTituloReceita.nomePessoa.abreviar(100).toUppercaseWords();

            OTituloReceita.documentoPessoa = OTituloReceita.documentoPessoa.abreviar(20);
            
            OTituloReceita.passaporteRecibo = OTituloReceita.passaporteRecibo.abreviar(20);
            
            OTituloReceita.rneRecibo = OTituloReceita.rneRecibo.abreviar(20);

            OTituloReceita.nomeRecibo = OTituloReceita.nomeRecibo.abreviar(100).toUppercaseWords();

            OTituloReceita.documentoRecibo = OTituloReceita.documentoRecibo.abreviar(20);

            OTituloReceita.cepRecibo = OTituloReceita.cepRecibo.onlyNumber().abreviar(8);

            OTituloReceita.logradouroRecibo = OTituloReceita.logradouroRecibo.abreviar(100).toUppercaseWords();

            OTituloReceita.numeroRecibo = OTituloReceita.numeroRecibo.abreviar(20);

            OTituloReceita.complementoRecibo = OTituloReceita.complementoRecibo.abreviar(50).toUppercaseWords();

            OTituloReceita.bairroRecibo = OTituloReceita.bairroRecibo.abreviar(80).toUppercaseWords();

            OTituloReceita.nomeCidadeRecibo = OTituloReceita.nomeCidadeRecibo.abreviar(100).toUppercaseWords();

            OTituloReceita.motivoDesconto = OTituloReceita.motivoDesconto.abreviar(500);

            OTituloReceita.observacao = OTituloReceita.observacao.abreviar(1000);


            //Anular relacionamentos que nao se deseja inserções
            OTituloReceita.idPessoa = OTituloReceita.idPessoa == 0 ? null : OTituloReceita.idPessoa;

            OTituloReceita.Pessoa = null;

            OTituloReceita.CidadeRecibo = null;

            OTituloReceita.CentroCusto = null;

            OTituloReceita.PeriodoRepeticao = null;

            OTituloReceita.CupomDesconto = null;

            if (OTituloReceita.id > 0) {

                this.atualizar(OTituloReceita);

            } else {

                this.inserir(OTituloReceita);

            }

            return OTituloReceita;
        }

        //Inserir os dados para um novo titulo de receita
        private void inserir(TituloReceita OTituloReceita) {

            OTituloReceita.setDefaultInsertValues();

            OTituloReceita.listaTituloReceitaPagamento = null;

            OTituloReceita.Categoria = null;

            OTituloReceita.CentroCusto = null;

            OTituloReceita.CidadeRecibo = null;

            OTituloReceita.ContaBancaria = null;

            OTituloReceita.MacroConta = null;

            OTituloReceita.Pessoa = null;

            if (OTituloReceita.listaDescontosAntecipacao != null) {

                OTituloReceita.listaDescontosAntecipacao.ForEach(x => { x.setDefaultInsertValues(); });
            }

            using (var dataContext = new DataContext()) {

                dataContext.TituloReceita.Add(OTituloReceita);

                dataContext.SaveChanges();

            }
        }

        //Atualizar os dados de um titulo de receita
        private void atualizar(TituloReceita OTituloReceita){

            OTituloReceita.listaDescontosAntecipacao = null;

            TituloReceita dbTitulo = this.db.TituloReceita.Find(OTituloReceita.id);

            var entryTitulo = db.Entry(dbTitulo);

            entryTitulo.CurrentValues.SetValues(OTituloReceita);

            OTituloReceita.setDefaultUpdateValues();

            entryTitulo.State = EntityState.Modified;

            entryTitulo.ignoreFields(new[] { "idPessoa", "idOrganizacao", "idUnidade", "idCentroCusto", "idReceita" });

            db.SaveChanges();
        }

        /// <summary>
        /// Preencher os dados de recibo de um titulo de receita
        /// </summary>
        protected void preencherRecibo(ref TituloReceita OTituloReceita, Pessoa OPessoa) {

            OTituloReceita.nomePessoa = OPessoa.flagTipoPessoa == "J" ? (!OPessoa.razaoSocial.isEmpty() ? OPessoa.razaoSocial : OPessoa.nome) : OPessoa.nome;

            OTituloReceita.documentoPessoa = OPessoa.nroDocumento;

            OTituloReceita.nroTelPrincipal = string.Concat(OPessoa.dddTelPrincipal, OPessoa.nroTelPrincipal);

            OTituloReceita.nomeRecibo = OTituloReceita.nomePessoa;

            OTituloReceita.documentoRecibo = OPessoa.nroDocumento;
            
            OTituloReceita.passaporteRecibo = OPessoa.passaporte;
            
            OTituloReceita.rneRecibo = OPessoa.rne;

            //Carregar E-mails da Pessoa 
            var listaEmails = OPessoa.listaEmails.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.email)).ToList();

            if (listaEmails.Any()) {

                OTituloReceita.emailPrincipal = listaEmails.Select(x => x.email).FirstOrDefault();

            }

            //Carregar E-mails da Pessoa 
            var listaTelefones = OPessoa.listaTelefones.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.nroTelefone)).ToList();

            if (listaTelefones.Any()) {

                OTituloReceita.nroTelPrincipal = listaTelefones.Select(x => x.nroTelefone).FirstOrDefault();

            }

            //Carregar Endereco da Pessoa 
            var listaEnderecos = OPessoa.listaEnderecos.Where(x => x.dtExclusao == null).ToList();

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