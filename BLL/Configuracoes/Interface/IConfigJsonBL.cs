namespace BLL.Configuracoes {

	public interface IConfigJsonBL {

		T carregar<T>(string fileConfig) where T : class;

	}
}