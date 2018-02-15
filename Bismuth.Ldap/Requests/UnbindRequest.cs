using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class UnbindRequest : LdapRequest
	{
		public UnbindRequest (int messageId)
			: base (messageId, ProtocolOperation.Unbind)
		{
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol)
			).ToBytes ();
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			// UNbind requests don't return a reponse
			return null;
		}
	}
}

