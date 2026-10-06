using System.Net.Sockets;

namespace ChatServer
{
    public class Program
    {
        const int SERVER_PORT = 5000;

        static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, SERVER_PORT);
            listener.Start();
            Console.WriteLine($"Server started on port {SERVER_PORT}");

            using var client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");

            using var stream =client.GetStream();

            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) 
            { 
                AutoFlush = true
            };

            while (true)
            {
                string? message = await reader.ReadLineAsync();
                if(message is null || message == "/exit")
                {
                    break;
                }

                Console.WriteLine($"Client: {message}");
                Console.Write("Server: ");
                string response = Console.ReadLine() ?? string.Empty;
                await writer.WriteLineAsync(response);
            }
        }
    }
}
