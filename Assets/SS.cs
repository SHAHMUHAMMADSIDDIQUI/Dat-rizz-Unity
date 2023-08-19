using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS : MonoBehaviour
{
    [ContextMenu("SS")]
       public void Ss()
    {
        ScreenCapture.CaptureScreenshot("screenshot.png");

    }

   }
