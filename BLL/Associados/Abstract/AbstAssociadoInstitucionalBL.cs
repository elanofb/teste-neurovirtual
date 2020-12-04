using System;
using System.Data.Entity;
using System.Linq;
using DAL.AreasAtuacao;
using DAL.Associados;
using EntityFramework.Extensions;
using BLL.Email;
using System.Collections.Generic;
using DAL.Pessoas;
using BLL.Services;

namespace BLL.Associados {

	public abstract class AbstAssociadoInstitucionalBL : DefaultBL {

        public abstract bool salvar(Associado OAssociado);

        //
        public Associado carregar(int id) { 
            var query = from Pes in db.Associado
                            .Include(x => x.Pessoa)
                            .Include(x => x.Pessoa.CidadeOrigem)
                            .Include(x => x.Pessoa.listaEnderecos)
                            //.Include(x => x.listaTitulo)
                        where Pes.id == id && !Pes.dtExclusao.HasValue
                        select Pes;
            Associado OAssociado = query.FirstOrDefault();
            return OAssociado;
        }

        //Verificar se já existe um registro com o documento/email informado, no entanto, que possua id diferente do informado
        public bool existe(int idTipoDocumento, string documento, string email, string login, int id) {

            var query = from Pes in db.Associado
                        where Pes.Pessoa.id != id && Pes.Pessoa.flagExcluido == "N" && Pes.dtExclusao == null
                        select Pes;

            if (idTipoDocumento > 0 && !String.IsNullOrEmpty(documento)) {
                query = query.Where(x => x.Pessoa.idTipoDocumento == idTipoDocumento && x.Pessoa.nroDocumento == documento);
            }

            if (!String.IsNullOrEmpty(email)) {
                query = query.Where(x => x.Pessoa.emailPrincipal == email || x.Pessoa.emailSecundario == email);
            }

            if (!String.IsNullOrEmpty(login)) {
                query = query.Where(x => x.Pessoa.login == login);
            }

            var OPessoa = query.Take(1).FirstOrDefault();
            return (OPessoa == null ? false : true);
        }

        //
        public bool excluir(int[] ids) {
            this.db.AreaAtuacao.Where(x => ids.Contains(x.id))
                .Update(x => new AreaAtuacao { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = this.db.AreaAtuacao.Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
            return (listaCheck.Count == 0);
        }

        //
        public object getAutoComplete(string term) {
            var query = from p in db.Pessoa
                        where p.nome.Contains(term) && !p.flagExcluido.Equals("S")
                        select new {
                            value = p.nome,
                            id = p.id,
                            label = p.nome,
                            telPrincipal = p.dddTelPrincipal + p.nroTelPrincipal,
                            telSecundario = p.dddTelSecundario + p.nroTelSecundario,
                            cnpf = p.nroDocumento,
                            emailPrincipal = p.emailPrincipal,
                            emailSecundario = p.emailSecundario
                        };
            return query.ToList();
        }

        //
        public Associado login(string login, string senha) {
            string cryptSenha = UtilCrypt.SHA512(senha);

            var query = from Ass in db.Associado.Include(x => x.Pessoa).Include(x => x.Pessoa.listaEnderecos)
                        where (Ass.Pessoa.login == login || Ass.Pessoa.emailPrincipal == login) && Ass.Pessoa.senha == cryptSenha && Ass.dtExclusao == null
                        select Ass;

            Associado OAssociado = query.FirstOrDefault();
            return OAssociado;
        }

        //
        public bool alterarSenha(int idUsuario, string senha) {
            if (idUsuario > 0) {
                var User = this.carregar(idUsuario);
                string cryptSenha = UtilCrypt.SHA512(senha);

                User.Pessoa.senha = cryptSenha;
                db.SaveChanges();

                return true;
            }

            return false;
        }

        //
        public Associado verificarPorEmail(string email) {
            var query = from Ass in db.Associado.Include(x => x.Pessoa)
                        where Ass.Pessoa.listaEmails.Select(i => i.email).Contains(email)
                        select Ass;
            Associado OAssociado = query.FirstOrDefault();
            return OAssociado;
        }

        //
        public bool verificarSenha(int idUsuario, string senha) {
            if (idUsuario == 0 || String.IsNullOrEmpty(senha)) {
                return false;
            }

            string cryptSenha = UtilCrypt.SHA512(senha);
            Associado OAssociado = db.Associado.Include(x => x.Pessoa).Where(x => x.id == idUsuario && x.Pessoa.senha == cryptSenha).FirstOrDefault();
            if (OAssociado == null) {
                return false;
            }

            return true;
        }
	}
}
