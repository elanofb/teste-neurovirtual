using AutoMapper;
using DAL.Permissao;
using WEB.Areas.Permissao.ViewModels;

namespace WEB.Areas.Permissao.Config.Mapper {

	//Criação de Mapeamentos
	public class PermissaoProfile : Profile {

        public PermissaoProfile() {

            CreateMap<UsuarioSistema, UsuarioSistemaForm>().ReverseMap();

            //CreateMap<PerfilAcesso, PerfilAcessoVM>().ReverseMap();

            CreateMap<AcessoRecurso, AcessoRecursoForm>().ReverseMap();
        }
	}
}