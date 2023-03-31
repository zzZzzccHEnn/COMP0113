using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;
using Ubiq.Extensions;
using System;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour, IComponent
{
    
    private bool timerActive = false;
    float currentTime;
    public int startMinutes;
    public TextMeshProUGUI currentTimeText;

    public NetworkId NetworkId { get; set; } 
    private NetworkContext context;



    void Start()
    {

        context = NetworkScene.Register(this);

    }

    
    void Update()
    {
        
    }

    public void StartTimer(){
        timerActive = true;
    }
    public void StopTimer(){
        timerActive = false;
    }
    public void ResetTimer(){
        startMinutes = 1;
        currentTime = startMinutes * 60;
    }

    public void SendUpdate()
    {

    }

}
