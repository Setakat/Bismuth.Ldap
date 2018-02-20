using Bismuth.Ldap.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bismuth.Ldap.Responses
{
	public class SearchResult
	{
		public string ObjectName { get; protected set; }

		public ObjectAttribute [] Attributes { get; protected set; }

		public SearchResult (LdapStreamReader reader)
		{
			int contentLength = reader.ReadElementLength ();
			ObjectName = reader.ReadStringElement ();

			Attributes = GetAttributes (reader.GetElementReader(0x30));
		}

		ObjectAttribute [] GetAttributes (LdapStreamReader reader)
		{
			List<ObjectAttribute> attributes = new List<ObjectAttribute> ();

			while (true) {
				int nextByte = reader.Peek ();
				if (nextByte == 0x30)
					attributes.Add (GetAttribute (reader.GetElementReader(0x30)));
				else if (nextByte == -1)
					break;
			}

			return attributes.ToArray ();
		}

		ObjectAttribute GetAttribute (LdapStreamReader reader)
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

	public class ObjectAttribute
	{
		public string Type { get; set; }

		public string [] Values { get; set; }

		public ObjectAttribute ()
			: this ("", "")
		{ }

		public ObjectAttribute (string type, string value)
			: this (type, new string [] { value })
		{ }

		public ObjectAttribute (string type, params string [] values)
		{
			Type = type;
			Values = values;
		}
	}
}

