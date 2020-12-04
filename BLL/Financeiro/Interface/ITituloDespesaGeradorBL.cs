using System;
using DAL.Financeiro;

namespace BLL.Financeiro{

    public interface ITituloDespesaGeradorBL{

        UtilRetorno gerarLote(object OrigemTitulo);

        UtilRetorno gerar(object OrigemTitulo);

        TituloDespesa salvar(TituloDespesa OTituloDespesa);
    }
}