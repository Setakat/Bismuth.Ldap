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
		
		protected string _directory;
		protected int _port;

        protected bool _unbound = false;

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
		}

		/// <summary>
		/// Sends a LdapRequest to the current Ldap server.
		/// </summary>
		/// <param name="ldapRequest">LDAP request.</param>
		public TResponse Send <TResponse>(LdapRequest ldapRequest) where TResponse : LdapResponse
		{
			CurrentMessageId = ldapRequest.MessageId;
			LdapResponse response = null;
			byte [] messageBytes = ldapRequest.ToBytes ();

			NetworkStream stream = currentConnection.GetStream ();
			stream.Write (messageBytes, 0, messageBytes.Length);
			response = ldapRequest.GetResponse (stream);

            // if we are unbinding, flag it her so we don't need to do it when disposing.
            if (ldapRequest is UnbindRequest)
                _unbound = true;

			return (TResponse)response;
		}

		public bool Bind (string userDN, string password, BindAuthentication bindAuthentication)
		{
			LdapResponse response = Send<BindResponse> (new BindRequest (NextMessageId) {
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
			Send<LdapResponse> (new UnbindRequest (NextMessageId));
            _unbound = true;
		}

		protected override void Dispose (bool freeManagedObjects)
		{
            if (!_unbound) {
                Unbind();
			}
			if (currentConnection != null && currentConnection.Connected)
				currentConnection.Close ();
		}
	}
}

