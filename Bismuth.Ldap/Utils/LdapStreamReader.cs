using System;
using System.IO;

namespace Bismuth.Ldap.Utils
{
	public class LdapStreamReader
	{
		protected BinaryReader reader;

		public LdapStreamReader (Stream stream)
		{
			reader = new BinaryReader(stream);
		}

		public int ReadIntElement ()
		{
			int value = 0;
			if (NextElementIs (0x2)) {
				int length = ReadElementLength ();
				byte [] bytes = { 00, 00, 00, 00 };
				reader.Read (bytes, 0, length);
				value = BitConverter.ToInt32 (bytes, 0);
			}
			return value;
		}

		public int ReadEnumElement ()
		{
			int value = 0;
			if (NextElementIs (0xa)) {
				int length = ReadElementLength ();
				byte [] bytes = { 00, 00, 00, 00 };
				reader.Read (bytes, 0, length);
				value = BitConverter.ToInt32 (bytes, 0);
			}
			return value;
		}

		public string ReadStringElement ()
		{
			string value = string.Empty;
			if (NextElementIs (0x4)) {
				int length = ReadElementLength ();
				byte [] bytes = new byte [length];
				reader.Read (bytes, 0, length);
				value = System.Text.Encoding.UTF8.GetString (bytes);
			}
			return value;
		}

		public int ReadByte ()
		{
			return reader.ReadByte ();
		}

		public byte [] ReadBytes (int count)
		{
			byte [] bytes = new byte [count];
			reader.Read (bytes, 0, count);
			return bytes;
		}

		public int Peek ()
		{
			return reader.PeekChar ();
		}

		public bool NextElementIs (int elementType)
		{
			int type = reader.ReadByte ();
			if (type != elementType)
				throw new Exception ("Tried to read element type (" + elementType + "), got (" + type + ") instead");
			return true;
		}

		/// <summary>
		/// Returns a new LdapStreamReader for a particular subelement of an LdapMessage.
		/// </summary>
		/// <returns>A new LdapStreamReader.</returns>
		/// <param name="elementType">The X690 code of the element to be read.</param>
		public LdapStreamReader GetElementReader (int elementType)
		{
			LdapStreamReader subReader = null;
			if (NextElementIs (elementType)) {
				int elementLength = ReadElementLength ();
				byte [] buffer = new byte [elementLength];
				reader.Read (buffer, 0, elementLength);
				subReader = new LdapStreamReader (new MemoryStream (buffer));
			}
			return subReader;
		}

		public int ReadElementLength ()
		{
			int length = reader.ReadByte ();
			if (length > 127) {
				// check for long form here
				int bytesToRead = length - 127;
				byte [] buffer = new byte [4];
				reader.Read (buffer, 0, bytesToRead);
				Array.Reverse (buffer);
				buffer = ByteArray.RemoveLeadingZeros (buffer);
				length = BitConverter.ToInt32 (buffer, 0);
			}
			// return short form
			return length;
		}
	}
}

