
using UI.PauseMenu;
using UnityEngine;

class PausedBehavour : MonoBehaviour
{

    protected virtual void InnerUpdate()
    {

    }

    protected virtual void InnerFixedUpdate()
    {

    }
    private void Update()
    {
        if (PauseMenuController.Instance != null && PauseMenuController.Instance.Paused)
            return;
        InnerUpdate();
    }
    private void FixedUpdate()
    {
        if (PauseMenuController.Instance != null && PauseMenuController.Instance.Paused)
            return;
        InnerFixedUpdate();
    }
}