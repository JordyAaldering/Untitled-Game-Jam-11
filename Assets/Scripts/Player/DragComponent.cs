using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Camera))]
    public class DragComponent : MonoBehaviour
    {
        private bool isDragging;
        private GameObject target;
        private Vector3 origin;
        private Vector3 offset;

        private float mouseDownTime;
        private Camera cam;

        private void Awake() => cam = GetComponent<Camera>();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownTime = Time.time;
                Select();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isDragging && Time.time - mouseDownTime < 0.2f)
                    Rotate();
                isDragging = false;
            }

            if (isDragging)
            {
                Drag();
            }
        }

        private void Select()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
            {
                isDragging = true;
                target = hit.collider.gameObject;

                Vector3 pos = target.transform.position;
                origin = cam.WorldToScreenPoint(pos);
                offset = pos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                             origin.z));
            }
        }

        private void Drag()
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, origin.z);
            Vector3 nextPos = cam.ScreenToWorldPoint(mousePos) + offset;
            target.transform.position = nextPos;
        }

        private void Rotate()
        {
            target.transform.Rotate(0f, 0f, 90f);
        }
    }
}
