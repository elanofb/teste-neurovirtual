using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using BLL.Contribuicoes;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using PagedList;
using WEB.Areas.AssociadosContribuicoes.ViewModels;

namespace WEB.Areas.ContribuicoesPainel.ViewModels {

    public class PainelCobrancaVM {

        //Atributos
        private IContribuicaoBL _ContribuicaoBL;
        
        private IAssociadoBL _AssociadoBL;
        
        private IAssociadoContribuicaoBoletoBL _AssociadoContribuicaoBoletoBL;
        
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        private IAssociadoContribuicaoResumoBL _IAssociadoContribuicaoResumoBL;

        //Servicos
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();
        
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        
        private IAssociadoContribuicaoBoletoBL OAssociadoContribuicaoBoletoBL => _AssociadoContribuicaoBoletoBL = _AssociadoContribuicaoBoletoBL ?? new AssociadoContribuicaoBoletoBL();
        
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        
        private IAssociadoContribuicaoResumoBL OAssociadoContribuicaoResumoBL => _IAssociadoContribuicaoResumoBL = _IAssociadoContribuicaoResumoBL ?? new AssociadoContribuicaoResumoBL();
        
        //Propriedades
        private List<int> idsTipoAssociado { get; set; }
        
        public DateTime dtVencimento { get; set; }
        
        private int ano { get; set; }
        
        private int mes { get; set; }
        
        private string flagSituacao { get; set; }
        
        private string flagSituacaoContribuicao { get; set; }
        
        private string ativo { get; set; }
        
        private string valorBusca { get; set; }
        
        public DateTime dtFiltroFim { get; set; }
        
        public DateTime dtFiltroInicio { get; set; }

        //Propriedades
        public Contribuicao Contribuicao { get; set; }
        
        public List<AssociadoContribuicaoResumoVW> listaContribuicoes { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaAssociados { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listagemFiltrada { get; set; }
        
        public IPagedList<AssociadoContribuicaoItemLista> listaAssociadosPager { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaEmAberto { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaQuitados { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaIsentos { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaVencidos { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaCobrancas { get; set; }
        
        public List<AssociadoContribuicaoItemLista> listaNaoCobrados { get; set; }
        
        public IPagedList<AssociadoContribuicaoBoleto> listaBoletos { get; set; }
        
        public long qtdeBoletosGerados { get; set; }

        //Construtor
        public PainelCobrancaVM() {

            this.idsTipoAssociado = new List<int>();

            this.listaContribuicoes = new List<AssociadoContribuicaoResumoVW>();

            this.listaAssociados = new List<AssociadoContribuicaoItemLista>();

            this.listagemFiltrada = new List<AssociadoContribuicaoItemLista>();

            this.listaEmAberto = new List<AssociadoContribuicaoItemLista>();

            this.listaQuitados = new List<AssociadoContribuicaoItemLista>();

            this.listaIsentos = new List<AssociadoContribuicaoItemLista>();

            this.listaVencidos = new List<AssociadoContribuicaoItemLista>();

            this.listaCobrancas = new List<AssociadoContribuicaoItemLista>();

            this.listaNaoCobrados = new List<AssociadoContribuicaoItemLista>();

            this.listaAssociadosPager = new List<AssociadoContribuicaoItemLista>().ToPagedList(1, 200);

            this.listaBoletos = new List<AssociadoContribuicaoBoleto>().ToPagedList(1, 200);
        }

        /// <summary>
        /// Carregar as informacoes necessarias para dinamizar o painel
        /// </summary>
        public void carregarDadosContribuicao(int idContribuicao, int[] idsAssociados) {

            if (Contribuicao == null) {
                
                this.carregarContribuicao(idContribuicao);
            }

            this.carregarFiltros();

            this.carregarContribuicoes();

            this.carregarAssociados(idsAssociados);

            this.preencherContribuicaoAssociados();
        }
        
        /// <summary>
        /// Carregar a contribuicao pelo ID
        /// </summary>
        public void carregarContribuicao(int idContribuicao) {

            this.Contribuicao = this.OContribuicaoBL.query().condicoesSeguranca()
                                    .Where(x => x.id == idContribuicao)
                                    .Select(x => new {
                                        x.id, 
                                        x.idOrganizacao, 
                                        x.dtValidade, 
                                        x.idPeriodoContribuicao, 
                                        x.idTipoVencimento, 
                                        x.descricao,
                                        x.ativo,
                                        x.emailCobrancaTitulo,
                                        x.emailCobrancaHtml,
                                        PeriodoContribuicao = new {
                                            id = x.idPeriodoContribuicao, 
                                            x.PeriodoContribuicao.qtdeMeses, 
                                            x.PeriodoContribuicao.qtdeAnos
                                        },
                                        listaContribuicaoPreco = x.listaContribuicaoPreco.Select(v => new {
                                                                     v.id,
                                                                     v.idContribuicao,
                                                                     v.idTipoAssociado,
                                                                     v.idTabelaPreco,
                                                                     v.flagIsento,
                                                                     v.valorFinal,
                                                                     v.flagExcluido
                                                                 }).ToList(),
                                        listaTabelaPreco = x.listaTabelaPreco.Select(v => new {
                                                               v.id,
                                                               v.idContribuicao,
                                                               v.descricao,
                                                               v.dtInicioVigencia,
                                                               v.ativo,
                                                               v.flagExcluido,
                                                               listaPrecos = v.listaPrecos.Select(p => new {
                                                                    p.id,
                                                                    p.idTabelaPreco,
                                                                    p.idTipoAssociado,
                                                                    TipoAssociado = new {
                                                                        id = p.idTipoAssociado, 
                                                                        p.TipoAssociado.idOrganizacao, 
                                                                        p.TipoAssociado.descricao
                                                                    },
                                                                    p.flagIsento,
                                                                    p.valorFinal,
                                                                    p.flagExcluido
                                                               }).ToList()    
                                                            }).ToList(),
                                        listaContribuicaoVencimento = x.listaContribuicaoVencimento.Select(v => new {
                                                                          v.id, 
                                                                          v.diaInicioVigencia,
                                                                          v.diaFimVigencia,
                                                                          v.mesInicioVigencia, 
                                                                          v.mesFimVigencia,
                                                                          v.diaVencimento,
                                                                          v.mesVencimento,
                                                                          v.idContribuicao,
                                                                          v.dtExclusao
                                                                      }).ToList()

                                    }).FirstOrDefault().ToJsonObject<Contribuicao>() ?? new Contribuicao();
        }

        //Carregar os filtros enviados via form
        private void carregarFiltros() {

            this.idsTipoAssociado = UtilRequest.getListInt("idsTipoAssociado");

            this.flagSituacao = UtilRequest.getString("flagSituacao");

            this.valorBusca = UtilRequest.getString("valorBusca").ToLower();

            this.ano = UtilRequest.getInt32("ano");

            this.mes = UtilRequest.getInt32("mes");

            this.flagSituacaoContribuicao = UtilRequest.getString("flagSituacaoContribuicao");

            this.ativo = UtilRequest.getString("ativo");

            string mesDiaVencimento = UtilRequest.getString("mesDiaVencimento");

            if (!string.IsNullOrEmpty(mesDiaVencimento)) {

                string[] data = mesDiaVencimento.Split('/');

                int dia = data.Length > 0 ? UtilNumber.toInt32(data[0]) : 0;

                int mes = data.Length > 1 ? UtilNumber.toInt32(data[1]) : 0;

                if (dia > 0 && mes > 0) {
                    dtVencimento = new DateTime(ano, mes, dia);
                }

                return;
            }

            //Se a contribuição for anual e com data fixa deve-se capturar a única data de vencimento possível para o ano
            if (Contribuicao.PeriodoContribuicao?.qtdeAnos >= 1 && !this.Contribuicao.flagVencimentoVariado()) {

                var OVencimento = Contribuicao.retornarListaVencimento().FirstOrDefault() ?? new ContribuicaoVencimento();

                dtVencimento = new DateTime(this.ano, OVencimento.mesVencimento.toInt(), OVencimento.diaVencimento.toInt());
            }
        }


        //Carregar todas as cobranças ja realizadas para a contribuicao escolhida
        private void carregarContribuicoes() {

            this.dtFiltroFim = this.dtVencimento.AddDays(1);

            this.dtFiltroInicio = this.dtVencimento;

            if (this.Contribuicao.flagVencimentoVariado()){

                this.mes = this.mes > 0 ? this.mes : DateTime.Today.Month;

                this.dtFiltroInicio = new DateTime(this.ano, this.mes, 1);

                this.dtFiltroFim = dtFiltroInicio.AddMonths(1);
            }

            var query = this.OAssociadoContribuicaoResumoBL.listar(Contribuicao.id, 0);

            if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELO_ULTIMO_PAGAMENTO) {

                query = query.Where(x => (   x.dtPagamento != null && 
                                             x.dtPagamento >= this.dtFiltroInicio && 
                                             x.dtPagamento < this.dtFiltroFim
                                         ) || (
                                             x.dtVencimentoAtual >= this.dtFiltroInicio && 
                                             x.dtVencimentoAtual < this.dtFiltroFim
                                         )
                                     ).AsQueryable();

            } else {
                
                query = query.Where(x => x.dtVencimentoOriginal >= this.dtFiltroInicio && x.dtVencimentoOriginal < this.dtFiltroFim).AsQueryable();
            }    

            this.listaContribuicoes = query.ToList();
        }

        /// <summary>
        /// Carregar os associados que devem/deveriam estar vinculados à contribuicao
        /// </summary>
        private void carregarAssociados(int[] idsAssociados) {

            var listaAssociadoContribuicao = new List<AssociadoContribuicao>();

            var flagDataVariada = this.Contribuicao.flagVencimentoVariado();

            var OTabelaPreco = this.Contribuicao.retornarTabelaVigente();

            var listaPrecos = OTabelaPreco.listaPrecos.Where(x => x.flagExcluido == "N").ToList();

            var idsTiposAssociado = listaPrecos.Where(x => x.TipoAssociado.idOrganizacao == Contribuicao.idOrganizacao).Select(x => x.idTipoAssociado).ToArray();

            var idsAssociadosVinculados = listaContribuicoes.Select(x => x.idAssociado).Distinct().ToArray();

            var baseQuery = this.OAssociadoBL.listar(0, String.Empty, String.Empty, "S")
                                .Where(x => idsTiposAssociado.Contains(x.idTipoAssociado) || idsAssociadosVinculados.Contains(x.id));

            if (ano > 0) {

                var dtCorteAno = new DateTime(ano, 12, 31);

                if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {

                    baseQuery = baseQuery.Where(x => x.dtAdmissao <= dtCorteAno || x.dtAdmissao == null || idsAssociadosVinculados.Contains(x.id));

                }

                if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO) {

                    dtCorteAno = new DateTime(ano, mes, 1).AddMonths(1);

                    baseQuery = baseQuery.Where(x => x.dtAdmissao <= dtCorteAno || idsAssociadosVinculados.Contains(x.id));
                }

                if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELO_ULTIMO_PAGAMENTO) {

                    dtCorteAno = new DateTime(ano, mes, 1).AddMonths(1);

                    listaAssociadoContribuicao = this.OAssociadoContribuicaoBL.listar(this.Contribuicao.id, 0, null, null)
                                                                              .Where(x => x.dtExclusao == null && 
                                                                                         (x.dtVencimentoAtual <= dtCorteAno || x.dtPagamento <= dtCorteAno)
                                                                              ).Select(x => new {
                                                                                  x.id, 
                                                                                  x.idAssociado, 
                                                                                  x.idAssociadoContribuicaoPrincipal, 
                                                                                  x.dtPagamento, 
                                                                                  x.dtVencimentoOriginal, 
                                                                                  x.dtVencimentoAtual
                                                                              })
                                                                              .ToListJsonObject<AssociadoContribuicao>();

                    var idsAssociadosContrubuicao = listaAssociadoContribuicao.Select(x => x.idAssociado).ToArray();

                    baseQuery = baseQuery.Where(x => idsAssociadosContrubuicao.Contains(x.id) || idsAssociadosVinculados.Contains(x.id));
                }

            }

            //Se a contribuicao nao for anuidade, filtramos somente os associados nao vinculados que foram admitidos antes do vencimento atual
            if (dtVencimento != DateTime.MinValue && this.Contribuicao.PeriodoContribuicao.qtdeAnos == 0) {

                baseQuery = baseQuery.Where(x => x.dtAdmissao <= dtVencimento || (x.dtAdmissao == null && !flagDataVariada) || idsAssociadosVinculados.Contains(x.id));
            }

            if (Contribuicao.flagGerarParaTodos != true){
                
            }

            if (this.idsTipoAssociado.Count > 0) {

                baseQuery = baseQuery.Where(x => this.idsTipoAssociado.Contains(x.idTipoAssociado));
            }


            if (!String.IsNullOrEmpty(this.ativo)) {

                baseQuery = baseQuery.Where(x => x.ativo == this.ativo);
            }
            
            var query = baseQuery.Select(x => new {
                idAssociado = x.id,
                x.idPessoa,
                nomeAssociado = x.Pessoa.nome,

                nroAssociado = x.nroAssociado,
                nroDocumentoAssociado = x.Pessoa.nroDocumento,

                statusAssociado = x.ativo,

                x.dtAdmissao,
                dtUltimaContribuicao = DateTime.Today,
                descricaoTipoAssociado = x.TipoAssociado.descricao,

                listEmails = x.Pessoa.listaEmails,

                listTelefones = x.Pessoa.listaTelefones
                
            }).AsQueryable();

            
            if (idsAssociados != null && idsAssociados.Length > 0) {

                query = query.Where(x => idsAssociados.Contains(x.idAssociado));
            }

            var listaDinamicaAssociados = query.ToList();

            foreach (var Item in listaDinamicaAssociados) {

                var ItemLista = new AssociadoContribuicaoItemLista();

                ItemLista.AssociadoContribuicao = new AssociadoContribuicaoResumoVW();

                ItemLista.AssociadoContribuicao.idAssociado = Item.idAssociado;

                ItemLista.AssociadoContribuicao.idPessoa = Item.idPessoa;

                ItemLista.AssociadoContribuicao.nomeAssociado = Item.nomeAssociado;

                ItemLista.AssociadoContribuicao.descricaoTipoAssociado = Item.descricaoTipoAssociado;

                ItemLista.AssociadoContribuicao.dtVencimentoOriginal = dtVencimento;
                
                ItemLista.AssociadoExcel.nroAssociado = Item.nroAssociado;
                
                ItemLista.AssociadoExcel.nroDocumentoAssociado = Item.nroDocumentoAssociado;
                
                ItemLista.AssociadoExcel.statusAssociado = Item.statusAssociado;

                ItemLista.listEmails = Item.listEmails.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.email)).Take(2).ToList();

                ItemLista.listTelefones = Item.listTelefones.Where(x => x.dtExclusao == null && !string.IsNullOrEmpty(x.nroTelefone)).Take(3).ToList();
                

                if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELA_ADMISSAO_ASSOCIADO) {

                    ItemLista.AssociadoContribuicao.dtVencimentoOriginal = new DateTime(this.ano, UtilNumber.toInt32(Item.dtAdmissao?.Month), UtilNumber.toInt32(Item.dtAdmissao?.Day));
                }

                if (this.Contribuicao.idTipoVencimento == TipoVencimentoConst.VENCIMENTO_PELO_ULTIMO_PAGAMENTO){

                    var dtUltimaContribuicao = listaAssociadoContribuicao.Where(i => i.idAssociado == Item.idAssociado).Select(i => (i.dtPagamento != null ? i.dtPagamento.Value : i.dtVencimentoAtual)).LastOrDefault();

                    ItemLista.AssociadoContribuicao.dtVencimentoOriginal = dtUltimaContribuicao != DateTime.MinValue ? new DateTime(this.ano, dtUltimaContribuicao.Month.toInt(), dtUltimaContribuicao.Day.toInt()) : DateTime.Today;
                }

                this.listaAssociados.Add(ItemLista);
            }
        }

        /// <summary>
        /// Preencher os dados da contribuicao para cada um dos associados
        /// </summary>
        private void preencherContribuicaoAssociados() {

            this.listaAssociados = this.listaAssociados.Where(x => x.AssociadoContribuicao.dtVencimentoOriginal >= this.dtFiltroInicio && x.AssociadoContribuicao.dtVencimentoOriginal < this.dtFiltroFim).ToList();

            foreach (var Item in listaAssociados) {

                var OAssociadoContribuicao = this.listaContribuicoes.FirstOrDefault(x => x.idAssociado == Item.AssociadoContribuicao.idAssociado && x.idAssociadoContribuicaoPrincipal.toInt() == 0);

                if (OAssociadoContribuicao == null) {

                    Item.AssociadoContribuicao.id = 0;

                    continue;
                }

                var listaDependentes = this.listaContribuicoes.Where(x => x.idAssociadoContribuicaoPrincipal == OAssociadoContribuicao.id).ToList();

                OAssociadoContribuicao.descricaoTipoAssociado = OAssociadoContribuicao.descricaoTipoAssociado.isEmpty() ? Item.AssociadoContribuicao.descricaoTipoAssociado : OAssociadoContribuicao.descricaoTipoAssociado;

                Item.carregarDados(OAssociadoContribuicao, listaDependentes);

            }

            this.listagemFiltrada = this.listaAssociados.ToList();

            if (flagSituacao == "cobrados") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id > 0).ToList();

            }
            if (flagSituacao == "nao_cobrados") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id == 0).ToList();

            }

            if (flagSituacao == "nao_pagos") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id > 0 && x.AssociadoContribuicao.flagIsento == false && x.AssociadoContribuicao.dtPagamento == null).ToList();

            }

            if (flagSituacao == "atrasados") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id > 0 && x.AssociadoContribuicao.flagIsento == false && x.AssociadoContribuicao.flagEmAtraso()).ToList();
    
            }

            if (flagSituacao == "isentos") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id > 0 && x.AssociadoContribuicao.flagIsento == true).ToList();

            }

            if (flagSituacao == "quitados") {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.id > 0 && x.AssociadoContribuicao.dtPagamento != null && x.AssociadoContribuicao.flagIsento == false).ToList();

            }

            if (!string.IsNullOrEmpty(valorBusca)) {

                this.listagemFiltrada = this.listagemFiltrada.Where(x => x.AssociadoContribuicao.nomeAssociado.ToLower().Contains(valorBusca) || x.AssociadoContribuicao.descricaoTipoAssociado.ToLower().Contains(valorBusca)).ToList();

            }

            this.listaAssociadosPager = this.listagemFiltrada.OrderBy(x => x.AssociadoContribuicao.nomeAssociado).ToPagedList(UtilRequest.getNroPagina(), 200);
    
            this.listaCobrancas = listaAssociados.Where(x => x.AssociadoContribuicao.id > 0).ToList();

            this.listaQuitados = listaAssociados.Where(x => x.AssociadoContribuicao.dtPagamento != null && x.AssociadoContribuicao.id > 0).ToList();

            this.listaIsentos = listaAssociados.Where(x => x.AssociadoContribuicao.flagIsento == true && x.AssociadoContribuicao.id > 0).ToList();

            this.listaEmAberto = listaAssociados.Where(x => !x.AssociadoContribuicao.flagEmAtraso() && !x.AssociadoContribuicao.flagQuitado() && x.AssociadoContribuicao.id > 0).ToList();

            this.listaVencidos = listaAssociados.Where(x => x.AssociadoContribuicao.flagEmAtraso() && x.AssociadoContribuicao.id > 0).ToList();

            this.listaNaoCobrados = listaAssociados.Where(x => x.AssociadoContribuicao.id == 0).ToList();
        }


        /// <summary>
        /// Carregar boletos
        /// </summary>
        public IQueryable<AssociadoContribuicaoBoleto> carregarBoletos() {

            var idsCobrancas = this.listaContribuicoes.Select(x => x.id).ToArray();

            var idsAssociados = this.listagemFiltrada.Select(x => x.AssociadoContribuicao.idAssociado).ToArray();

            var query = this.OAssociadoContribuicaoBoletoBL.listar(0).Where(x => idsCobrancas.Contains(x.idAssociadoContribuicao) && idsAssociados.Contains(x.idAssociado) && x.idTituloReceitaPagamento > 0 && x.boletoUrl.Length > 0);

            return query;
        }

        /// <summary>
        /// Carregar os boletos para exportacao
        /// </summary>
        public List<string> carregarBoletosExportacao(int idContribuicao, List<int> idsTituloReceitaPagamento) {

            if (idsTituloReceitaPagamento != null && idsTituloReceitaPagamento.Any()) {

                var listarUrls = this.OAssociadoContribuicaoBoletoBL.listar(0)
                                     .Where(x => idsTituloReceitaPagamento.Contains(x.idTituloReceitaPagamento.Value) && x.dtExclusaoParcela == null)
                                     .Select(x => x.boletoUrl).ToList();

                return listarUrls;

            }

            this.Contribuicao = this.OContribuicaoBL.carregar(idContribuicao) ?? new Contribuicao();

            this.carregarFiltros();

            this.carregarContribuicoes();

            var idsContribuicoes = this.listaContribuicoes.Select(x => x.id).ToArray();

            return this.OAssociadoContribuicaoBoletoBL.listar(0)
                       .Where(x => idsContribuicoes.Contains(x.idAssociadoContribuicao))
                       .OrderBy(x => x.idTituloReceitaPagamento)
                       .Select(x => x.boletoUrl).ToList();
            
        }
        
    }
    
}