using Bismuth.Ldap.Utils;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Bismuth.Ldap.Responses
{
	public class SearchResponse : LdapResponse
	{
		public List<SearchResult> Results { get; protected set; }

		public SearchResponse (NetworkStream stream)
			: base (stream)
		{
			ReadResponse (new LdapStreamReader (stream), ProtocolOperation.SearchResultDone);
		}

		protected override void ReadResponse (LdapStreamReader reader, ProtocolOperation protocol)
		{
			Results = new List<SearchResult> ();
			while (true) {
				if (reader.NextElementIs (0x30)) {
					int messageLength = reader.ReadElementLength ();
					MessageId = reader.ReadIntElement ();
					int operation = reader.ReadByte ();
					if (operation == (int)ProtocolOperation.SearchResultEntry) {
						Results.Add(new SearchResult (reader));

					}
					else if (operation == (int)ProtocolOperation.SearchResultDone) {
						int contentLength = reader.ReadElementLength ();
						ReadResponseBody (reader);
						break;
					}
				}
			}
		}
	}
}

