using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionScreenController : MonoBehaviour
{
    public enum TransitionScreenDirection
    {
        In,
        Out
    }

    [SerializeField] private UnityEvent _onSlideInComplete;
    [SerializeField] private UnityEvent _onSlideOutComplete;
    
    private TransitionScreenDirection _transitionScreenDirection;

    public void SetTransitionScreenDirection(TransitionScreenDirection direction)
    {
        _transitionScreenDirection = direction;
    }

    public void AnimationComplete()
    {
        switch (_transitionScreenDirection)
        {
            case TransitionScreenDirection.In:
                _onSlideInComplete.Invoke();
                break;
            case TransitionScreenDirection.Out:
                _onSlideOutComplete.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
