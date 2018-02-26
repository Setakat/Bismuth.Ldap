using Bismuth.Ldap.Messaging;
using Bismuth.Ldap.Responses;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Bismuth.Ldap.Requests
{
	public class SearchRequest : LdapRequest
	{
		/// <summary>
		/// Gets or sets the location in the DIT (Directory Information Tree) where the search will begin.
		/// </summary>
		/// <value>The base object.</value>
		public string BaseObject { get; set; }

		/// <summary>
		/// Determines how far the search should go from the BaseObject. Defaults to <see cref="SearchScope.BaseObject"/>
		/// </summary>
		/// <value>The scope of the search.</value>
		public SearchScope Scope { get; set; }

		/// <summary>
		/// Determines whether or not aliases will be used when searching for attributes. Defaults to <see cref="DeferencePolicy.Always"/>.
		/// </summary>
		/// <value>The deference policy.</value>
		public DeferencePolicy DeferencePolicy { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of results that can be returned, with 0 being unlimited. Defaults to 100.
		/// </summary>
		/// <value>The size limit.</value>
		public int SizeLimit { get; set; }

		/// <summary>
		/// Gets or sets the time in seconds to spend searching before timing out, with 0 indicating no timeout. Defaults to 0.
		/// </summary>
		/// <value>The time to spend searching in seconds.</value>
		public int TimeLimit { get; set; }

		/// <summary>
		/// Determines if attribute types only or attribute types and their values should be returned. Defaults to false.
		/// </summary>
		/// <value><c>true</c> to return attribute types only; otherwise, <c>false</c>.</value>
		public bool AttributeTypesOnly { get; set; }

		/// <summary>
		/// Gets or sets the ldap search filter which determines which results to return. Defaults to "(objectClass=*)".
		/// </summary>
		/// <value>The search filter.</value>
		public string SearchFilter { get; set; }

		/// <summary>
		/// Gets or sets the attributes to be returned in each SearchResult. Defaults to an empty array.
		/// </summary>
		/// <value>The attributes to be returned.</value>
		public string [] Attributes { get; set; }
		
		public SearchRequest (int messageId)
			: base (messageId, ProtocolOperation.SearchRequest)
		{
			Scope = SearchScope.BaseObject;
			DeferencePolicy = DeferencePolicy.Always;
			SizeLimit = 100;
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
					new EnumMessageElement ((int)DeferencePolicy),
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

