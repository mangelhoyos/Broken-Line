using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LineMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    public static LineMovement Instance { private set; get; }

    [Header("Player prefs")] public float velocity;
    public float timeForWin;
    public Vector2 initialDirection;
    private float actualTime;

    private Vector3 actualAxis;

    private bool isDead = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Instance = this;
    }

    [HideInInspector]public bool initialized = false;
    
    void Start()
    {
        actualAxis = initialDirection;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        float actualVelocity = velocity;
        velocity = 0;
        yield return new WaitForSeconds(1);
        velocity = actualVelocity;
        AudioManager.instance.Play("PencilLoop");
        yield return new WaitForSeconds(1.5f);
        initialized = true;
    }

    private bool warningLaunched = false;
    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime >= timeForWin && !isDead)
        {
            isDead = true;
            GameManager.Instance.BlinkEye(GameManager.BlinkEvent.NEXTLEVEL);
            AudioManager.instance.Stop("PencilLoop");
        }else if (actualTime >= (timeForWin - 11) && !warningLaunched)
        {
            warningLaunched = true;
            AudioManager.instance.Play("Warning");
        }

        if (initialized)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 moveAxis = new Vector3(h, v, 0f);

            CheckChangeInAxis(h, v, moveAxis);

            transform.localPosition += actualAxis * velocity * Time.deltaTime;
        }
    }

    private void CheckChangeInAxis(float h, float v, Vector3 moveAxis)
    {
        if (h != 0 || v != 0)
        {
            if (moveAxis != actualAxis)
            {
                //Sonido cambio dirección
            }
            actualAxis = moveAxis;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isDead && initialized)
        {
            GameManager.Instance.BlinkEye(GameManager.BlinkEvent.RESTART);
            AudioManager.instance.Stop("PencilLoop");
            isDead = true;
        }
    }
}
