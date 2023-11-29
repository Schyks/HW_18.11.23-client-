using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Введiть IP-адресу сервера (127.0.0.1):");
        string ipAddress = Console.ReadLine();

        Console.WriteLine("Введiть порт для пiдключення (8001):");
        int port = Int32.Parse(Console.ReadLine());

        using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            try
            {
                await clientSocket.ConnectAsync(ipAddress, port);
                Console.WriteLine("Пiдключено до сервера.");
                Console.Write("Введiть повiдомлення (або 'bye' для завершення): ");
                while (true)
                {
                    Console.Write("Введiть повiдомлення: ");
                    string message = Console.ReadLine();

                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await clientSocket.SendAsync(buffer, SocketFlags.None);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = await clientSocket.ReceiveAsync(responseBuffer, SocketFlags.None);
                    string serverResponse = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine($"Сервер вiдповiдає: {serverResponse}");
                    if (serverResponse == "До зустрiчi!")
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}
