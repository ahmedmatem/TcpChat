using System.Net.Sockets;
using System.Threading.Channels;

namespace ChatClient
{
    public class Program
    {
        const string SERVER_NAME = "127.0.0.1";
        const int SERVER_PORT = 5000;

        static async Task Main(string[] args)
        {
            using var client = new TcpClient();
            client.Connect(SERVER_NAME, SERVER_PORT);

            Console.WriteLine("Connected to server.");

            using var stream = client.GetStream();

            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            while (true)
            {
                Console.Write("Client: ");
                string message = Console.ReadLine() ?? string.Empty;

                if (message is null || message == "/exit") break;

                await writer.WriteLineAsync(message);

                string? reply = await reader.ReadLineAsync();
                if (reply is null) break;

                Console.WriteLine($"Server: {reply}");
            }
        }
    }
}
