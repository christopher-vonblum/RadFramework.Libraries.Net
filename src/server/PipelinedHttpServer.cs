using RadFramework.Abstractions.Extensibility.Pipeline;

namespace server_client_ui;

public class PipelinedHttpServer : IDisposable
{
    private readonly IPipeline<HttpRequest, byte[]> httpPipeline;

    private HttpServer server;
    
    public PipelinedHttpServer(int port, IPipeline<HttpRequest, byte[]> httpPipeline)
    {
        this.httpPipeline = httpPipeline;
        server = new HttpServer(port, request => httpPipeline.Process(request));
    }

    public void Dispose()
    {
        server.Dispose();
    }
}