using Bismuth.Ldap.Utils;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Responses
{
	public class DeleteResponse : LdapResponse
	{
		public DeleteResponse (NetworkStream stream)
			: base (stream)
		{
			ReadResponse (new LdapStreamReader (stream), ProtocolOperation.DeleteResponse);
		}
	}
}

