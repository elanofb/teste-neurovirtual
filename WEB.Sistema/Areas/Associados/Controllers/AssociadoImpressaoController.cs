using System.Web.Mvc;
using BLL.Associados;
using BLL.Arquivos;
using DAL.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using BLL.ConfiguracoesCarteirinha;
using BLL.DocumentosDigitais;
using DAL.Arquivos.Extensions;
using DAL.Associados;
using DAL.ConfiguracoesCateirinha;
using DAL.DocumentosDigitais;
using DAL.Pessoas;
using MvcFlashMessages;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoImpressaoController:Controller {

        //Atributos
        private IAssociadoBL _AssociadoBL;
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;
        private IDocumentoDigitalBL _DocumentoDigitalBL;
        private IConfiguracaoTipoAssociadoBL _ConfiguracaoTipoAssociadoBL;

        //Propriedades
        private IAssociadoBL OAssociadoBL => this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();
        private IDocumentoDigitalBL ODocumentoDigitalBL => this._DocumentoDigitalBL = this._DocumentoDigitalBL ?? new DocumentoDigitalBL();
        private IConfiguracaoTipoAssociadoBL OConfiguracaoTipoAssociadoBL => this._ConfiguracaoTipoAssociadoBL  = this._ConfiguracaoTipoAssociadoBL  ?? new ConfiguracaoTipoAssociadoBL();

        //Visualizar admissao
        [AllowAnonymous, ActionName("visualizar-admissao")]
        public ActionResult visualizarAdmissao(string id) {

            var paramId = UtilString.decodeURL(id);
            var decryptId = Convert.ToInt32(UtilCrypt.toBase64Decode(paramId));

            var OAssociado = this.OAssociadoBL.carregar(decryptId);

            if(OAssociado == null) {
                return HttpNotFound();
            }

            return HttpNotFound();
        }

        //Imprimir admissao
        [ActionName("imprimir-admissao")]
        public ActionResult imprimirAdmissao(int id) {
            
            var OAssociado = this.OAssociadoBL.carregar(id);

            if(OAssociado == null) {
                return HttpNotFound();
            }


            return HttpNotFound();
        }

        //Imprimir Carteira
        [ActionName("imprimir-carteirinha")]
        public ActionResult imprimirCarteirinha(int id) {

            var OAssociado = this.OAssociadoBL.carregar(id);

            if(OAssociado == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível emitir a carteirinha para este associado.");
                return View();
            }

            if (!OAssociado.dtAdmissao.HasValue) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível imprimir carteirinha para associados que não foram admitidos.");

                return View();
            }

            if (OAssociado.ativo == "N") {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não é possível imprimir carteirinha para associados desativados.");

                return View();
            }


            var OFoto = this.OArquivoUploadFotoBL.carregarPrincipal(id, EntityTypes.FOTO_ASSOCIADO);

            var ConfiguracaoCarteirinha = ConfiguracaoCarteirinhaBL.getInstance.carregar(OAssociado.idOrganizacao) ?? new ConfiguracaoCarteirinha();

            var htmlCarteirinha = ConfiguracaoCarteirinha.htmlCarteirinha;
            var qtdeMesesValidade = ConfiguracaoCarteirinha.qtdeMesesValidade ?? 1;
            var dtValidadeFixa = ConfiguracaoCarteirinha.dtValidadeFixa.exibirData();

            var OConfiguracaoTipoAssociado = OConfiguracaoTipoAssociadoBL.carregar(OAssociado.idTipoAssociado, OAssociado.idOrganizacao) ?? new ConfiguracaoTipoAssociado();

            if (!OConfiguracaoTipoAssociado.htmlCarteirinha.isEmpty()){
                htmlCarteirinha = OConfiguracaoTipoAssociado.htmlCarteirinha;
                qtdeMesesValidade = OConfiguracaoTipoAssociado.qtdeMesesValidade ?? 1;
                dtValidadeFixa = OConfiguracaoTipoAssociado.dtValidadeFixa.exibirData();

            }

            var sexo = "";
            if (!OAssociado.Pessoa.flagSexo.isEmpty()){
                sexo = (OAssociado.Pessoa.flagSexo == "M") ? "Masculino" : "Feminino";                
            } 
            
            if (OFoto != null)
            {
                OFoto.path = OFoto.linkImagem();
                htmlCarteirinha = htmlCarteirinha.Replace("##PATH_IMAGEM##", OFoto.path);                
            }            

            htmlCarteirinha = htmlCarteirinha.Replace("##ID_ASSOCIADO##", OAssociado.nroAssociado > 0? OAssociado.nroAssociado.ToString(): OAssociado.id.ToString());

            htmlCarteirinha = htmlCarteirinha.Replace("##DT_ADMISSAO##", OAssociado.dtAdmissao.exibirData());

            htmlCarteirinha = htmlCarteirinha.Replace("##DT_NASCIMENTO##", OAssociado.Pessoa.dtNascimento.exibirData());

            htmlCarteirinha = htmlCarteirinha.Replace("##VALIDADE##", String.Concat(qtdeMesesValidade, " meses"));

            htmlCarteirinha = htmlCarteirinha.Replace("##VALIDADE_FIXA##", dtValidadeFixa);

            htmlCarteirinha = htmlCarteirinha.Replace("##DADOS_CUSTOMIZADO_01##", OAssociado.dadoCustomizado01);

            htmlCarteirinha = htmlCarteirinha.Replace("##DADOS_CUSTOMIZADO_02##", OAssociado.dadoCustomizado02);

            htmlCarteirinha = htmlCarteirinha.Replace("##DADOS_CUSTOMIZADO_03##", OAssociado.dadoCustomizado03);

            htmlCarteirinha = htmlCarteirinha.Replace("##CPF##", UtilString.formatCPFCNPJ(OAssociado.Pessoa.nroDocumento));

            htmlCarteirinha = htmlCarteirinha.Replace("##RG##", OAssociado.Pessoa.rg);

            htmlCarteirinha = htmlCarteirinha.Replace("##ORGAO_EMISSOR_RG##", OAssociado.Pessoa.orgaoEmissorRg);

            htmlCarteirinha = htmlCarteirinha.Replace("##NOME##", OAssociado.Pessoa.nome);

            htmlCarteirinha = htmlCarteirinha.Replace("##SEXO_SIGLA##", OAssociado.Pessoa.flagSexo);

            htmlCarteirinha = htmlCarteirinha.Replace("##SEXO##", sexo);

            htmlCarteirinha = htmlCarteirinha.Replace("##NOME_PAI##", OAssociado.Pessoa.nomePai);

            htmlCarteirinha = htmlCarteirinha.Replace("##NOME_MAE##", OAssociado.Pessoa.nomeMae);
            
            htmlCarteirinha = htmlCarteirinha.Replace("##NRO_REGISTRO_ORGAO_CLASSE##", OAssociado.Pessoa.nroRegistroOrgaoClasse);

            if (htmlCarteirinha.Contains("##INICIO_QRCODE##"))
            {
                var match = Regex.Match(htmlCarteirinha, "##INICIO_QRCODE##(.*)##FIM_QRCODE##");
                var URL = match.Groups[1].Value;
                var URL_CODE = URL.Replace("##ID_ASSOCIADO_CRYPT##", UtilCrypt.toBase64Encode(OAssociado.id));

                htmlCarteirinha = ""; //"##INICIO_QRCODE##"+URL+"##FIM_QRCODE##", ZxingExtensions.gerarQrCode(URL_CODE, 100, 100).ToString());                
            }
                        
            ViewBag.htmlCarteirinha = htmlCarteirinha;

            return View();
        }
        
        #region NONACTIONS

        [NonAction]
        private DocumentoDigital getLayoutFichaCadastral(string flagTipoPessoa) {

            var idTipoDocumentoDigital = TipoDocumentoDigitalConst.FICHA_CADASTRAL;
            var ODocumentoDigital = ODocumentoDigitalBL.listar("", idTipoDocumentoDigital, flagTipoPessoa, true)
                                                       .OrderByDescending(x => x.id).FirstOrDefault();

            if (ODocumentoDigital == null || ODocumentoDigital.htmlCorpo.isEmpty()) {
                ODocumentoDigital = new DocumentoDigital();
                ODocumentoDigital.htmlCorpo = this.carregarFichaPadrao(flagTipoPessoa);
            }

            return ODocumentoDigital;
        }

        [NonAction]
        private string carregarFichaPadrao(string flagTipoPessoa) {

            if (flagTipoPessoa == "F") {
                return this.fichaPadraoAssociadoPF();
            }

            if (flagTipoPessoa == "J") {
                return this.fichaPadraoAssociadoPJ();
            }

            return "";

        }

        [NonAction]
        private string fichaPadraoAssociadoPF() {

            var htmlCorpo = "" 
                        +"<style>"
                        +"	.center{text-align:center;}"
                        +"	tbody tr td {"
                        +"		font: 8pt Arial; line-height:1.5;"
                        +"	}"
                        +"	tbody tr td b {"
                        +"		text-transform: uppercase;"
                        +"	}"
                        +"	tbody tr td:not(:last-child) {"
                        +"		border-right: #000000 1px solid;"
                        +"	}"
                        +"	tbody tr:not(:last-child) {"
                        +"		border-bottom: #000000 1px solid;"
                        +"	}"
                        +"	.noborder {"
                        +"		border: 0px !important;"
                        +"	}"
                        +"	.nopadding {"
                        +"		padding: 0px;"
                        +"	}"
                        +"	#CSS_CUSTOMIZADO#"
                        +"</style>"
                        +""
                        +"<table width=\"957\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\" style=\"border: #000000 1px solid; margin: 0 auto;\">"
                        +"	<tbody>"
                        +"		<tr>"
                        +"			<td valign=\"middle\" class=\"noborder\" colspan=\"2\">"
                        +"				<img src=\"#LINK_LOGO#\" alt=\"#NOME_ASSOCIACAO#\">"
                        +"			</td>"
                        +"			<td colspan=\"2\" align=\"right\">"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody>"
                        +"		<tr bgcolor=\"#EEEEEE\">"
                        +"			<td>"
                        +"				<b>Código: </b>#CODIGO# (Nº #NRO_ASSOCIADO#)"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Data de Cadastro: </b>#DT_CADASTRO#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Data da Admissão: </b>#DT_ADMISSAO#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Tipo de Associado: </b>#TIPO_ASSOCIADO#"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody>"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>DADOS CADASTRAIS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody>"
                        +"		<tr class=\"#INFO_PF#\">"
                        +"			<td colspan=\"3\">"
                        +"				<b>Nome Completo: </b>#NOME#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Data de Nascimento: </b>#DT_NASCIMENTO#"
                        +"			</td>"
                        +"		</tr>"
                        +"		<tr class=\"#INFO_PF#\">"
                        +"			<td>"
                        +"				<b>Sexo: </b>#SEXO#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Nacionalidade: </b>#NACIONALIDADE#"
                        +"			</td>"
                        +"			<td colspan=\"2\">"
                        +"				<b>Natural de: </b>#NATURALIDADE#"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_ENDERECOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>ENDEREÇOS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_ENDERECOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>Endereço Completo</b></td>"
                        +"                            <td><b>Zona</b></td>"
                        +"							<td><b>Bairro</b></td>"
                        +"                            <td><b>CEP</b></td>"
                        +"                            <td><b>Cidade</b></td>"
                        +"                            <td><b>Estado</b></td>"
                        +"                        </tr>"
                        +"						#LISTA_ENDERECOS#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +""
                        +"	<tbody class=\"#INFO_TELEFONES#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>Telefones</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_TELEFONES#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>Tipo</b></td>"
                        +"                            <td><b>Operadora</b></td>"
                        +"							<td><b>Numero</b></td>"
                        +"                        </tr>"
                        +"						#LISTA_TELEFONES#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"		<tbody class=\"#INFO_EMAILS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>Emails</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_EMAILS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>Tipo</b></td>"
                        +"                            <td><b>Email</b></td>"
                        +"                        </tr>"
                        +"						#LISTA_EMAILS#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody>"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>DOCUMENTOS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody>		"
                        +"		<tr class=\"#INFO_PF#\">"
                        +"			<td colspan=\"2\">"
                        +"				<b>CPF: </b>#NRO_DOCUMENTO#"
                        +"			</td>"
                        +"			"
                        +"			<td colspan=\"2\">"
                        +"				<b>RG: </b>#RG_IE#"
                        +"			</td>"
                        +"		</tr>"
                        +"		"
                        +"	</tbody>"
                        +"	<tbody>"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>DADOS PROFISSIONAIS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_PF#\">"
                        +"		<tr>"
                        +"			<td colspan=\"2\">"
                        +"				<b>Universidade: </b>#UNIVERSIDADE#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>MATRÍCULA: </b>#NRO_MATRICULA#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>ANO DE FORMAÇÃO: </b>#ANO_FORMACAO#"
                        +"			</td>"
                        +"		</tr>"
                        +"		"
                        +"		<tr>"
                        +"			<td>"
                        +"				<b>PROFISSÃO: </b>#PROFISSAO#"
                        +"			</td>"
                        +"			<td colspan=\"2\">"
                        +"				<b>Órgão de Classe: </b>#ORGAO_CLASSE#"
                        +"			</td>"
                        +"			<td>"
                        +"				<b>Nº Registro: </b>#NRO_REGISTRO_ORGAO_CLASSE#"
                        +"			</td>"
                        +"		</tr>"
                        +"		<tr>"
                        +"			<td colspan=\"4\">"
                        +"				<b>Observações: </b>#OBSERVACOES#"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"		"
                        +"	<tbody class=\"#INFO_CONTATOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>CONTATOS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_CONTATOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>Nome</b></td>"
                        +"                            <td><b>E-mail</b></td>"
                        +"                            <td><b>Área</b></th>"
                        +"                            <td><b>Tel. Comercial</b></td>"
                        +"                            <td><b>Tel. Celular</b></td>"
                        +"                            <td><b>OBS</b></td>"
                        +"                        </tr>"
                        +"						#LISTA_CONTATOS#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"		"
                        +"	<tbody class=\"#INFO_AREAS_ATUACOES#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>#TITULO_AREAS_ATUACOES_PLURAL#</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_AREAS_ATUACOES#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>#TITULO_AREAS_ATUACOES_SINGULAR#</b></td>"
                        +"                           #AREAS_ATUACOES_COLUNAS_CUSTOMZADAS#"
                        +"                        </tr>"
                        +"						#LISTA_AREAS_ATUACOES#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_TITULOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                        +"				<b>TÍTULOS</b>"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"	<tbody class=\"#INFO_TITULOS#\">"
                        +"		<tr>"
                        +"			<td colspan=\"4\" class=\"nopadding\">"
                        +"				"
                        +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                        +"                    <tbody>"
                        +"                        <tr>"
                        +"                            <td><b>Instituição</b></td>"
                        +"							<td><b>Título de Especialista</b></td>"
                        +"							<td><b>Data de Aquisição</b></td>"
                        +"							<td><b>Data de Renovação</b></td>"
                        +"							<td><b>Tipo</b></td>"
                        +"							<td><b>Situação</b></td>"
                        +"                        </tr>"
                        +"						#LISTA_TITULOS#"
                        +"                    </tbody>"
                        +"                </table>"
                        +"				"
                        +"			</td>"
                        +"		</tr>"
                        +"	</tbody>"
                        +"		"
                        +"</table>"
                        +""
                        +"<div class=\"col-md-4 text-center\" style=\"margin-top: 40px;\">"
                        +"  <p>_____________________________________</p>"
                        +"  <p>Assinatura Associado</p>"
                        +"</div>"
                        +"<div class=\"col-md-4 pull-right text-center\" style=\"margin-top: 40px;\">"
                        +"  <p>_____________________________________</p>"
                        +"  <p>Associação</p>"
                        +"</div>";

            

            return htmlCorpo;
        }

        [NonAction]
        private string fichaPadraoAssociadoPJ() {

            var htmlCorpo = ""
                            +"<style>"
                            +"	.center{text-align:center;}"
                            +"	tbody tr td {"
                            +"		font: 8pt Arial; line-height:1.5;"
                            +"	}"
                            +"	tbody tr td b {"
                            +"		text-transform: uppercase;"
                            +"	}"
                            +"	tbody tr td:not(:last-child) {"
                            +"		border-right: #000000 1px solid;"
                            +"	}"
                            +"	tbody tr:not(:last-child) {"
                            +"		border-bottom: #000000 1px solid;"
                            +"	}"
                            +"	.noborder {"
                            +"		border: 0px !important;"
                            +"	}"
                            +"	.nopadding {"
                            +"		padding: 0px;"
                            +"	}"
                            +"	#CSS_CUSTOMIZADO#"
                            +"</style>"
                            +""
                            +"<table width=\"957\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\" style=\"border: #000000 1px solid; margin: 0 auto;\">"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td valign=\"middle\" class=\"noborder\" colspan=\"2\">"
                            +"				<img src=\"#LINK_LOGO#\" alt=\"#NOME_ASSOCIACAO#\">"
                            +"			</td>"
                            +"			<td colspan=\"2\" align=\"right\">"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr bgcolor=\"#EEEEEE\">"
                            +"			<td>"
                            +"				<b>Código: </b>#CODIGO# (Nº #NRO_ASSOCIADO#)"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Data de Cadastro: </b>#DT_CADASTRO#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Data da Admissão: </b>#DT_ADMISSAO#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Tipo de Associado: </b>#TIPO_ASSOCIADO#"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>DADOS EMPRESARIAIS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td colspan=\"2\">"
                            +"				<b>Razão Social: </b>#RAZAO_SOCIAL#"
                            +"			</td>"
                            +"			"
                            +"			<td colspan=\"2\">"
                            +"				<b>Nome Fantasia: </b>#NOME#"
                            +"			</td>"
                            +"		</tr>"
                            +"		<tr>"
                            +"			<td>"
                            +"				<b>FUNDAÇÃO: </b>#DT_FUNDACAO#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>SETOR DE ATUAÇÃO: </b>#SETOR_ATUACAO#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>PORTE DA EMPRESA: </b>#PORTE_EMPRESA#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>SIMPLES NACIONAL? </b>#FLAG_SIMPLES_NACIONAL#"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_ENDERECOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>ENDEREÇOS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_ENDERECOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Endereço Completo</b></td>"
                            +"                            <td><b>Zona</b></td>"
                            +"							<td><b>Bairro</b></td>"
                            +"                            <td><b>CEP</b></td>"
                            +"                            <td><b>Cidade</b></td>"
                            +"                            <td><b>Estado</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_ENDERECOS#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +""
                            +"	<tbody class=\"#INFO_TELEFONES#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>Telefones</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_TELEFONES#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Tipo</b></td>"
                            +"                            <td><b>Operadora</b></td>"
                            +"							<td><b>Numero</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_TELEFONES#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"		<tbody class=\"#INFO_EMAILS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>Emails</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_EMAILS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Tipo</b></td>"
                            +"                            <td><b>Email</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_EMAILS#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>DOCUMENTOS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>		"
                            +"		<tr class=\"#INFO_PJ#\">"
                            +"			<td colspan=\"2\">"
                            +"				<b>CNPJ: </b>#NRO_DOCUMENTO#"
                            +"			</td>"
                            +"			"
                            +"			<td>"
                            +"				<b>I.E.: </b>#IE#"
                            +"			</td>"
                            +"			"
                            +"			<td>"
                            +"				<b>Inscrição Municipal: </b>#INSCRICAO_MUNICIPAL#"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>RESPONSÁVEL</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody>		"
                            +"		<tr>"
                            +"			<td colspan=\"2\">"
                            +"				<b>NOME: </b>#NOME_RESPONSAVEL#"
                            +"			</td>"
                            +"			"
                            +"			<td colspan=\"2\">"
                            +"				<b>CPF: </b>#CPF_RESPONSAVEL#"
                            +"			</td>"
                            +"		</tr>"
                            +"		<tr>"
                            +"			<td colspan=\"4\">"
                            +"				<b>Observações: </b>#OBSERVACOES_RESPONSAVEL#"
                            +"			</td>"
                            +"		</tr>		"
                            +"	</tbody>"
                            +"	<tbody>"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>DADOS CORPORATIVOS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"		"
                            +"	<tbody class=\"#INFO_PJ#\">"
                            +"		"
                            +"		<tr class=\"#INFO_DADOS_CORPORATIVOS#\">"
                            +"			<td colspan=\"2\">"
                            +"				<b>Órgão de Classe: </b>#ORGAO_CLASSE#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Tem fins lucrativos ?: </b>#FLAG_FINS_LUCRATIVOS#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Optante simples nacional ?: </b>#FLAG_SIMPLES_NACIONAL#"
                            +"			</td>"
                            +"		</tr>"
                            +"		"
                            +"		<tr>"
                            +"			<td>"
                            +"				<b>Nº Funcionários CLT: </b>#NRO_FUNCIONARIOS_CLT#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Nº Funcionários Terceirizados: </b>#NRO_FUNCIONARIOS_TERCEIRIZADOS#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Nº Estagiários: </b>#NRO_ESTAGIARIOS#"
                            +"			</td>"
                            +"			<td>"
                            +"				<b>Nº Menores Aprendizes: </b>#NRO_MENORES_APRENDIZES#"
                            +"			</td>"
                            +"		</tr>"
                            +"		<tr>"
                            +"			<td colspan=\"4\">"
                            +"				<b>Observações: </b>#OBSERVACOES#"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_CONTATOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>CONTATOS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_CONTATOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Nome</b></td>"
                            +"                            <td><b>E-mail</b></td>"
                            +"                            <td><b>Área</b></th>"
                            +"                            <td><b>Tel. Comercial</b></td>"
                            +"                            <td><b>Tel. Celular</b></td>"
                            +"                            <td><b>OBS</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_CONTATOS#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_REPRESENTANTES# #INFO_PJ#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>REPRESENTANTES</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_REPRESENTANTES# #INFO_PJ#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Tipo</b></td>"
                            +"							<td><b>Junto a Associação</b></td>"
                            +"							<td><b>Nome</b></td>"
                            +"							<td><b>CPF</b></td>"
                            +"							<td><b>RG</b></td>"
                            +"							<td><b>Telefone</b></td>"
                            +"							<td><b>E-mail</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_REPRESENTANTES#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"		<tbody class=\"#INFO_AREAS_ATUACOES#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>#TITULO_AREAS_ATUACOES_PLURAL#</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_AREAS_ATUACOES#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>#TITULO_AREAS_ATUACOES_SINGULAR#</b></td>"
                            +"                           #AREAS_ATUACOES_COLUNAS_CUSTOMZADAS#"
                            +"                        </tr>"
                            +"						#LISTA_AREAS_ATUACOES#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_TITULOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" align=\"center\" bgcolor=\"#EEEEEE\">"
                            +"				<b>TÍTULOS</b>"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"	<tbody class=\"#INFO_TITULOS#\">"
                            +"		<tr>"
                            +"			<td colspan=\"4\" class=\"nopadding\">"
                            +"				"
                            +"				<table width=\"100%\" cellpadding=\"7\" cellspacing=\"0\" rules=\"GROUPS\">"
                            +"                    <tbody>"
                            +"                        <tr>"
                            +"                            <td><b>Instituição</b></td>"
                            +"							<td><b>Título de Especialista</b></td>"
                            +"							<td><b>Data de Aquisição</b></td>"
                            +"							<td><b>Data de Renovação</b></td>"
                            +"							<td><b>Tipo</b></td>"
                            +"							<td><b>Situação</b></td>"
                            +"                        </tr>"
                            +"						#LISTA_TITULOS#"
                            +"                    </tbody>"
                            +"                </table>"
                            +"				"
                            +"			</td>"
                            +"		</tr>"
                            +"	</tbody>"
                            +"		"
                            +"</table>"
                            +""
                            +"<div class=\"col-md-4 text-center\" style=\"margin-top: 40px;\">"
                            +"  <p>_____________________________________</p>"
                            +"  <p>Assinatura Associado</p>"
                            +"</div>"
                            +"<div class=\"col-md-4 pull-right text-center\" style=\"margin-top: 40px;\">"
                            +"  <p>_____________________________________</p>"
                            +"  <p>Associação</p>"
                            +"</div>";

            return htmlCorpo;

        }

        #endregion
    }
}