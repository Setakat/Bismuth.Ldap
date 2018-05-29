using System;
using System.Collections.Generic;
using System.Linq;
using Bismuth.Ldap.Utils;

namespace Bismuth.Ldap.Messaging
{
	public abstract class MessageElement
	{
		public byte EncodingType { get; protected set; }

		public int Length { get; protected set; }

		public abstract byte [] ToBytes ();
	}

	public abstract class MessageElement<T> : MessageElement
	{
		public T Value { get; protected set; }

		public MessageElement (T value)
		{
			Value = value;
		}

		protected void SetLength (List<byte> messageBytes, byte [] valueBytes)
		{
			if (valueBytes.Length < 128)
				messageBytes.Add (Convert.ToByte (valueBytes.Length));
			else {
				int count = (int)((valueBytes.Length / 256) + 1f);
				messageBytes.Add ((byte)(128 + count));
				var bytes = BitConverter.GetBytes (valueBytes.Length);
				Array.Reverse (bytes);
				bytes = ByteArray.RemoveLeadingZeros (bytes);
				messageBytes.AddRange (bytes);
				//messageBytes.Add (Convert.ToByte (valueBytes.Length));
			}
		}

		public override byte [] ToBytes ()
		{
			byte [] valueBytes = ValueToBytes ();
			//valueBytes = ByteArray.RemoveLeadingZeros (valueBytes);

			List<byte> elementBytes = new List<byte> ();
			elementBytes.Add (EncodingType);
			SetLength (elementBytes, valueBytes);
			elementBytes.AddRange (valueBytes);

			return elementBytes.ToArray ();
		}

		protected abstract byte [] ValueToBytes ();
	}

	public class ListMessageElement : MessageElement<List<MessageElement>>
	{
		public ListMessageElement ()
			: this (new List<MessageElement>())
		{
			EncodingType = 0x30;
		}

		public ListMessageElement (byte encoding)
			: this (new List<MessageElement> ())
		{
			EncodingType = encoding;
		}

		public ListMessageElement (List<MessageElement> value)
			: base (value)
		{
			EncodingType = 0x30;
		}

		public ListMessageElement AddElements (params MessageElement [] elements)
		{
			Value.AddRange (elements);
			return this;
		}

		protected override byte [] ValueToBytes ()
		{
			return Value.SelectMany (item => item.ToBytes ()).ToArray ();
		}
	}

	public class StringMessageElement : MessageElement<string>
	{
		public StringMessageElement (string value)
			: this (0x4, value)
		{ }

		public StringMessageElement (byte encoding, string value)
			: base (value ?? string.Empty)
		{
			EncodingType = encoding;
		}

		protected override byte [] ValueToBytes ()
		{
			byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes (Value);
			return valueBytes;
		}
	}

	public class IntegerMessageElement : MessageElement<int>
	{
		public IntegerMessageElement (int value)
			: base (value)
		{
			EncodingType = 0x2;
		}

		protected override byte [] ValueToBytes ()
		{
			if (Value == 0)
				return new byte[] { 0x0 };

			byte [] valueBytes = BitConverter.GetBytes (Value);
			if (BitConverter.IsLittleEndian)
				Array.Reverse (valueBytes);

			valueBytes = ByteArray.RemoveLeadingZeros (valueBytes);

			return valueBytes;
		}
	}

	public class BooleanMessageElement : MessageElement<bool>
	{
		public BooleanMessageElement (bool value)
			: base (value)
		{
			EncodingType = 0x1;
		}

		protected override byte [] ValueToBytes ()
		{
			byte [] valueBytes = BitConverter.GetBytes (Value);
			return valueBytes;
		}
	}

	public class EnumMessageElement : MessageElement<int>
	{
		public EnumMessageElement (int value)
			: base (value)
		{
			EncodingType = 0x0a;
		}

		protected override byte [] ValueToBytes ()
		{
			if (Value == 0)
				return new byte[] { 0x0 };
			byte [] valueBytes = BitConverter.GetBytes (Value);
			if (BitConverter.IsLittleEndian)
				Array.Reverse (valueBytes);
			valueBytes = ByteArray.RemoveLeadingZeros (valueBytes);
			return valueBytes;
		}
	}

	public class SearchFilterMessageElement : StringMessageElement
	{
		public SearchFilterMessageElement (string value)
			: this (0x4, value)
		{ }

		public SearchFilterMessageElement (byte encoding, string value)
			: base (value)
		{
			EncodingType = encoding;
		}

		public override byte [] ToBytes ()
		{
			return ValueToBytes ();
		}

		protected override byte [] ValueToBytes ()
		{
			//byte [] valueBytes = System.Text.Encoding.UTF8.GetBytes (Value);
			Filter filter = new FilterParser ().Parse (Value);
			byte [] valueBytes = filter.ToMessageElement().ToBytes();

			return valueBytes;
		}
	}
}

