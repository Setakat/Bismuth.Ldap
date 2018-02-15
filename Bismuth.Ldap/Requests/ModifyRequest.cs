using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class ModifyRequest : LdapRequest
	{
		public string EntryName { get; set; }

		public List<ObjectAttribute> Attributes { get; set; }

		public ModifyRequest (int messageId)
			: base (messageId, ProtocolOperation.ModifyRequest)
		{
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new ModifyResponse (stream);
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol).AddElements (
					new StringMessageElement(EntryName),
					GetModifications()
				)
			).ToBytes ();
		}

		protected ListMessageElement GetModifications ()
		{
			return new ListMessageElement ().AddElements(
			).AddElements (
				Attributes.Select (a => GetModificationItem (a)).ToArray ()
			);
		}

		protected ListMessageElement GetModificationItem (ObjectAttribute attribute)
		{
			return new ListMessageElement ().AddElements (
				new EnumMessageElement (2)
			).AddElements (
				GetModificationDescription (attribute)
			);
		}

		protected ListMessageElement GetModificationDescription (ObjectAttribute attribute)
		{
			return new ListMessageElement ().AddElements (
				new StringMessageElement (attribute.Type)
			).AddElements (
				GetAttributeValues (attribute)
			);
		}

		protected ListMessageElement GetAttributeValues (ObjectAttribute attribute)
		{
			return new ListMessageElement (0x31).AddElements (attribute.Values.Select (v => new StringMessageElement (v)).ToArray ());
		}
	}
}

