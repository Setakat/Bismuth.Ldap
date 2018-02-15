using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class SearchRequest : LdapRequest
	{
		public string BaseObject { get; set; }

		public SearchScope Scope { get; set; }

		public int DeferencePolicy { get; set; }

		public int SizeLimit { get; set; }

		public int TimeLimit { get; set; }

		public bool AttributeTypesOnly { get; set; }

		public string SearchFilter { get; set; }

		public string [] Attributes { get; set; }
		
		public SearchRequest (int messageId)
			: base (messageId, ProtocolOperation.SearchRequest)
		{
			Scope = SearchScope.BaseObject;
			DeferencePolicy = 0;
			SizeLimit = 1000;
			TimeLimit = 0;
			AttributeTypesOnly = false;
			SearchFilter = "(objectClass=*)";
			Attributes = new string [0];
		}

		public override byte [] ToBytes ()
		{
			MessageElement element = new ListMessageElement ().AddElements (
				new IntegerMessageElement (MessageId),
				new ListMessageElement ((byte)Protocol).AddElements (
					new StringMessageElement (BaseObject),
					new EnumMessageElement ((int)Scope),
					new EnumMessageElement (DeferencePolicy),
					new IntegerMessageElement (SizeLimit),
					new IntegerMessageElement (TimeLimit),
					new BooleanMessageElement (AttributeTypesOnly),
					new SearchFilterMessageElement (SearchFilter),
					GetAttributesList ()
				)
			);
			return element.ToBytes ();
		}

		public override LdapResponse GetResponse (NetworkStream stream)
		{
			return new SearchResponse (stream);
		}

		protected ListMessageElement GetAttributesList ()
		{
			return new ListMessageElement ().AddElements(
				Attributes.Select (a => new StringMessageElement (a)).ToArray()
			);
		}
	}
}

