using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using System.Collections.Generic;

namespace BLL.Associados {

    public class PendenciaCadastralBL : DefaultBL, IPendenciaCadastralBL {

        //Atributos

        //Propriedades

        //
        public PendenciaCadastralBL() {
        }

        //Carregamento de registro único pelo ID
        public PendenciaCadastralVW carregar(int idAssociado) {

            var query = from PC in db.PendenciaCadastralVW where PC.id == idAssociado select PC;

            query = query.condicoesSeguranca();

            PendenciaCadastralVW OAssociado = query.FirstOrDefault();

            return OAssociado;
        }

        //
        public IQueryable<PendenciaCadastralVW> listar(List<int> idsTipoAssociado, int? qtdEmails, int? qtdTel, int? qtdEnderecos, string flagSituacaoContribuicao, string ativo, string valorBusca) {

            var query = from C in db.PendenciaCadastralVW.AsNoTracking() select C;

            query = query.condicoesSeguranca();

            if (idsTipoAssociado.Any()) {
                query = query.Where(x => idsTipoAssociado.Contains(x.idTipoAssociado));
            }

            if (qtdEmails != null) {
                query = query.Where(x => x.qtdEmails == qtdEmails);
            }

            if (qtdTel != null) {
                query = query.Where(x => x.qtdTelefones == qtdTel);
            }

            if (qtdEnderecos != null) {
                query = query.Where(x => x.qtdEnderecos == qtdEnderecos);
            }

            if (!String.IsNullOrEmpty(flagSituacaoContribuicao)) {
                query = query.Where(x => x.flagSituacaoContribuicao == flagSituacaoContribuicao);
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.nome.Contains(valorBusca) || x.razaoSocial.Contains(valorBusca) ||
                                         x.nroDocumento == valorBusca || x.nroAssociado == intValorBusca);
            }

            return query;
        }
    }
}