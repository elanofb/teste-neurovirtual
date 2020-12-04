using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pessoas;
using DAL.Documentos;

namespace WEB.Areas.Pessoas.Extensions{
    public static class PessoaExtensions{

        // helper
        public static string formatarDocumento(this Pessoa OPessoa) {
			string documento = OPessoa.nroDocumento;

			if (OPessoa.idTipoDocumento == TipoDocumentoConst.CPF || OPessoa.idTipoDocumento == TipoDocumentoConst.CNPJ) { 
				return UtilString.formatCPFCNPJ(documento);
			}
            return documento;
        }

		//Formatar número de telefone principal
        public static string formatarTelPrincipal(this Pessoa OPessoa, bool flagDDI = false) {

            string telFormatado = "";

            var OPessoaTelefone = OPessoa?.listaTelefones?.FirstOrDefault(x => x.dtExclusao == null) ?? new PessoaTelefone();

            if (OPessoaTelefone.id > 0)
            {
                if (OPessoaTelefone.ddi > 0) { 
				    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
			    }

			    telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone) );
            }            

            return telFormatado;
        }

		//Formatar número de telefone secundario
        public static string formatarTelSecundario(this Pessoa OPessoa, bool flagDDI = false) {

			string telFormatado = "";

            var listaTelefones = OPessoa?.listaTelefones?.Take(2).ToList() ?? new List<PessoaTelefone>();
            var OPessoaTelefone = (listaTelefones.Count == 2) ? listaTelefones.LastOrDefault(x => x.dtExclusao == null) : null;

            if (UtilNumber.toInt32(OPessoaTelefone?.id) > 0)
            {
                if (OPessoaTelefone.ddi > 0) { 
				    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
			    }

			    telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone) );
            }            

            return telFormatado;
        }

		//Formatar número de telefone secundario
        public static string formatarTelTerciario(this Pessoa OPessoa, bool flagDDI = false) {

			string telFormatado = "";

            var listaTelefones = OPessoa?.listaTelefones?.Take(3).ToList() ?? new List<PessoaTelefone>();
            var OPessoaTelefone = (listaTelefones.Count == 3) ? listaTelefones.LastOrDefault(x => x.dtExclusao == null) : null;

            if (UtilNumber.toInt32(OPessoaTelefone?.id) > 0)
            {
                if (OPessoaTelefone.ddi > 0) { 
				    telFormatado = String.Concat(OPessoaTelefone.ddi, " ");
			    }

			    telFormatado = String.Concat(telFormatado, UtilString.formatPhone(OPessoaTelefone.nroTelefone) );
            }            

            return telFormatado;
        }

        //Retorna o primeiro e-mail cadastrado.
        public static string emailPrincipal(this Pessoa OPessoa) {

            string email = "";

            var listaEmail = OPessoa.ToEmailList();

            if (listaEmail.Count > 0)
            {                
			    email = listaEmail[0];
            }            

            return email;
        }

        //Retorna o segundo e-mail cadastrado.
        public static string emailSecundario(this Pessoa OPessoa) {

            string email = "";

            var listaEmail = OPessoa.ToEmailList();

            if (listaEmail.Count > 1)
            {                
			    email = listaEmail[1];
            }            

            return email;
        }
    }
}