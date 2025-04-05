using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    class KeySelector: MonoBehaviour
    {
        private EventSystem _eventSystem;
        public GameObject firstSelected;

        private void Start()
        {
            _eventSystem = EventSystem.current;
            _eventSystem.SetSelectedGameObject(null);
        }
        
        private void Update()
        {
            if (_eventSystem.currentSelectedGameObject is null && (Input.GetKeyDown(KeyCode.DownArrow) 
                                                                   || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                _eventSystem.SetSelectedGameObject(firstSelected, new BaseEventData(_eventSystem));
            }

            if (_eventSystem.currentSelectedGameObject is not null && Input.GetKeyDown(KeyCode.Escape))
            {
                _eventSystem.SetSelectedGameObject(null);   
            }
        }

    }
}