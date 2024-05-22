
// //handle picking up 3d objects while in 3d 
// using UnityEngine;

// private void Handle3DInteractions()
// {
//     var closestGOToCamera = PlayerBehaviour.Instance.GetClosest3DObjectOnLayers(interactableLayerMask);

//     //handle picking up objects
//     if (closestGOToCamera != null)
//     {
//         //perform raycast to the object from the camera
//         //var ray = new Ray(Camera.main.transform.position, closestGOToCamera.transform.position - Camera.main.transform.position);

//         var position = PlayerBehaviour.Instance.player3D.transform.position;
//         var ray = new Ray(position, closestGOToCamera.transform.position - position);



//         //if the raycast hits something that wasnt the object then return
//         //PickupBlockingLayers is a class variable layer mask
//         if (Physics.Raycast(ray, out var hit,
//             100f, PickupBlockingLayers) && hit.collider.gameObject != closestGOToCamera)
//         {
//             return;
//         }

//         if (closestGOToCamera.layer == LayerInfo.INTERACTABLE_OBJECT)
//         {
//             var tObject = closestGOToCamera.transform.parent.GetComponent<GrabbableObject>();
//             //only process interactions with 3d objects while in 3d
//             if (tObject != null && tObject.Is3D)
//             {
//                 //held object is a class variable that just holds a reference to the picked up object
//                 HeldObject = tObject;

//                //disable the visuals of the object, make it a child of the player or something
//             }
//         }
//         //closest object to camera is an Interactable object no pickup
//         //this is maybe where we would put the door instead of interacting with an object
//         else if (closestGOToCamera.layer == LayerInfo.INTERACTABLE_OBJECT_NO_PICKUP)
//         {
//             //interact with the button
//             closestGOToCamera.GetComponent<ReceivableParent>().Activate();
//         }
//     }

// }

// //PlayerBehaviour.cs
// public GameObject GetClosest3DObjectOnLayers(LayerMask layers) {
//         // Perform the overlap sphere and get the colliders within the specified radius.
//         Collider[] interactableColliders = Physics.OverlapSphere(player3D.transform.position, interactDisplayRadius, layers);

//         return GetClosest3DObjectInColliderArray(interactableColliders);


//     }
//     private GameObject GetClosest3DObjectInColliderArray(List<Collider> interactableColliders) {
//         return GetClosest3DObjectInColliderArray(interactableColliders.ToArray());
//     }
//     private GameObject GetClosest3DObjectInColliderArray(Collider[] colliders) {
//         if (colliders.Length == 0)
//             return null;
//         if (colliders.Length == 1)
//             return colliders[0].gameObject;
//         // Initialize variables to keep track of the closest object.
//         GameObject closestObject = null;
//         float smallestOrthogonalDistance = float.MaxValue;
//         Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

//         foreach (var collider in colliders) {
//             // Get the closest point on the camera ray to the object's position.
//             Vector3 closestPointOnRay = cameraRay.GetPoint(Vector3.Dot(collider.transform.position - cameraRay.origin, cameraRay.direction));
//             // Calculate the orthogonal distance from the object to the ray.
//             float orthogonalDistance = Vector3.Distance(collider.transform.position, closestPointOnRay);

//             // Check if this collider is closer to the camera's forward direction than the previous ones.
//             if (orthogonalDistance < smallestOrthogonalDistance) {
//                 Vector3 viewportPos = Camera.main.WorldToViewportPoint(collider.transform.position);

//                 // Check if the object is within the viewport bounds
//                 bool isOnScreen = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

//                 //allow picking up items in 90 degree cone in front of camera

//                 if (isOnScreen) {
//                     smallestOrthogonalDistance = orthogonalDistance;
//                     closestObject = collider.gameObject;
//                 }
//             }
//         }

//         // Return the closest interactable object or null if none was found.
//         return closestObject;
//     }



