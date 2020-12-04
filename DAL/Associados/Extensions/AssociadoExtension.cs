
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Associados {

    public static class AssociadoExtensions {


        //Link completo da imagem
        public static bool flagEstudante(this Associado OAssociado) {

            if (OAssociado == null) {
                return false;
            }

            if (OAssociado.idTipoAssociado == TipoAssociadoConst.ACADEMICO ||
                OAssociado.idTipoAssociado == TipoAssociadoConst.ESTUDANTE ||
                OAssociado.idTipoAssociado == TipoAssociadoConst.POS_GRADUANDO) {
                return true;
            }

            return false;
        }

        //Link completo da imagem
        public static string descricaoOrigemCadastro(this Associado OAssociado) {

            if (OAssociado == null || OAssociado.idOrigem.toInt() == 0) {
                return "-";
            }


            if (OAssociado.idOrigem == OrigemCadastroConst.SISTEMA) {
                return "Sistema";
            }

            if (OAssociado.idOrigem == OrigemCadastroConst.PORTAL) {
                return "Portal";
            }

            if (OAssociado.idOrigem == OrigemCadastroConst.AREA_ASSOCIADO) {
                return "Área do Associado";
            }


            if (OAssociado.idOrigem == OrigemCadastroConst.IMPORTACAO) {
                return "Importação de dados";
            }

            return "-";
        }
        
        /// <summary>
        /// Informe se o associado está ativo e adimplente
        /// </summary>
        public static bool flagAssociadoAtivoAdimplente(this Associado OAssociado){

            return OAssociado.ativo == "S";

        }
        
        // Limpar atributos e inserir valor nulo para objetos relacionados
        public static Associado limparAtributosDependentes(this Associado OAssociado) {
            
/*            OAssociado.listaDependentes.ForEach(Item => {

                Item.Pessoa.limparAtributos();

                Item.EstadoPrimeiraCOCEP = null;

                Item.EstadoSegundaCOCEP = null;

                Item.TipoAssociado = null;
                
                Item.listaDependentes = null;
                
                Item.observacoes = Item.observacoes.abreviar(1000);

                Item.outrasInformacoes = Item.outrasInformacoes.abreviar(1000);
                
            });*/
            
            return OAssociado;
        }
        
        /// <summary>
        /// Filtrar somente dependentes não excluídos.
        /// </summary>
        public static Associado limparListas(this Associado OAssociado) {

            /*OAssociado.listaDependentes = OAssociado.listaDependentes?.Where(x => x.dtExclusao == null).ToList();*/

            OAssociado.Pessoa.limparListas();

            return OAssociado;
        }
        
        //Link de indicação do usuário
        public static string linkIndicacaoMembro(this Associado OAssociado) {
            
            if (OAssociado == null || OAssociado.rotaConta.isEmpty()) {
                return "";
            }
                                                 
            string urlIndicacao = String.Concat(UtilConfig.linkAbsSistema, "indicacao-usuario", "/", OAssociado.rotaConta);
            
            return urlIndicacao;
            
        }
        
        //Link de indicação do usuário
        public static string linkIndicacaoComerciante(this Associado OAssociado) {
            
            if (OAssociado == null || OAssociado.rotaConta.isEmpty()) {
                return "";
            }
            
            string urlIndicacao = String.Concat(UtilConfig.linkAbsSistema, "indicacao-comerciante", "/", OAssociado.rotaConta);
            
            return urlIndicacao;
            
        }
        
        
    }

}
