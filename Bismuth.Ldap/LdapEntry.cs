using System;
using System.Collections.Generic;

namespace Bismuth.Ldap
{
	public class LdapEntry
	{
		public string ObjectName { get; protected set; }

		public Dictionary<string, ObjectAttribute> Attributes { get; protected set; }

		public LdapEntry (string objectName)
		{
			ObjectName = objectName;
			Attributes = new Dictionary<string, ObjectAttribute> ();
		}

		public LdapEntry AddAttribute (string attributeName, string value)
		{
			Attributes.Add (attributeName, new ObjectAttribute (attributeName, value));
			return this;
		}

		public LdapEntry AddAttribute (string attributeName, params string[] values)
		{
			Attributes.Add (attributeName, new ObjectAttribute (attributeName, values));
			return this;
		}

		public string GetAttributeValue (string attributeName)
		{
			return GetAttributeValue (attributeName, string.Empty);
		}

		public string GetAttributeValue(string attributeName, string defaultValue)
		{
			ObjectAttribute attribute = null;
			if (Attributes.TryGetValue (attributeName, out attribute))
				return attribute.Value;
			return defaultValue;
		}

		public string[] GetAttributeValues (string attributeName)
		{
			ObjectAttribute attribute = null;
			if (Attributes.TryGetValue (attributeName, out attribute))
				return attribute.Values;
			return new string[0];
		}
	}

	public class ObjectAttribute
	{
		public string Type { get; set; }

		public string Value {
			get { return (Values != null && Values.Length > 0) ? Values [0] : ""; }
			set { Values = new string [] { value }; }
		}

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

	public class ModifyAttribute : ObjectAttribute
	{
		public ModificationType Modification { get ; set; }

		public ModifyAttribute (string type, ModificationType modification, string value)
			: this (type, modification, new string [] { value })
		{ }

		public ModifyAttribute (string type, ModificationType modification, params string [] values)
		{
			Type = type;
			Modification = Modification;
			Values = values;
		}
	}
}

