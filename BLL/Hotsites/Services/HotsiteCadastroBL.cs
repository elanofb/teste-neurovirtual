using System;
using DAL.Hotsites;
using System.Json;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices.ComTypes;
using EntityFramework.Extensions;
using UTIL.Resources;

namespace BLL.Hotsites {

	public class HotsiteCadastroBL : HotsiteConsultaBL, IHotsiteCadastroBL {

		//
		public HotsiteCadastroBL() {
		}

		//Salvar um novo registro ou atualizar um existente
		public bool salvar(Hotsite OHotsite) {

			if(OHotsite.id == 0){	
				return this.inserir(OHotsite);
			}

			return this.atualizar(OHotsite);
		}

		//Persistir e inserir um novo registro 
		//Inserir Hotsite, Pessoa e lista de Endereços vinculados
		private bool inserir(Hotsite OHotsite) { 

			OHotsite.setDefaultInsertValues<Hotsite>();

		    db.Hotsite.Add(OHotsite);
			db.SaveChanges();

			return OHotsite.id > 0;
		}

		//Persistir e atualizar um registro existente 
		//Atualizar dados da Hotsite, Pessoa e lista de endereços
		private bool atualizar(Hotsite OHotsite) { 

			//Localizar existentes no banco
			Hotsite dbHotsite = this.carregar(OHotsite.id);

            if (dbHotsite == null) {
                return false;
            }


			//Configurar valores padrão
			OHotsite.setDefaultUpdateValues<Hotsite>();

			//Atualizacao da Hotsite
			var HotsiteEntry = db.Entry(dbHotsite);
			HotsiteEntry.CurrentValues.SetValues(OHotsite);
			HotsiteEntry.ignoreFields<Hotsite>();

            db.SaveChanges();
			return OHotsite.id > 0;
		}

		//Alteracao do status da Hotsite
        public JsonMessageStatus alterarStatus(int id) {

            var retorno = new JsonMessageStatus();

            Hotsite Objeto = this.carregar(id);
            if (Objeto == null) {
                retorno.error = true;
                retorno.message = NotificationMessages.invalid_register_id;
            } else {
                Objeto.ativo = (Objeto.ativo != true);
                db.SaveChanges();
                retorno.active = Objeto.ativo == true ? "S" : "N";
                retorno.message = "Os dados foram alterados com sucesso.";
            }
            return retorno;
        }
		
		public bool salvarDadosIniciais(Hotsite OHotsite) {
            
			OHotsite.tituloPagina = OHotsite.tituloPagina.abreviar(100);
			OHotsite.conteudoApresentacao = OHotsite.conteudoApresentacao.abreviar(3000); 
            
			db.Hotsite.condicoesSeguranca().Where(x => x.id == OHotsite.id)
				.Update(x => new Hotsite {

					tituloPagina = OHotsite.tituloPagina,
					flagContagemRegressiva = OHotsite.flagContagemRegressiva,
					conteudoApresentacao = OHotsite.conteudoApresentacao,
					idArquivoBannerPrincipal = OHotsite.idArquivoBannerPrincipal

				});
            

			return true;

		}
		
		public bool salvarDadosFormatacao(Hotsite OHotsite) {
            
			OHotsite.cssFormatacao = OHotsite.cssFormatacao.abreviar(8000); 
			OHotsite.scriptJS = OHotsite.scriptJS.abreviar(8000);
			OHotsite.scriptAdicional = OHotsite.scriptAdicional.abreviar(8000);
            
			db.Hotsite.condicoesSeguranca().Where(x => x.id == OHotsite.id)
			  .Update(x => new Hotsite {

				  cssFormatacao = OHotsite.cssFormatacao,
			      scriptJS = OHotsite.scriptJS,
				  scriptAdicional = OHotsite.scriptAdicional

			  });
            

			return true;

		}

		public bool salvarDadosCustomizacao(Hotsite OHotsite) {

			db.Hotsite.condicoesSeguranca()
				.Where(x => x.id == OHotsite.id)
				.Update(x => new Hotsite {
					htmlApresentacao = OHotsite.htmlApresentacao,
					htmlRodape = OHotsite.htmlRodape,
					idEventoGaleriaFoto = OHotsite.idEventoGaleriaFoto
				});

			return true;
		}

		public bool salvarBannerPrincipal(Hotsite OHotsite) {

			db.Hotsite.condicoesSeguranca()
				.Where(x => x.id == OHotsite.id)
				.Update(x => new Hotsite {
					idEventoGaleriaFoto = OHotsite.idEventoGaleriaFoto
				});

			return true;
		}
	}
}