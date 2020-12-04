using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.NaoAssociados {

    public class NaoAssociadoConsultaBL : DefaultBL, INaoAssociadoConsultaBL {



        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public Associado carregar(int idOrganizacaoParam, int idAssociado) {

            var query = from Ass in db.Associado
                            .Include(x => x.Organizacao)
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones)
                        where Ass.id == idAssociado && Ass.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE
                              && !Ass.dtExclusao.HasValue
                        select Ass;

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query.FirstOrDefault();
        }

        //Listar os associado considerando os parametros informados
        public IQueryable<Associado> listar(int idTipoAssociado, string flagSituacao, string valorBusca, string ativo, int? idOrganizacaoInf = null) {

            if (idOrganizacaoInf.toInt() == 0) {

                idOrganizacaoInf = idOrganizacao;
            }

            var query = from Ass in db.Associado.AsNoTracking()
                .Include(x => x.Pessoa)
                .Include(x => x.Pessoa.listaEnderecos)
                .Include(x => x.Pessoa.listaEmails)
                .Include(x => x.Pessoa.listaTelefones)
                .Include(x => x.TipoAssociado)
                .Include(x => x.TipoAssociado.Categoria)
                        where
                            Ass.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE
                            && !Ass.dtExclusao.HasValue
                        select Ass;

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

            if (idOrganizacaoInf > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoInf);
            }


            return query;
        }

        /// <summary>
        /// Montagem de query para consultas
        /// </summary>
        public IQueryable<Associado> query(int idOrganizacaoParam) {

            if (idOrganizacaoParam == 0) {

                idOrganizacaoParam = idOrganizacao;
            }
            
            var query = from Ass in db.Associado.AsNoTracking()
                                    .Include(x => x.Pessoa)
                        where
                            Ass.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE
                            && !Ass.dtExclusao.HasValue
                        select Ass;

            if (idOrganizacaoParam > 0) {

                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);

            }

            return query;
        }
    }
}