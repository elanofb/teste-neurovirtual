using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using BLL.Configuracoes;
using DAL.Configuracoes;
using DAL.AssociadosContribuicoes;
using BLL.Associados;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoBL : DefaultBL, IAssociadoContribuicaoBL {

        //Atributos
        protected AssociadoBL _AssociadoBL;

        //Propriedades
        protected AssociadoBL OAssociadoBL => (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL());
        protected ConfiguracaoContribuicao OConfiguracaoContribuicao { get; set; }

        //Construtor
        public AssociadoContribuicaoBL() {
            this.OConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar();
        }

        //
        public IQueryable<AssociadoContribuicao> query(int? idOrganizacaoParam = null) {
            
            var query = from Obj in db.AssociadoContribuicao
                        where
                            Obj.dtExclusao == null
                        select Obj;

            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregar o Registro através do ID
        public AssociadoContribuicao carregar(int id) {

            var query = this.query().Include(p => p.Associado)
                                    .Include(p => p.Associado.Pessoa)
                                    .Include(p => p.Contribuicao)
                                    .Include(p => p.TipoAssociado);
            
            query = query.condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }

        //Listagem de registros conforme parametros informados
        public virtual IQueryable<AssociadoContribuicao> listar(int idContribuicao, int idAssociado, bool? flagIsento, bool? flagPago, string valorBusca = "") {

            var query = this.query().Include(p => p.Associado).Include(x => x.Associado)
                                    .Include(x => x.Associado.Pessoa).Include(x => x.Contribuicao)
                                    .Include(x => x.TipoAssociado).Include(x => x.UsuarioCadastro)
                                    .Where(x => x.idAssociadoContribuicaoPrincipal == null && 
                                                x.Associado.dtExclusao == null && x.Contribuicao.dtCancelamento == null);

            query = query.condicoesSeguranca();

            if (idContribuicao > 0) {
                query = query.Where(x => x.idContribuicao == idContribuicao);
            }

            if (idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }

            if (flagIsento.HasValue) {
                query = query.Where(x => x.flagIsento == flagIsento);
            }

            if (flagPago == true) {
                query = query.Where(x => x.dtPagamento != null);
            }

            if (flagPago == false) {
                query = query.Where(x => x.dtPagamento == null);
            }

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.Associado.Pessoa.nome.Contains(valorBusca) || x.Associado.Pessoa.razaoSocial.Contains(valorBusca));
            }

            return query;
        }

        /// <summary>
        /// Atualizar valores
        /// </summary>
	    public UtilRetorno atualizarValor(AssociadoContribuicao OAssociadoContribuicao) {

            if (OAssociadoContribuicao.valorAtual <= 0) {

                return UtilRetorno.newInstance(true, "O valor informado é inválido.");

            }

            db.AssociadoContribuicao.Where(x => x.id == OAssociadoContribuicao.id)
                                    .Update(x => new AssociadoContribuicao { valorAtual = OAssociadoContribuicao.valorAtual, observacoes = "Valor configurado manualmente" });

            return UtilRetorno.newInstance(false, "O valor atual da contribuição foi configurado com sucesso.");
        }
    }
}
