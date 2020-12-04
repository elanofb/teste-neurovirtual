using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Arquivos;
using BLL.Services;

namespace BLL.Arquivos {

	public class ArquivoAssociadoVWBL : DefaultBL, IArquivoAssociadoVWBL {
        
        //Construtor
        public ArquivoAssociadoVWBL(){
        }

		//Carregar registro único
		public ArquivoAssociadoVW carregar(int id) {

			var query = from Arq in db.ArquivoAssociadoVW
                        where
							Arq.idArquivoUpload == id &&
                            Arq.ativo == "S"
						select Arq;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
		}
		
		//Listagem de registro de acordo com parametros informados
		public IQueryable<ArquivoAssociadoVW> listar(List<int> idsTipoAssociado, string flagSituacaoContribuicao, string buscaAssociado, int idEntidadeArquivo, string formatoArquivo, string valorBusca, string ativo) {
            
			var query = from Arq in db.ArquivoAssociadoVW
                        where !String.IsNullOrEmpty(Arq.path)
						select Arq;

            query = query.condicoesSeguranca();

            if (idsTipoAssociado.Any()) {
                query = query.Where(x => idsTipoAssociado.Contains(x.idTipoAssociado));
            }

            if (!String.IsNullOrEmpty(flagSituacaoContribuicao)) {
                //query = query.Where(x => (x.flagSituacaoContribuicao == null ? "IN" : x.flagSituacaoContribuicao) == flagSituacaoContribuicao);
            }

            if (!String.IsNullOrEmpty(buscaAssociado)) {
                string valorBuscaSoNumeros = UtilString.onlyNumber(buscaAssociado);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.idAssociado == intValorBusca ||
                                         x.nome.Contains(buscaAssociado) || x.razaoSocial.Contains(buscaAssociado) ||
                                         x.nroDocumento == valorBuscaSoNumeros || x.rg == buscaAssociado ||
                                         x.nroAssociado == intValorBusca);
            }

		    if (idEntidadeArquivo > 0) {
			    query = query.Where(x => x.idEntidadeArquivo == idEntidadeArquivo);
		    }

			if (!String.IsNullOrEmpty(formatoArquivo)) {
				if (formatoArquivo == "img") {
					query = query.Where(x => ArquivoUploadExtensao.LISTATIPOIMG.Contains(x.extensao));
				} else {
					query = query.Where(x => ArquivoUploadExtensao.LISTATIPODOC.Contains(x.extensao));
				}
			}

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.legenda.Contains(valorBusca) || x.titulo.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}

        public List<string> carregarArquivosExportacao(List<int> idsArquivos) {

            var query = from Arq in db.ArquivoAssociadoVW
                        where !String.IsNullOrEmpty(Arq.path)
                        select Arq;

            if (idsArquivos.Any()) {

                var listaUrl = query.Where(x => idsArquivos.Contains(x.idArquivoUpload)).Select(x => x.path);

                return listaUrl.ToList();

            }

            return new List<string>();
        }
    }
}