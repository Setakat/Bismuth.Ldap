using Bismuth.Ldap.Utils;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap.Responses
{
	public class LdapResponse
	{
		public ProtocolOperation Protocol { get; protected set; }

		public int MessageId { get; protected set; }

		public int ResultCode { get; protected set; }

		public string MatchedObject { get; protected set; }

		public string ErrorMessage { get; protected set; }

		public LdapResponse (NetworkStream stream)
		{
			
		}

		protected virtual void ReadResponse (LdapStreamReader reader, ProtocolOperation protocol)
		{
			if (reader.NextElementIs (0x30)) {
				int messageLength = reader.ReadElementLength ();
				MessageId = reader.ReadIntElement ();
				if (reader.NextElementIs ((int)protocol)) {
					int contentLength = reader.ReadElementLength ();
					ReadResponseBody (reader);
				}
			}
		}

		protected virtual void ReadResponseBody (LdapStreamReader reader)
		{
			ReadResponseDetails (reader);
		}

		protected void ReadResponseDetails (LdapStreamReader reader)
		{
			ResultCode = reader.ReadEnumElement ();
			MatchedObject = reader.ReadStringElement ();
			ErrorMessage = reader.ReadStringElement ();
		}
	}
}

