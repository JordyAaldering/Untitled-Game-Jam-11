using UnityEngine;

namespace Game
{
    public class MovingWall : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;

        private float currentMoveSpeed = 0f;
        private float endZ = 0f;

        public void Initialise(float startZ, float endZ)
        {
            transform.position = new Vector3(0f, 0f, startZ);
            currentMoveSpeed = moveSpeed;
            this.endZ = endZ;
        }
        
        private void Update()
        {
            if (transform.position.z <= endZ)
            {
                // Check Game State
                return;
            }
            
            transform.Translate(Time.deltaTime * currentMoveSpeed * -transform.forward);
        }
    }
}
