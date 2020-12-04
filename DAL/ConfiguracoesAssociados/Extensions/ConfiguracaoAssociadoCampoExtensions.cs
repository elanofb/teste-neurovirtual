using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using DAL.Configuracoes.Const;

namespace DAL.ConfiguracoesAssociados {

    //
    public static class ConfiguracaoAssociadoCampoExtensions {


        /// <summary>
        /// Validar um campo dinamico de acordo com o valor informado
        /// </summary>
        public static List<ConfiguracaoAssociadoCampo> bind(this List<ConfiguracaoAssociadoCampo> listaCampos, NameValueCollection dados, HttpFileCollectionBase files = null) {

            foreach (var OCampo in listaCampos) {

                if (!OCampo.valorFixo.isEmpty()) {

                    OCampo.valorAtual = OCampo.valorFixo;

                    OCampo.flagValidado = true;

                    continue;
                }

                if (files != null && OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.FILE) {

                    var fileEnviado = files[OCampo.name];

                    OCampo.valorAtual = fileEnviado?.FileName;

                } else {

                    string valorEnviado = dados[OCampo.name];

                    OCampo.valorAtual = valorEnviado;

                }

                OCampo.filtrar();

                OCampo.validar();
            }

            return listaCampos;
        }

        /// <summary>
        /// Validar um campo dinamico de acordo com o valor informado
        /// </summary>
        public static ConfiguracaoAssociadoCampo filtrar(this ConfiguracaoAssociadoCampo OCampo) {

            string info = OCampo.valorAtual;

            if (OCampo.idFuncaoFiltro.toShort() == 0) {

                return OCampo;

            }

            if (OCampo.idFuncaoFiltro == FuncaoFiltroConst.SOMENTE_NUMERO) {

                OCampo.valorAtual = info.onlyNumber();

                return OCampo;
            }

            return OCampo;
        }

        /// <summary>
        /// Validar um campo dinâmico de acordo com o valor informado
        /// </summary>
        public static ConfiguracaoAssociadoCampo validar(this ConfiguracaoAssociadoCampo OCampo) {

            string info = OCampo.valorAtual;

            OCampo.flagValidado = true;

            if (OCampo.flagObrigatorio != true && info.isEmpty()) {

                return OCampo;

            }

            if (OCampo.flagObrigatorio == true && info.isEmpty()) {

                OCampo.adicionarErro($"O campo {OCampo.label} é obrigatório");

                return OCampo;
            }

            if (OCampo.maxlength.toInt() > 0 && OCampo.maxlength.toInt() < info.stringOrEmpty().Length) {

                OCampo.adicionarErro($"O campo {OCampo.label} deve ter no máximo {OCampo.maxlength} caracteres");

                return OCampo;
            }

            return OCampo;
        }

        /// <summary>
        /// Adicionar mensagem de erro e incluir atributos referentes à validação do campo
        /// </summary>
        public static ConfiguracaoAssociadoCampo adicionarErro(this ConfiguracaoAssociadoCampo OCampo, string mensagem) {

            OCampo.cssClassCampo = string.Concat(OCampo.cssClassCampo, " input-validation-error");

            OCampo.mensagemErro = mensagem;

            OCampo.htmlAposCampo = string.Concat(OCampo.htmlAposCampo, $"<span class='field-validation-error' data-valmsg-for='{OCampo.name}'>{mensagem}</span>");

            OCampo.flagValidado = false;

            return OCampo;
        }
    }
}