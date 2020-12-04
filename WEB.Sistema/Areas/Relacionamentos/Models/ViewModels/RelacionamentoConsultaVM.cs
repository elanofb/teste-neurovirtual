using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using BLL.Arquivos;
using BLL.Relacionamentos;
using DAL.Entities;
using DAL.Relacionamentos;

namespace WEB.Areas.Relacionamentos.ViewModels {

    public class RelacionamentoConsultaVM {

        // Atributos Serviços
        private IPessoaRelacionamentoVWBL _PessoaRelacionamentoVWBL;
        private IArquivoUploadBL _ArquivoUploadBL; 

        // Propriedades Serviços
        private IPessoaRelacionamentoVWBL OPessoaRelacionamentoVWBL => _PessoaRelacionamentoVWBL = _PessoaRelacionamentoVWBL ?? new PessoaRelacionamentoVWBL();
        
        // Propriedades
        public IPagedList<PessoaRelacionamentoVW> listaPessoaRelacionamentos { get; set; }
        private IArquivoUploadBL OArquivoUploadBL => (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL());

        //
        public RelacionamentoConsultaVM() {
            
            this.listaPessoaRelacionamentos = new List<PessoaRelacionamentoVW>().ToPagedList(1, 20);
            
        }
        
        // 
        public IQueryable<PessoaRelacionamentoVW> montarQuery() {

            var idPessoa = UtilRequest.getInt32("idPessoa");

            var idsOcorrencias = UtilRequest.getListInt("idsOcorrencias");

            var dtOcorrenciaInicial = UtilRequest.getDateTime("dtOcorrenciaInicial");
            var dtOcorrenciaFinal = UtilRequest.getDateTime("dtOcorrenciaFinal");
            
            var idUsuarioCadastro = UtilRequest.getInt32("idUsuarioCadastro");            
            
            var valorBusca = UtilRequest.getString("valorBusca");

            var flagTemArquivos = UtilRequest.getString("flagTemArquivos");
            
            var query = this.OPessoaRelacionamentoVWBL.listar(valorBusca);

            if (idPessoa > 0) {
                query = query.Where(x => x.idPessoa == idPessoa);
            }

            if (idsOcorrencias?.Any() == true) {
                query = query.Where(x => idsOcorrencias.Contains(x.idOcorrenciaRelacionamento));
            }

            if (dtOcorrenciaInicial.HasValue) {
                query = query.Where(x => x.dtOcorrencia >= dtOcorrenciaInicial);
            }
            
            if (dtOcorrenciaFinal.HasValue) {
                var dtFiltro = dtOcorrenciaFinal.Value.AddDays(1); 
                query = query.Where(x => x.dtOcorrencia < dtFiltro);
            }
            
            if (idUsuarioCadastro > 0) {
                query = query.Where(x => x.idUsuarioCadastro == idUsuarioCadastro);
            }

            if (flagTemArquivos == "S") {
			    
                var ids = query.Select(x => x.id).ToList();
                var idsComArquivo = OArquivoUploadBL.listarDocumentos(0, EntityTypes.PESSOADOCUMENTO_RELACIONAMENTO).Where(x => ids.Contains(x.idReferenciaEntidade)).Select(x => x.idReferenciaEntidade).ToList();

                query = query.Where(x => idsComArquivo.Contains(x.id));
            }
            
            return query;
        }


    }

}