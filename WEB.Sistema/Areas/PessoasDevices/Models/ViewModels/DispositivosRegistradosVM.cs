using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Notificacoes;
using DAL.Notificacoes;
using PagedList;
using WEB.Areas.PessoasDevices.Helpers;
using WEB.Extensions;

namespace WEB.Areas.PessoasDevices.ViewModels{
    
	public class DispositivosRegistradosVM {

		// Atributos Serviços
		private IPessoaDeviceConsultaBL _IPessoaDeviceConsultaBL;
 
		// Propriedades Serviços
		private IPessoaDeviceConsultaBL OPessoaDeviceConsultaBL => _IPessoaDeviceConsultaBL = _IPessoaDeviceConsultaBL ?? new PessoaDeviceConsultaBL();
		
		// Propriedades
		public string valorBusca { get; set; }
		
		public string plataforma { get; set; }
		
		public string flagTipoSaida { get; set; }

		public IPagedList<PessoaDevice> listaDispositivos { get; set; }

		//
		public DispositivosRegistradosVM() {
			
			this.listaDispositivos = new List<PessoaDevice>().ToPagedList(1, 20);
		}

		//
		public void carregarInformacoes() {
			
			this.capturarParametros();
			
			this.carregarDispositivos();
		}
		
		//
		private void capturarParametros() {

			this.valorBusca = UtilRequest.getString("valorBusca");
			
			this.plataforma = UtilRequest.getString("plataforma");
			
			this.flagTipoSaida = UtilRequest.getString("flagTipoSaida");
		}
		
		//
		public IQueryable<PessoaDevice> montarQuery() {
			
			var query = this.OPessoaDeviceConsultaBL.listar(this.valorBusca);

			if (this.plataforma.Equals(PlataformaHelper.ANDROID)) {
				query = query.Where(x => x.flagAndroid == true);
			}

			if (this.plataforma.Equals(PlataformaHelper.IOS)) {
				query = query.Where(x => x.flagIOS == true);
			}

			return query;
		}

		//
		private void carregarDispositivos() {

			var query = this.montarQuery();

			this.listaDispositivos = query.Select(x => new {
										x.id, 
										x.idDevice,
										x.flagAndroid,
										x.flagIOS,
										x.versao,
										x.idPessoa,
										x.dtCadastro,
										Pessoa = new {
											x.Pessoa.nome
										}
									 }).OrderBy(x => x.id)
									   .ToPagedListJsonObject<PessoaDevice>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
		}

	}
    
}