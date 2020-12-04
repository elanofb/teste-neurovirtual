using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;
using DAL.Exceptions.Extensions;

namespace DAL.Repository.Base {
      
    public partial class DataContext : DbContext {
            
        //Iniciar conexão com banco de dados
        public DataContext() : base("STDefaultConnection") {
        }

        /// <summary>
        /// Executar validacoes nativas
        /// </summary>
        /// <returns></returns>
        public UtilRetorno validateAndSave() {

            try {

                int qtdeAfetados = base.SaveChanges();

                return UtilRetorno.newInstance(false, "", qtdeAfetados);

            } catch (DbEntityValidationException ex) {

                StringBuilder ErroDescricao = ex.carregarDescricaoErro();

                return UtilRetorno.newInstance(true, ErroDescricao.ToString(), 0);
                
            } catch (Exception ex) {
                
                UtilLog.saveError(ex, "");
                
                return UtilRetorno.newInstance(true, ex.Message + ": " + ex.InnerException, 0);
                
            }
        }

        /// <summary>
        /// Executar validacoes nativas
        /// </summary>
        /// <returns></returns>
        public async Task<UtilRetorno> validateAndSaveAsync() {

            try {

                int qtdeAfetados = await base.SaveChangesAsync();

                return UtilRetorno.newInstance(false, "", qtdeAfetados);

            } catch (UpdateException ex) {

                StringBuilder ErroDescricao = ex.carregarDescricaoErro();

                return UtilRetorno.newInstance(true, ErroDescricao.ToString(), 0);

            } catch (DbEntityValidationException ex) {

                StringBuilder ErroDescricao = ex.carregarDescricaoErro();

                return UtilRetorno.newInstance(true, ErroDescricao.ToString(), 0);
                
            } catch (Exception ex) {
                
                UtilLog.saveError(ex, "");
                
                return UtilRetorno.newInstance(true, ex.Message + ": " + ex.InnerException, 0);
                
            }
        }

        //Aqui faz o mapeamento das tabelas e a conexão com o banco de dados
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            this.mapperModuloErros(modelBuilder);

            this.mapperModuloLocalizacao(modelBuilder);

            this.mapperModuloArquivoUpload(modelBuilder);

            this.mapperModuloPermissao(modelBuilder);

            this.mapperModuloEmail(modelBuilder);

            this.mapperModuloConfiguracao(modelBuilder);

            this.mapperModuloEnderecos(modelBuilder);

            this.mapperModuloFuncionarios(modelBuilder);

            this.mapperModuloHistoricos(modelBuilder);

            this.mapperModuloNotificacoes(modelBuilder);

            this.mapperModuloPessoas(modelBuilder);

            this.mapperModuloTelefones(modelBuilder);

            this.mapperModuloAreasAtuacao(modelBuilder);

            this.mapperModuloCargos(modelBuilder);

            this.mapperModuloEscolaridades(modelBuilder);

            this.mapperModuloInstituicoes(modelBuilder);

            this.mapperModuloBeneficios(modelBuilder);

            this.mapperModuloContribuicoes(modelBuilder);

            this.mapperModuloAssociados(modelBuilder);

            this.mapperModuloRelacionamento(modelBuilder);

            this.mapperModuloMateriaisApoio(modelBuilder);

            this.mapperModuloContato(modelBuilder);

            this.mapperModuloDocumentos(modelBuilder);

            this.mapperModuloEmpresas(modelBuilder);

            this.mapperModuloPlanos(modelBuilder);

            this.mapperModuloBeneficiarios(modelBuilder);

            this.mapperModuloProdutos(modelBuilder);

            this.mapperModuloPedidos(modelBuilder);

            this.mapperModuloFrete(modelBuilder);

            this.mapperModuloFornecedores(modelBuilder);

            this.mapperModuloFinanceiro(modelBuilder);

            this.mapperModuloBancos(modelBuilder);

            this.mapperModuloContasBancarias(modelBuilder);
            
            this.mapperModuloDadosBancarios(modelBuilder);

            this.mapperModuloBoletosBancarios(modelBuilder);

            this.mapperModuloCompras(modelBuilder);

            this.mapperModuloPublicacoes(modelBuilder);

            this.mapperModuloMailings(modelBuilder);

            this.mapperModuloInstitucionais(modelBuilder);

            this.mapperModuloLinksUteis(modelBuilder);

            this.mapperModuloAssociadosContribuicoes(modelBuilder);

            this.mapperModuloCupomDesconto(modelBuilder);

            this.mapperModuloPopups(modelBuilder);

            this.mapperModuloRelatorios(modelBuilder);

            this.mapperModuloRelatoriosImediatos(modelBuilder);
            
            this.mapperModuloRelatoriosPedidos(modelBuilder);

            this.mapperModuloAssociadosCarteirinha(modelBuilder);

            this.mapperModuloRamosAtividade(modelBuilder);

            this.mapperModuloOrgaosClasses(modelBuilder);

            this.mapperModuloMeiosDivulgacao(modelBuilder);

            this.mapperModuloRelatoriosFinanceiros(modelBuilder);

            this.mapperModuloOrganizacoes(modelBuilder);

            this.mapperModuloUnidades(modelBuilder);

            this.mapperModuloLogsPermissao(modelBuilder);

            this.mapperModuloConfiguracoesAssociados(modelBuilder);

            this.mapperModuloConfiguracoesTextos(modelBuilder);

            this.mapperModuloConfiguracoesAreaAssociado(modelBuilder);

            this.mapperModuloConfiguracoesCarteirinha(modelBuilder);

            this.mapperModuloConfiguracoesRecibo(modelBuilder);

            this.mapperModuloConfiguracoesScripts(modelBuilder);

            this.mapperModuloConfiguracoesRedesSociais(modelBuilder);

            this.mapperModuloDocumentosDigitais(modelBuilder);

            this.mapperModuloDiretorias(modelBuilder);

            this.mapperModuloRelatoriosAssociados(modelBuilder);

            this.mapperModuloTipos(modelBuilder);

            this.mapperModuloOrganizacoesCobrancas(modelBuilder);

            this.mapperModuloFinanceiroLancamentos(modelBuilder);

            this.mapperModuloPaginas(modelBuilder);

            this.mapperModuloLogsAlteracoes(modelBuilder);

            this.mapperModuloProcessoAdmissoes(modelBuilder);

            this.mapperModuloPortais(modelBuilder);

            this.mapperModuloAtendimentos(modelBuilder);

            this.mapperModuloConfiguracoesEcommerce(modelBuilder);

            this.mapperModuloPedidosTemp(modelBuilder);

            this.mapperModuloConfiguracoesEtiquetas(modelBuilder);
            
            this.mapperModuloOrganizacaoConfiguracoes(modelBuilder);

            this.mapperModuloDocumentosFiscais(modelBuilder);
            
            this.mapperModuloUnidadesCertificados(modelBuilder);
            
            this.mapperModuloHotsites(modelBuilder);
            
            this.mapperModuloSegmentosAtuacao(modelBuilder);
            
            this.mapperModuloProfissoes(modelBuilder);
            
            this.mapperModuloRepresentantes(modelBuilder);
            
            this.mapperModuloTransacoes(modelBuilder);
            
            this.mapperModuloRedes(modelBuilder);
                
            base.OnModelCreating(modelBuilder);

        }
    }
}
