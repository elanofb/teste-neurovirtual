using System;
using DAL.Financeiro;

namespace BLL.Financeiro{

    public interface ITituloReceitaGeradorBL{

        UtilRetorno gerarLote(object OrigemTitulo);

        UtilRetorno gerar(object OrigemTitulo);

        TituloReceita salvar(TituloReceita OTituloReceita);
    }
}