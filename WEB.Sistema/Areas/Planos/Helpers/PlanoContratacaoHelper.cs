using System.Web.Mvc;
using System.Linq;
using DAL.Beneficiarios;
using DAL.Empresas;

namespace WEB.Areas.Planos.Helpers{

    public class PlanoContratacaoHelper{

        private static BeneficiarioBL _BeneficiarioBL;

        public static BeneficiarioBL getService()
        {
            _BeneficiarioBL = _BeneficiarioBL ?? new BeneficiarioBL();
            return _BeneficiarioBL;
        }

        //
        public static SelectList getComboEstipulante(string selected, int idBeneficiario)
        {

            Beneficiario OBeneficiario = getService().carregar(idBeneficiario);
            Empresa OEmpresa = OBeneficiario.Empresa;

            var list = new[] { 
                    new{id = "OPRO", nome = OBeneficiario.Pessoa.nome}
            }.ToList();

            if (OEmpresa != null)
            {
                list.Add(new { id = "EMPR", nome = OEmpresa.Pessoa.nome });

            }

            return new SelectList(list, "id", "nome", selected);
        }

        //
        public static SelectList getComboBandeirasCartao(int selected)
        {

            var list = new[] { 
                    new { id = "1", nome = "MasterCard"},
                    new { id = "1", nome = "VISA"},
                    new { id = "1", nome = "American Express"},
                    new { id = "1", nome = "Aura"},
                    new { id = "1", nome = "Elo"},
                    new { id = "1", nome = "Hipercard"},
                    new { id = "1", nome = "Sorocred"},
                    new { id = "1", nome = "Diners Club"}
            }.ToList();

            return new SelectList(list, "id", "nome", selected);
        }
    }
}