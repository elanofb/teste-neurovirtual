using System;
using System.Json;
using DAL.Profissoes;
using UTIL.Resources;

namespace BLL.Profissoes {

    public class ProfissaoCadastroBL : ProfissaoConsultaBL, IProfissaoCadastroBL {

        //
        public ProfissaoCadastroBL(){

        }

        //Salvar um novo registro ou atualizar um existente
        public bool salvar(Profissao OProfissao) {

            OProfissao.descricao = OProfissao.descricao.abreviar(100);

            bool flagSucesso = false;

            if(OProfissao.id > 0) {
                flagSucesso = this.atualizar(OProfissao);
            }

            if(OProfissao.id == 0) {
                flagSucesso = this.inserir(OProfissao);
            }
            
            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        //Inserir Profissao, Pessoa e lista de Endereços vinculados
        private bool inserir(Profissao OProfissao) {

            OProfissao.setDefaultInsertValues();

            db.Profissao.Add(OProfissao);

            db.SaveChanges();

            return OProfissao.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da Profissao, Pessoa e lista de endereços
        private bool atualizar(Profissao OProfissao) {

            //Localizar existentes no banco
            Profissao dbProfissao = this.carregar(OProfissao.id);

            if (dbProfissao == null) {
                return false;
            }

            //Configurar valores padrão
            OProfissao.setDefaultUpdateValues<Profissao>();

            //Atualizacao da Profissao
            var ProfissaoEntry = db.Entry(dbProfissao);
            ProfissaoEntry.CurrentValues.SetValues(OProfissao);
            ProfissaoEntry.ignoreFields<Profissao>();

            db.SaveChanges();
            return OProfissao.id > 0;
        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
			var retorno = new JsonMessageStatus();

			var item = this.carregar(id);

			if (item == null) {
				retorno.error = true;
				retorno.message = NotificationMessages.invalid_register_id;
			} else {
				item.ativo = (item.ativo != true);
				db.SaveChanges();
				retorno.active = item.ativo == true ? "S" : "N";
				retorno.message = NotificationMessages.updateSuccess;
			}
			return retorno;
		}

    }
}