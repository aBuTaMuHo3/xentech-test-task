using System;

namespace MinLibs.MVC
{
	public class NotRegisteredException : Exception
	{
		public NotRegisteredException (string message) : base(message)
		{
		}
	}

	public class AlreadyRegisteredException : Exception
	{
		public AlreadyRegisteredException (string message) : base(message)
		{
		}
	}

	public class CannotRegisterInterfaceAsValueException : Exception
	{
		public CannotRegisterInterfaceAsValueException (string message) : base(message)
		{
		}
	}

	public class AlreadyInjectedException : Exception
	{
		public AlreadyInjectedException (string message) : base(message)
		{
		}
	}
}
