using System;
using System.Linq;
using BLL.Associados;
using DAL.Associados;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoConsultaVM {

        // Atributos
        private IAssociadoRelatorioVWBL _AssociadoRelatorioVWBL;

        // Propriedades
        private IAssociadoRelatorioVWBL OAssociadoRelatorioVWBL => _AssociadoRelatorioVWBL = _AssociadoRelatorioVWBL ?? new AssociadoRelatorioVWBL();


        public IQueryable<AssociadoRelatorioVW> montarQuery(AssociadoConsultaForm ViewModel) {

            var query = this.OAssociadoRelatorioVWBL.listar(0, ViewModel.flagSituacaoContribuicao, ViewModel.valorBusca, ViewModel.ativo);
            
            if (ViewModel.idsTipoAssociado.Any()) {
                query = query.Where(x => ViewModel.idsTipoAssociado.Contains(x.idTipoAssociado.Value));
            }

            if (ViewModel.dtCadastroInicio.HasValue) {
                query = query.Where(x => x.dtCadastro >= ViewModel.dtCadastroInicio);
            }

            if (ViewModel.dtCadastroFim.HasValue) {
                var dtFiltro = ViewModel.dtCadastroFim.Value.AddDays(1);
                query = query.Where(x => x.dtCadastro < dtFiltro);
            }

            if (!ViewModel.ativo.isEmpty()) {
                query = query.Where(x => x.ativo.Equals(ViewModel.ativo));
            }

            if (!ViewModel.flagTipoPessoa.isEmpty()) {
                query = query.Where(x => x.flagTipoPessoa.Equals(ViewModel.flagTipoPessoa));
            }

            if (!ViewModel.flagSexo.isEmpty()) {
                query = query.Where(x => x.flagSexo.Equals(ViewModel.flagSexo));
            }                        
            
            query = this.montarBuscaLote(query, ViewModel.valorBuscaLote);

            return query;
        }

        private IQueryable<AssociadoRelatorioVW> montarBuscaLote(IQueryable<AssociadoRelatorioVW> query, string valorBuscaLote) {
            
            if (!valorBuscaLote.isEmpty()) {

                string[] separadores = { "\r\n" };
                string[] valoresBusca = valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                
                var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();

                var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();

                query = query.Where(x => valoresNumericos.Contains(x.nroAssociado) ||
                                         valoresSoNumeros.Contains(x.nroDocumento));
                
            }

            return query;

        } 
        
    }
}