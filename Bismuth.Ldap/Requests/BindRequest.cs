using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class BindRequest : LdapRequest
	{
		public string BindDN { get; set; }

		public string Password { get; set; }

		public int Authentication { get; set; }

		public BindRequest (int messageId)
			: base (messageId, ProtocolOperation.BindRequest)
		{
			BindDN = string.Empty;
			Password = string.Empty;
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol).AddElements (
					new IntegerMessageElement (3),
					new StringMessageElement (BindDN),
					new StringMessageElement (0x80, Password)
				)
			).ToBytes ();
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new BindResponse (stream);
		}
	}
}

