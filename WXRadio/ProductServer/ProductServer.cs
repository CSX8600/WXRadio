using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace ProductServer
{
    internal static class ProductServer
    {
        private static TcpListener listener = null;
        private static Thread listeningThread = null;
        private static int _port;
        private static bool _isRunning = false;
        public static bool IsRunning => _isRunning;

        public static void Start(int port)
        {
            if (listeningThread != null && listeningThread.IsAlive)
            {
                return;
            }

            _port = port;
            listeningThread = new Thread(new ThreadStart(Thread));
            _isRunning = true;
            listeningThread.Start();
        }

        public static void Stop()
        {
            if (listener != null)
            {
                try
                {
                    listener.Stop();
                }
                catch { }
            }

            if (listeningThread != null && listeningThread.IsAlive)
            {
                listeningThread.Join();
            }
        }

        private static void Thread()
        {
            listener = new TcpListener(IPAddress.Any, _port);

            try
            {
                listener.Start();
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    List<ProductTransmission> productTransmissions = new List<ProductTransmission>();
                    foreach(BaseProduct product in ProductManager.INSTANCE.GetProducts())
                    {

                        ProductTransmission transmission = new ProductTransmission()
                        {
                            ID = product.ProductGuid,
                            Name = product.GetType().Name.ToDisplayString(),
                            Description = product.GetDetailedInformation(),
                            Coordinates = product.GetPolygonCoordinates()
                        };

                        if (product is ISummarizable summarizable)
                        {
                            transmission.Summary = summarizable.GetSummary();
                        }

                        if (product is ICancellable cancellable && cancellable.IsCancelled)
                        {
                            transmission.IsCancelled = cancellable.IsCancelled;
                        }

                        productTransmissions.Add(transmission);
                    }

                    using (StreamWriter writer = new StreamWriter(client.GetStream()))
                    {
                        writer.Write(JsonConvert.SerializeObject(productTransmissions));
                    }
                }
            }
            catch(SocketException se)
            {
                if (se.ErrorCode != 10004)
                {
                    throw se;
                }
            }

            _isRunning = false;
        }

        private class ProductTransmission
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
            public string Description { get; set; }
            public Coordinate[] Coordinates { get; set; }
            public bool IsCancelled { get; set; }
        }
    }
}
