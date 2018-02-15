using Bismuth.Ldap.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bismuth.Ldap.Utils
{
	public abstract class Filter
	{
		public List<Filter> Filters { get; protected set; }

		public Filter ()
		{
			Filters = new List<Filter> ();
		}

		public Filter Add (Filter filter)
		{
			Filters.Add (filter);
			return this;
		}

		public abstract MessageElement ToMessageElement ();
	}

	public class AndFilter : Filter
	{
		public override MessageElement ToMessageElement ()
		{
			ListMessageElement element = new ListMessageElement (0xa0);
			element.AddElements (Filters.Select (f => f.ToMessageElement ()).ToArray());
			return element;
		}
	}

	public class OrFilter : Filter
	{
		public override MessageElement ToMessageElement ()
		{
			ListMessageElement element = new ListMessageElement (0xa1);
			element.AddElements (Filters.Select (f => f.ToMessageElement ()).ToArray ());
			return element;
		}
	}

	public class NotFilter : Filter
	{
		public override MessageElement ToMessageElement ()
		{
			ListMessageElement element = new ListMessageElement (0xa2);
			element.AddElements (Filters [0].ToMessageElement ());
			return element;
		}
	}

	public class PresentFilter : Filter
	{
		public string Attribute { get; set; }

		public override MessageElement ToMessageElement ()
		{
			return new StringMessageElement (0x87, Attribute);
		}
	}

	public class EqualityFilter : PresentFilter
	{
		public string Value { get; set; }

		public override MessageElement ToMessageElement ()
		{
			return new ListMessageElement (0xa3).AddElements (
				new StringMessageElement (Attribute),
				new StringMessageElement (Value)
			);
		}
	}

	public class GreaterThanEqualsFilter : EqualityFilter
	{
		
	}

	public class LessThanEqualsFilter : EqualityFilter
	{
		
	}

	public class ApproximateFilter : EqualityFilter
	{

	}
}

