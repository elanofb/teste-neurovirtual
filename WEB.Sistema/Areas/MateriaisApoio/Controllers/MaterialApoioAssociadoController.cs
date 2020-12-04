using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.MateriaisApoio;
using DAL.Pessoas;
using BLL.Pessoas;
using MvcFlashMessages;

namespace WEB.Areas.MateriaisApoio.Controllers {

    public class MaterialApoioAssociadoController : Controller {

        // Atributos
        private PessoaBL _PessoaBL;

        // Propriedades
        private PessoaBL OPessoaBL { get { return this._PessoaBL = this._PessoaBL ?? new PessoaBL(); } }

        public MaterialApoioAssociadoController() {

        }

        [ActionName("partial-associados-especificos")]
        public PartialViewResult index(int? idMaterialApoio)
        {
            ViewBag.idMaterialApoio = UtilNumber.toInt32(idMaterialApoio);

            return PartialView("partial-associados-especificos");
        }

        [HttpPost]
        public PartialViewResult adicionarAssociadoEspecifico() {

            // Autocomplete de inserção individual
            int idPessoa = UtilRequest.getInt32("idAssociadoEspecifico");
            string nomeAssociado = UtilRequest.getString("nomeAssociadoEspecifico");
            string cnpfAssociado = UtilRequest.getString("cnpfAssociadoEspecifico");

            // Campo de inserção em lote
            string cnpfAssociadosLote = UtilRequest.getString("cnpfAssociadosLote");

            if (idPessoa == 0 && String.IsNullOrEmpty(cnpfAssociadosLote)) { 
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Selecione um associado ou insira uma lista de CPF/CNPJ para adicionar associados à lista.");
                return PartialView("partial-associados-especificos");
            }

            if (idPessoa > 0) { 
                this.adicionarAssociadoIndividual(idPessoa, nomeAssociado, cnpfAssociado);
            }

            if (!String.IsNullOrEmpty(cnpfAssociadosLote)) { 
                this.adicionarAssociadosEmLote(cnpfAssociadosLote);
            }

            return PartialView("partial-associados-especificos");
        }

        //
        [HttpPost]
        public JsonResult excluirAssociadoEspecifico(int id)
        {

            int idMaterialApoio = UtilRequest.getInt32("idMaterialApoio");

            var list = SessionMateriaisApoio.getListAssociadosEspecificos();
            list.Remove(list.Where(x => x.id == id).FirstOrDefault());

            SessionMateriaisApoio.setListAssociadosEspecificos(list);

            new MaterialApoioPessoaBL().excluirPessoa(idMaterialApoio, id);

            return Json(true);
        }

        #region NonActions

        [NonAction]
        public void adicionarAssociadoIndividual(int idPessoa, string nomeAssociado, string cnpfAssociado) { 

            var listAssociadosEspecificos = SessionMateriaisApoio.getListAssociadosEspecificos() ?? new List<Pessoa>();

            // Verificar se o associado já está adicionado
            if (listAssociadosEspecificos.Where(x => x.id == idPessoa).FirstOrDefault() != null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O associado "+ nomeAssociado +" com o documento "+ cnpfAssociado +" já está adicionado na sua lista.");
                return;
            }

            var OPessoa = new Pessoa() { id = idPessoa, nome = nomeAssociado, nroDocumento = cnpfAssociado };
            listAssociadosEspecificos.Add(OPessoa);

            SessionMateriaisApoio.setListAssociadosEspecificos(listAssociadosEspecificos);
        }

        [NonAction]
        public void adicionarAssociadosEmLote(string cnpfAssociadosLote) { 
        
            string[] arrayCnpfAssociadosLote = cnpfAssociadosLote.Replace("\r\n", ";").Split(';').Where(x => !String.IsNullOrEmpty(x))
                                                                 .Select(x => x.Trim()).ToArray();

            foreach (string cnpf in arrayCnpfAssociadosLote) {

                // Validação de documento
                if (!UtilValidation.isCPFCNPJ(cnpf)) { 
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O documento " + cnpf + " é inválido.");    
                    continue;
                }

                var cnpfOnlyNumber = UtilString.onlyNumber(cnpf);

                // Verificação de existencia de associado por documento
                var OPessoa = this.OPessoaBL.listar("", "").Where(x => x.nroDocumento.Equals(cnpfOnlyNumber)).FirstOrDefault();
                if (OPessoa == null) { 
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Nenhum associado com documento " + cnpfOnlyNumber+ " foi encontrado.");    
                    continue;
                }

                this.adicionarAssociadoIndividual(OPessoa.id, OPessoa.nome, cnpfOnlyNumber);
            }

        }

        #endregion

    }
}