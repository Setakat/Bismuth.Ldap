using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class AddRequest : LdapRequest
	{
		public string EntryName { get; set; }

		public List<ObjectAttribute> Attributes { get; set; }

		public AddRequest (int messageId)
			: base (messageId, ProtocolOperation.AddRequest)
		{
			Attributes = new List<ObjectAttribute> ();
		}

		public AddRequest (int messageId, LdapEntry ldapEntry)
			: base (messageId, ProtocolOperation.AddRequest)
		{
			EntryName = ldapEntry.ObjectName;
			Attributes = new List<ObjectAttribute>(ldapEntry.Attributes.Values);
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new AddResponse (stream);
		}

		public override byte [] ToBytes ()
		{
			return new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol).AddElements (
					new StringMessageElement (EntryName),
					GetAttributeList ()
				)
			).ToBytes ();
		}

		protected ListMessageElement GetAttributeList ()
		{
			return new ListMessageElement ().AddElements (
				Attributes.Select(a => GetAttribute(a)).ToArray()
			);
		}

		protected ListMessageElement GetAttribute (ObjectAttribute attribute)
		{
			return new ListMessageElement ().AddElements (
				new StringMessageElement(attribute.Type)
			).AddElements(
				GetAttributeValues (attribute)
			);
		}

		protected ListMessageElement GetAttributeValues (ObjectAttribute attribute)
		{
			return new ListMessageElement(0x31).AddElements(attribute.Values.Select (v => new StringMessageElement (v)).ToArray ());
		}
	}
}

