using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Atendimentos;
using DAL.Atendimentos;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Atendimentos.Extensions {

    public static class AtendimentoUsuarioExtensions {

        // Atributos
        private static IAtendimentoConsultaBL _IAtendimentoConsultaBL;

        // Propriedades 
        private static IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();


        public static int? idAtendimentoAberto(this IPrincipal User) {

            var idUsuarioLogado = User.id();

            var listaStatusAberto = new List<int> { AtendimentoStatusConst.EM_ATENDIMENTO, AtendimentoStatusConst.RETORNO_REALIZADO };

            var OAtendimentoAberto = OAtendimentoConsultaBL.listar(true).FirstOrDefault(x => x.idUltimoUsuarioAtendimento == idUsuarioLogado &&
                                                                                     listaStatusAberto.Contains(x.idStatusAtendimento));

            return OAtendimentoAberto?.id.toInt();

        }

    }

}