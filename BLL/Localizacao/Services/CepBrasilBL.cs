using System;
using System.Threading.Tasks;
using BLL.Request;
using DAL.Localizacao;
using DAL.Repository.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BLL.Localizacao {

    public class CepBrasilBL : TableRepository<CepBrasil>, ICepBrasilBL {

        //Construtor
        public CepBrasilBL() : base(null) {
            this.defaultPredicate = (x => true);
        }

        //Constantes
        private static string urlServicoCEP = "http://servicos.sinchost.com.br/locate/";

        //Atributos
        private IRequestAsync _RequestAsync;

        //Propriedades
        private IRequestAsync ORequestAsync => (this._RequestAsync = this._RequestAsync ?? new RequestAsync());


        //Consultar um endereco a partir de um cep
        //Ler a resposta e converter em objeto CepBrasil
        public async Task<CepBrasil> buscarEndereco(string cep) {

            string urlConsulta = String.Concat(urlServicoCEP, "buscar-cep?cep=", cep);

            var resposta = await this.ORequestAsync.doRequestAsync(urlConsulta);

            string resultado = resposta.ReadToEnd();

            CepBrasil OCepBrasil = JsonConvert.DeserializeObject<CepBrasil>(resultado);

            return OCepBrasil;
        }

        //
        public async Task<CepBrasil> carregar(string cep) {

            string urlConsulta = String.Concat(urlServicoCEP, "buscar-cep?cep=", cep);

            var resposta = await this.ORequestAsync.doRequestAsync(urlConsulta);

            CepBrasil OCepBrasil = new CepBrasil();

            var resultado = resposta.ReadToEnd();

            if (!String.IsNullOrEmpty(resultado)){
                OCepBrasil = JsonConvert.DeserializeObject<CepBrasil>(resultado) ?? new CepBrasil();
            }

            return OCepBrasil;
        }

        /// <summary>
        /// Listagem de acordo com uma lista de CEPs informada
        /// </summary>
        public async Task<List<CepBrasil>> listarLoteCep(List<string> listaCep) {

            string urlConsulta = String.Concat(urlServicoCEP, "listar-ceps");

            var sb = new StringBuilder();
            int i = 0;

            foreach (var item in listaCep) {
                sb.Append($"listaCep[{i}]={item}&");
                i++;
            }


            byte[] data = Encoding.ASCII.GetBytes(sb.ToString());

            var resultado = await this.ORequestAsync.postRequestAsync(urlConsulta, data);
            var resposta = resultado.ReadToEnd();

            List<CepBrasil> listaCEP;

            try{
                listaCEP = JsonConvert.DeserializeObject<List<CepBrasil>>(resposta) ?? new List<CepBrasil>();
            }
            catch (Exception e){
                UtilLog.saveError(e, "Erro consulta de cep");
                listaCEP = new List<CepBrasil>();
            }
            return listaCEP;
        }
    }
}