using Interface;
using UnityEngine;

namespace MilitaryGame.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private Camera _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition),
                _camera.transform.position);

            if (hit.collider is null)
                return;
        
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.CompareTag("GameBoard"))
                {
                    MilitaryGameEventLib.Instance.CloseInformationPanel?.Invoke();
                    return;
                }

                if (hit.transform.gameObject.TryGetComponent(out ILeftClickable clickable))
                {
                    clickable.OnLeftClick();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (hit.transform.gameObject.TryGetComponent(out IRightClickable clickable))
                    clickable.OnRightClick();
            }
        }   
    }
}
