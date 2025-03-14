using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public AudioClip audioClip;
    public AudioClip audioClip2;
    
    private int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        i++;
        
        if (i % 2 == 0)
        {
            Managers.Sound.Play(audioClip, Define.Sound.Bgm);
        }
        else
        {
            Managers.Sound.Play(audioClip2, Define.Sound.Bgm);
        }
    }
}
