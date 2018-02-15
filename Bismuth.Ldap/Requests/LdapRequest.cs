using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public abstract class LdapRequest
	{
		public int MessageId { get; protected set; }

		public ProtocolOperation Protocol { get; protected set; }

		public LdapRequest (int messageId, ProtocolOperation protocol)
		{
			MessageId = messageId;
			Protocol = protocol;
		}

		public abstract byte [] ToBytes ();

		public abstract LdapResponse GetResponse (NetworkStream stream);
	}
}

