﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages traffic light statuses and updates them with correct sprites
/// </summary>
public class TrafficLightManager : MonoBehaviour
{
    List<TrafficLight> trafficLights = new List<TrafficLight>(){ // Whoops hardcoded lights, gotta fix this some time
        new TrafficLight() { Name = "motorised/0/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/1/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/2/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/3/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/4/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/5/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/6/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/7/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "motorised/8/traffic_light/0", Status = TrafficLightStatus.Red },
    };

    List<TrafficLight> otherLights = new List<TrafficLight>{
        new TrafficLight() { Name = "vessel/0/null/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "vessel/1/null/traffic_light/0", Status = TrafficLightStatus.Red },

        new TrafficLight() { Name = "cycle/0/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/1/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/2/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/3/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/3/traffic_light/1", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/4/traffic_light/0", Status = TrafficLightStatus.Red },
        new TrafficLight() { Name = "cycle/4/traffic_light/1", Status = TrafficLightStatus.Red }
    };

    #region SINGLETON PATTERN
    public static TrafficLightManager _instance;
    public static TrafficLightManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TrafficLightManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("TrafficSingleton");
                    _instance = container.AddComponent<TrafficLightManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TrafficLight light in trafficLights)
        {
            if (light.UpdateRequired)
            {
                light.UpdateRequired = false;
                var gameObject = GameObject.Find(light.Name);
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                switch (light.Status)
                {
                    case TrafficLightStatus.Green:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/MotorisedGreen");
                        break;
                    case TrafficLightStatus.Red:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/MotorisedRed");
                        break;
                    case TrafficLightStatus.Orange:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/MotorisedOrange");
                        break;
                    case TrafficLightStatus.Off:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/MotorisedOff");
                        break;
                }
            }
        }
        foreach(TrafficLight light in otherLights) 
        {
            if(light.UpdateRequired) 
            {
                light.UpdateRequired = false;
                var gameObject = GameObject.Find(light.Name);
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                switch (light.Status)
                {
                    case TrafficLightStatus.Green:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/OtherGreen");
                        break;
                    case TrafficLightStatus.Red:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/OtherRed");
                        break;
                    case TrafficLightStatus.Off:
                        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Lights/OtherOff");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Updates a motorised light to the preferred status
    /// </summary>
    /// <param name="lightName">Ex. motorised/6/traffic_light/0</param>
    /// <param name="status">Status of the light</param>
    internal void UpdateMotorisedLight(string lightName, TrafficLightStatus status)
    {
        trafficLights.Find(a => a.Name == lightName).Status = status;
        trafficLights.Find(a => a.Name == lightName).UpdateRequired = true;
    }

    /// <summary>
    /// Updates a light with 2 statuses to the preferred status
    /// </summary>
    /// <param name="lightName">Ex. vessel/0/traffic_light/0</param>
    /// <param name="status">Status of the light</param>
    internal void UpdateOtherLight(string lightName, TrafficLightStatus status)
    {
        otherLights.Find(a => a.Name == lightName).Status = status;
        otherLights.Find(a => a.Name == lightName).UpdateRequired = true;
    }

    /// <summary>
    /// Gets the status of a light
    /// </summary>
    /// <param name="lightName">Ex. motorised/6/traffic_light/0</param>
    /// <returns></returns>
    internal TrafficLightStatus CheckLightStatus(string lightName)
    {
        var allLights = trafficLights.Concat(otherLights).ToList();
        TrafficLight light = allLights.Find(a => a.Name == lightName.ToLower());

        if (light != null)
        {
            return light.Status;
        }
        return TrafficLightStatus.Off;
    }
}
