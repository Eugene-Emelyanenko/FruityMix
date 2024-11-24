using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drum : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public float rotationSpeed = 100f; // Скорость вращения барабана
    public int numberOfDigits = 10; // Количество цифр на барабане
    
    public int SelectedNumber { get; private set; }
    
    private float anglePerDigit; // Угол между цифрами на барабане
    private bool isDragging = false;
    private float currentRotationAngle = 0f; // Текущий угол поворота барабана

    private void Start()
    {
        SelectedNumber = 0;
        anglePerDigit = 360f / numberOfDigits;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            float rotationDelta = -eventData.delta.x * rotationSpeed * Time.deltaTime;
            currentRotationAngle += rotationDelta;
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotationAngle);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            DetermineSelectedNumber();
        }
    }
    
    void DetermineSelectedNumber()
    {
        float normalizedAngle = Mathf.Repeat(currentRotationAngle, 360f);
        SelectedNumber = Mathf.FloorToInt(normalizedAngle / anglePerDigit);
        Debug.Log("Selected number: " + SelectedNumber);
        RotateToNearestNumber(SelectedNumber * anglePerDigit);
    }

    void RotateToNearestNumber(float targetAngle)
    {
        StartCoroutine(RotateToAngle(targetAngle));
    }

    IEnumerator RotateToAngle(float targetAngle)
    {
        float angleDifference = Mathf.DeltaAngle(currentRotationAngle, targetAngle);
        float rotationSpeed = 180f;

        while (Mathf.Abs(angleDifference) > 0.01f)
        {
            float rotationStep = Mathf.Sign(angleDifference) * rotationSpeed * Time.deltaTime;
            float newAngle = currentRotationAngle + rotationStep;
            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
            angleDifference = Mathf.DeltaAngle(newAngle, targetAngle);
            currentRotationAngle = newAngle;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        currentRotationAngle = targetAngle;
    }
}