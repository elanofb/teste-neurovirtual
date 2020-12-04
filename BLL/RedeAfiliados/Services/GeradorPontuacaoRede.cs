using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DAL.Associados;
using DAL.Pedidos;
using DAL.RedeAfiliados;
using DAL.RedeAfiliados.Extensions;
using DAL.Repository.Base;

namespace BLL.RedeAfiliados.Services {

    public class GeradorPontuacaoRede : IGeradorPontuacaoRede {
        
        //Atributos
        private IRedeEsquerdaConsultaBL _EsquerdaConsultaBL;
        private IRedeDireitaConsultaBL  _DireitaConsultaBL;
        
        //Servicos
        private IRedeEsquerdaConsultaBL EsquerdaConsultaBL => _EsquerdaConsultaBL = _EsquerdaConsultaBL ?? new RedeEsquerdaConsultaBL();
        private IRedeDireitaConsultaBL  DireitaConsultaBL  => _DireitaConsultaBL = _DireitaConsultaBL ?? new RedeDireitaConsultaBL();

        /// <summary>
        /// Construtor
        /// </summary>
        public GeradorPontuacaoRede() {
            
        }

		/// <summary>
		/// 
		/// </summary>
        public UtilRetorno gerarPontos( PedidoProduto ItemProduto, byte idChaveBinaria, int idMembroInicio) {

            var listaMembros = new List<Associado>();

            var Rede = this.carregarRedeBinaria(idChaveBinaria, idMembroInicio);

            var listaRede = Rede.toListaMembros().Where(x => x.id > 0).ToList();
            
            listaMembros.AddRange(listaRede);
            
            int idUltimoNivel = Rede.idMembroNivel10.toInt();

            if (idUltimoNivel > 0) {

                while (idUltimoNivel > 0) {
                
                    var RedeAdicional = this.carregarRedeBinaria(idChaveBinaria, idUltimoNivel);

                    var listaRedeAdicional = RedeAdicional.toListaMembros().Where(x => x.id > 0).ToList();
            
                    listaMembros.AddRange(listaRedeAdicional);
                
                    idUltimoNivel = RedeAdicional.idMembroNivel10.toInt();
                }
                
            }

			using (var db = new DataContext()) {

				SqlConnection connection = (SqlConnection) db.Database.Connection;
				connection.Open();

				SqlBulkCopy copy = new SqlBulkCopy(connection);
				copy.DestinationTableName = "tb_rede_pontuacao";
				copy.ColumnMappings.Add("idMembro", "idMembro");
				copy.ColumnMappings.Add("qtdePontos", "qtdePontos");
				copy.ColumnMappings.Add("flagPago", "flagPago");
				copy.ColumnMappings.Add("idPedidoProdutoOrigem", "idPedidoProdutoOrigem");
				copy.ColumnMappings.Add("flagImportado", "flagImportado");
				copy.ColumnMappings.Add("flagLadoEsquerdo", "flagLadoEsquerdo");
				copy.ColumnMappings.Add("flagLadoDireito", "flagLadoDireito");
				copy.ColumnMappings.Add("dtCadastro", "dtCadastro");

				DataTable dt = new DataTable();
				dt.Columns.Add("idMembro", typeof(int));
				dt.Columns.Add("qtdePontos", typeof(decimal));
				dt.Columns.Add("flagPago", typeof(bool));
				dt.Columns.Add("idPedidoProdutoOrigem", typeof(int));
				dt.Columns.Add("flagImportado", typeof(bool));
				dt.Columns.Add("flagLadoEsquerdo", typeof(bool));
				dt.Columns.Add("flagLadoDireito", typeof(bool));
				dt.Columns.Add("dtCadastro", typeof(DateTime));

				
				foreach (var Membro in listaMembros) {

					if (Membro.id == 0) {
						continue;
					}

					DataRow Item = dt.NewRow();
					Item["idMembro"] = Membro.id;
					Item["qtdePontos"] = ItemProduto.qtdePontosPlanoCarreira.toDecimal();
					Item["flagPago"] = false;
					Item["flagLadoEsquerdo"] = idChaveBinaria == ChaveBinariaConst.ESQUERDA;
					Item["flagLadoDireito"] = idChaveBinaria != ChaveBinariaConst.ESQUERDA;
					Item["flagImportado"] = false;
					Item["dtCadastro"] = DateTime.Now;

					dt.Rows.Add(Item);

				}

				copy.WriteToServer(dt);
			}
            
         
            return UtilRetorno.newInstance(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private RedeBinariaBase carregarRedeBinaria(byte idChaveBinaria, int idIndicador) {

            RedeBinariaBase RedeIndicador;

            if (idChaveBinaria == ChaveBinariaConst.ESQUERDA) {

                RedeIndicador = this.EsquerdaConsultaBL.query().FirstOrDefault(x => x.idMembro == idIndicador) ?? new RedeBinariaEsquerdaVW();

                return RedeIndicador;

            } 
            
            RedeIndicador = this.DireitaConsultaBL.query().FirstOrDefault(x => x.idMembro == idIndicador) ?? new RedeBinariaDireitaVW();
            
            return RedeIndicador;

        }        
    }

}
