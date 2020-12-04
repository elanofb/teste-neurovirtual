namespace BLL.Core.Events {

	public interface IHandler<Tmessage> {

		void execute(Tmessage source);
	}
}