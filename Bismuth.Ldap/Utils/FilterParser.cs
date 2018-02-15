using System;
using System.Collections.Generic;
using System.IO;

namespace Bismuth.Ldap.Utils
{
	public class FilterParser
	{
		/*
		 * GOAL = FILTER
		 * FILTER = '(' NOT FILTER | OPERATION FILTER FILTER ... | ASSIGNMENT ')'
		 * NOT = '!'
		 * OPERATION = '&' | '|'
		 * ASSIGNMENT = ATTRIBUTE EQUALITY VALUE
		 * EQUALITY = '<=' | '>=' | '~=' | '='
		 * ATTRIBUTE = string
		 * VALUE = string | '*'
		 **/

		readonly string _ldapFilterName = "Ldap Search Filter";

		public Filter Parse (string searchFilter)
		{
			Filter filter = null;
			using (StringReader stringReader = new StringReader (searchFilter)) {
				filter = ParseFilter (stringReader);
				int end = stringReader.Peek ();
				if (end != -1)
					throw new Exception ("Error parsing " + _ldapFilterName + ": Expected EOL, found '" + (char)end + "'");
			}
			return filter;
		}

		protected Filter ParseFilter (StringReader stringReader)
		{
			Filter filter = null;
			if (NextCharacterIs (stringReader, '(')) {
				stringReader.Read ();
				if (NextCharacterIs (stringReader, '(')) {
					filter = ParseFilter (stringReader);
				}
				else if (NextCharacterIs (stringReader, '!')) {
					filter = ParseNotFilter (stringReader);
				}
				else if (NextCharacterIs (stringReader, '&') || NextCharacterIs (stringReader, '|')) {
					filter = ParseOperationFilter (stringReader);
				}
				else {
					filter = ParseEqualityFilter (stringReader);
				}

				if (!NextCharacterIs (stringReader, ')'))
					throw new FormatException (_ldapFilterName + " is not correctly formatted");
				stringReader.Read ();
			}
			else
				throw new Exception(_ldapFilterName + " is not opened correctly");
			return filter;
		}

		protected Filter ParseNotFilter (StringReader stringReader)
		{
			stringReader.Read ();

			return new NotFilter ().Add(ParseFilter(stringReader));
		}

		protected Filter ParseOperationFilter (StringReader stringReader)
		{
			Filter filter = null;
			char c = (char)stringReader.Read ();
			if (c == '&')
				filter = new AndFilter ();
			else if (c == '|')
				filter = new OrFilter ();
			while (NextCharacterIs (stringReader, '(')) {
				filter.Add (ParseFilter (stringReader));
			}

			return filter;
		}

		protected Filter ParseEqualityFilter (StringReader stringReader)
		{
			List<char> reserved = new List<char> { '<', '>', '~', '=' };

			string content = "";

			do {
				if (stringReader.Peek () == -1)
					throw new Exception (_ldapFilterName + " ended unexpectedly");
				char c = CheckNextCharacter (stringReader);
				if (c == ')')
					break;
				content += (char)stringReader.Read ();
			}
			while (true);

			string [] parts = null;
			if (content.Contains ("<=")) {
				parts = Split (content, "<=");
				return new LessThanEqualsFilter { Attribute = parts [0], Value = parts [1] };
			}
			if (content.Contains (">=")) {
				parts = Split (content, ">=");
				return new GreaterThanEqualsFilter { Attribute = parts [0], Value = parts [1] };
			}
			if (content.Contains ("~=")) {
				parts = Split (content, "~=");
				return new ApproximateFilter { Attribute = parts [0], Value = parts [1] };
			}
			if (content.Contains ("=*")) {
				parts = Split (content, "=*");
				return new PresentFilter { Attribute = parts [0] };
			}
			if (content.Contains ("=")) {
				parts = Split (content, "=");
				return new EqualityFilter { Attribute = parts [0], Value = parts [1] };
			}

			return new PresentFilter { Attribute = content };
		}

		char CheckNextCharacter (StringReader stringReader)
		{
			return (char)stringReader.Peek ();
		}

		bool NextCharacterIs (StringReader stringReader, char c)
		{
			return CheckNextCharacter (stringReader) == c;
		}

		string [] Split (string assignment, string equality)
		{
			return assignment.Split (new string [] { equality }, StringSplitOptions.None);
		}
	}
}

