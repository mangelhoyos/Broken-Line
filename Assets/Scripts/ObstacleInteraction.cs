using System;
using UnityEngine;

public class ObstacleInteraction : MonoBehaviour
{
    public enum Animation
    {
        SCALE,
        ROTATE,
    }
    [Header("General settings")]
    public Animation animInteraction;
    public float animationIntensity;

    [Header("Scale pref")] 
    public float scaleMultiplier;

    private Vector3 initialScale;
    private Vector3 desiredScale;

    private void Start()
    {
        initialScale = transform.localScale;
        desiredScale = initialScale * scaleMultiplier;
    }

    void Update()
    {
        switch (animInteraction)
        {
            case Animation.SCALE:
                ScaleAnim();
                break;
            
            case Animation.ROTATE:
                RotateAnim();
                break;
        }
    }

    void RotateAnim()
    {
        transform.Rotate (new Vector3 (0, 0, animationIntensity) * Time.deltaTime);
    }

    private bool expanding = false;
    void ScaleAnim()
    {
        if (expanding)
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, desiredScale, animationIntensity * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, desiredScale) < 0.3f)
            {
                expanding = false;
            }
        }else
        {
            transform.localScale =
                Vector3.Lerp(transform.localScale, initialScale, animationIntensity * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, initialScale) < 0.3f)
            {
                expanding = true;
            }
        }
    }
    
}
