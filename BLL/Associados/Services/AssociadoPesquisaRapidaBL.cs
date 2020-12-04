using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using DAL.Procedures;

namespace BLL.Associados {

    public class AssociadoPesquisaRapidaBL : DefaultBL {
        
        //Events
        
        //Listar os associados considerando os parametros informados
        public List<SpAssociadosPesquisaRapida> listar(string valorBusca) {

            var valorBuscaNumerico = UtilString.onlyNumber(valorBusca);
            int valorBuscaInt = UtilNumber.toInt32(valorBuscaNumerico);
            
            var idOrganizacaoLogada = User.idOrganizacao();
            var idUnidade = User.idUnidade();
            
            var valorBuscaParam = new SqlParameter("valorBusca", valorBusca);
            var valorBuscaNumericoParam = new SqlParameter("valorBuscaNumerico", valorBuscaNumerico);
            var valorBuscaIntParam = new SqlParameter("valorBuscaInt", valorBuscaInt);
            var idOrganizacaoParam = new SqlParameter("idOrganizacao", idOrganizacaoLogada);
            var idUnidadeParam = new SqlParameter("idUnidade", idUnidade);
            
            var query = db.Database.SqlQuery<SpAssociadosPesquisaRapida>(
                        String.Concat(SpNomes.spAssociadosPesquisaRapida, " @valorBusca, @valorBuscaNumerico, @valorBuscaInt, @idOrganizacao, @idUnidade"), 
                        valorBuscaParam, valorBuscaNumericoParam, valorBuscaIntParam, idOrganizacaoParam, idUnidadeParam);

            return query.ToList();

        }
        
    }

}