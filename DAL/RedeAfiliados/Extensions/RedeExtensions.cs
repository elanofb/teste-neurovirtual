using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;

namespace DAL.RedeAfiliados.Extensions {

    public static class RedeExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static bool flagTemTodos(this RedeBinariaBase Rede) {

            var listaMembros = Rede.toListaMembros();

            return listaMembros.All(x => x.id > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Associado proximoSemFilho(this List<Associado> listaMembros) {

            var Membro = listaMembros[0];
            
            for (int i = 1; i < listaMembros.Count; i++) {

                var Item = listaMembros[i];

                if (Item.id > 0) {

                    continue;
                }

                int indicePai = i - 1;
                    
                Membro = listaMembros[indicePai];

                break;

            }

            return Membro;
        }        
        
        /// <summary>
        /// 
        /// </summary>
        public static List<Associado> toListaMembros(this RedeBinariaBase Rede) {

            var Membro = new Associado {id = Rede.idMembro.toInt(), nroAssociado = Rede.nroMembro};
            
            var Nivel01 = new Associado();
            
            var Nivel02 = new Associado();
            
            var Nivel03 = new Associado();
            
            var Nivel04 = new Associado();
            
            var Nivel05 = new Associado();
            
            var Nivel06 = new Associado();
            
            var Nivel07 = new Associado();
            
            var Nivel08 = new Associado();
            
            var Nivel09 = new Associado();
            
            var Nivel10 = new Associado();

            if (Rede.idMembroNivel01 > 0) {
                
                Nivel01 = new Associado {id = Rede.idMembroNivel01.toInt(), nroAssociado = Rede.nroMembroNivel01};
            }

            if (Rede.idMembroNivel02 > 0) {
                
                Nivel02 = new Associado {id = Rede.idMembroNivel02.toInt(), nroAssociado = Rede.nroMembroNivel02};
            }

            if (Rede.idMembroNivel03 > 0) {
                
                Nivel03 = new Associado {id = Rede.idMembroNivel03.toInt(), nroAssociado = Rede.nroMembroNivel03};
            }

            if (Rede.idMembroNivel04 > 0) {
                
                Nivel04 = new Associado {id = Rede.idMembroNivel04.toInt(), nroAssociado = Rede.nroMembroNivel04};
            }

            if (Rede.idMembroNivel05 > 0) {
                
                Nivel05 = new Associado {id = Rede.idMembroNivel05.toInt(), nroAssociado = Rede.nroMembroNivel05};
            }

            if (Rede.idMembroNivel06 > 0) {
                
                Nivel06 = new Associado {id = Rede.idMembroNivel06.toInt(), nroAssociado = Rede.nroMembroNivel06};
            }

            if (Rede.idMembroNivel07 > 0) {
                
                Nivel07 = new Associado {id = Rede.idMembroNivel07.toInt(), nroAssociado = Rede.nroMembroNivel07};
            }

            if (Rede.idMembroNivel08 > 0) {
                
                Nivel08 = new Associado {id = Rede.idMembroNivel08.toInt(), nroAssociado = Rede.nroMembroNivel08};
            }

            if (Rede.idMembroNivel09 > 0) {
                
                Nivel09 = new Associado {id = Rede.idMembroNivel09.toInt(), nroAssociado = Rede.nroMembroNivel09};
            }

            if (Rede.idMembroNivel10 > 0) {
                
                Nivel10 = new Associado {id = Rede.idMembroNivel10.toInt(), nroAssociado = Rede.nroMembroNivel10};
            }

            var listaMembros = new List<Associado>();

            listaMembros.Add(Membro);
            
            listaMembros.Add(Nivel01);
            
            listaMembros.Add(Nivel02);
            
            listaMembros.Add(Nivel03);
            
            listaMembros.Add(Nivel04);
            
            listaMembros.Add(Nivel05);
            
            listaMembros.Add(Nivel06);
            
            listaMembros.Add(Nivel07);
            
            listaMembros.Add(Nivel08);
            
            listaMembros.Add(Nivel09);
            
            listaMembros.Add(Nivel10);

            return listaMembros;
        }
    }

}
