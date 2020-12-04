using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFlashMessages;
using BLL.Associados;
using DAL.Associados;
using DAL.Emails;
using PagedList;
using DAL.Notificacoes;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    public class AvisoNotificacaoAssociadoController : Controller {

        // Atributos
        private IAssociadoBL _AssociadoBL;

        // Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();


        public AvisoNotificacaoAssociadoController() {

        }

        [ActionName("partialAssociadosEspecificos")]
        public PartialViewResult index() { 
            return PartialView("partialAssociadosEspecificos");
        }

        [HttpPost]
        public PartialViewResult adicionarAssociadoEspecifico() {

            // Autocomplete de inserção individual
            int idAssociado = UtilRequest.getInt32("idAssociado");

            // Campo de inserção em lote
            string cnpfAssociadosLote = UtilRequest.getString("cnpfAssociadosLote");

            if (idAssociado == 0 && String.IsNullOrEmpty(cnpfAssociadosLote)) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Selecione um associado ou insira uma lista de CPF/CNPJ para adicionar associados à lista.");
                return PartialView("partialAssociadosEspecificos");
            }

            if (idAssociado > 0) {
                this.adicionarAssociadoIndividual(idAssociado);
            } 

            if (!String.IsNullOrEmpty(cnpfAssociadosLote)) { 
                this.adicionarAssociadosEmLote(cnpfAssociadosLote);
            }

            return PartialView("partialAssociadosEspecificos");
        }
            
        //
        //Lista de associados para aucomplete de campos
        [ActionName("autocompletar-associados")]
		public ActionResult autocompletarAssociados(string term, int? idPessoa) {

			var queryAssociados = this.OAssociadoBL.autocompletar(term, UtilNumber.toInt32(idPessoa));
			
            if (SessionNotificacoes.getListAssociadosEspecificos().Any()) {
                var idsPessoasAdicionadas = SessionNotificacoes.getListAssociadosEspecificos().Select(x => x.id).ToList();
                queryAssociados = queryAssociados.Where(x => !idsPessoasAdicionadas.Contains(x.idPessoa));
            }

            var page = UtilRequest.getInt32("page");

            page = page > 0 ? page : 1;

            var listAssociados = queryAssociados.OrderBy(x => x.value).ToPagedList(page, 30);

            var listaJson = listAssociados.Select(x => new { x.id, text = x.value.ToUpper() + " (" + UtilString.formatCPFCNPJ(x.nroDocumento) + ")" }).ToList();

            return Json(new { items = listaJson, page, total_count = listAssociados.TotalItemCount}, JsonRequestBehavior.AllowGet);
            
		}

        //
        [HttpPost]
        public JsonResult excluirAssociadoEspecifico(int id) {

            var list = SessionNotificacoes.getListAssociadosEspecificos();

            list.Remove(list.FirstOrDefault(x => x.idReferencia == id));

            SessionNotificacoes.setListAssociadosEspecificos(list);

            return Json(true);
        }

        #region NonActions

        [NonAction]
        public void adicionarAssociadoIndividual(int idAssociado) { 

            var listAssociadosEspecificos = SessionNotificacoes.getListAssociadosEspecificos() ?? new List<NotificacaoSistemaEnvio>();

            // Verificar se o associado já está adicionado
            if (listAssociadosEspecificos.FirstOrDefault(x => x.idReferencia == idAssociado) != null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O associado já está adicionado na sua lista.");
                return;
            }
            
            var OAssociado = this.OAssociadoBL.listar(0, "", "", "").Where(x => x.id == idAssociado)
                                 .Select(x => new AssociadoDadosBasicos() {
                                         id = x.id, idPessoa = x.idPessoa, nome = x.Pessoa.nome, nroDocumento = x.Pessoa.nroDocumento,
                                         emailPrincipal = x.Pessoa.listaEmails.FirstOrDefault(y => y.idTipoEmail == TipoEmailConst.PESSOAL && y.dtExclusao == null).email,
                                         emailSecundario = x.Pessoa.listaEmails.FirstOrDefault(y => y.idTipoEmail == TipoEmailConst.COMERCIAL && y.dtExclusao == null).email
                                 }).FirstOrDefault();

            // Verificar se o associado já está adicionado
            if (OAssociado == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O associado não foi encontrado.");
                return;
            }

            if (!UtilValidation.isEmail(OAssociado.emailPrincipal) && 
                !UtilValidation.isEmail(OAssociado.emailSecundario)) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"O associado { OAssociado.nome } (Doc.: { UtilString.formatCPFCNPJ(OAssociado.nroDocumento) }) não possui e-mails válidos cadastrados.");
                return;
            }
            
            var listaEmails = new List<string> { OAssociado.emailPrincipal, OAssociado.emailSecundario };

            foreach (var email in listaEmails) {
                
                var OEnvio = new NotificacaoSistemaEnvio();
                
                OEnvio.idReferencia = OAssociado.id;
                
                OEnvio.idPessoa = OAssociado.idPessoa;
                
                OEnvio.nome = OAssociado.nome;
                
                OEnvio.email = email;
                
                if (!UtilValidation.isEmail(OEnvio.email)) {

                    OEnvio.flagExcluido = true;

                    OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                }

                listAssociadosEspecificos.Add(OEnvio);   
                
            }

            SessionNotificacoes.setListAssociadosEspecificos(listAssociadosEspecificos);
        }

        [NonAction]
        public void adicionarAssociadosEmLote(string cnpfAssociadosLote) { 
        
            string[] arrayCnpfAssociadosLote = cnpfAssociadosLote.Replace("\r\n", ";").Split(';')
                                                                 .Where(x => !String.IsNullOrEmpty(x))
                                                                 .Select(x => x.Trim()).ToArray();

            foreach (string cnpf in arrayCnpfAssociadosLote) {

                // Validação de documento
                if (!UtilValidation.isCPFCNPJ(cnpf)) { 
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O documento " + cnpf + " é inválido.");    
                    continue;
                }

                var cnpfOnlyNumber = UtilString.onlyNumber(cnpf);

                // Verificação de existencia de associado por documento
                var idAssociado = this.OAssociadoBL.listar(0, "", "" , "").Where(x => x.Pessoa.nroDocumento.Equals(cnpfOnlyNumber)).Select(x => x.id).FirstOrDefault();
                if (idAssociado == 0) { 
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Nenhum associado com documento " + cnpfOnlyNumber+ " foi encontrado.");    
                    continue;
                }

                this.adicionarAssociadoIndividual(idAssociado);

            }

        }

        #endregion

    }
}