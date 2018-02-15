using System;
using System.Collections.Generic;

namespace Bismuth.Ldap.Utils
{
	public class ByteArray
	{
		public static byte [] RemoveLeadingZeros (byte [] bytes)
		{
			int offset = 0;
			for (offset = 0; offset < bytes.Length; offset++)
				if (bytes [offset] != 0x0)
					break;

			byte [] newArray = new byte [bytes.Length - offset];
			Array.Copy (bytes, offset, newArray, 0, newArray.Length);
			return newArray;
		}

		//public static AddLeadingZeros (byte [] bytes)
		//{

		//}
	}
}

