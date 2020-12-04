using System;
using System.Linq;
using BLL.Services;
using DAL.Contatos;
using System.Collections.Generic;

namespace BLL.Contatos {

    public class PessoaContatoVWBL : DefaultBL, IPessoaContatoVWBL {

        //
        public PessoaContatoVW carregar(int idContato) {

            var query = from PC in db.PessoaContatoVW
                        where PC.idContato == idContato
                        select PC;

            query = query.condicoesSeguranca();

            PessoaContatoVW OPessoaVW = query.FirstOrDefault();

            return OPessoaVW;
        }

        //
        public IQueryable<PessoaContatoVW> listar(string ativo, string flagSituacaoContribuicao, List<int> idsTipoContato, List<int> idsTipoAssociado, string valorBusca) {

            var query = (from PC in
                             db.PessoaContatoVW
                         select PC);

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            if (!String.IsNullOrEmpty(flagSituacaoContribuicao)) {
                query = query.Where(x => x.flagSituacaoContribuicao == flagSituacaoContribuicao);
            }

            if (idsTipoContato.Count() > 0) {
                query = query.Where(x => idsTipoContato.Contains(x.idTipoContato ?? 0));
            }

            if (idsTipoAssociado.Count() > 0) {
                query = query.Where(x => idsTipoAssociado.Contains(x.idTipoAssociado ?? 0));
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nomeContato.Contains(valorBusca) || x.nomeAssociado.Contains(valorBusca));
            }

            return query;
        }
    }
}