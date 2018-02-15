using System;
namespace Bismuth.Ldap
{
	public enum BindAuthentication
	{
		Simple = 0,
		SASL = 3
	}

	public enum ProtocolOperation
	{
		Unbind = 0x42,
		DeleteRequest = 0x4a,
		BindRequest = 0x60,
		BindResponse = 0x61,
		SearchRequest = 0x63,
		SearchResultEntry = 0x64,
		SearchResultDone = 0x65,
		ModifyRequest = 0x66,
		ModifyResponse = 0x67,
		AddRequest = 0x68,
		AddResponse = 0x69,
		DeleteResponse = 0x6b,
	}

	public enum BerType
	{
		Boolean = 0x1,
		Integer = 0x2,
		OctetString = 0x4,
		Enum = 0xA
	}

	public enum LdapResult
	{
		Success = 0,
		OperationsError = 1,
		ProtocolError = 2,
		InvalidCredentials = 49,
		UnwillingToPreform = 53,
	}

	public enum SearchScope
	{
		BaseObject = 0,
		SingleLevel = 1,
		Subtree = 2
	}
}

