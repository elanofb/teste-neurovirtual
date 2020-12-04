using System;
using System.Web.Mvc;
using System.Text;
using DAL.Permissao;
using DAL.Permissao.Security;

namespace WEB.Helpers {

    public static class FormHelperCustom {

        //
        public static MvcHtmlString inputFileSimples(this HtmlHelper helper, string nomeCampo, string flagPreview = "true", bool flagMultiple = true, string flagRemove = "true") {
            StringBuilder htm = new StringBuilder();
            htm.Append("<div class=\"\">");
            htm.Append("  <input type=\"file\" class=\"input-file file width-300\" id='" + nomeCampo + "' name=" + nomeCampo + (flagMultiple ? " multiple=\"multiple\" " : "") + " data-show-upload=\"false\" data-show-preview=\"" + flagPreview + "\" data-show-remove=\"" + flagRemove + "\" data-preview-settings=\"[width:'auto', height:'100px']\" data-browse-label=\"Procurar ...\" data-remove-label=\"Remover\" data-show-caption=\"true\" data-preview-file-type=\"image\" />");
            htm.Append("</div>");

            return new MvcHtmlString(htm.ToString());
        }

        //
        public static MvcHtmlString exibirBotoesFormulario(this HtmlHelper helper, string urlVoltar, string flagSistema, string urlNovo = "0", bool flagExibirBotaoNovo = true) {

            StringBuilder htm = new StringBuilder();

            var idPerfilLogado = UtilNumber.toInt32(SecurityCookie.idPerfil);

            string btVoltar = "<a href=\"" + urlVoltar + "\" class=\" btn btn-md btn-default margin-left-20\"><i class=\"fa fa-arrow-left\"></i> Voltar</a>";
            string btNovo = "<a href=\"" + urlNovo + "\" class=\"btn btn-md btn-default bg-gray margin-left-20\"><i class=\"fa fa-plus-circle\"></i> Novo Registro</a>";

            if (flagSistema == "S" && idPerfilLogado != PerfilAcessoConst.DESENVOLVEDOR) {
                htm.Append("<a href=\"\" class=\" btn btn-md btn-danger\"><i class=\"fa fa-exclamation-triangle\"></i> Esse registro é protegido pelo sistema.</a>");
                htm.Append(btVoltar);
                if (flagExibirBotaoNovo) {
                    htm.Append(btNovo);
                }
                return new MvcHtmlString(htm.ToString());
            }

            string btSalvar = helper.botaoSalvar().ToString();

            htm.Append(btVoltar);
            if (flagExibirBotaoNovo) {
                htm.Append(btNovo);
            }
            htm.Append(btSalvar);

            return new MvcHtmlString(htm.ToString());
        }

        /// <summary>
        /// Helper para gerar HTML de botão padrao de submit dos formulários
        /// </summary>
	    public static MvcHtmlString botaoSalvar(this HtmlHelper helper) {

            string btSalvar = "<button type=\"submit\" name=\"enviar\" class=\"btn btn-md btn-primary margin-left-20 link-loading\"><i class=\"far fa-hdd\"></i> Salvar Dados</button>";

            return new MvcHtmlString(btSalvar);
        }

        /**
		 * 
		 */
        public static MvcHtmlString labelRequired(this HtmlHelper helper, string textoLabel, string extraClasses = "", bool flagAsterisco = true) {

            StringBuilder html = new StringBuilder();
            html.Append("<label class=\"" + extraClasses + "\">");
            if (flagAsterisco) {
                html.Append("<i class=\"fa fa-asterisk label-required\"></i> ");
            }
            html.Append(textoLabel);
            html.Append("</label>");

            return new MvcHtmlString(html.ToString());
        }
        
        /**
		 * 
		 */
        public static MvcHtmlString spanFieldForm(this HtmlHelper helper, string textoLabel, string extraClasses = "fs-12", bool flagAsterisco = true) {

            StringBuilder html = new StringBuilder();
            html.Append("<span class=\"" + extraClasses + "\">");
            if (flagAsterisco) {
                html.Append("<i class=\"far fa-asterisk text-red\"></i> ");
            }
            html.Append(textoLabel);
            html.Append("</span>");

            return new MvcHtmlString(html.ToString());
        }        

        public static MvcHtmlString labelInfo(this HtmlHelper helper, string textoLabel, string info, string extraClasses = "", bool flagAsterisco = false) {

            StringBuilder html = new StringBuilder();
            html.Append("<label class=\"width-100p " + extraClasses + "\">");

            if (flagAsterisco) {
                html.Append("<i class=\"fa fa-asterisk label-required\"></i> ");
            }

            html.Append(textoLabel);

            html.Append("<i class=\"fa fa-info-circle fs-12 pull-right margin-bottom-5\" data-toggle=\"tooltip\" title=\"");
            html.Append(info);
            html.Append("\"></i></label>");

            html.Append("</label>");

            return new MvcHtmlString(html.ToString());
        }
        
        //
        public static MvcHtmlString labelHelp(this HtmlHelper helper, string textoLabel, string textoAjuda, string extraClasses = "", bool flagAsterisco = false) {

            var htmlIconeHelp = $"<a href=\"javascript:void(0)\" class=\"fs-11\" data-toggle=\"tooltip\" data-title=\"{ textoAjuda }\"><i class=\"fa fa-question-circle\"></i></a>";

            textoLabel = String.Concat(textoLabel, " ", htmlIconeHelp);

            StringBuilder html = new StringBuilder();
            html.Append("<label class=\"" + extraClasses + "\">");

            if (flagAsterisco) {
                html.Append("<i class=\"fa fa-asterisk label-required\"></i> ");
            }

            html.Append(textoLabel);
            html.Append("</label>");

            return new MvcHtmlString(html.ToString());
            
        }

        //Legenda para campos obrigatorios
        public static MvcHtmlString legendaRequired(this HtmlHelper helper) {

            StringBuilder html = new StringBuilder();

            html.Append("<div class=\"label-required-message\">");
            html.Append("   <label>(&nbsp;<i class=\"fa fa-asterisk label-required\"></i>) Informações obrigatórias.</label>");
            html.Append("</div>");

            return new MvcHtmlString(html.ToString());
        }

        //Instrucao para inserir no formulario
        public static MvcHtmlString instrucao(this HtmlHelper helper, string instrucao) {

            StringBuilder html = new StringBuilder();
            html.Append("<em class=\"legenda-label fs-10\"><i class=\"fa fa-exclamation\"></i> " + instrucao + "</em>");

            return new MvcHtmlString(html.ToString());
        }

        // Informações do cadastro
        public static MvcHtmlString infoCadastro(this HtmlHelper helper, int? id, DateTime? dtCadastro, DateTime? dtAlteracao) {

            StringBuilder html = new StringBuilder();

            html.Append("<div class=\"row\">");

            html.Append("<div class=\"col-lg-6 col-md-4 col-sm-4 padding-top-10\">");

            html.Append(helper.legendaRequired());

            html.Append("</div>");

            if (id > 0) {

                html.Append("<div class=\"col-lg-6 col-md-8 col-sm-8\">");

                if (dtAlteracao == null) {
                    html.Append("<div class=\"col-sm-4\"></div>");
                }

                html.Append(fieldInfoCadastro("ID: ", id.ToString()));

                html.Append(fieldInfoCadastro("Cadastrado em: ", dtCadastro.exibirData(true)));

                if (dtAlteracao != null) {

                    html.Append(fieldInfoCadastro("Alterado em: ", dtAlteracao.exibirData(true)));

                }

                html.Append("</div>");

            }

            html.Append("</div>");

            return new MvcHtmlString(html.ToString());
        }

        private static MvcHtmlString fieldInfoCadastro(string label, string value) {

            StringBuilder html = new StringBuilder();

            html.Append("<div class=\"col-sm-4\">");

            html.Append($"<label class=\"no-margin\">{ label }</label>");

            html.Append($"<input type=\"text\" class=\"form-control input-sm text-center\" readonly=\"readonly\" value=\"{ value }\"/>");

            html.Append("</div>");

            return new MvcHtmlString(html.ToString());

        }

    }
}

