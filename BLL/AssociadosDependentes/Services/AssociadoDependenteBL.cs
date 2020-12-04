using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.AssociadosDependentes {

    public class AssociadoDependenteBL : DefaultBL, IAssociadoDependenteBL {

        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public Associado carregar(int idAssociado) {

            var query = from Ass in db.Associado
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones)
                        where Ass.id == idAssociado
                        && !Ass.dtExclusao.HasValue
                        select Ass;

            query = query.condicoesSeguranca();

            return query.FirstOrDefault();
        }


        //Listar os associado considerando os parametros informados
        public IQueryable<Associado> listar(int idAssociadoEstipulante, string valorBusca, string ativo) {

            var query = from Ass in db.Associado.AsNoTracking()
                                      .Include(x => x.Pessoa)
                                      .Include(x => x.Pessoa.listaEnderecos)

                        where 
                            !Ass.dtExclusao.HasValue 
                        select Ass;

            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.razaoSocial.Contains(valorBusca) ||
                                         x.Pessoa.nroDocumento == valorBuscaSoNumeros || x.Pessoa.rg == valorBusca ||
                                         x.Pessoa.listaEmails.Any(y => y.email.Contains(valorBusca)) 
                                         );
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }


            query = query.condicoesSeguranca();

            return query;

        }

    }
}