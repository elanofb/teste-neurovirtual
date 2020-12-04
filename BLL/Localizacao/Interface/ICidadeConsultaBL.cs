using DAL.Localizacao;
using System;
using System.Json;
using System.Linq;

namespace BLL.Localizacao {


    public interface ICidadeConsultaBL {
	    
	    IQueryable<Cidade> query();
	    
		Cidade carregar(int id);
        
        Cidade carregar(string nomeMunicipio, string uf);

        Cidade carregarPorNome(string nomeMunicipio, int? idEstado);
		
        Cidade carregarPorNome(string nomeMunicipio, string siglaEstado);
		
        IQueryable<Cidade> listar(int idEstado, string valorBusca, string ativo);
		
        /// <summary>
        /// Listagem de cidades com base no estado de uma determinada cidade
        /// </summary>
        IQueryable<Cidade> listar(int idCidade);
		
        IQueryable<CidadeAutoComplete> autocompletar(int idEstado, string valorBusca);


    }
}
