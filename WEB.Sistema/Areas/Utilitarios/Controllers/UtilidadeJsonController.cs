using System.Collections.Generic;
using System.Web.Mvc;
using UTIL.UtilClasses;

namespace WEB.Areas.Utilitarios.Controllers {

    public class UtilidadeJsonController : Controller {
        
        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-onoff-line")]
        public ActionResult listarOpcoesOnOffLine() {
        
            var lista = new List<OptionSelect>(); 

            lista.Add(new OptionSelect{ value = "True", text = "Online"});

            lista.Add(new OptionSelect{ value = "False", text = "Offline"});

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        
        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-ativo")]
        public ActionResult listarOpcoesAtivo() {

            var lista = new List<OptionSelect>(); 

            lista.Add(new OptionSelect{ value = "S", text = "Ativo"});

            lista.Add(new OptionSelect{ value = "N", text = "Desativado"});

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        
        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-ativo-binario")]
        public ActionResult listarOpcoesAtivoBinario() {

            var lista = new List<OptionSelect>(); 

            lista.Add(new OptionSelect{ value = "True", text = "Ativo"});

            lista.Add(new OptionSelect{ value = "False", text = "Desativado"});

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-sim-nao")]
        public ActionResult listarOpcoesSimNao() {

            var lista = new List<OptionSelect>();

            lista.Add(new OptionSelect { value = "S", text = "Sim" });

            lista.Add(new OptionSelect { value = "N", text = "Não" });

            return Json(lista, JsonRequestBehavior.AllowGet);

        }

        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-sim-nao-binario")]
        public ActionResult listarOpcoesSimNaoBinario() {
                
            var lista = new List<OptionSelect>();

            lista.Add(new OptionSelect { value = "True", text = "Sim" });

            lista.Add(new OptionSelect { value = "False", text = "Não" });

            return Json(lista, JsonRequestBehavior.AllowGet);

        }
        
        
        // GET: Utilitarios/Utilidade
        [ActionName("listar-opcoes-sexo")]
        public ActionResult listarOpcoesSexo() {
                
            var lista = new List<OptionSelect>();

            lista.Add(new OptionSelect { value = "M", text = "Masculino" });

            lista.Add(new OptionSelect { value = "F", text = "Feminino" });

            return Json(lista, JsonRequestBehavior.AllowGet);

        }
        
        
        // GET: Utilitarios/Utilidade
        [ActionName("listar-flag-campo")]
        public ActionResult listarFlagCampo() {
                
            var lista = new List<OptionSelect>();

            lista.Add(new OptionSelect { value = 0, text = "Não solicitar" });

            lista.Add(new OptionSelect { value = 1, text = "Sim, obrigatório" });
            
            lista.Add(new OptionSelect { value = 2, text = "Sim, opcional" });

            return Json(lista, JsonRequestBehavior.AllowGet);

        }
    }
}