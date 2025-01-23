using System.Diagnostics;
using System.Text;
using server_client_ui;

namespace server
{
    class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                HttpServer server = new HttpServer(
                    80,
                    request =>
                        Encoding.UTF8.GetBytes("<html><body>Wie viel brauchste?</body></html>"));

                Console.ReadLine();

                server.Dispose();
            }
            catch
            {
                Console.ReadLine();
                return -1;
            }


            return 0;
        }
    }
}

