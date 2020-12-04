using System;
using System.Threading.Tasks;
using BLL.Request;
using DAL.Localizacao;
using DAL.Repository.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BLL.Localizacao {

    public class CepBrasilSyncBL : TableRepository<CepBrasil>, ICepBrasilSyncBL {

        //Construtor
        public CepBrasilSyncBL() {
            
        }

        //Constantes
        private static string urlServicoCEP = "http://servicos.sinchost.com.br/locate/";

        //Atributos
        private IRequestGet _IRequestGet;
        private IRequestPost _IRequestPost;

        //Propriedades
        private IRequestGet ORequestGet => _IRequestGet = _IRequestGet ?? new RequestGet();
        private IRequestPost ORequestPost => _IRequestPost = _IRequestPost ?? new RequestPost();


        //Consultar um endereco a partir de um cep
        //Ler a resposta e converter em objeto CepBrasil
        public CepBrasil buscarEndereco(string cep) {

            string urlConsulta = String.Concat(urlServicoCEP, "buscar-cep?cep=", cep);

            var resposta = this.ORequestGet.doRequest(urlConsulta);
            
            CepBrasil OCepBrasil = JsonConvert.DeserializeObject<CepBrasil>(resposta);

            return OCepBrasil;
        }

        //
        public CepBrasil carregar(string cep) {

            string urlConsulta = String.Concat(urlServicoCEP, "buscar-cep?cep=", cep);

            var resposta = this.ORequestGet.doRequest(urlConsulta);

            CepBrasil OCepBrasil = new CepBrasil();
            
            if (!String.IsNullOrEmpty(resposta)){
                OCepBrasil = JsonConvert.DeserializeObject<CepBrasil>(resposta) ?? new CepBrasil();
            }

            return OCepBrasil;
        }

        /// <summary>
        /// Listagem de acordo com uma lista de CEPs informada
        /// </summary>
        public List<CepBrasil> listarLoteCep(List<string> listaCep) {

            string urlConsulta = String.Concat(urlServicoCEP, "listar-ceps");

            var sb = new StringBuilder();
            int i = 0;

            foreach (var item in listaCep) {
                sb.Append($"listaCep[{i}]={item}&");
                i++;
            }


            byte[] data = Encoding.ASCII.GetBytes(sb.ToString());

            var resultado = this.ORequestPost.postRequest(urlConsulta, data);

            List<CepBrasil> listaCEP;

            try{
                listaCEP = JsonConvert.DeserializeObject<List<CepBrasil>>(resultado) ?? new List<CepBrasil>();
            }
            catch (Exception e){
                UtilLog.saveError(e, "Erro consulta de cep");
                listaCEP = new List<CepBrasil>();
            }
            return listaCEP;
        }
    }
}