using System;
using System.Linq;
using DAL.Financeiro;
using BLL.Services;
using System.Data.Entity;

namespace BLL.Financeiro {

    public class GestaoTituloVWBL : DefaultBL, IGestaoTituloVWBL {

        public IQueryable<GestaoTituloVW> listar(string valorBusca, string destinatario, string nf, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim) {

            var query = (from GestaoTituloVW in
                             db.GestaoTituloVW
                         select GestaoTituloVW);

            if(!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if(!String.IsNullOrEmpty(destinatario)) {
                var docDestinatario = UtilString.onlyNumber(destinatario);
                query = query.Where(x => x.nomeDestinatario.Contains(destinatario) || x.docDestinatario == docDestinatario);
            }

            if(!String.IsNullOrEmpty(nf)) {
                var nroNotaFiscal = Convert.ToUInt32(UtilString.onlyNumber(nf));
                query = query.Where(x => x.nroNotaFiscal == nroNotaFiscal);
            }

            if(!String.IsNullOrEmpty(flagPago)) {
                query = query.Where(x => x.flagPago == flagPago);
            }

            if(pesquisarPor == "P") {
                query = query.Where(x => (x.dtPagamento >= dtInicio) && (x.dtPagamento <= dtFim));
            } else {
                query = query.Where(x => (x.dtVencimento >= dtInicio) && (x.dtVencimento <= dtFim));
            }

            return query;
        }        
    }
}