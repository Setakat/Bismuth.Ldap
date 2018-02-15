using System;

namespace Bismuth.Ldap
{
	public class Disposable : IDisposable
	{
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool freeManagedObjects)
		{

		}

		~Disposable ()
		{
			Dispose (false);
		}
	}
}

