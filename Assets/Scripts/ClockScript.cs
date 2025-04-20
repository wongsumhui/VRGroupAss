using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{ 
    [SerializeField] private Transform HoursPointer;
    [SerializeField] private Transform MinutesPointer;
    [SerializeField] private Transform SecondsPointer;

    private DateTime _Now;
    private Vector3 _DefaultPointer_Euler;
    private AudioSource _AudioSource;

    public int ExtraHours;
    public int ExtraMinutes;
    public int ExtraSeconds;


    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
        _DefaultPointer_Euler = HoursPointer.eulerAngles;
        StartCoroutine(UpdateClock());
    }


   private IEnumerator UpdateClock()
    {
        while (true)
        {
            _Now = DateTime.Now;
            yield return new WaitForSeconds(1);
           
             if (_AudioSource)
            _AudioSource.Play();

            _Now = _Now.AddHours(ExtraHours);
            _Now = _Now.AddSeconds(ExtraSeconds);
            _Now = _Now.AddMinutes(ExtraMinutes);
            ClockSetTime(_Now.Second, _Now.Minute, _Now.Hour);
        }
    }

    private void ClockSetTime(int Seconds,int Minutes,int Hours)
    {
        HoursPointer.eulerAngles = _DefaultPointer_Euler;
        MinutesPointer.eulerAngles = _DefaultPointer_Euler;
        SecondsPointer.eulerAngles = _DefaultPointer_Euler;


        float LerpSeconds = Mathf.InverseLerp(0,60f,Seconds);
        float LerpMinutes = Mathf.InverseLerp(0,60f,Minutes);
        float LerpHours = Mathf.InverseLerp(0, 12,Hours = Hours % 12);

        SecondsPointer.eulerAngles += (Vector3.right) * 360 * LerpSeconds;
        MinutesPointer.eulerAngles += Vector3.right * 360 * LerpMinutes;
        HoursPointer.eulerAngles += (Vector3.right * 360 * LerpHours) + (Vector3.right * 30 * LerpMinutes);

    }
}
