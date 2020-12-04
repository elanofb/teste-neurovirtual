using System;
using System.Web;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.LogsAlteracoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.LogsAlteracoes {

    public class LogAlteracaoBL : DefaultBL, ILogAlteracaoBL {


        public LogAlteracao carregar(int id) {

            var query = (from Log in db.LogAlteracao.Include(x => x.UsuarioAlteracao)
                         where Log.id == id select Log);

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }

        public IQueryable<LogAlteracao> listar(int idEntidadeReferencia, int idReferencia, string valorBusca) {

            var query = db.LogAlteracao
                          .Include(x => x.UsuarioAlteracao);

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {

                query = query.Where(x => x.nomeCampoDisplay.Contains(valorBusca) ||
                                         x.nomeCampoAlterado.Contains(valorBusca) ||
                                         x.valorAntigo.Contains(valorBusca) ||
                                         x.valorNovo.Contains(valorBusca));
            }

            if (idReferencia > 0) {
                query = query.Where(x => x.idReferencia == idReferencia);
            }

            if (idEntidadeReferencia > 0) {
                query = query.Where(x => x.idEntidadeReferencia == idEntidadeReferencia);
            }

            return query;
        }

        public bool salvar(LogAlteracao OLogAlteracao) {

            OLogAlteracao.idUsuarioAlteracao = User.id();
            OLogAlteracao.dtAlteracao = DateTime.Now;
            OLogAlteracao.ip = HttpContext.Current.Request.UserHostAddress;
            OLogAlteracao.id = 0;

            return this.inserir(OLogAlteracao);
        }


        private bool inserir(LogAlteracao OLogAlteracao) {

            OLogAlteracao.setDefaultInsertValues();

            db.LogAlteracao.Add(OLogAlteracao);
            db.SaveChanges();

            return OLogAlteracao.id > 0;
        }
    }
}