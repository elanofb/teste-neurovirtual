using System;
using System.Json;
using System.Linq;
using BLL.Planos.Services;
using BLL.Services;
using DAL.Planos;
using DAL.Produtos;
using UTIL.Resources;

namespace BLL.Planos {

    public class PlanoCarreiraCadastroBL : DefaultBL, IPlanoCarreiraCadastroBL {
        
        //Atributos
        private IPlanoCarreiraAlteracaoDadoBL _PlanoCarreiraAlteracaoDadoBL;        
        
        //Propriedades
        private IPlanoCarreiraAlteracaoDadoBL OPlanoCarreiraAlteracaoDadoBL => _PlanoCarreiraAlteracaoDadoBL = _PlanoCarreiraAlteracaoDadoBL ?? new PlanoCarreiraAlteracaoDadoBL();
        
        //Verificar se deve-se atualizar um registro existente ou criar um novo
        public bool salvar(PlanoCarreira OPlanoCarreira) {
            
            var flagSucesso = false;
            
            if (OPlanoCarreira.id > 0) {
                flagSucesso = this.atualizar(OPlanoCarreira);
            }
            
            if (OPlanoCarreira.id == 0) {
                flagSucesso = this.inserir(OPlanoCarreira);
            }

            return flagSucesso;
        }

        //Persistir o objecto e salvar na base de dados
        private bool inserir(PlanoCarreira OPlanoCarreira) {
            
            OPlanoCarreira.setDefaultInsertValues();

            db.PlanoCarreira.Add(OPlanoCarreira);

            db.SaveChanges();

            return (OPlanoCarreira.id > 0);
        }

        //Persistir o objecto e atualizar informações
        private bool atualizar(PlanoCarreira OPlanoCarreira) {

            //Localizar existentes no banco
            var dbPlanoCarreira = db.PlanoCarreira.condicoesSeguranca().FirstOrDefault(x => x.id == OPlanoCarreira.id);
            
            if (dbPlanoCarreira == null) {
                return false;
            }
            
            var dbEntry = db.Entry(dbPlanoCarreira);
            
            OPlanoCarreira.setDefaultUpdateValues();
            
            dbEntry.CurrentValues.SetValues(OPlanoCarreira);
            
            dbEntry.ignoreFields();
            
            //Gerar log de alterações
            this.OPlanoCarreiraAlteracaoDadoBL.alterarCampo(OPlanoCarreira.id, "descricao", OPlanoCarreira.descricao, "Descrição");
            this.OPlanoCarreiraAlteracaoDadoBL.alterarCampo(OPlanoCarreira.id, "pontuacao", OPlanoCarreira.pontuacao.ToString(), "Pontuação");
            
            db.SaveChanges();                        
            
            return (OPlanoCarreira.id > 0);

        }

        //Alteracao de status
        public JsonMessageStatus alterarStatus(int id) {
            var retorno = new JsonMessageStatus();

            var item = db.PlanoCarreira.condicoesSeguranca().FirstOrDefault(x => x.id == id);

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