using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public class AssociadoConsultaBL : DefaultBL, IAssociadoConsultaBL {

        //
        public IQueryable<Associado> query(int? idOrganizacaoParam = null) {
            
            var query = from Ass in db.Associado
                        where 
                            Ass.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR && 
                            
                             !Ass.dtExclusao.HasValue
                        select Ass;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            return query;

        }
        
        //
        public IQueryable<Associado> queryNoFilter(int? idOrganizacaoParam = null) {
            
            var query = from Ass in db.Associado
                where                                                  
                    !Ass.dtExclusao.HasValue
                select Ass;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }
            
            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
            return query;

        }
        
        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public Associado carregar(int id) {

            var query = this.query().condicoesSeguranca();
            
            return query.FirstOrDefault(x => x.id == id);

        }

        //Listar os associado considerando os parametros informados
        public IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo) {
            
            var query = this.query().condicoesSeguranca().AsNoTracking();

            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.razaoSocial.Contains(valorBusca) ||
                                         x.Pessoa.nroDocumento == valorBuscaSoNumeros || x.Pessoa.rg == valorBusca ||
                                         x.nroAssociado == intValorBusca ||
                                         x.Pessoa.listaEmails.Any(y => y.email.Contains(valorBusca)));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }


            if (idTipoAssociado > 0) {
                query = query.Where(x => x.idTipoAssociado == idTipoAssociado);
            }
            
            return query;
        }

    }
}