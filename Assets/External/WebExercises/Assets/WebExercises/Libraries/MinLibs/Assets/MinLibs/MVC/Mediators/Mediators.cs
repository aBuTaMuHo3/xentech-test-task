using System;
using System.Collections.Generic;
using MinLibs.Utils;

namespace MinLibs.MVC
{
	public class Mediators : IMediators
	{
		[Inject] public IContext context;

		readonly IDictionary<Type, List<Type>> mediatingMap = new Dictionary<Type, List<Type>>();

		public void Map<TMediated, TMediator>() where TMediated : IMediated where TMediator : IMediator
		{
			var mediatorTypes = mediatingMap.Retrieve(typeof(TMediated));
			mediatorTypes.Add(typeof(TMediator));
		}

		public void Mediate(IMediated mediated)
		{
			var mediatedType = mediated.GetType();
			var hasMediators = Create(mediated, mediatedType);
			var mediatedInterfaces = mediatedType.GetInterfaces();
			mediatedInterfaces.Each(i => hasMediators |= Create(mediated, i));

			if (hasMediators)
			{
				mediated.HandleMediation();
			}
		}

		bool Create<T>(T mediated, Type mediatedType) where T : IMediated
		{
			var mediatorTypes = mediatingMap.Retrieve(mediatedType);
			mediatorTypes.Each(t => Get(t).Init(mediated));

			return mediatorTypes.Count > 0;
		}

		IMediator Get(Type type)
		{
			//if (!type.IsInterface && !context.Has(type)) {
			//	context.RegisterType(type, RegisterFlags.NoCache);
			//}

			return context.Get<IMediator>(type);
		}
	}
}
