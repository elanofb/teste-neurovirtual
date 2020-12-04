using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Associados;
using EntityFramework.Extensions;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace BLL.NaoAssociados {

    public class NaoAssociadoBL : DefaultBL, INaoAssociadoBL {

        //Carregar o associado fazendo join com as tabelas necessárias através do ID
        public IQueryable<Associado> query(int? idOrganizacaoParam = null) {

            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                where
                Ass.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE
                && !Ass.dtExclusao.HasValue
                select Ass;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;
        }

        //Carregar não associado fazendo join com as tabelas necessárias através do ID
        public IQueryable<Associado> carregar(int idAssociado) {

            var query = this.query().condicoesSeguranca()
                            .Include(x => x.Organizacao)
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            .Include(x => x.Pessoa.listaEmails)
                            .Include(x => x.Pessoa.listaTelefones);

            return query.Where(x => x.id == idAssociado);
        }

        //Carregar nao associado fazendo join com as tabelas necessarias através do ID
        public Associado carregarPorPessoa(int idPessoa) {

            var query = this.query().condicoesSeguranca()
                                    .Include(x => x.Pessoa)
                                    .Include(x => x.Pessoa.CidadeOrigem)
                                    .Include(x => x.Pessoa.listaEnderecos)
                                    .Include(x => x.Pessoa.listaEmails)
                                    .Include(x => x.Pessoa.listaTelefones);
            
            return query.FirstOrDefault(x => x.idPessoa == idPessoa);
        }

        //Listar os nao associados considerando os parametros informados
        public IQueryable<Associado> listar(string valorBusca, string ativo) {

            var query = this.query().condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.razaoSocial.Contains(valorBusca) ||
                                         x.Pessoa.nroDocumento == valorBuscaSoNumeros || x.Pessoa.rg == valorBusca ||
                                         x.nroAssociado == intValorBusca || x.Pessoa.listaEmails.Any(y => y.email.Contains(valorBusca)));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }
            
            return query;
        }

        //Verificar se já existe um registro com o documento/email informado, que possua id diferente do informado
        public bool existe(int idTipoDocumento, string nroDocumento, string email, string login, int idDesconsiderado) {
           
			nroDocumento = UtilString.onlyNumber(nroDocumento);

            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                        where Ass.id != idDesconsiderado &&  Ass.dtExclusao == null
                        select Ass;

            query = query.condicoesSeguranca();

            if (!String.IsNullOrEmpty(nroDocumento)) {
                query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento && x.Pessoa.idTipoDocumento == idTipoDocumento);
            }

            if (!String.IsNullOrEmpty(email)) {
                query = query.Where(x => x.Pessoa.ToEmailList().Contains(email));
            }

            if (!String.IsNullOrEmpty(login)) {
                query = query.Where(x => x.Pessoa.login == login && !string.IsNullOrEmpty(login));
            } else {
                query = query.condicoesSeguranca();
            }

            var OAssociado = query.FirstOrDefault();

            return (OAssociado != null);
        }

        //Assinar o evento de exclusao
        //Fazer a exclusao logica do não associado
        public bool excluir(int idAssociado, string motivo) {

            var ONaoAssociado = this.carregar(idAssociado).condicoesSeguranca().FirstOrDefault();

            if (ONaoAssociado == null) {
                return false;
            }

            //this.onAssociadoExcluido.subscribe(new AssociadoExcluidoHandler());

            int idUsuarioLogado = User.id();

            this.db.Associado.Where(x => x.id == idAssociado)
                    .Update(x => new Associado { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuarioLogado, observacaoDesligamento = motivo});

            //this.onAssociadoExcluido.publish((idAssociado as object));
            return true;
        }
    }
}