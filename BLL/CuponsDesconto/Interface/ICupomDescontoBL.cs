using System;
using System.Linq;
using System.Json;
using DAL.CuponsDesconto;
using DAL.Financeiro;

namespace BLL.CuponsDesconto {

	public interface ICupomDescontoBL {

        IQueryable<CupomDesconto> listar(string valorBusca = "", string ativo = "S");

        CupomDesconto carregar(int id);

        CupomDesconto carregarPorCodigo(string codigo);

        bool salvar(CupomDesconto OCupomDesconto);

        JsonMessage enviarCupom(int idCupomDesconto);

        UtilRetorno excluir(int id);

        bool registrarUso(int idCupomDesconto);

	}
}
