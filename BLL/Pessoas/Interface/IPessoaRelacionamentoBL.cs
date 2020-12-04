using DAL.Pessoas;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Arquivos;

namespace BLL.Pessoas {
    public interface IPessoaRelacionamentoBL {
    
        PessoaRelacionamento carregar(int id);

        IQueryable<PessoaRelacionamento> listar(int idPessoa, int idOcorrenciaRelacionamento);

        bool salvar(PessoaRelacionamento OPessoaRelacionamento);

        PessoaRelacionamento salvar(int idPessoa, int idOcorrencia, int idUsuario, string observacoes);

        UtilRetorno excluir(int id);

    }
}
