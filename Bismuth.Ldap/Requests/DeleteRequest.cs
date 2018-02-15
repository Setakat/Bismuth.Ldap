using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class DeleteRequest : LdapRequest
	{
		public string ObjectName { get; set; }

		public DeleteRequest (int messageId)
			: base (messageId, ProtocolOperation.DeleteRequest)
		{
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new DeleteResponse (stream);
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new StringMessageElement ((byte)Protocol, ObjectName)
			).ToBytes ();
		}
	}
}

