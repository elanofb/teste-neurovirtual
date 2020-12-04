using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Localizacao;
using DAL.Pessoas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DAL.ContasBancarias;
using DAL.CuponsDesconto;
using DAL.Financeiro.Entities;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Financeiro {

    //
    public class TituloReceita {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int? idUnidade { get; set; }

        public int? idTituloReceitaOrigem { get; set; }

        public TituloReceita TituloReceitaOrigem { get; set; }
        
        public int? idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public byte idTipoReceita { get; set; }

        public virtual TipoReceita TipoReceita { get; set; }

        public int? idReceita { get; set; }

        public byte? idGatewayPermitido { get; set; }
        
        public GatewayPagamento GatewayPermitido { get; set; }

        public bool? flagCartaoCreditoPermitido { get; set; }

        public bool? flagBoletoBancarioPermitido { get; set; }

        public bool? flagDepositoPermitido { get; set; }

        public int? idMacroConta { get; set; }

        public virtual MacroConta MacroConta { get; set; }

        public int? idCategoria { get; set; }

        public virtual CategoriaTitulo Categoria { get; set; }

        public int? idCentroCusto { get; set; }

        public virtual CentroCusto CentroCusto { get; set; }

        public int? idContaBancaria { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }

        public int? idPeriodoRepeticao { get; set; }

        public virtual PeriodoRepeticao PeriodoRepeticao { get; set; }

        public byte? limiteParcelamento { get; set; }

        public string descricao { get; set; }

        public string nomePessoa { get; set; }

        public string documentoPessoa { get; set; }
        
        public bool? flagEstrangeiro { get; set; }

        public string nroTelPrincipal { get; set; }

        public string nroTelSecundario { get; set; }

        public string emailPrincipal { get; set; }

        public byte? mesCompetencia { get; set; }

        public short? anoCompetencia { get; set; }

        public int? qtdeRepeticao { get; set; }
        
        public decimal? valorTotal { get; set; }
        
        public decimal? valorJuros { get; set; }
        
        public decimal? valorDesconto { get; set; }
        
        public string motivoDesconto { get; set; }

        public int? idCupomDesconto { get; set; }

        public virtual CupomDesconto CupomDesconto { get; set; }

        public int? idTabelaImposto { get; set; }

        //public virtual TabelaImposto TabelaImposto { get; set; }

        public string nomeRecibo { get; set; }

        public string documentoRecibo { get; set; }
        
        public string passaporteRecibo { get; set; }
        
        public string rneRecibo { get; set; }

        public string logradouroRecibo { get; set; }

        public string numeroRecibo { get; set; }

        public string complementoRecibo { get; set; }

        public string bairroRecibo { get; set; }

        public string cepRecibo { get; set; }

        public int? idCidadeRecibo { get; set; }

        public virtual Cidade CidadeRecibo { get; set; }

        public string nomeCidadeRecibo { get; set; }

        public DateTime? dtVencimentoOriginal { get; set; }

        public DateTime? dtVencimento { get; set; }

        public DateTime? dtLimitePagamento { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public DateTime? dtCompetencia { get; set; }

        public string nroDocumento { get; set; }

        public string nroContrato { get; set; }

        public string observacao { get; set; }

        public int? nroNotaFiscal { get; set; }

        public int? nroContabil { get; set; }

        public string flagCategoriaPessoa { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public bool ativo { get; set; }

        public string flagFixa { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual UsuarioSistema UsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }

        public virtual List<TituloReceitaPagamento> listaTituloReceitaPagamento { get; set; }

        public virtual List<TituloReceitaDescontoAntecipacao> listaDescontosAntecipacao { get; set; }
            
        //Constructor
        public TituloReceita() {

            this.listaTituloReceitaPagamento = new List<TituloReceitaPagamento>();
            
            this.listaDescontosAntecipacao = new List<TituloReceitaDescontoAntecipacao>();

        }
        
        /// <summary>
        /// Retornar a lista de pagamentos removendo os registros inválidos
        /// </summary>
        /// <returns></returns>
	    public List<TituloReceitaPagamento> retornarListaPagamentos() {

            var lista = this.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

            return lista;
        }

        /// <summary>
        /// Retornar a lista de descontos removendo os registros inválidos
        /// </summary>
        /// <returns></returns>
	    public List<TituloReceitaDescontoAntecipacao> retornarDescontosAntecipacao(DateTime dtBase) {

            var lista = this.listaDescontosAntecipacao.Where(x => x.dtExclusao == null && x.dtLimiteDesconto >= dtBase).OrderBy(x => x.dtLimiteDesconto).ToList();

            return lista;
        }


    }

    //Mapeamento de relacionamentos e configuracoes DB
    internal sealed class TituloReceitaMapper : EntityTypeConfiguration<TituloReceita> {

        public TituloReceitaMapper() {

            this.ToTable("tb_titulo_receita");

            this.HasKey(o => o.id);

            this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(o => o.TipoReceita).WithMany().HasForeignKey(o => o.idTipoReceita);

            this.HasOptional(o => o.TituloReceitaOrigem).WithMany().HasForeignKey(o => o.idTituloReceitaOrigem);
            
            this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);

            this.HasOptional(o => o.UsuarioExclusao).WithMany().HasForeignKey(o => o.idUsuarioExclusao);

            this.HasRequired(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

            this.HasOptional(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);

            this.HasOptional(o => o.GatewayPermitido).WithMany().HasForeignKey(o => o.idGatewayPermitido);
            
            this.HasOptional(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);

            this.HasOptional(o => o.Categoria).WithMany().HasForeignKey(o => o.idCategoria);

            this.HasOptional(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);

            this.HasOptional(o => o.CidadeRecibo).WithMany().HasForeignKey(o => o.idCidadeRecibo);

            this.HasOptional(o => o.ContaBancaria).WithMany().HasForeignKey(o => o.idContaBancaria);

            this.HasOptional(o => o.PeriodoRepeticao).WithMany().HasForeignKey(o => o.idPeriodoRepeticao);

            this.HasOptional(o => o.CupomDesconto).WithMany().HasForeignKey(o => o.idCupomDesconto);
            
            //this.HasOptional(o => o.TabelaImposto).WithMany().HasForeignKey(o => o.idTabelaImposto);
        }
    }
}