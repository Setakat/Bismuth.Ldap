using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Bismuth.Ldap.Requests
{
	public class RenameRequest : LdapRequest
	{
		public string EntryName { get; set; }

		public string NewEntryName { get; set; }

		public bool DeleteOld { get; set; }

		public string NewSuperiorName { get; set; }

		public RenameRequest (int messageId)
			: base (messageId, ProtocolOperation.RenameRequest)
		{
			
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new RenameResponse (stream);
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol).AddElements (
					GetMessageElements ()
				)
			).ToBytes ();
		}

		protected MessageElement[] GetMessageElements ()
		{
			List<MessageElement> messages = new List<MessageElement> {
				new StringMessageElement (EntryName),
				new StringMessageElement (NewEntryName),
				new BooleanMessageElement (DeleteOld),
			};
			if (!string.IsNullOrWhiteSpace (NewSuperiorName))
				messages.Add(new StringMessageElement (EntryName));

			return messages.ToArray ();
		}
	}
}

