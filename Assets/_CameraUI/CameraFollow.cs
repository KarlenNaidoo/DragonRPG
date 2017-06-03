using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        private float offset;
        private GameObject player;
        // Update is called once per frame
        private void LateUpdate()
        {
            transform.position = player.transform.position; // locks permission to player
        }

        // Use this for initialization
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}