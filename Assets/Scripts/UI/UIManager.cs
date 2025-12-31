using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject extraDrawButton;

    public void SetExtraDrawButton(bool isActive)
    {
        extraDrawButton.SetActive(isActive);
    }
}
