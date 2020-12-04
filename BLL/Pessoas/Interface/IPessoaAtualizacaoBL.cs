using DAL.Pessoas;

namespace BLL.Pessoas {

    public interface IPessoaAtualizacaoBL {

        void atualizarListas(Pessoa OPessoaAtualizacao, Pessoa dbPessoa);

    }
    
}