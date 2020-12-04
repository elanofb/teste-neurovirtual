using System;
using System.Web;
using System.Web.Mvc;
using DAL.Entities;
using DAL.Arquivos;
using BLL.Arquivos;
using System.Collections.Generic;
using BLL.Associados;
using DAL.Arquivos.Extensions;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoFotoController : Controller {

        //Atributos
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;
        private AssociadoBL _AssociadoBL;

        //Propriedades
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();
        private AssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();

        //Construtor
        public AssociadoFotoController() {

        }

        //Carregamento da foto do associado
        [ActionName("partial-foto")]
        public PartialViewResult partialFoto(int id) {

            var FotoPrincipal = this.OArquivoUploadFotoBL.carregarPrincipal(id, EntityTypes.FOTO_ASSOCIADO) ?? new ArquivoUpload() { path = "" };

            return PartialView(FotoPrincipal);
        }

        //Modal para corte da imagem
        [ActionName("modal-corte-imagem")]
        public ActionResult modalCorteImagem() {
            return View();
        }

        //Salvar a foto 
        [HttpPost, ActionName("salvar-foto")]
        public ActionResult salvarFoto(HttpPostedFileBase FileUpload, int idAssociado) {

            var listaThumbs = new List<ThumbDTO>();

            listaThumbs.Add(new ThumbDTO { folderName = "h200", height = 200, width = 0 });

            var OArquivo = new ArquivoUpload();

            OArquivo.legenda = "Foto do Associado";

            OArquivo.idOrganizacao = User.idOrganizacao();

            OArquivo.idUsuarioCadastro = User.id();

            OArquivo.idReferenciaEntidade = idAssociado;

            OArquivo.entidade = EntityTypes.FOTO_ASSOCIADO;

            this.OArquivoUploadFotoBL.salvar(OArquivo, FileUpload, "", listaThumbs);

            return Json(new { flagErro = false }, JsonRequestBehavior.AllowGet);
        }

        //Carregamento da foto do associado
        [ActionName("download-foto")]
        public FileResult downloadFoto(int id) {

            var OArquivo = this.OArquivoUploadFotoBL.carregar(id) ?? new ArquivoUpload() { path = "" };

            string caminhoArquivo = OArquivo.linkFisico("h200");

            var OAssociado = OAssociadoBL.carregar(OArquivo.idReferenciaEntidade);

            return File(caminhoArquivo, String.Concat("application/", OArquivo.extensao), String.Concat(OAssociado.Pessoa.nome, ".", OArquivo.extensao));
        }
    }
}