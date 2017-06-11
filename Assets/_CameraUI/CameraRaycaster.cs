using RPG.Characters; // So we can detect by type
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        private const int POTENTIALLY_WALKABLE = 8;
        [SerializeField] private Vector2 cursorHotspot = new Vector2(96, 96);
        [SerializeField] private Texture2D enemyCursor = null;

        private float maxRaycastDepth = 100f; // Hard coded value

        [SerializeField] private Texture2D walkCursor = null;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        private bool RaycastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);

            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }

            return false;
        }

        // declare new delegate type
        private void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Implement UI interaction
            }
            else
            {
                PerformRaycasts();
            }
        }

        private void PerformRaycasts()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Specify layer priorities here ....
            if (RaycastForEnemy(ray)) { return; }
            if (RaycastForPotentiallyWalkable(ray)) { return; }
        }
    }
}