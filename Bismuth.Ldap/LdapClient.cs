using Bismuth.Ldap.Requests;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Security;
using System.Net.Sockets;

namespace Bismuth.Ldap
{
	public class LdapClient : Disposable
	{
		TcpClient currentConnection;
		NetworkStream currentStream;
		
		protected string _directory;
		protected int _port;

		public int CurrentMessageId { get; protected set; }

		public int NextMessageId {
			get { return CurrentMessageId + 1; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Bismuth.Ldap.LdapClient"/> class.
		/// </summary>
		/// <param name="directory">Address of the Ldap server to connect to.</param>
		/// <param name="port">The port of the ldap server to connect to.</param>
		public LdapClient (string directory, int port)
		{
			_directory = directory;
			_port = port;
			CurrentMessageId = 0;
			currentConnection = new TcpClient (_directory, _port);
			currentStream = currentConnection.GetStream ();
		}

		/// <summary>
		/// Sends a LdapRequest to the current Ldap server.
		/// </summary>
		/// <param name="ldapRequest">LDAP request.</param>
		public LdapResponse Send (LdapRequest ldapRequest)
		{
			CurrentMessageId = ldapRequest.MessageId;
			LdapResponse response = null;
			byte [] messageBytes = ldapRequest.ToBytes ();

			NetworkStream stream = currentConnection.GetStream ();
			stream.Write (messageBytes, 0, messageBytes.Length);
			response = ldapRequest.GetResponse (stream);

			return response;
		}

		public bool Bind (string userDN, string password, BindAuthentication bindAuthentication)
		{
			LdapResponse response = Send (new BindRequest (NextMessageId) {
				Authentication = (int)bindAuthentication,
				BindDN = userDN,
				Password = password
			});
			if (response.ResultCode > 0)
				throw new Exception ("Unable to bind: (" + response.ResultCode + ")");
			return true;
		}

		public void Unbind ()
		{
			Send (new UnbindRequest (NextMessageId));
		}

		protected override void Dispose (bool freeManagedObjects)
		{
			if (currentStream != null) {
				currentStream.Close ();
				currentStream.Dispose ();
			}
			if (currentConnection != null && currentConnection.Connected)
				currentConnection.Close ();
		}
	}
}

