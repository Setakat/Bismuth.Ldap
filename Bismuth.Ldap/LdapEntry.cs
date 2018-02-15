using System;
using System.Collections.Generic;

namespace Bismuth.Ldap
{
	public class LdapEntry
	{
		public string DN { get; protected set; }

		public Dictionary<string, string> Properties { get; set; }

		public LdapEntry (string dn)
		{
			DN = dn;
			Properties = new Dictionary<string, string> ();
		}
	}
}

