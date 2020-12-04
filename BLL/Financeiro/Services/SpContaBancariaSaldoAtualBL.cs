using DAL.Financeiro.Procedures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Procedures;
using System.Data;

namespace BLL.Financeiro {

    public class SpContaBancariaSaldoAtualBL : DefaultBL, ISpContaBancariaSaldoAtualBL {

        // Propriedades
        public string spName { get; set; }

        //
        public SpContaBancariaSaldoAtualBL() {
            this.spName = "sp_conta_bancaria_saldo_atual_por_efetivacao";
        }

        //
        public SpContaBancariaSaldoAtual carregarContaBancaria(int idContaBancaria, DateTime dtLimite) {

            var dtLimiteFiltro = dtLimite.AddDays(1).ToString("yyyy/MM/dd");
            var dtLimiteParam = new SqlParameter("dtLimite", dtLimiteFiltro);

            var idOrganizacaoParam = new SqlParameter("idOrganizacao", User.idOrganizacao());
            var idContaBancariaParam = new SqlParameter("idContaBancaria", idContaBancaria);

            var query = db.Database.SqlQuery<SpContaBancariaSaldoAtual>(
                String.Concat(this.spName, " @idOrganizacao, @idContaBancaria, @dtLimite"), idOrganizacaoParam, idContaBancariaParam, dtLimiteParam);

            return query.FirstOrDefault();
        }

        //
        public List<SpContaBancariaSaldoAtual> listar(DateTime dtLimite) {

            var dtLimiteFiltro = dtLimite.AddDays(1).ToString("yyyy/MM/dd");
            var dtLimiteParam = new SqlParameter("dtLimite", dtLimiteFiltro);

            var idOrganizacaoParam = new SqlParameter("idOrganizacao", User.idOrganizacao());

            var idContaBancariaParam = new SqlParameter("idContaBancaria", 0);
            idContaBancariaParam.SqlDbType = SqlDbType.Int;
            idContaBancariaParam.SqlValue = 0;

            var query = db.Database.SqlQuery<SpContaBancariaSaldoAtual>(
                String.Concat(this.spName, " @idOrganizacao, @idContaBancaria, @dtLimite"), idOrganizacaoParam, idContaBancariaParam, dtLimiteParam);

            return query.ToList();
        }
    }
}
