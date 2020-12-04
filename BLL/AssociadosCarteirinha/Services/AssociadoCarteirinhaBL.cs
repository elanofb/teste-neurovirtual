using BLL.AssociadosCarteirinha.Events;
using BLL.Core.Events;
using DAL.AssociadosCarteirinha;
using EntityFramework.Extensions;
using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace BLL.AssociadosCarteireinha {

    public class AssociadoCarteirinhaBL : DefaultBL, IAssociadoCarteirinhaBL {

        //Eventos
        private readonly EventAggregator onEnvioCarteirinha = OnEnvioCarteirinha.getInstance;

        //Construtor
        public AssociadoCarteirinhaBL() {
            //Registrar a assinatura do Evento
             this.onEnvioCarteirinha.subscribe(new OnEnvioCarteirinhaHandler());
        }

        //Carregamento de um registro específico
        public AssociadoCarteirinha carregar(int id) {

            var query = (from AssCar in db.AssociadoCarteirinha
                                         .Include(x => x.Associado)
                         where
                             AssCar.id == id &&
                             AssCar.flagExcluido == "N"
                         select
                             AssCar
                        );

            return query.FirstOrDefault();

        }

        //Listagem de registros de acordo com parametros informados
        public IQueryable<AssociadoCarteirinha> listar(int idAssociado) {

            var query = from PesRel in db.AssociadoCarteirinha
                                    .Include(x => x.Associado)
                                    
                        where PesRel.flagExcluido == "N"
                        select PesRel;

            if (idAssociado > 0) {
                query = query.Where(x => x.idAssociado == idAssociado);
            }

            return query.AsNoTracking();
        }

        //Definir se é um insert ou update e enviar o registro para o banco de dados 
        public bool salvar(AssociadoCarteirinha OAssociadoCarteirinha) {
            
            var flagSucesso = false;

            if (OAssociadoCarteirinha.id == 0) {
                flagSucesso = this.inserir(OAssociadoCarteirinha);
            }

            flagSucesso = this.atualizar(OAssociadoCarteirinha);

            if (flagSucesso) {
                this.onEnvioCarteirinha.publish((OAssociadoCarteirinha as object));
            }

            return flagSucesso;
        }

        //Persistir e inserir um novo registro 
        private bool inserir(AssociadoCarteirinha OAssociadoCarteirinha) {

            OAssociadoCarteirinha.setDefaultInsertValues<AssociadoCarteirinha>();

            db.AssociadoCarteirinha.Add(OAssociadoCarteirinha);
            db.SaveChanges();
            return OAssociadoCarteirinha.id > 0;
        }

        //Persistir e atualizar um registro existente 
        private bool atualizar(AssociadoCarteirinha OAssociadoCarteirinha) {

            //Localizar existentes no banco
            AssociadoCarteirinha dbAssociadoCarteirinha = this.carregar(OAssociadoCarteirinha.id);

            //Configurar valores padrão
            OAssociadoCarteirinha.setDefaultUpdateValues<AssociadoCarteirinha>();

            //Atualizacao da Ocorrência de Envio de Carteirinha
            var AssociadoCarteirinhaEntry = db.Entry(dbAssociadoCarteirinha);
            AssociadoCarteirinhaEntry.CurrentValues.SetValues(OAssociadoCarteirinha);
            AssociadoCarteirinhaEntry.ignoreFields<AssociadoCarteirinha>(new string[] { "idAssociado" });

            db.SaveChanges();

            return OAssociadoCarteirinha.id > 0;
        }



        // Excluir Registro
        public UtilRetorno excluir(int id) {
            int idUsuarioLogado = User.id();

            db.AssociadoCarteirinha.Where(x => x.id == id)
                            .Update(x => new AssociadoCarteirinha { flagExcluido = "S", idUsuarioAlteracao = idUsuarioLogado, dtAlteracao = DateTime.Now });

            UtilRetorno Retorno = UtilRetorno.getInstance();
            Retorno.flagError = false;
            return Retorno;
        }
    }
}