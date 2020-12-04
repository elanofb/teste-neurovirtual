using DAL.Financeiro.Procedures;
using System;
using System.Collections.Generic;

namespace BLL.Financeiro {

    public interface ISpContaBancariaSaldoAtualBL {
        
        // Propriedades
        string spName { get; set; }

        //
        SpContaBancariaSaldoAtual carregarContaBancaria(int idContaBancaria, DateTime dtLimite);

        List<SpContaBancariaSaldoAtual> listar(DateTime dtLimite);
    }
}
