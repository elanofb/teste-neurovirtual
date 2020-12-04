using System.Linq;
using DAL.RedeAfiliados;
using DAL.RedeAfiliados.DTO;

namespace BLL.RedeAfiliados.Services {

    public interface IRedePontuacaoConsultaBL {
        
        /// <summary>
        /// 
        /// </summary>
        IQueryable<RedePontuacao> query();

        /// <summary>
        /// 
        /// </summary>
        IQueryable<RedePontuacao> listar(int? idMembro, bool? flagPago);

        /// <summary>
        /// 
        /// </summary>
        PontuacaoMembroDTO carregarPorMembro(int idMembro);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idMembro"></param>
        /// <returns></returns>
        PontuacaoMembroDTO carregarPontoCarreiraMembro(int idMembro);
        
        /// <summary>
        /// 
        /// </summary>
        PontuacaoMembroDTO carregarPontoPendenteMembro(int idMembro);
    }

}