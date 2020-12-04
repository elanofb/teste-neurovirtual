using System;
using System.Json;

namespace BLL.Planos {

	public interface IPlanoCarreiraExclusaoBL{
        
	    JsonMessage excluir(int[] ids);

    }
}