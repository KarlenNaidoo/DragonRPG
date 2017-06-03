using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {

        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;

        AICharacterControl aiCharacterControl = null;

        bool isIndirectMode = false;

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        GameObject walkTarget = null;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
            walkTarget = new GameObject("walkTarget");
        }


        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case enemyLayerNumber:
                    // navigate to the enemy
                    GameObject enemy = raycastHit.collider.gameObject;
                    aiCharacterControl.SetTarget(enemy.transform);
                    break;
                case walkableLayerNumber:
                    // navigate point on the ground
                    walkTarget.transform.position = raycastHit.point;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;
                default:
                    Debug.LogWarning("Don't know how to handle mouse click for player movement");
                    break;
            }

        }

        //TODO Make this get called again

        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }

    }

}