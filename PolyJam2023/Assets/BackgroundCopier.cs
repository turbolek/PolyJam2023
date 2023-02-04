using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCopier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BackgroundElement backgroundElement = collision.gameObject.GetComponent<BackgroundElement>();

        if (backgroundElement != null)
        {
             BackgroundElement newElement = Instantiate(backgroundElement.Prefab, backgroundElement.transform.parent);
            newElement.transform.position = backgroundElement.transform.position;
            newElement.transform.Translate(new Vector3(backgroundElement.Width, 0f, 0f));
            newElement.PlayerPreviousXCoord = backgroundElement.PlayerPreviousXCoord;
        }
    }
}
