using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animation))]
public class PlayAnimation : MonoBehaviour
{
    [SerializeField] Animation _animation;
    [SerializeField] PlayMode _playMode;
    [SerializeField] private UnityEvent _onAnimationStart;
    [SerializeField] private UnityEvent _onAnimationComplete;

    public void Play()
    {
        _animation.Play(_playMode);
        _onAnimationStart.Invoke();
    }

    private void Reset()
    {
        _animation = GetComponent<Animation>();
    }

    public void AnimationComplete()
    {
        _onAnimationComplete.Invoke();
        
    }
}
