using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppClient
{
    class Program
    {
        static void Main(string[] args)
        {

            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1024);
            Socket SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                //подключаеися к серверу
                SenderSocket.Connect(iPEndPoint);
                if (SenderSocket.Connected)
                {
                    //отправляем сообщение
                    Console.Write("Введите сообщение >>");
                    string sendMessage = Console.ReadLine();
                    byte[] buff = Encoding.Unicode.GetBytes(sendMessage);
                    SenderSocket.Send(buff);
                    //получаем ответ
                    buff = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = SenderSocket.Receive(buff);
                        builder.Append(Encoding.Unicode.GetString(buff,0,bytes));

                    } while (SenderSocket.Available>0);

                    Console.WriteLine($"Ответ от сервера  {builder}");

                    SenderSocket.Shutdown(SocketShutdown.Both);
                    SenderSocket.Close();


                }





            }
            catch (SocketException ex)
            {

                Console.WriteLine($"Error {ex.Message}");
            }



            Console.ReadLine();
        }
    }
}
