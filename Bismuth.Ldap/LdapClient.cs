using Bismuth.Ldap.Requests;
using Bismuth.Ldap.Responses;
using System;
using System.Net.Sockets;

namespace Bismuth.Ldap
{
	public class LdapClient : Disposable
	{
		TcpClient connection;
		
		protected string _directory;
		protected int _port;

		public int CurrentMessageId { get; protected set; }

		public LdapClient (string directory, int port)
		{
			_directory = directory;
			_port = port;
			connection = new TcpClient (directory, port);
		}

		public LdapResponse Send (LdapRequest ldapRequest)
		{
			LdapResponse response = null;
			byte [] messageBytes = ldapRequest.ToBytes ();

			using (TcpClient client = new TcpClient (_directory, _port)) {
				using (NetworkStream stream = client.GetStream ()) {
					stream.Write (messageBytes, 0, messageBytes.Length);
					response = ldapRequest.GetResponse (stream);
					stream.Close ();
				}
				client.Close ();
			}
			return response;
		}
	}
}

