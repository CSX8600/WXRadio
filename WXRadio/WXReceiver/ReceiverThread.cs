using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using WXRadio.WeatherManager;

namespace WXReceiver
{
    class ReceiverThread
    {
        private WXReceiverPlugin _plugin;
        private Thread thread;
        private string _streamAddress;
        private int _streamPort;
        TcpClient client = null;
        internal ReceiverThread(WXReceiverPlugin pluginInstance)
        {
            _plugin = pluginInstance;
        }

        internal void Start(string streamAddress, int streamPort)
        {
            if (thread != null && thread.ThreadState == ThreadState.Running)
            {
                return;
            }

            thread = new Thread(new ThreadStart(Run));
            _streamAddress = streamAddress;
            _streamPort = streamPort;
            thread.Start();
        }

        internal void Abort()
        {
            if (thread != null && thread.ThreadState != ThreadState.Running)
            {
                return;
            }

            if (client != null)
            {
                try
                {
                    client.Close();
                }
                catch (Exception ex)
                {
                    _plugin.ThreadException(ex);
                }
            }

            if (thread != null)
            {
                thread.Abort();
            }
        }

        private void Run()
        {
            try
            {
                client = new TcpClient(_streamAddress, _streamPort);
                using (StreamReader reader = new StreamReader(client.GetStream()))
                {
                    while(true)
                    {
                        char[] sizeBuffer = new char[8];
                        reader.Read(sizeBuffer, 0, 8);

                        string sizeString = new string(sizeBuffer);
                        int size = int.Parse(sizeString);

                        char[] messageBuffer = new char[size];
                        reader.Read(messageBuffer, 0, size);
                        string message = new string(messageBuffer);

                        var eventObject = new { @event = "" };
                        eventObject = JsonConvert.DeserializeAnonymousType(message, eventObject);

                        switch(eventObject.@event)
                        {
                            case "heartbeat":
                                continue;
                            case "initial":
                                UpdateMessage initialUpdateMessage = JsonConvert.DeserializeObject<UpdateMessage>(message);
                                foreach(NewStormMessage initalMessage in initialUpdateMessage.storms)
                                {
                                    HandleNewStormMessage(initalMessage);
                                }
                                break;
                            case "update":
                                UpdateMessage updateMessage = JsonConvert.DeserializeObject<UpdateMessage>(message);
                                HandleUpdateStormMessage(updateMessage);
                                break;
                            case "new":
                                NewStormMessage newMessage = JsonConvert.DeserializeObject<NewStormMessage>(message);
                                HandleNewStormMessage(newMessage);
                                break;
                            case "delete":
                                var deleteMessage = new { id = 0 };
                                deleteMessage = JsonConvert.DeserializeAnonymousType(message, deleteMessage);
                                HandleDeleteStormMessage(deleteMessage.id);
                                break;
                        }
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                _plugin.ThreadException(ex);
            }
        }

        private void HandleUpdateStormMessage(UpdateMessage message)
        {
            foreach(NewStormMessage storm in message.storms)
            {
                StormManager.INSTANCE.UpdateStorm(new StormEventInfo()
                {
                    Event = "update",
                    ID = storm.id,
                    Strength = storm.strength,
                    X = storm.posx,
                    Z = storm.posz
                });
            }
        }

        private void HandleNewStormMessage(NewStormMessage message)
        {
            StormManager.INSTANCE.AddStorm(new StormEventInfo()
            {
                Event = "new",
                ID = message.id,
                Strength = message.strength,
                X = message.posx,
                Z = message.posz
            });
        }

        private void HandleDeleteStormMessage(int stormID)
        {
            StormManager.INSTANCE.DeleteStorm(new StormEventInfo()
            {
                Event = "delete",
                ID = stormID
            });
        }

        private class NewStormMessage
        {
            public int id { get; set; }
            public int strength { get; set; }
            public double posx { get; set; }
            public double posz { get; set; }
        }

        private class UpdateMessage
        {
            public List<NewStormMessage> storms { get; set; }
        }
    }
}
