using Bismuth.Ldap.Requests;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap
{
	public class LdapClient : Disposable
	{
		TcpClient currentConnection;
		
		protected string _directory;
		protected int _port;

		public int CurrentMessageId { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Bismuth.Ldap.LdapClient"/> class.
		/// </summary>
		/// <param name="directory">Address of the Ldap server to connect to.</param>
		/// <param name="port">The port of the ldap server to connect to.</param>
		public LdapClient (string directory, int port)
		{
			_directory = directory;
			_port = port;
		}

		/// <summary>
		/// Sends a LdapRequest to the current Ldap server.
		/// </summary>
		/// <param name="ldapRequest">LDAP request.</param>
		public LdapResponse Send (LdapRequest ldapRequest)
		{
			LdapResponse response = null;
			byte [] messageBytes = ldapRequest.ToBytes ();

			using (currentConnection = new TcpClient (_directory, _port)) {
				using (NetworkStream stream = currentConnection.GetStream ()) {
					stream.Write (messageBytes, 0, messageBytes.Length);
					response = ldapRequest.GetResponse (stream);
					stream.Close ();
				}
				currentConnection.Close ();
			}
			return response;
		}

		protected override void Dispose (bool freeManagedObjects)
		{
			if (currentConnection != null && currentConnection.Connected)
				currentConnection.Close ();
		}
	}
}

