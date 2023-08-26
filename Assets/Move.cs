using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour, IPointerDownHandler,IPointerUpHandler 
{

    public Vector2 direction;
    public Grid grid;
    float threshold = 0.1f;
    bool pressed = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Press());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
    public IEnumerator Press()
    {
        pressed = true;
        while (pressed)
        {
            grid.Move(direction);
            if (direction == Vector2.up)
            {

            yield return new WaitForSeconds(threshold*2);
            }
            else
            {
                yield return new WaitForSeconds(threshold);

            }
        }
    }
}
