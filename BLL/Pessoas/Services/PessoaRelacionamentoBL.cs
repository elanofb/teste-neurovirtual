using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Pessoas;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Arquivos;
using DAL.Permissao.Security.Extensions;

namespace BLL.Pessoas {

    public class PessoaRelacionamentoBL : DefaultBL, IPessoaRelacionamentoBL {

        
        
        //Construtor
        public PessoaRelacionamentoBL() {
        }

        //Carregamento de um registro específico
        public PessoaRelacionamento carregar(int id) {

            var query = (from PesRel in db.PessoaRelacionamento
                                         .Include(x => x.Pessoa)
                                         .Include(x => x.OcorrenciaRelacionamento)
										 .Include(x => x.UsuarioCadastro)
                         where
                             PesRel.id == id &&
                             PesRel.flagExcluido == "N"
                         select
                             PesRel
                        );

            return query.FirstOrDefault();

        }

        //Listagem de registros de acordo com parametros informados
        public IQueryable<PessoaRelacionamento> listar(int idPessoa, int idOcorrenciaRelacionamento) {

            var query = from PesRel in db.PessoaRelacionamento
                                    .Include(x => x.Pessoa)
                                    .Include(x => x.OcorrenciaRelacionamento)
									.Include(x => x.UsuarioCadastro)
                                    
                        where PesRel.flagExcluido == "N"
                        select PesRel;

            if (idPessoa > 0) {
                query = query.Where(x => x.idPessoa == idPessoa);
            }

            if (idOcorrenciaRelacionamento > 0) {
                query = query.Where(x => x.idOcorrenciaRelacionamento == idOcorrenciaRelacionamento);
            }

            return query.AsNoTracking();
        }

        //Persistir e inserir um novo registro 
        public PessoaRelacionamento salvar(int idPessoa, int idOcorrencia, int idUsuario, string observacoes) {

            var Historico = new PessoaRelacionamento();

            Historico.idPessoa = idPessoa;

            Historico.idOcorrenciaRelacionamento = idOcorrencia;

            Historico.idUsuarioCadastro = idUsuario;

            Historico.observacao = observacoes;

            Historico.dtOcorrencia = DateTime.Now;

            Historico.setDefaultInsertValues();
            
            db.PessoaRelacionamento.Add(Historico);

            db.SaveChanges();

            return Historico;
        }

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(PessoaRelacionamento OPessoaRelacionamento){
            
            OPessoaRelacionamento.Pessoa = null;
            OPessoaRelacionamento.OcorrenciaRelacionamento = null;
            
            if (OPessoaRelacionamento.id == 0) {
                return this.inserir(OPessoaRelacionamento);
            }

            return this.atualizar(OPessoaRelacionamento);

        }

        //Persistir e inserir um novo registro 
        private bool inserir(PessoaRelacionamento OPessoaRelacionamento) {

            OPessoaRelacionamento.setDefaultInsertValues<PessoaRelacionamento>();

            db.PessoaRelacionamento.Add(OPessoaRelacionamento);

            db.SaveChanges();

            var retorno = OPessoaRelacionamento.id > 0;

            return retorno;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(PessoaRelacionamento OPessoaRelacionamento) {

            //Localizar existentes no banco
            PessoaRelacionamento dbPessoaRelacionamento = this.carregar(OPessoaRelacionamento.id);

            //Configurar valores padrão
            OPessoaRelacionamento.setDefaultUpdateValues<PessoaRelacionamento>();

            //Atualizacao do Relacionamento
            var PessoaRelacionamentoEntry = db.Entry(dbPessoaRelacionamento);
            PessoaRelacionamentoEntry.CurrentValues.SetValues(OPessoaRelacionamento);
            PessoaRelacionamentoEntry.ignoreFields<PessoaRelacionamento>(new string[] { "idPessoa", "idOcorrenciaRelacionamento" });

            db.SaveChanges();

            return OPessoaRelacionamento.id > 0;
        }



        // Excluir Registro
        public UtilRetorno excluir(int id) {
            int idUsuarioLogado = User.id();

            db.PessoaRelacionamento.Where(x => x.id == id)
                            .Update(x => new PessoaRelacionamento { flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now });

            UtilRetorno Retorno = UtilRetorno.getInstance();
            Retorno.flagError = false;
            return Retorno;
        }
    }
}