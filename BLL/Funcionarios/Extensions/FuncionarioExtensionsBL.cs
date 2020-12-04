using DAL.Funcionarios;

namespace BLL.Funcionarios {

    public static class FuncionarioExtensionsBL {
        
        /// <summary>
        /// Retorna a descrição do Modelo de Contratação a partir do idModeloContratacao
        /// </summary>
	    public static string descricaoModeloContratacao(this Funcionario OFuncionario) {

            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.CLT) {
                return "CLT";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.ESTAGIARIO) {
                return "Estagiário";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.MENOR_APRENDIZ) {
                return "Menor Aprendiz";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.PJ) {
                return "PJ";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.TERCEIRIZADO) {
                return "Terceirizado";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.TRAINNE) {
                return "Trainne";
            }
            
            if (OFuncionario.idModeloContratacao == ModeloContratacaoConst.VOLUNTARIO) {
                return "Voluntário";
            }

            return "";
        }

    }
}