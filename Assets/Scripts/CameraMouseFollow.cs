using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseFollow : MonoBehaviour
{

    private int screenWidth;
    private int screenHeigth;
    private int middlePointX;
    private int middlePointY;

    private Vector3 cameraInitialPosition;

    public float offsetX;
    public float offsetY;
    
    void Start()
    {
        cameraInitialPosition = transform.position;
        
        screenWidth = Screen.width;
        screenHeigth = Screen.height;

        middlePointX = screenWidth / 2;
        middlePointY = screenHeigth / 2;
    }
    
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        float xValue = WidthValue(mousePos);
        float yValue = HeigthValue(mousePos);

        Vector3 offsetVector = new Vector3(xValue * offsetX, 1, yValue * offsetY);

        transform.position = Vector3.Scale(cameraInitialPosition, offsetVector);

    }

    float WidthValue(Vector3 mousePos)
    {
        float value = 0;
        if (mousePos.x > middlePointX)
        {
            value = (mousePos.x - middlePointX) / middlePointX;
        }else if (mousePos.x < middlePointX)
        {
            value = (middlePointX - mousePos.x) / middlePointX;
            value *= -1;
        }
        else
        {
            value = 0;
        }

        return value;
    }

    float HeigthValue(Vector3 mousePos)
    {
        float value = 0;
        if (mousePos.y > middlePointY)
        {
            value = (mousePos.y - middlePointY) / middlePointY;
        }else if (mousePos.y < middlePointY)
        {
            value = (middlePointY - mousePos.y) / middlePointY;
            value *= -1;
        }
        else
        {
            value = 0;
        }
    
        return value;
    }
}
