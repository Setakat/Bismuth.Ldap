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
		RenameRequest = 0x6c,
		RenameResponse = 0x6d
	}

	public enum LdapResult
	{
		Success = 0,
		OperationsError = 1,
		ProtocolError = 2,
		TimeLimitExceeded = 3,
		SizeLimitExceeded = 4,
		CompareFalse = 5,
		CompareTrue = 6,
		AuthenticationNotSupported = 7,
		StrongAuthenticationRequired = 8,
		Referral = 10,
		AdminLimitExceeded = 11,
		UnavailableCriticalExtension = 12,
		ConfidentialityRequired = 13,
		SASLBindInProgress = 14,
		NoSuchAttribute = 16,
		UndefinedType = 17,
		InappropriateMatching = 18,
		ConstraintViolation = 19,
		TypeOrValueExists = 20,
		InvalidSyntax = 21,
		NoSuchObject = 32,
		AliasProblem = 33,
		InvalidDNSyntax = 34,
		IsLeaf = 35,
		AliasDerefProblem = 36,
		InappropriateAuthentication = 48,
		InvalidCredentials = 49,
		InsufficentAccess = 50,
		Busy = 51,
		Unavailable = 52,
		UnwillingToPreform = 53,
		LoopDetected = 54,
		NamingViolation = 64,
		ObjectClassViolation = 65,
		NotAllowedOnNonLead = 66,
		NotAllowedOnRDN = 67,
		AlreadyExists = 68,
		NoObjectClassMods = 69,
		ResultsToLarge = 70,
		AffectsMultipleDSAS = 71,
		Other = 80,
		TLSNotSupported = 112,
	}

	public enum SearchScope
	{
		BaseObject = 0,
		SingleLevel = 1,
		Subtree = 2
	}

	public enum DeferencePolicy
	{
		Never = 0,
		InSearching = 1,
		FindingBaseObj = 2,
		Always = 3
	}

	public enum ModificationType
	{
		Add = 0,
		Delete = 1,
		Replace = 2
	}
}

