namespace BLL.Compras{

    public interface ICarrinhoItemQuantidadeBL{

        void atualizarQuantidade(int idOrganizacaoParam, int id, string idSessao, byte novaQtde);
    }
}