using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]


    public class Door : MonoBehaviour
    {
        private GameObject player;
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        Quaternion target;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        public bool unlockOnUse = true;

        // public bool locked = false;

        //public LockBehaviour lockObject;
        //  public Transform playerPos;

        public enum DoorState
        {
            Moving,
            NotMoving
        }

        public DoorState state = DoorState.NotMoving;

        // Use this for initialization
        void Start()
        {
            asource = GetComponent<AudioSource>();
            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            if (Quaternion.Angle(transform.localRotation, target) <= 0.02f) 
            {
                var collider = GetComponent<BoxCollider>();
                collider.enabled = true;
                state = DoorState.NotMoving; 
            }
        }

        public void OpenDoor(int direction)
        {
            if (!unlockOnUse) return;
            if (state == DoorState.NotMoving)
            {
              //  DisableColliderOnOpen();
                state = DoorState.Moving;
                if (open) { target = Quaternion.Euler(0, DoorCloseAngle, 0); }
                else
                {
                    target = Quaternion.Euler(0, DoorOpenAngle * direction, 0);
                }

                open = !open;
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
            }
        }

        void OnUse(Transform player)
        {
            if (unlockOnUse)
                DetermineSide();
        }
        public void Unlock()
        {
          //  locked  = false;
            DetermineSide();
        }


        public void DetermineSide()
        {

            Vector3 directionToTarget = player.transform.position - transform.position;
            Vector3 forwardDirection = transform.forward;

            float dotProductForward = Vector3.Dot(directionToTarget.normalized, forwardDirection);

            if (dotProductForward > 0)
            {
                OpenDoor(1);
            }
            else
            {
                OpenDoor(-1);
            }
        }






        //public void StartLockpicking()
        //{
        //    if (!locked || !lockObject)
        //    {
        //        OpenDoor();
        //        return;
        //    }
        //    if (open) return;
        //    //Debug.Log("Starting lockpick (door)");

        //    lockObject.gameObject.SetActive(true);
        //    lockObject.StartLockPicking();

        //    PlayerSingleton.Instance.StartLockPicking(this);


        //}
    }
}