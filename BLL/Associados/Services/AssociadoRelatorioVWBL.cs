using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public class AssociadoRelatorioVWBL : DefaultBL, IAssociadoRelatorioVWBL {

        //Carregar o associado fazendo join com as tabelas necessarias através do ID
        public AssociadoRelatorioVW carregar(int id) {

            var query = from Ass in db.AssociadoRelatorioVW where Ass.id == id select Ass;

            query = query.condicoesSeguranca();

            AssociadoRelatorioVW OAssociado = query.FirstOrDefault();

            return OAssociado;
        }


        //Listar os associado considerando os parametros informados
        public IQueryable<AssociadoRelatorioVW> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo) {

            var query = from Ass in db.AssociadoRelatorioVW.AsNoTracking()
                        where Ass.idAssociadoEstipulante == 0 || Ass.idAssociadoEstipulante == null
                        select Ass;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.nome.Contains(valorBusca) || 
                                         x.razaoSocial.Contains(valorBusca) ||
                                         (x.nroDocumento == valorBuscaSoNumeros && !string.IsNullOrEmpty(valorBuscaSoNumeros)) ||
                                         (x.nroAssociado == intValorBusca && intValorBusca > 0));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }


            if (idTipoAssociado > 0) {
                query = query.Where(x => x.idTipoAssociado == idTipoAssociado);
            }

            return query;
        }
    }
}