using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class DragAndDrop : MonoBehaviour
    {
        private Vector3 startPosition;
        private bool _dragging = false;
        void Update()
        {
            if (_dragging)
            {
                this.transform.position = Input.mousePosition;
                if (Input.GetMouseButtonUp(Keys.BMouseLeft))
                {
                    this.enabled = false;
                }
            }
        }

        public void EnableDrag()
        {
            startPosition = Input.mousePosition;
            _dragging = true;
        }
        public void Test()
        {
            Debug.Log("asdfasdf");
        }
    }
}