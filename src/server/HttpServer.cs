using System.Collections;
using System.Net.Sockets;
using server_client_ui.Internal;

namespace server_client_ui;

public class HttpServer : SocketServer
{
    private readonly Func<HttpRequest, byte[]> processRequest;

    public HttpServer(int port, Func<HttpRequest, byte[]> processRequest)
        : base(ProtocolType.Tcp, port)
    {
        this.processRequest = processRequest;
    }

    protected override void ProcessSocketConnection(Socket socketConnection)
    {
        NetworkStream networkStream = new NetworkStream(socketConnection);
        
        StreamReader reader = new StreamReader(networkStream);
        
        string firstRequestLine = reader.ReadLine();
        
        HttpRequest requestModel = new HttpRequest();
        
        requestModel.Method = HttpRequestParser.ExtractHttpMethod(firstRequestLine);
        requestModel.Url = HttpRequestParser.ExtractUrl(firstRequestLine);
        requestModel.HttpVersion = HttpRequestParser.ExtractHttpVersion(firstRequestLine);

        string currentHeaderLine = null;
        
        while ((currentHeaderLine = reader.ReadLine()) != "")
        {
            var header = HttpRequestParser.ReadHeader(currentHeaderLine);
            requestModel.Headers.Add(header.header, header.value);
        }

        networkStream.Write(processRequest(requestModel));
    }
}