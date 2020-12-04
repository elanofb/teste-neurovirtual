using DAL.Associados;

namespace BLL.Associados{
    public interface IAreaAssociadoAssociadoCadastroBL{

        /// <summary>
        /// Realizar tratamentos, limpeza e persistências de dados
        /// Fazer o hub para enviar para atualização ou inserção de um novo registro 
        /// </summary>
        Associado salvar(Associado OAssociado);
    }
}