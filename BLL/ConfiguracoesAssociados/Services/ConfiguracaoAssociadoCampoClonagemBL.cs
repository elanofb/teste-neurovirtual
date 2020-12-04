using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Configuracoes.Services;
using BLL.Services;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesAssociados {

    public class ConfiguracaoAssociadoCampoClonagemBL : DefaultBL, IConfiguracaoAssociadoCampoClonagemBL {

        public bool clonarDefaultSistema(int idOrganizacaoInf, int idTipoCampoCadastro) {

            var idUsuarioLogado = User.id();
            if (idOrganizacao > 0) {
                idOrganizacaoInf = idOrganizacao;
            }

            if (idOrganizacaoInf == 0) {
                return false;
            }

            var listaGruposDefault = ConfiguracaoJsonBL.getInstance.carregar<List<ConfiguracaoAssociadoCampoGrupo>>(ConfiguracaoJsonBL.CADASTRO_ASSOCIADO_CAMPOS);
            listaGruposDefault = listaGruposDefault.Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro).ToList();


            var listaCamposDefault = listaGruposDefault.SelectMany(x => x.listaConfiguracaoAssociadoCampos).ToList();
            listaCamposDefault = listaCamposDefault.Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro).ToList();

            var listaPropriedadesDefault = listaCamposDefault.SelectMany(x => x.listaCampoPropriedades).ToList();
            var listaOpcoesDefault = listaCamposDefault.SelectMany(x => x.listaCampoOpcoes).ToList();

            var listaGrupos = new List<ConfiguracaoAssociadoCampoGrupo>();
            foreach (var OGrupoDefault in listaGruposDefault) {

                var listaCamposDefaultGrupo = listaCamposDefault.Where(x => x.idAssociadoCampoGrupo == OGrupoDefault.id).ToList();

                var OGrupo = new ConfiguracaoAssociadoCampoGrupo();
                OGrupo.idOrganizacao = idOrganizacaoInf;
                OGrupo.idUsuarioCadastro = idUsuarioLogado;
                OGrupo.idTipoCampoCadastro = OGrupoDefault.idTipoCampoCadastro;
                OGrupo.descricao = OGrupoDefault.descricao;
                OGrupo.cssBoxGrupo = OGrupoDefault.cssBoxGrupo;
                OGrupo.htmlAposBox = OGrupoDefault.htmlAposBox;
                OGrupo.ativo = OGrupoDefault.ativo;
                OGrupo.ordemExibicao = OGrupoDefault.ordemExibicao;

                var listaCampos = new List<ConfiguracaoAssociadoCampo>();
                foreach (var OCampoDefault in listaCamposDefaultGrupo) {
                    
                    var listaPropriedadesDefaultCampo = listaPropriedadesDefault.Where(x => x.idConfiguracaoAssociadoCampo == OCampoDefault.id).ToList();
                    var listaOpcoesDefaultCampo = listaOpcoesDefault.Where(x => x.idConfiguracaoAssociadoCampo == OCampoDefault.id).ToList();

                    var OCampo = new ConfiguracaoAssociadoCampo();
                    OCampo.idOrganizacao = idOrganizacaoInf;
                    OCampo.idUsuarioCadastro = idUsuarioLogado;
                    OCampo.idTipoCampoCadastro = OCampoDefault.idTipoCampoCadastro;
                    OCampo.label = OCampoDefault.label;
                    OCampo.idAssociadoCampoGrupo = OCampoDefault.idAssociadoCampoGrupo;
                    OCampo.idTipoCampo = OCampoDefault.idTipoCampo;
                    OCampo.idFuncaoFiltro = OCampoDefault.idFuncaoFiltro;
                    OCampo.nameHelper = OCampoDefault.nameHelper;
                    OCampo.methodHelper = OCampoDefault.methodHelper;
                    OCampo.parametrosHelper = OCampoDefault.parametrosHelper;
                    OCampo.name = OCampoDefault.name;
                    OCampo.idDOM = OCampoDefault.idDOM;
                    OCampo.flagAreaAssociado = OCampoDefault.flagAreaAssociado;
                    OCampo.flagAreaAdm = OCampoDefault.flagAreaAdm;
                    OCampo.flagCadastro = OCampoDefault.flagCadastro;
                    OCampo.flagEdicao = OCampoDefault.flagEdicao;
                    OCampo.flagAssociadoPodeEditar = OCampoDefault.flagAssociadoPodeEditar;
                    OCampo.flagExibir = OCampoDefault.flagExibir;
                    OCampo.flagObrigatorio = OCampoDefault.flagObrigatorio;
                    OCampo.flagExibirOptionVazio = OCampoDefault.flagExibirOptionVazio;
                    OCampo.valorFixo = OCampoDefault.valorFixo;
                    OCampo.valorPadrao = OCampoDefault.valorPadrao;
                    OCampo.minlength = OCampoDefault.minlength;
                    OCampo.maxlength = OCampoDefault.maxlength;
                    OCampo.mask = OCampoDefault.mask;
                    OCampo.cssClassBox = OCampoDefault.cssClassBox;
                    OCampo.cssClassCampo = OCampoDefault.cssClassCampo;
                    OCampo.textoInstrucoes = OCampoDefault.textoInstrucoes;
                    OCampo.mensagemErro = OCampoDefault.mensagemErro;
                    OCampo.htmlAfterBox = OCampoDefault.htmlAfterBox;
                    OCampo.ordemExibicao = OCampoDefault.ordemExibicao;
                    OCampo.ativo = OCampoDefault.ativo;

                    var listaPropriedades = new List<ConfiguracaoAssociadoCampoPropriedade>();
                    foreach (var OPropriedadesDefault in listaPropriedadesDefaultCampo) {

                        var OPropriedade = new ConfiguracaoAssociadoCampoPropriedade();
                        OPropriedade.idUsuarioCadastro = idUsuarioLogado;
                        OPropriedade.nome = OPropriedadesDefault.nome;
                        OPropriedade.valor = OPropriedadesDefault.valor;
                        OPropriedade.dtCadastro = DateTime.Now;

                        listaPropriedades.Add(OPropriedade);
                    }

                    var listaOpcoes = new List<ConfiguracaoAssociadoCampoOpcao>();
                    foreach (var OOpcoesDefault in listaOpcoesDefaultCampo) {
                        var OOpcao = new ConfiguracaoAssociadoCampoOpcao();
                        OOpcao.idUsuarioCadastro = idUsuarioLogado;
                        OOpcao.texto = OOpcoesDefault.texto;
                        OOpcao.value = OOpcoesDefault.value;
                        OOpcao.dtCadastro = DateTime.Now;

                        listaOpcoes.Add(OOpcao);
                    }

                    OCampo.listaCampoPropriedades = listaPropriedades;
                    OCampo.listaCampoOpcoes = listaOpcoes;

                    listaCampos.Add(OCampo);
                }

                OGrupo.listaConfiguracaoAssociadoCampos = listaCampos;
                listaGrupos.Add(OGrupo);
            }

            using (var ctx = db) {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;

                ctx.ConfiguracaoAssociadoCampoGrupo.Where(x => x.idOrganizacao == idOrganizacaoInf && x.idTipoCampoCadastro == idTipoCampoCadastro)
                    .Update(x => new ConfiguracaoAssociadoCampoGrupo { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado });

                ctx.ConfiguracaoAssociadoCampo.Where(x => x.idOrganizacao == idOrganizacaoInf && x.idTipoCampoCadastro == idTipoCampoCadastro)
                    .Update(x => new ConfiguracaoAssociadoCampo { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado });

                ctx.ConfiguracaoAssociadoCampoGrupo.AddRange(listaGrupos);
                ctx.SaveChanges();
            }

            return true;
        }
    }
}