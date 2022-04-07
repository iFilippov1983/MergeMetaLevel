using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial.Game
{
    public class BgDevourTest :MonoBehaviour, IPointerClickHandler
    {
        public BgController BgController;
        public Camera Camera;
        public Transform Container;

        private void OnEnable()
        {
            BgController.Init(Container);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);
            mousePos.z = 0;
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log("Left click");
                BgController.Add(new Vector2Int((int) mousePos.x, (int) mousePos.y));
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                Debug.Log("Middle click");
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Right click");
                BgController.Delete(new Vector2Int((int) mousePos.x, (int) mousePos.y));
            }
        }
        
    }
}