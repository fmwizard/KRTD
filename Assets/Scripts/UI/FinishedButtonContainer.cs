using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedButtonContainer : MonoBehaviour
{
    public GameObject finishedButtonContainer;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        finishedButtonContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Finished)
        {
            if (!isActive)
            {
                finishedButtonContainer.SetActive(true);
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
                finishedButtonContainer.SetActive(false);
                isActive = false;
            }
        }
    }
}
