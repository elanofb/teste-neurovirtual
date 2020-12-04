using System;
using DAL.Associados;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using BLL.ConfiguracoesAssociados;
using BLL.ConfiguracoesCarteirinha;
using DAL.ConfiguracoesAssociados;
using DAL.ConfiguracoesCateirinha;
using DAL.Permissao.Security.Extensions;
using FastMember;
using Newtonsoft.Json;
using UTIL.Extensions;
using FluentValidation.Attributes;

namespace WEB.Areas.Associados.ViewModels {

    [Validator(typeof(AssociadoCadastroPFFormValidator))]
    public class AssociadoCadastroPFForm {
        
        //Atributos
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;
        private IConfiguracaoMembroConsultaBL _ConfiguracaoMembroConsultaBL;
        

        //Serviços
        private IConfiguracaoAssociadoCampoGrupoBL OConfiguracaoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();
        private IConfiguracaoAssociadoCampoBL OConfiguracaoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();
        private IConfiguracaoMembroConsultaBL OConfiguracaoMembroConsultaBL => _ConfiguracaoMembroConsultaBL = _ConfiguracaoMembroConsultaBL ?? new ConfiguracaoMembroConsultaBL();
        
        //Propriedades
        public List<ConfiguracaoAssociadoCampoGrupo> listaGrupos { get; set; }

        public List<ConfiguracaoAssociadoCampo> listaCampos { get; set; }

        public ConfiguracaoAssociadoPF ConfiguracaoAssociadoPF { get; set; }
        
        public ConfiguracaoMembro ConfiguracaoMembro { get; set; }

        public ConfiguracaoCarteirinha ConfiguracaoCarteirinha { get; set; }

        public Associado Associado { get; set; }
        
        public MembroSaldo Saldo { get; set; }
        
        public bool? flagRetornoAjax { get; set; }
        
        //Construtor
        public AssociadoCadastroPFForm() {

            this.listaGrupos = new List<ConfiguracaoAssociadoCampoGrupo>();

            this.listaCampos = new List<ConfiguracaoAssociadoCampo>();
        }
        
        //Atribuir valores padrão para quando estiverem em branco
        public void carregarConfiguracoes() {
            this.ConfiguracaoAssociadoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar();
            this.ConfiguracaoCarteirinha = ConfiguracaoCarteirinhaBL.getInstance.carregar();
        }
        
        //Atribuir valores padrão para quando estiverem em branco
        public void carregarConfiguracaoMembro() {

            this.ConfiguracaoMembro = OConfiguracaoMembroConsultaBL.carregarPorMembro(this.Associado.id) ?? new ConfiguracaoMembro();

        }
        
        /// <summary>
        /// Carregar os dados
        /// </summary>
	    public void carregaDados(bool flagTelaEdicao = false) {

            this.carregarGrupos();

            this.carregarCampos(flagTelaEdicao);
        }

        /// <summary>
        /// Montar query e carregar lista dos grupos
        /// </summary>
        protected void carregarGrupos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoGrupoBL.listarFromCacheOrDefault(idOrganizacao, true)
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PF && x.ativo != false);

            this.listaGrupos = query.OrderBy(x => x.ordemExibicao ?? 10000).ToList();

            if (!this.listaGrupos.Any()) {
                this.listaGrupos = this.OConfiguracaoGrupoBL.listarFromDefault()
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PF && x.ativo != false)
                                            .OrderBy(x => x.ordemExibicao ?? 10000).ToList();
            }
        }

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        protected void carregarCampos(bool flagTelaEdicao = false) {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao, true)
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PF && x.flagAreaAdm == true && x.ativo != false);

            if(flagTelaEdicao) {
                query = query.Where(x => x.flagEdicao == true);
            } else {
                query = query.Where(x => x.flagCadastro == true);
            }

            if (this.Associado.idTipoAssociado > 0) {
                query = query.Where(x => x.listaTipoAssociado.Any(y => y.idTipoAssociado == this.Associado.idTipoAssociado) || !x.listaTipoAssociado.Any());
            }

            this.listaCampos = query.OrderBy(x => x.ordemExibicao).ToList();

            if (!this.listaCampos.Any()) {

                this.listaCampos = this.OConfiguracaoCampoBL.listarFromDefault()
                                            .Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PF && x.flagAreaAdm == true && x.ativo != false)
                                            .OrderBy(x => x.ordemExibicao ?? 10000).ToList();
            }
        }
        
        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public AssociadoCadastroPFForm atribuirValoresFixos(AssociadoCadastroPFForm ViewModel) {

            var listaCamposFixos = this.listaCampos.Where(x => !x.valorFixo.isEmpty()).ToList();

            var objectAcessor = ObjectAccessor.Create(ViewModel);

            foreach (var OCampoFixo in listaCamposFixos) {
                objectAcessor.assignValueToProperty(OCampoFixo.name, OCampoFixo.valorFixo);
            }

            return (AssociadoCadastroPFForm)objectAcessor.Target;
        }
        
        /// <summary>
        /// Atribuir os valores fixos 
        /// </summary>
        public void carregarValorCampos(AssociadoCadastroPFForm ViewModel) {

            var listaCamposNaoFixos = this.listaCampos.Where(x => x.valorFixo.isEmpty()).ToList();

            var objectAcessor = ObjectAccessor.Create(ViewModel);
            
            foreach (var OCampo in listaCamposNaoFixos) {

                try {

                    OCampo.valorAtual = objectAcessor.getValueProperty(OCampo.name).stringOrEmpty();

                } catch (ArgumentOutOfRangeException ex) {

                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");

                } catch (Exception ex) {

                    UtilLog.saveError(ex, $"Erro ao atribuir valor {OCampo.name}");
                }

            }

        }
        
        /// <summary>
        /// Criar um arquivo json que servirá de padrao para quando não houver novas configuracoes
        /// </summary>
        public void criarArquivoPadrao(int idOrganizacao) {

            var lista = this.OConfiguracaoGrupoBL.listarFromCache(idOrganizacao, false)
                                                 .Select(x => new {
                                                     x.id,
                                                     x.idOrganizacao,
                                                     x.ativo,
                                                     x.cssBoxGrupo,
                                                     x.descricao,
                                                     x.idTipoCampoCadastro,
                                                     x.dtExclusao,
                                                     x.htmlAposBox,
                                                     x.ordemExibicao,
                                                     listaConfiguracaoAssociadoCampos = x.listaConfiguracaoAssociadoCampos.Where(y => y.dtExclusao == null).Select(cam => new {
                                                         cam.id,
                                                         cam.label,
                                                         cam.idAssociadoCampoGrupo,
                                                         cam.ativo,
                                                         cam.cssClassBox,
                                                         cam.cssClassCampo,
                                                         cam.flagAreaAdm,
                                                         cam.flagAreaAssociado,
                                                         cam.flagAssociadoPodeEditar,
                                                         cam.flagCadastro,
                                                         cam.flagDependente,
                                                         cam.flagEdicao,
                                                         cam.flagExibir,
                                                         cam.flagExibirOptionVazio,
                                                         cam.flagObrigatorio,
                                                         cam.flagValidado,
                                                         cam.idTipoCampoCadastro,
                                                         cam.htmlAfterBox,
                                                         cam.htmlAposCampo,
                                                         cam.idDOM,
                                                         cam.name,
                                                         cam.nameDescription,
                                                         cam.nameHelper,
                                                         cam.methodHelper,
                                                         cam.parametrosHelper,
                                                         cam.valorAtual,
                                                         cam.valorPadrao,
                                                         cam.valorFixo,
                                                         cam.maxlength,
                                                         cam.minlength,
                                                         cam.mask,
                                                         cam.textoInstrucoes,
                                                         cam.idTipoCampo,
                                                         cam.idFuncaoFiltro,
                                                         cam.ordemExibicao,
                                                         cam.dtExclusao,
                                                         AssociadoCampoGrupo = new { x.ordemExibicao },
                                                         TipoCampo = new{
                                                             cam.TipoCampo.id,
                                                             cam.TipoCampo.descricao,
                                                             cam.TipoCampo.flagOpcoes,
                                                             cam.TipoCampo.tipo
                                                         },
                                                         listaCampoPropriedades = cam.listaCampoPropriedades.Where(item => item.dtExclusao == null).Select(prop => new{
                                                             prop.id,
                                                             prop.idConfiguracaoAssociadoCampo,
                                                             prop.nome,
                                                             prop.valor
                                                         }).ToList(),
                                                         listaCampoOpcoes = cam.listaCampoOpcoes.Where(item => item.dtExclusao == null).Select(op => new{
                                                             op.id,
                                                             op.idConfiguracaoAssociadoCampo,
                                                             op.value,
                                                             op.texto
                                                         }).ToList()
                                                     }).ToList()

                                                 }).ToList();

            var stringJson = JsonConvert.SerializeObject(lista, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            string path = HttpContextFactory.Current.Server.MapPath("~/files/config/cadastro_associado_campos.json");

            UtilIO.writeFile(path, stringJson);

        }
    }

}