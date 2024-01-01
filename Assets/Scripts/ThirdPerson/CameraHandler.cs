using System;
using System.Collections;
using System.Collections.Generic;
using Pattern;
using ThirdPerson.Character;
using UnityEngine;


public class CameraHandler : MonoBehaviour
{

    [SerializeField] private InputHandler inputHandler;
    
   [SerializeField] private Transform targetTransform;
   [SerializeField] private Transform cameraTransform;
   [SerializeField] private Transform cameraPivotTransform;
   [SerializeField] private Transform myTransform;
   [SerializeField] private Vector3 cameraTransformPosition;
   [SerializeField] private LayerMask ignoreMask;



   [SerializeField] private float lookSpeed = 0.1f;
   [SerializeField] private float followSpeed = 0.1f;
   [SerializeField] private float pivotSpeed = 0.03f;

   private float targetPosition;
   private float defaultPosition;
   private float lookAngle;
   private float pivotAngle;
   private Vector3 cameraFollowVelocity = Vector3.zero;
   public float minimumPivot = -35;
   public float maximumPivot = 35;
   public float cameraSphereRadius = 0.2f;
   public float cameraCollisionOffset = 0.2f;
   public float minimumCollisionOffset = 0.2f;

   public Transform currentLockOnTarget;
   private List<CharacterManager> availableTarget = new List<CharacterManager>();
   public Transform nearestLockOnTarget;
   public Transform leftLockTarget;
   public Transform rightLockTarget;
   public float maximumLockOnDistance;
   public LayerMask IgnoreMask => ignoreMask;

   public static CameraHandler singleton;
   private  void Awake()
   {
       singleton = this;
       myTransform = transform;
       defaultPosition = cameraTransform.localPosition.z;
       targetTransform = FindObjectOfType<PlayerManager>().transform;
   }

   public void FollowTarget(float delta)
   {
       Vector3 targetPos = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
       myTransform.position = targetPos;
       HandleCameraCollisions(delta);
   }

   public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
   {
       if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
       {
           lookAngle += (mouseXInput * lookSpeed) / delta;
           pivotAngle -= (mouseYInput * pivotSpeed) / delta;
           pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

           Vector3 rotation = Vector3.zero;
           rotation.y = lookAngle;
           Quaternion targetRotation = Quaternion.Euler(rotation);
           myTransform.rotation = targetRotation;

           rotation = Vector3.zero;
           rotation.x = pivotAngle;
       
           targetRotation = Quaternion.Euler(rotation);
           cameraPivotTransform.localRotation = targetRotation;
       }
       else
       {
           float velocity = 0;

           Vector3 dir = currentLockOnTarget.position - transform.position;
           dir.Normalize();
           dir.y = 0;
           
           Quaternion targetRotation =  Quaternion.LookRotation(dir);
           transform.rotation = targetRotation;

           dir = currentLockOnTarget.position - cameraPivotTransform.position;
           dir.Normalize();

           
           targetRotation = Quaternion.LookRotation(dir);
           Vector3 eulerAngle = targetRotation.eulerAngles;
           eulerAngle.y = 0;
           cameraPivotTransform.localEulerAngles = eulerAngle;
       }
    
   }

   public void HandleLockOn()
   {
       float shortestDistance = Mathf.Infinity;
       float shortestDistanceOfLeftTarget = Mathf.Infinity;
       float shortestDistanceOfRightTarget = Mathf.Infinity;

       Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);
       for (int i = 0; i < colliders.Length; i++)
       {
           CharacterManager characterManager = colliders[i].GetComponent<CharacterManager>();
       

           if (characterManager != null)
           {
               Vector3 lockTargetDirection = characterManager.transform.position - targetTransform.position;
               float distanceFromTarget =
                   Vector3.Distance(targetTransform.position , characterManager.transform.position);
               float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

               if (characterManager.transform.root != targetTransform.transform.root && viewableAngle > -50 &&
                   viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
               {
                   availableTarget.Add(characterManager);
               }
           }
       }

       for (int k = 0; k < availableTarget.Count; k++)
       {
           float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget[k].transform.position);

           if (distanceFromTarget < shortestDistance)
           {
               shortestDistance = distanceFromTarget;
               nearestLockOnTarget = availableTarget[k].lockOnTransform;
           }

           if (inputHandler.lockOnFlag)
           {
               Vector3 relativeEnemyPosition =
                   currentLockOnTarget.InverseTransformPoint(availableTarget[k].transform.position);
               var distanceFromLeftTarget =
                   currentLockOnTarget.transform.position.x - availableTarget[k].transform.position.x;
               var distanceFromRightTarget =
                   currentLockOnTarget.transform.position.x + availableTarget[k].transform.position.x;

               if (relativeEnemyPosition.x > 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
               {
                   shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                   leftLockTarget = availableTarget[k].lockOnTransform;
               }

               if (relativeEnemyPosition.x < 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
               {
                   shortestDistanceOfRightTarget = distanceFromRightTarget;
                   rightLockTarget = availableTarget[k].lockOnTransform;
               }
           }
       }

   }
   
   
   public void ClearLockOnTargets()
   {
       availableTarget.Clear();
       nearestLockOnTarget = null;
       currentLockOnTarget = null;
   }

   private void HandleCameraCollisions(float delta)
   {
       targetPosition = defaultPosition;
       RaycastHit hit;
       Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
       direction.Normalize();

       if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreMask))
       {
           float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
           targetPosition = -(dis - cameraCollisionOffset);
       }

       if (MathF.Abs(targetPosition) < minimumCollisionOffset)
       {
           targetPosition = -minimumCollisionOffset;
       }

       cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
       cameraTransform.localPosition = cameraTransformPosition;
   }
}
