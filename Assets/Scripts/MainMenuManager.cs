using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Light settings")]
    public Light eyeLight;
    public float intensityLerpSpeed;

    private bool buttonUsed = false;

    private void Start()
    {
        AudioManager.instance.Play("OST");
    }

    private void Update()
    {
        LightEffect();
    }
    
    private bool glowing = true;
    private void LightEffect()
    {
        if (glowing)
        {
            eyeLight.intensity = Mathf.Lerp(eyeLight.intensity, 55f, intensityLerpSpeed * Time.deltaTime);
            if (eyeLight.intensity > 50)
            {
                glowing = false;
            }
        }
        else
        {
            eyeLight.intensity = Mathf.Lerp(eyeLight.intensity, 1f, intensityLerpSpeed * Time.deltaTime);
            if (eyeLight.intensity < 3)
            {
                glowing = true;
            }
        }
    }

    public void Play()
    {
        if (!buttonUsed)
        {
            buttonUsed = true;
            AudioManager.instance.Play("ButtonEffect");
            AudioManager.instance.Stop("OST");
            GameManager.Instance.BlinkEye(GameManager.BlinkEvent.INITIALIZEGAME);
        }
    }

    public void Salir()
    {
        if (!buttonUsed)
        {
            AudioManager.instance.Play("ButtonEffect");
            AudioManager.instance.Stop("OST");
            buttonUsed = true;
            GameManager.Instance.BlinkEye(GameManager.BlinkEvent.ENDGAME);
        }
    }
}
