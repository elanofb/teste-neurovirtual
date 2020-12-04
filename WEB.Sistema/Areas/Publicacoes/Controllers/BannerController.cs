using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using BLL.Publicacoes;
using BLL.Arquivos;
using DAL.Entities;
using System.Data.Entity;
using DAL.Arquivos.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

	public class BannerController : Controller {

		//Constantes

        //Atributos
		private IBannerBL _BannerBL;
		private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

		//Propriedades
		private IBannerBL OBannerBL => _BannerBL = _BannerBL ?? new BannerBL();
	    private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

	    //
		public BannerController() { 
		}

		//GET 
		public ActionResult listar() {

			string posicaoBanner = UtilRequest.getString("posicaoBanner");
			string valorBusca = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");
		    int idPortal = UtilRequest.getInt32("idPortal");
		    

            var query = this.OBannerBL.listar(posicaoBanner, valorBusca, ativo);

		    if (idPortal > 0){
		        query = query.Where(x => x.idPortal == idPortal);
		    }

		    var listaBanners = query.AsNoTracking().OrderBy(x => x.descricao).ToList();

            int[] idsBanners = listaBanners.Select(x => x.id).ToArray();

			var listaArquivo = this.OArquivoUploadFotoBL.listar(0, EntityTypes.BANNER, "S")
						           .Where(x => idsBanners.Contains(x.idReferenciaEntidade) ).AsNoTracking().ToList();

			listaBanners.ForEach(Item => {
				Item.Arquivo = listaArquivo.fotoPrincipal(Item.id);
			});

			return View(listaBanners);
		}

		//GET 
		[HttpGet]
		public ActionResult editar(int? id) {

			BannerForm ViewModel = new BannerForm();

			ViewModel.Banner = this.OBannerBL.carregar(UtilNumber.toInt32(id)) ?? new Banner();

			return View(ViewModel);
		}

		//POST
		[HttpPost]
		public ActionResult editar(BannerForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

		    bool flagValidarDimensao = this.validarDimensoes(ViewModel);

		    if (!flagValidarDimensao) {
                
                return View(ViewModel);
		    }

			var flagSucesso = this.OBannerBL.salvar(ViewModel.Banner, ViewModel.OImagem);

			if (flagSucesso) {
				this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
				return RedirectToAction("listar");
			}

			this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
			return View(ViewModel);
		}
		
        //Validar as dimensoes do banner e incluir mensagem caso esteja incorreto
        private bool validarDimensoes(BannerForm ViewModel) {

            //var jsonConfig = OConfigJsonBL.load("banner_dimensoes.json");

            //if (string.IsNullOrEmpty(jsonConfig)) {
            //    return true;
            //}

            //List<BannerConfig> listaConfiguracoes = JsonConvert.DeserializeObject<List<BannerConfig>>(jsonConfig);

            //var OConfig = listaConfiguracoes.FirstOrDefault(x => x.posicao == ViewModel.Banner.posicao);

            //if (OConfig == null) {
            //    return true;
            //}

            //if (OConfig.flagValidar == false) {
            //    return true;
            //}

            //Image OImagem = Image.FromStream(ViewModel.OImagem.InputStream);

            //int height = OImagem.Height;

            //int width = OImagem.Width;

            //if (width < OConfig.largura - 10 || width > OConfig.largura + 10) {
            //    ModelState.AddModelError("OImagem", $"A largura da imagem deve ser de {OConfig.largura}px");
            //    return false;
            //}

            //if (height < OConfig.altura - 10 || height > OConfig.altura + 10) {
            //    ModelState.AddModelError("OImagem", $"A altura da imagem deve ser de {OConfig.altura}px");
            //    return false;
            //}

            return true;
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OBannerBL.alterarStatus(id));
        }

		//POST
		[HttpPost]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();

			foreach (int idExclusao in id) { 
				bool flagExcluido = this.OBannerBL.excluir(idExclusao);

				if (!flagExcluido) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

		    if (Retorno.error == false) {
		        Retorno.error = false;
		        Retorno.message = "Os registros foram excluídos com sucesso.";
            }

			return Json(Retorno);
		}
	}
}