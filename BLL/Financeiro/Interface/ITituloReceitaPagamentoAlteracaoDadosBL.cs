﻿using System;

namespace BLL.Financeiro.Services {

    public interface ITituloReceitaPagamentoAlteracaoDadosBL {

        UtilRetorno alterarCampo(int id, string nomeCampo, string novoValor, string nomeCampoDisplay = "", string oldValueSelect = "", string newValueSelect = "", string justificativa = "");

    }
}
