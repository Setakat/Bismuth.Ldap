# Bismuth.Ldap

Bismuth.Ldap is a lightweight LDAP client for the .NET framework that implements a synchronous API. It has been developed against OpenLdap, but it should work against any LDAP server that implements the LDAP protocol.

The Bismuth.Ldap library is still in development, and as such is not feature complete. For further info see [Contributions](#contributions).

## Getting Started

You can install Bismuth.Ldap by downloading the source code and adding the [Bismuth.Ldap](Bismuth.Ldap/Bismuth.Ldap.csproj) project file to your solution. Alternatively, you can open and build the solution, and then add the Bismuth.Ldap.dll file as a reference to your project.

Bismuth.Ldap has no external dependancies.

## How to Use

To start off, first install the Bismuth.Ldap as either a project or a dll reference, and then add a using statement in order to gain access to the Bismuth.Ldap members.
```csharp
using Bismuth.Ldap;
```

Once done, create a new instance of the ```LdapClient``` class. This implements IDisposable, so you can use it within a using statement if you choose.
```csharp
using (LdapClient myLdapClient = new LdapClient(myLdapServer, ldapPort))
{
}
```

Now that the client is ready to go, we can create a request. Requests can be found within the ```Bismuth.Ldap.Requests``` namespace. We'll create a anomymous bind request, send it, and then disconnect from the remote server.
```csharp
using (LdapClient myLdapClient = new LdapClient(myLdapServer, ldapPort))
{
    // connect to the ldap server
    BindRequest bindRequest = new BindRequest(1) { Authentication = (int)BindAuthentication.Simple };
    myLdapClient.Send(bindRequest);
    // now disconnect
    myLdapClient.Send(new UnbindRequest(2));
}
```

The ```Send``` method of the ```LdapClient``` returns an ```LdapResponse```, which can be found within the ```Bismuth.Ldap.Response```s namespace. Each response has the following properties:
```
ProtocolOperation Protocol - Contains a ProtocolOperation code that matches the original request.
int MessageId - Contains the Id of the original request.
int ResultCode - Contains the result of the operation. A non 0 value indicates an error.
string MatchedObject - 
string ErrorMessage - Contains the error message from the LDAP server if one occured.
```

A variety of Request and Response objects are available that cover the majority of the LDAP operations

## Contributions

At this moment, Bismuth.Ldap has only been developed against an OpenLdap server. It should work with any other directory server that implements the LDAP protocol. In the event that it doesn't, code contributions to enable support with other LDAP servers are welcomed.

Any bug reports or suggested features can be submitted as an [Issue](../../issues).

## License

This project is licensed under the GNU Affero General Public License v3.0  - see the [LICENSE.txt](LICENSE.txt) file for details
