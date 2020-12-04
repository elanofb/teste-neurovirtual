using System.Collections.Generic;
using DAL.Associados;

namespace DAL.RedeAfiliados.DTO {

    public class RedeUnilevel {
        
        public byte nivel { get; set; }
        
        public List<AssociadoDadosBasicos> listaMembros { get; set; }
    }

}
