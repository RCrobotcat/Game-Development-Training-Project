using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDshadowController : MonoBehaviour
{
    public GameObject CDshadow; // used to store the CD shadow

    // close the CD shadow
    public void CDshadowClose()
    {
        CDshadow.SetActive(false);
    }
}
