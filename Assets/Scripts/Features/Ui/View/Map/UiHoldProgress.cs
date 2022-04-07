using UnityEngine;

namespace Components.Ui.Map
{
    public class UiHoldProgress : MonoBehaviour
    {
        public Animator Animator;
        public Animation Animation;

        public void Show()
        {
            gameObject.SetActive(true);
            Animation.Play();
            transform.position = Input.mousePosition;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}