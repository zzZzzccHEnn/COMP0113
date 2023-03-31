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
 
public class ControlPanel : MonoBehaviour, IGraspable, IComponent
{
    
    bool timerActive = false;
    float currentTime;
    public int startMinutes;
    public TextMeshProUGUI currentTimeText;
    /// <summary>
    /// // This property fulfils INetworkSpawnable. Spawnable objects need to 
    /// have their Ids set by the Object Spawner before they are registered, so
    /// all spawned objects can communicate with eachother.
    /// </summary>
 
    public NetworkId NetworkId { get; set; } 
 
    private FollowHelper follow;
    private NetworkContext context;
    private ContraptionManager manager;
 
    public void Grasp(Hand controller)
    {
        follow.Grasp(controller);
    }
 
    public void Release(Hand controller)
    {
        follow.Release(controller);
    }
 
    private void Awake()
    {
        follow = new FollowHelper(transform);
    }
 
    void Start()
    {
        context = NetworkScene.Register(this);
        currentTime = startMinutes * 60;
    }
 
    
    void Update()
    {
        if (timerActive == true && currentTime >0 ){
            currentTime = currentTime - Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        SendUpdate();
    }
 
    public void StartTimer(){
        timerActive = true;
    }
    public void StopTimer(){
        timerActive = false;
    }
    public void ResetTimer(){
        startMinutes = 2;
        currentTime = startMinutes * 60;
    }
 
    private struct Message
    {
        public float value;
        public bool active;
    }
 
    public void SendUpdate()
    {
        context.SendJson(new Message()
        {
            active = timerActive,
            value = currentTime // Include the current time value in the message
        });
    }
 
    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        TimeSpan updatedTime = TimeSpan.FromSeconds(message.value);
        string updatedText = updatedTime.Minutes.ToString() + ":" + updatedTime.Seconds.ToString(); // Update the UI text to show the new time value
        if (updatedText != currentTimeText.text || timerActive != message.active)
        {
            timerActive = message.active;
            currentTime = message.value; // Update the current time value based on the received message
            currentTimeText.text = updatedText;
        }
    }
}