using System.Net;
using System.Net.Sockets;
using server_client_ui.Internal;

namespace server_client_ui;

public abstract class SocketServer : IDisposable
{
    private Socket tcpListener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    private Thread acceptThread;

    private HttpSocketQueue httpSocketQueue;
    
    private bool disposed = false;
    
    public SocketServer(ProtocolType protocolType, int port)
    {
        httpSocketQueue = new HttpSocketQueue(this.ProcessSocketConnection);
        
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        
        tcpListener.Bind(endPoint);
        tcpListener.Listen();
        acceptThread = new Thread(AcceptSockets);
        acceptThread.Start();
    }

    private void AcceptSockets()
    {
        while (!disposed)
        {
            try
            {
                Socket newConnection = tcpListener.Accept();

                httpSocketQueue.Enqueue(newConnection);
            }
            catch
            {
            }
        }
    }

    protected abstract void ProcessSocketConnection(Socket socketConnection);

    public void Dispose()
    {
        disposed = true;
        
        while (!httpSocketQueue.CanShutdown)
        {
            Thread.Sleep(100);
        }
        
        tcpListener.Close();
        tcpListener.Dispose();
    }
}