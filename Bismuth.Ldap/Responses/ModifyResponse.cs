using Bismuth.Ldap.Utils;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Responses
{
	public class ModifyResponse : LdapResponse
	{
		public ModifyResponse (NetworkStream stream)
			: base (stream)
		{
			ReadResponse (new LdapStreamReader (stream), ProtocolOperation.ModifyResponse);
		}
	}
}

