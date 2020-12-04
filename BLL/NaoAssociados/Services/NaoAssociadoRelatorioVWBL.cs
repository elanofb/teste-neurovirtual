using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.NaoAssociados {

    public class NaoAssociadoRelatorioVWBL : DefaultBL, INaoAssociadoRelatorioVWBL {

        //Carregar o associado fazendo join com as tabelas necessarias através do ID
        public NaoAssociadoRelatorioVW carregar(int id) {

            var query = from Ass in db.NaoAssociadoRelatorioVW where Ass.id == id select Ass;

            query = query.condicoesSeguranca();

            NaoAssociadoRelatorioVW OAssociado = query.FirstOrDefault();

            return OAssociado;
        }


        //Listar os associado considerando os parametros informados
        public IQueryable<NaoAssociadoRelatorioVW> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo) {

            var query = from Ass in db.NaoAssociadoRelatorioVW.AsNoTracking()
                        where Ass.idAssociadoEstipulante == 0 || Ass.idAssociadoEstipulante == null
                        select Ass;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.nome.Contains(valorBusca) || x.razaoSocial.Contains(valorBusca) ||
                                         x.nroDocumento == valorBuscaSoNumeros || x.rg == valorBusca ||
                                         x.nroAssociado == intValorBusca || x.emails.Contains(valorBusca));
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