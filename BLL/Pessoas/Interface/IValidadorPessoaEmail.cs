using System;
using System.Collections.Generic;
using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IValidadorPessoaEmail {

        UtilRetorno validar(List<PessoaEmail> listaEmails, bool flagPermitirDuplicidade);
    }

}
