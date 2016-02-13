using NHapi.Base.Model;
using NHapi.Base.Parser;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace PatientGenerator.HL7v2
{
	/// <summary>
	/// MLLP Message Sender
	/// </summary>
	public class MllpMessageSender
	{
		// Endpoint
		private Uri m_endpoint = null;

		/// <summary>
		/// Last server response time
		/// </summary>
		public TimeSpan LastResponseTime { get; private set; }

		/// <summary>
		/// Creates a new message sender
		/// </summary>
		/// <param name="endpoint">The endpoint in the form : llp://ipaddress:port</param>
		public MllpMessageSender(Uri endpoint)
		{
			this.m_endpoint = endpoint;
		}

		/// <summary>
		/// Send a message and receive the message
		/// </summary>
		public IMessage SendAndReceive(IMessage message)
		{
			// Encode the message
			var parser = new PipeParser();
			string strMessage = String.Empty;
			var id = Guid.NewGuid().ToString();

			strMessage = parser.Encode(message);

			// Open a TCP port
			using (TcpClient client = new TcpClient(AddressFamily.InterNetwork))
			{
				try
				{
					// Connect on the socket
					client.Connect(this.m_endpoint.Host, this.m_endpoint.Port);
					DateTime start = DateTime.Now;
					// Get the stream
					using (var stream = client.GetStream())
					{
						// Write message in ASCII encoding
						byte[] buffer = System.Text.Encoding.UTF8.GetBytes(strMessage);
						byte[] sendBuffer = new byte[buffer.Length + 3];
						sendBuffer[0] = 0x0b;
						Array.Copy(buffer, 0, sendBuffer, 1, buffer.Length);
						Array.Copy(new byte[] { 0x1c, 0x0d }, 0, sendBuffer, sendBuffer.Length - 2, 2);
						// Write start message
						//stream.Write(new byte[] { 0x0b }, 0, 1);
						stream.Write(sendBuffer, 0, sendBuffer.Length);
						// Write end message
						//stream.Write(new byte[] { 0x1c, 0x0d }, 0, 2);
						stream.Flush(); // Ensure all bytes get sent down the wire

						// Now read the response
						StringBuilder response = new StringBuilder();
						buffer = new byte[1024];
						while (!buffer.Contains((byte)0x1c)) // HACK: Keep reading until the buffer has the FS character
						{
							int br = stream.Read(buffer, 0, 1024);

							int ofs = 0;
							if (buffer[ofs] == '\v')
							{
								ofs = 1;
								br = br - 1;
							}
							response.Append(System.Text.Encoding.UTF8.GetString(buffer, ofs, br));
						}

						//Debug.WriteLine(response.ToString());
						// Parse the response
						//this.LastResponseTime = DateTime.Now.Subtract(start);
						return parser.Parse(response.ToString());
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.ToString());
					throw;
				}
			}
		}
	}
}