using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Extensions;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;

public class Eraser: MonoBehaviour, IComponent
{
    /// <summary>
    /// // This property fulfils INetworkSpawnable. Spawnable objects need to 
    /// have their Ids set by the Object Spawner before they are registered, so
    /// all spawned objects can communicate with eachother.
    /// </summary>
    public NetworkId NetworkId { get; set; } 
    private NetworkContext context;

    void Start()
    {
        context = NetworkScene.Register(this);
    }


    public void SendUpdate()
    {

    }

}
