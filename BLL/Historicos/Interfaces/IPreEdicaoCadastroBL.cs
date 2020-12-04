using System;
using System.Linq;
using DAL.DadosBancarios;
using DAL.Eventos;
using DAL.Historicos;

namespace BLL.Historicos.Interfaces {
    
    public interface IPreEdicaoCadastroBL {
        
        UtilRetorno salvar(object Origem);
        
    }
    
    
}