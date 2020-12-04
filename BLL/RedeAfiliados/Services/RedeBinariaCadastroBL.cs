using System;
using System.Linq;
using BLL.Services;
using DAL.RedeAfiliados;
using DAL.RedeAfiliados.DTO;

namespace BLL.RedeAfiliados.Services {

    public class RedeBinariaCadastroBL : DefaultBL, IRedeBinariaCadastroBL {

        //
        public RedeBinariaCadastroBL(){

        }

        //Salvar um novo registro ou atualizar um existente
        public RedeBinaria salvar(NovoMembroRede ONovoMembro) {

            var ORedeBinaria = this.db.RedeBinaria.FirstOrDefault(x => x.idMembro == ONovoMembro.idMembroPai);

            if (ORedeBinaria == null) {
                
                ORedeBinaria = new RedeBinaria();

                ORedeBinaria.idMembro = ONovoMembro.idMembroPai;

                if (ONovoMembro.flagEsquerda == true) {
                    
                    ORedeBinaria.idMembroEsquerda = ONovoMembro.idMembro;
                    
                    ORedeBinaria.dtCadastroEsquerda = DateTime.Now;
                    
                } else {
                    
                    ORedeBinaria.idMembroDireita = ONovoMembro.idMembro;
                    
                    ORedeBinaria.dtCadastroDireita = DateTime.Now;
                }
            }

            if(ORedeBinaria.id > 0) {
                this.atualizar(ORedeBinaria);
            }

            if(ORedeBinaria.id == 0) {
                this.inserir(ORedeBinaria);
            }

            return ORedeBinaria;

        }
        
        //Salvar um novo registro ou atualizar um existente
        public bool salvar(RedeBinaria ORedeBinaria) {

            bool flagSucesso = false;

            if(ORedeBinaria.id > 0) {
                flagSucesso = this.atualizar(ORedeBinaria);
            }

            if(ORedeBinaria.id == 0) {
                flagSucesso = this.inserir(ORedeBinaria);
            }
            
            return flagSucesso;

        }

        //Persistir e inserir um novo registro 
        //Inserir RedeBinaria, Pessoa e lista de Endereços vinculados
        private bool inserir(RedeBinaria ORedeBinaria) {

            ORedeBinaria.setDefaultInsertValues();

            db.RedeBinaria.Add(ORedeBinaria);

            db.SaveChanges();

            return ORedeBinaria.id > 0;
        }

        //Persistir e atualizar um registro existente 
        //Atualizar dados da RedeBinaria, Pessoa e lista de endereços
        private bool atualizar(RedeBinaria ORedeBinaria) {

            //Localizar existentes no banco
            RedeBinaria dbRedeBinaria = this.db.RedeBinaria.Find(ORedeBinaria.id);

            if (dbRedeBinaria == null) {
                return false;
            }

            //Configurar valores padrão
            ORedeBinaria.setDefaultUpdateValues();

            //Atualizacao da RedeBinaria
            var RedeBinariaEntry = db.Entry(dbRedeBinaria);
            RedeBinariaEntry.CurrentValues.SetValues(ORedeBinaria);
            RedeBinariaEntry.ignoreFields( new []{"idMembro"});

            db.SaveChanges();
            return ORedeBinaria.id > 0;
        }

    }
}