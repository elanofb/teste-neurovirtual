using AutoMapper;
using DAL.Empresas;

namespace WEB.Areas.Empresas.Config.Mapper {

	//Criação de Mapeamentos
	public class EmpresaProfile : Profile {

		public EmpresaProfile() {

            var config = new MapperConfiguration(cfg => {
                
			    var mapEmpresa = cfg.CreateMap<Empresa, Empresa>();

                mapEmpresa.ForMember( dest => dest.idPessoa, opt => opt.Ignore());

            });
		}
	}
}