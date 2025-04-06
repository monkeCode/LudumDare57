using UnityEngine;

namespace Core
{
    public class InteractionController: MonoBehaviour
    {
        private IInteractable _interactable;
    
        private bool isInteractable;

        private void Start()
        {
            _interactable = gameObject.GetComponent<IInteractable>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && isInteractable)
            {
                _interactable.Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player.Player _))
            {
                isInteractable = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player.Player _))
            {
                isInteractable = false;
            }
        }
    }
}