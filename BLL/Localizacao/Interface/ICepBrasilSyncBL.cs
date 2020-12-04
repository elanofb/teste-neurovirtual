using System;
using System.Threading.Tasks;
using BLL.Request;
using DAL.Localizacao;
using DAL.Repository.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BLL.Localizacao {

    public interface ICepBrasilSyncBL {

        CepBrasil buscarEndereco(string cep);
        
        CepBrasil carregar(string cep);
        
        List<CepBrasil> listarLoteCep(List<string> listaCep);

    }

}