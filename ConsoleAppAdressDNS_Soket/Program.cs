using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

//С использованием класса Socket напишите клиентское и серверное
//приложения. Клиент отправляет серверу введенную пользователем строку. Сервер
//отображает IP-адрес и порт подключившегося клиента, а также полученную
//строку, после чего посылает клиенту текущее время. Клиент отображает
//полученное время. 
namespace ConsoleAppAdressDNS_Soket
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Loopback;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 1024);
            Socket Listensocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
           

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                Listensocket.Bind(iPEndPoint);
                // начинаем прослушивание
                Listensocket.Listen(5);
               
                Console.WriteLine("The server was started");

                while (true)
                {
                    Socket socket = Listensocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    //количесвто полученных байт 
                    int byteLength = 0;
                    //буфер для получаемых данных
                    byte[] buff = new byte[256];

                   
                  
                    Console.WriteLine($"Сервер начал прослушивание на порту {iPEndPoint.Port}");
                    Console.WriteLine($"Подключился хост {socket.RemoteEndPoint}");

                    do
                    {
                        //количесвто переданных байт
                        byteLength = socket.Receive(buff);
                        builder.Append(Encoding.Unicode.GetString(buff,0,byteLength));


                    } while (socket.Available>0);
                    //выводим мообщение от клиента
                   
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} : {builder} ");

                    //отправляем ответы
                    string message = $"{iPAddress} : данные от клиента получены {DateTime.Now.ToShortDateString()} ";
                    buff = Encoding.Unicode.GetBytes(message);

                    socket.Send(buff);
                    //Закрываем сокет
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
            }
            catch (SocketException ex)
            {

                Console.WriteLine($"Error {ex.Message}");
            }



        }
    }
}
