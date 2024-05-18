using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "System/Sound pitch Channel")]
public class CH_SoundPitch : SO_ChannelDefaut
{
    public UnityAction<AudioClip, float> OnFunctionEvent;
    public void RaiseEvent(AudioClip _audio, float _pitch)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_audio, _pitch);
    }
    public void RaiseEvent(AudioClip _audio, float _pitchMin, float _pitchMax)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_audio, Random.Range(_pitchMin, _pitchMax));
    }
    public void RaiseEvent(R_SoundEffect _audio)
    {
        if (OnFunctionEvent != null)
            OnFunctionEvent.Invoke(_audio.sound, Random.Range(_audio.pitchMin, _audio.pitchMax));
    }
}
