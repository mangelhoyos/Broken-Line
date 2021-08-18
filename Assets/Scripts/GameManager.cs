using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    private int actualLevelIndex = 0;

    public GameObject[] levelPrefabs;

    private GameObject actualGamePref;

    public Animator upperEye;
    public Animator bottomEye;
    
    private DepthOfField depth;
    public float blurSpeed;

    public enum BlinkEvent
    {
        RESTART,
        NEXTLEVEL,
        INITIALIZEGAME,
        ENDGAME
    }

    void Awake()
    {
        Instance = this;
        
        VolumeProfile volumeProfile = FindObjectOfType<Volume>()?.profile;
        if(!volumeProfile) throw new NullReferenceException(nameof(VolumeProfile));

        if(!volumeProfile.TryGet(out depth)) throw new NullReferenceException(nameof(depth));

    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(upperEye.transform.parent.gameObject);

        Application.targetFrameRate = 70;
    }


    public void BlinkEye(BlinkEvent blinkEvent)
    {
        if (blinkEvent != BlinkEvent.INITIALIZEGAME)
        {
            StartCoroutine(BlinkBlur(false));
            upperEye.Play("BlinkUpper");
            bottomEye.Play("BlinkBottom");
        }

        StartCoroutine(ExecuteEvent(blinkEvent));
    }

    IEnumerator BlinkBlur(bool skip)
    {
        float x = 35;
        if (!skip)
        {
            do
            {
                x -= Time.deltaTime * blurSpeed;
                depth.focusDistance.Override(x);
                yield return null;
            } while (x > 2);
        }
        else
        {
            x = 1;
        }

        yield return new WaitForSeconds(0.8f);
        do
        {
            x += Time.deltaTime * blurSpeed;
            depth.focusDistance.Override(x);
            yield return null;
        } while (x < 35);
        depth.focusDistance.Override(35);
    }

    IEnumerator ExecuteEvent(BlinkEvent blinkEvent)
    {
        if (blinkEvent == BlinkEvent.NEXTLEVEL || blinkEvent == BlinkEvent.RESTART || blinkEvent == BlinkEvent.ENDGAME )
        {
            if (actualLevelIndex % 2 != 0)
            {
                AudioManager.instance.Stop(Convert.ToString(actualLevelIndex));
            }
        }
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("Restart");
        yield return new WaitForSeconds(0.5f);
        switch (blinkEvent)
        {
            case BlinkEvent.RESTART:
                Destroy(actualGamePref.gameObject);
                AudioManager.instance.Stop("Warning");
                AudioManager.instance.Play(Convert.ToString(actualLevelIndex));
                actualGamePref = Instantiate(levelPrefabs[actualLevelIndex], levelPrefabs[actualLevelIndex].transform.position, levelPrefabs[actualLevelIndex].transform.rotation);
                break;
            
            case BlinkEvent.NEXTLEVEL:
                Destroy(actualGamePref);
                actualLevelIndex++;
                if (actualLevelIndex < levelPrefabs.Length)
                {
                    if (actualLevelIndex % 2 != 0)
                    {
                        AudioManager.instance.Play(Convert.ToString(actualLevelIndex));
                        actualGamePref = Instantiate(levelPrefabs[actualLevelIndex],
                            levelPrefabs[actualLevelIndex].transform.position,
                            levelPrefabs[actualLevelIndex].transform.rotation);
                    }else
                    {
                        TextWriter.instance.NextText();
                    }
                }

                break;
            
            case BlinkEvent.INITIALIZEGAME:

                yield return new WaitForSeconds(1.8f);
                
                StartCoroutine(BlinkBlur(false));
                upperEye.Play("BlinkUpper");
                bottomEye.Play("BlinkBottom");

                yield return new WaitForSeconds(1f);
                
                StopCoroutine("BlinkBlur");
                depth.focusDistance.Override(1);
                SceneManager.LoadScene("Main");
                yield return null;

                VolumeProfile volumeProfile = FindObjectOfType<Volume>()?.profile;
                if(!volumeProfile) throw new NullReferenceException(nameof(VolumeProfile));

                if(!volumeProfile.TryGet(out depth)) throw new NullReferenceException(nameof(depth));
                
                
                TextWriter.instance.NextText();
                break;
            case BlinkEvent.ENDGAME:
                Application.Quit();
                break;
        }
    }
}
