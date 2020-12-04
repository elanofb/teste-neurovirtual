namespace BLL.Notificacoes.Services
{
    public interface INotificacaoLeituraBL
    {
        /// <summary>
        /// Registrar a leitura da mensagem
        /// </summary>
        void registrarLeitura(int idNotificacaoPessoa);
    }
}