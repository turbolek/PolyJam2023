using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BackgroundElement backgroundElement = collision.gameObject.GetComponent<BackgroundElement>();

        if (backgroundElement != null)
        {
            Destroy(backgroundElement.gameObject);
        }
    }
}
