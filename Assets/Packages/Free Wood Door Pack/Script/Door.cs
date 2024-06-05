using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]


    public class Door : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;
        Quaternion target;
        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

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
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            if (Quaternion.Angle(transform.localRotation, target) <= 0.02f) { state = DoorState.NotMoving; }
        }

        public void OpenDoor()
        {
            if (state == DoorState.NotMoving)
            {
                state = DoorState.Moving;
                if (open) { target = Quaternion.Euler(0, DoorCloseAngle, 0); }
                else { target = Quaternion.Euler(0, DoorOpenAngle, 0); }

                open = !open;
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
            }
        }
        void OnUse(Transform player)
        {
            
            OpenDoor();
        }
        public void Unlock()
        {
          //  locked  = false;
            OpenDoor();
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