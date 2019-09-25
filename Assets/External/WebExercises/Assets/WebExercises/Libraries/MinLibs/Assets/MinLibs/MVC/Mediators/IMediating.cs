using MinLibs.Signals;

namespace MinLibs.MVC
{
	public interface IMediating : IMediated
	{
		Signal<IMediated> OnMediate { get; set; }
	}
}
