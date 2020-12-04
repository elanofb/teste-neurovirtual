using AutoMapper;
using DAL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;

namespace WEB.Areas.Publicacoes.Config.Mapper {

	//Criação de Mapeamentos
	public class PublicacoesProfile : Profile {

		public PublicacoesProfile() {

            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<Noticia, NoticiaForm>().ReverseMap();

                cfg.CreateMap<Noticia, ComunicadoForm>().ReverseMap();

                cfg.CreateMap<Noticia, VagaEstagioForm>().ReverseMap();

                cfg.CreateMap<Noticia, IniciacaoCientificaForm>().ReverseMap();

                cfg.CreateMap<GaleriaFoto, GaleriaFotoForm>().ReverseMap();

                cfg.CreateMap<Video, VideoForm>().ReverseMap();

            });

        }
	}
}