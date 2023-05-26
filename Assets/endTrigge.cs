using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endTrigge : MonoBehaviour
{
    public Interactor interactor;
    //public GameObject endscreenPanel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // endscreenPanel.SetActive(true);
            other.gameObject.GetComponent<Interactor>().ShowTable();
            other.gameObject.GetComponent<Interactor>().ExitMuseum();

        }
    }
}
