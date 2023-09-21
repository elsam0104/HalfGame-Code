using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Google.Protobuf;
using PROTO;

public class PacketAndID
{
    public IMessage packet;
    public PacketID packetId;
}

public class JobQueue
{
    public Queue<PacketAndID> _RecvQueue = new Queue<PacketAndID>();


    public void Update()
    {
        if (_RecvQueue.Count > 0)
        {
            {
                List<PacketAndID> packets = new List<PacketAndID>(_RecvQueue.ToArray());
                foreach (PacketAndID element in packets)
                {
                    Debug.Log((PacketID)element.packetId);
                    PacketManager.Instance.PacketHandler[element.packetId].Process(element.packet);
                }
                _RecvQueue.Clear();
            }

        }
    }
}
public class NetworkManager : MonoSingleTon<NetworkManager>
{
    public WebSocket ws;
    bool SendAble = true;
    public bool isLocalHost;
    public Queue<PacketAndID> _RecvQueue = new Queue<PacketAndID>();
    public Queue<PacketAndID> _SendQueue = new Queue<PacketAndID>();

    PacketManager packetManager;
    JobQueue jobQueue;

    object locker = new object();

    public bool isConnected
    {
        get
        {
            return ws != null;
        }
    }
    [SerializeField]
    private string url = "ws://localhost:8080";

    private void Awake()
    {
        packetManager = PacketManager.Instance;
        ws = new WebSocket(isLocalHost ? "ws://localhost:8080" : url);
        AddOnMessageListener(OnMessage);
        AddOpenListener(OnOpen);
        Connect();
        jobQueue = new JobQueue();
    }

    //connect to server
    public void Connect()
    {
        ws.ConnectAsync();
    }

    public void AddOnMessageListener(Action<object, MessageEventArgs> action)
    {
        EventHandler<MessageEventArgs> handler = (sender, e) => action(sender, e);
        ws.OnMessage += handler;
    }

    public void AddOpenListener(Action<object, EventArgs> action)
    {
        EventHandler handler = (sender, e) => action(sender, e);
        ws.OnOpen += handler;
    }

    public void OnMessage(object sender, MessageEventArgs e)
    {
        lock (locker)
        {
            print("Recv Success");
            if (e.IsBinary == true)
            {
                print("OnMessage+isBinary");
                ArraySegment<byte> data = new ArraySegment<byte>(e.RawData);
                int size = BitConverter.ToUInt16(data.Array, data.Offset);
                PacketID packetID = (PacketID)BitConverter.ToUInt16(data.Array, data.Offset + 2);
                print("Start ONRecv");
                packetManager._OnRecv[(PacketID)packetID](data, (PacketID)packetID);

            }
        }
    }

    public void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("Connected");
    }

    private void Update()
    {
        //lock(locker)
        {
            if (isConnected)
            {
                if (_RecvQueue.Count > 0)
                {
                    lock (locker)
                    {
                        print("_Recv Cont" + _RecvQueue.Count);
                        List<PacketAndID> packets = new List<PacketAndID>(_RecvQueue.ToArray());
                        foreach (PacketAndID element in packets)
                        {
                            Debug.Log((PacketID)element.packetId);
                            jobQueue._RecvQueue.Enqueue(element);
                        }
                        _RecvQueue.Clear();
                    }
                }
                jobQueue.Update();
                if (_SendQueue.Count > 0 && SendAble)
                {
                    PacketAndID element = _SendQueue.Dequeue();
                    SendPacket(element);
                }
            }
        }

    }


    public void Send(IMessage packet, PacketID id)
    {
        _SendQueue.Enqueue(new PacketAndID { packet = packet, packetId = id });
    }

    public void Send(PacketAndID packetAndID)
    {
        _SendQueue.Enqueue(packetAndID);
    }


    byte[] data = new byte[1024 * 10];
    void SendPacket(PacketAndID packetAndID)
    {
        SendAble = false;

        IMessage packet = packetAndID.packet;
        PacketID packetId = packetAndID.packetId;

        int len = packet.CalculateSize();
        ushort id = (ushort)packetId;

        Array.Copy(BitConverter.GetBytes((ushort)(len + 4)), 0, data, 0, 2);
        Array.Copy(BitConverter.GetBytes(id), 0, data, 2, 2);
        Array.Copy(packet.ToByteArray(), 0, data, 4, len);

        byte[] send = new byte[len + 4];
        Array.Copy(data, send, len + 4);


        ws.SendAsync(send, (bool success) =>
        {
            SendAble = true;
            if (success)
            {
                Debug.Log($"{packetId} Send Success");
            }
            else
            {
                Debug.Log($"{packetId} Send Failed");
            }
        });
    }
}

