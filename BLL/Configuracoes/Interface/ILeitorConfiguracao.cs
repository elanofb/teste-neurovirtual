namespace BLL.Configuracoes {

	public interface ILeitorConfiguracao {

		T carregar<T>() where T : class;

	}
}