using Bismuth.Ldap.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bismuth.Ldap.Responses
{
	public class SearchResult : LdapEntry
	{
		public SearchResult (LdapStreamReader reader)
			: base ("")
		{
			int contentLength = reader.ReadElementLength ();
			ObjectName = reader.ReadStringElement ();

			Attributes = ReadAttributes (reader.GetElementReader(0x30));
		}

		Dictionary<string, ObjectAttribute> ReadAttributes (LdapStreamReader reader)
		{
			Dictionary<string, ObjectAttribute> attributes = new Dictionary<string, ObjectAttribute> ();

			while (true) {
				int nextByte = reader.Peek ();
				if (nextByte == 0x30) {
					ObjectAttribute attribute = ReadAttribute (reader.GetElementReader (0x30));
					attributes.Add (attribute.Type, attribute);
				}
				else if (nextByte == -1)
					break;
			}

			return attributes;
		}

		ObjectAttribute ReadAttribute (LdapStreamReader reader)
		{
			List<string> attributes = new List<string> ();
			ObjectAttribute attribute = new ObjectAttribute ();

			attribute.Type = reader.ReadStringElement ();
			LdapStreamReader valueReader = reader.GetElementReader (0x31);
			while (valueReader.Peek() != -1) {
				attributes.Add (valueReader.ReadStringElement ());
			}
			attribute.Values = attributes.ToArray();

			return attribute;
		}
	}
}

