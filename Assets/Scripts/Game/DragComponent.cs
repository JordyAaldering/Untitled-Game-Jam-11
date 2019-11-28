#pragma warning disable 0649
using System.Text.RegularExpressions;
using Board;
using Cut;
using Grid;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class DragComponent : MonoBehaviour
    {
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private CutSettings cutSettings;
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private GridCurrent gridCurrent;
        
        private bool isDragging;
        private Vector3 origin;
        private Vector3 offset;
        
        private GameObject target;
        private Collider targetCol;

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
                if (isDragging && Time.time - mouseDownTime < 0.1f)
                    TryRotate();
                isDragging = false;
            }

            if (isDragging)
            {
                TryDrag();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var component in gridCurrent.components)
            {
                if (component.Key == 0)
                    continue;
                
                foreach (Vector2Int v in component.Value)
                {
                    float x = (cutSettings.horizontalCuts.GetClamped(v.x) + cutSettings.horizontalCuts.GetClamped(v.x - 1)) * 0.5f;
                    float y = (cutSettings.verticalCuts.GetClamped(v.y) + cutSettings.verticalCuts.GetClamped(v.y - 1)) * 0.5f;
                    Gizmos.DrawSphere(new Vector3(x, y), 0.1f);
                }
            }
        }

        private void Select()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
            {
                isDragging = true;
                targetCol = hit.collider;
                target = targetCol.gameObject;

                Vector3 pos = target.transform.position;
                origin = cam.WorldToScreenPoint(pos);
                offset = pos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, origin.z));
            }
        }

        private void TryDrag()
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, origin.z);
            Vector3 nextPos = cam.ScreenToWorldPoint(mousePos) + offset;
            
            Vector3 targetPos = target.transform.position;
            Vector2Int dir = new Vector2Int(
                Mathf.RoundToInt(Mathf.Clamp(nextPos.x - targetPos.x, -1f, 1f)),
                Mathf.RoundToInt(Mathf.Clamp(nextPos.y - targetPos.y, -1f, 1f))
            );

            if (dir != Vector2Int.zero && gridCurrent.TryMove(gridSettings, int.Parse(Regex.Match(target.name, @"\d+").Value), dir))
            {
                int x = Mathf.FloorToInt(targetPos.x) + dir.x;
                int y = Mathf.FloorToInt(targetPos.y) + dir.y;

                nextPos.x = cutSettings.horizontalCuts.ClampBounds(x, 0f, boardSettings.boardWidth);
                nextPos.y = cutSettings.verticalCuts.ClampBounds(y, 0f, boardSettings.boardHeight);
                target.transform.position = nextPos;
                
                origin = cam.WorldToScreenPoint(nextPos);
                offset = nextPos - cam.ScreenToWorldPoint(mousePos);
            }
        }

        private void TryRotate()
        {
            int i = int.Parse(Regex.Match(target.name, @"\d+").Value);
            if (gridCurrent.TryRotate(gridSettings, i))
            {
                Vector3 point = new Vector3(gridCurrent.components[i][0].x, gridCurrent.components[i][0].y, target.transform.position.z);
                target.transform.RotateAround(point, Vector3.forward, 90f);
            }
        }
    }
}
