﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRTK;

public class VRTK_ButtonDownLoading : MonoBehaviour
{
    public VRTK_ControllerEventRegistrator.Button button;
    public VRTK_ControllerEvents controllerEvents;
    public VRTK_InteractGrab grab;

    public float loadingTime = 2f;
    public float tick = 0.1f;

    public Color color;

    public UnityEvent OnStartLoading;
    public UnityEvent OnLoading;
    public UnityEvent OnCanceled;
    public UnityEvent OnFinishLoading;
    

    private DateTime startLoadingTime;
    private bool isFinished = false;
    private bool isLoading = false;
    private float duration;

    void Awake()
    {
        VRTK_ControllerEventRegistrator.Register(controllerEvents, button, VRTK_ControllerEventRegistrator.ButtonAction.Pressed, ControllerEvents_ButtonPressed);
        VRTK_ControllerEventRegistrator.Register(controllerEvents, button, VRTK_ControllerEventRegistrator.ButtonAction.Released, ControllerEvents_ButtonReleased);
        GetComponentInChildren<Image>().color = color;
    }

    private void ControllerEvents_ButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!grab.IsGrabButtonPressed())
        {
            Debug.Log("Start loading");
            startLoadingTime = DateTime.Now;
            isFinished = false;
            isLoading = true;
            OnStartLoading.Invoke();
            StartCoroutine(Tick());
        }
    }

    private void ControllerEvents_ButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!isFinished)
        {
            OnCanceled.Invoke();
            StopCoroutine(Tick());
            isLoading = false;
            Debug.Log("Canceled");
        }
    }

    private IEnumerator Tick()
    {

        duration = 0f;

        while (duration <= loadingTime && isLoading)
        {
            yield return new WaitForSecondsRealtime(tick);
            duration = (float)(System.DateTime.Now - startLoadingTime).TotalSeconds;
            OnLoading.Invoke();
            Debug.Log(GetCurrentPercentage().ToString() + "%");
        };

        if (isLoading)
        {
            Debug.Log("Finished");
            isFinished = true;
            isLoading = false;
            OnFinishLoading.Invoke();
        }

    }

    public float GetCurrentPercentage()
    {
        return (duration/loadingTime) * 100f;
    }
}
