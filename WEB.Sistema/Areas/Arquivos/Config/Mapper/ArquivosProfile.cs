using AutoMapper;
using DAL.Arquivos;
using WEB.Areas.Arquivos.ViewModels;

namespace WEB.Areas.Arquivos.Config.Mapper {

	public class ArquivosProfile : Profile {

		public ArquivosProfile() {

            CreateMap<ArquivoUpload, ArquivoUploadVM>().ReverseMap();

		}
	}
}