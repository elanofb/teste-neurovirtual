using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using DAL.Configuracoes.Const;
using DAL.ConfiguracoesAssociados;
using WEB.Helpers;

namespace WEB.Areas.ConfiguracoesAssociados.Helpers {

    public static class ConfiguracaoAssociadoCampoExtensions {

        // 
        public static MvcHtmlString buildField(this HtmlHelper html, ConfiguracaoAssociadoCampo OCampo, bool flagConfiguracao = false) {

            MvcHtmlString campoHTML = new MvcHtmlString("");

            try {
                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_FIXO) {

                    campoHTML = dropdown(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_DINAMICO) {

                    campoHTML = dropdown(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.TEXTO_SIMPLES) {

                    campoHTML = inputText(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.EMAIL) {

                    campoHTML = inputText(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.PASSWORD) {

                    campoHTML = inputText(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.NUMERO_INTEIRO) {

                    campoHTML = inputText(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.TEXTO_MULTI_LINHA) {

                    campoHTML = textarea(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.FILE) {

                    campoHTML = inputFile(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.HIDDEN && !flagConfiguracao) {

                    campoHTML = inputHidden(html, OCampo);

                    return campoHTML;
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.HIDDEN && flagConfiguracao) {

                    campoHTML = inputText(html, OCampo);

                    return campoHTML;
                }

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao criar o campo {OCampo.name}");

                string msgErro = UtilConfig.emProducao() ? "Campo não exibido" : $"<span>Erro ao criar o campo {OCampo.name}</span>";

                campoHTML = new MvcHtmlString(msgErro);
            }
            return campoHTML;
        }


        //Input text
        private static MvcHtmlString inputText(HtmlHelper html, ConfiguracaoAssociadoCampo OCampo) {

            var attrs = new Dictionary<string, object>();

            attrs.Add("class", OCampo.cssClassCampo);

            attrs.Add("id", OCampo.idDOM);

            string tipoCampo = OCampo.TipoCampo.tipo;

            if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.HIDDEN) {
                tipoCampo = "text";
            }

            attrs.Add("type", tipoCampo);

            attrs.Add("alt", OCampo.mask);

            if (OCampo.maxlength > 0) {
                attrs.Add("maxlength", OCampo.maxlength);
            }

            if (!string.IsNullOrEmpty(OCampo.mensagemErro)) {
                attrs.Add("data-error-message", OCampo.mensagemErro);
            }
            
            if (OCampo.flagReadOnly == true) {
                attrs.Add("readonly", "readonly");
                OCampo.name = String.Concat(OCampo.name, "Display");
            }

            if (OCampo.flagReadOnly != true){
                
                var listaPropriedades = OCampo.listaCampoPropriedades.Where(x => x.dtExclusao == null).ToList();

                foreach (var Prop in listaPropriedades) {
                    attrs.Add(Prop.nome, Prop.valor.decodeString());
                }
                
            }

            

            object postedValue = html.ViewContext.HttpContext.Request[OCampo.name];

            string valor = postedValue?.ToString() ?? (OCampo.valorAtual.isEmpty()? OCampo.valorPadrao : OCampo.valorAtual);

            MvcHtmlString inputRetorno = html.TextBox(OCampo.name, valor, attrs);

            return new MvcHtmlString(inputRetorno.ToHtmlString().decodeString());
        }


        //Input text
        private static MvcHtmlString textarea(HtmlHelper html, ConfiguracaoAssociadoCampo OCampo) {

            var attrs = new Dictionary<string, object>();

            attrs.Add("class", OCampo.cssClassCampo);

            attrs.Add("id", OCampo.idDOM);

            attrs.Add("alt", OCampo.mask);

            if (OCampo.maxlength > 0) {
                attrs.Add("maxlength", OCampo.maxlength);
            }

            if (!string.IsNullOrEmpty(OCampo.mensagemErro)) {
                attrs.Add("data-error-message", OCampo.mensagemErro);
            }
            
            if (OCampo.flagReadOnly == true) {
                attrs.Add("readonly", "readonly");
                OCampo.name = String.Concat(OCampo.name, "Display");
            }

            if (OCampo.flagReadOnly != true){
                
                var listaPropriedades = OCampo.listaCampoPropriedades.Where(x => x.dtExclusao == null).ToList();

                foreach (var Prop in listaPropriedades) {
                    attrs.Add(Prop.nome, Prop.valor.decodeString());
                }    
            }
            
            object postedValue = html.ViewContext.HttpContext.Request[OCampo.name];

            string valor = postedValue?.ToString() ?? (OCampo.valorAtual.isEmpty()? OCampo.valorPadrao : OCampo.valorAtual);

            MvcHtmlString inputRetorno = html.TextArea(OCampo.name, valor, attrs);

            return new MvcHtmlString(inputRetorno.ToHtmlString().decodeString());
        }

        //Input text
        private static MvcHtmlString inputHidden(HtmlHelper html, ConfiguracaoAssociadoCampo OCampo) {

            var attrs = new Dictionary<string, object>();

            attrs.Add("id", OCampo.idDOM);

            attrs.Add("type", "hidden");

            var listaPropriedades = OCampo.listaCampoPropriedades.Where(x => x.dtExclusao == null).ToList();

            foreach (var Prop in listaPropriedades) {
                attrs.Add(Prop.nome, Prop.valor.decodeString());
            }

            MvcHtmlString inputRetorno = html.Hidden(OCampo.name, OCampo.valorPadrao ?? OCampo.valorAtual, attrs);

            return new MvcHtmlString(inputRetorno.ToHtmlString().decodeString());;
        }

        // Dropdownlist 
        private static MvcHtmlString dropdown(HtmlHelper html, ConfiguracaoAssociadoCampo OCampo) {

            var attrs = new Dictionary<string, object>(); // {@class = OCampo.cssClassField, id = OCampo.idDOM};

            attrs.Add("class", OCampo.cssClassCampo + (OCampo.flagMultiSelect == true ? " input-multiselect" : ""));

            attrs.Add("id", OCampo.idDOM);
            
            string postedValue = html.ViewContext.HttpContext.Request[OCampo.name] ?? "";

            string valorPadrao = postedValue.isEmpty()? OCampo.valorPadrao: postedValue;

            string valorAtual = OCampo.valorAtual.isEmpty() ? valorPadrao : OCampo.valorAtual;

            if (!valorAtual.isEmpty()) {
                attrs.Add("data-selected", valorAtual);
            }
            
            if (OCampo.flagReadOnly == true) {
                attrs.Add("disabled", "disabled");
                OCampo.name = String.Concat(OCampo.name, "Display");
            }
            
            if (OCampo.flagReadOnly != true){
            
                var listaPropriedades = OCampo.listaCampoPropriedades.Where(x => x.dtExclusao == null).ToList();

                foreach (var Prop in listaPropriedades) {
                    attrs.Add(Prop.nome, Prop.valor.decodeString());
                }
                
            }
            
            SelectList listaItens = new SelectList(new object[] { "" });
            MultiSelectList listaMultiItens = new MultiSelectList(new object[] { "" });

            MvcHtmlString ddRetorno;

            if (OCampo.flagMultiSelect == true) {
                
                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_DINAMICO && !OCampo.nameHelper.isEmpty()) {
                    listaMultiItens = SelectListHelper.multiSelectListDinamico(OCampo.nameHelper, OCampo.methodHelper, OCampo.parametrosHelper, valorAtual, html.ViewContext.HttpContext.Request.Form);
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_FIXO) {

                    var listaOpcoes = OCampo.listaCampoOpcoes.Where(x => x.dtExclusao == null).ToList();

                    if (listaOpcoes.Any()) {
                        listaMultiItens = new MultiSelectList(listaOpcoes, "value", "texto", valorAtual);
                    }
                }

                ddRetorno = html.ListBox(OCampo.name, listaMultiItens, attrs);
            } else {
                

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_DINAMICO && !OCampo.nameHelper.isEmpty()) {
                    listaItens = SelectListHelper.selectListDinamico(OCampo.nameHelper, OCampo.methodHelper, OCampo.parametrosHelper, valorAtual, html.ViewContext.HttpContext.Request.Form);
                }

                if (OCampo.idTipoCampo == ConfiguracaoTipoCampoConst.SELECT_FIXO) {

                    var listaOpcoes = OCampo.listaCampoOpcoes.Where(x => x.dtExclusao == null).ToList();

                    if (listaOpcoes.Any()) {
                        listaItens = new SelectList(listaOpcoes, "value", "texto", valorAtual);
                    }
                } 

                if (OCampo.flagExibirOptionVazio == true) {
                    ddRetorno = html.DropDownList(OCampo.name, listaItens, "...", attrs);
                } else {
                    ddRetorno = html.DropDownList(OCampo.name, listaItens, attrs);
                }
            }

            return new MvcHtmlString(ddRetorno.ToHtmlString().decodeString());
        }

        //Input text
        private static MvcHtmlString inputFile(HtmlHelper html, ConfiguracaoAssociadoCampo OCampo) {

            var attrs = new Dictionary<string, string>();

             var input = new TagBuilder("input");

            input.Attributes.Add("type", "file");

            input.Attributes.Add("name", OCampo.name);

            input.Attributes.Add("class", OCampo.cssClassCampo);

            input.Attributes.Add("id", OCampo.idDOM);

            input.Attributes.Add("alt", OCampo.mask);

            input.Attributes.Add("data-show-upload", "false");

            input.Attributes.Add("data-preview", "false");

            input.Attributes.Add("data-browse-label", "Procurar...");

            if (!string.IsNullOrEmpty(OCampo.mensagemErro)) {

                input.Attributes.Add("data-error-message", OCampo.mensagemErro);

            }

            var listaPropriedades = OCampo.listaCampoPropriedades.Where(x => x.dtExclusao == null).ToList();

            foreach (var Prop in listaPropriedades) {

                input.Attributes.Add(Prop.nome, Prop.valor.decodeString());

            }
           

            MvcHtmlString inputRetorno = new MvcHtmlString(input.ToString());

            return inputRetorno;
        }
    }
}
