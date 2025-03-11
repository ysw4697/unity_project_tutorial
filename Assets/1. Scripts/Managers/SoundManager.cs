using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    public void Play(Define.Sound type, string path, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }
        
        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.LogError($"오디오 클립을 못 찾음: {path}");
                return;
            }
        }
        else
        {
            
        }
    }
}
