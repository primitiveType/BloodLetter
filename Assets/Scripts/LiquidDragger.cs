using System;
using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class LiquidDragger : MonoBehaviour
{
    private ActorRoot ActorRoot;
    [SerializeField] private float liquidAmount;

    [SerializeField] private UltimateDecal leftFootprintPrefab;
    [SerializeField] private UltimateDecal rightFootprintPrefab;

    private float leftRightOffset = .5f;

    private LiquidType LiquidType { get; set; }
    public void AddLiquid(float amount, LiquidType type) 
    {
        liquidAmount += amount;
        LiquidType = type;//always use the most recent type
    }

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        ActorRoot.ActorEvents.OnStepEvent += ActorEventsOnOnStepEvent;
    }


    private void OnDestroy()
    {
        ActorRoot.ActorEvents.OnStepEvent -= ActorEventsOnOnStepEvent;
    }

    private int StepIndex { get; set; }


    private void ActorEventsOnOnStepEvent(object sender, OnStepEventArgs args)
    {
        if (liquidAmount > 20)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit info, 3, LayerMask.GetMask("Default"),
                QueryTriggerInteraction.Ignore))
            {
                StepIndex++;
                var isLeftFoot = StepIndex % 2 == 0;

                var amountToUse = (liquidAmount / 2.5f);
                liquidAmount -= amountToUse;
                var rotation = transform.rotation;
                var footPrintRotation = rotation;
                if (args.NewPosition != null && args.LastPosition != null)
                {
                    footPrintRotation = GetFootPrintRotation(args, rotation, isLeftFoot);
                }

                var footprintPrefab = isLeftFoot ? leftFootprintPrefab : rightFootprintPrefab;
                var offset = isLeftFoot ? leftRightOffset : -leftRightOffset;
                var decal = Instantiate(footprintPrefab, info.point + ((footPrintRotation * Vector3.right) * offset),
                    footPrintRotation);
                
                var tValue = 1 - (10f / amountToUse);

                decal.alphaCutoff = Mathf.Lerp(1f, .01f, tValue);
                decal.GetComponent<LiquidTypeComponent>().LiquidType = LiquidType;
            }
        }
    }

    private Quaternion GetFootPrintRotation(OnStepEventArgs args, Quaternion transformRotation, bool isLeftFoot)
    {
        Quaternion footPrintRotation;
        var movementRotation =
            Quaternion.LookRotation((Vector3) ((Vector3) args.NewPosition - args.LastPosition));

        var transformForward = transformRotation * Vector3.forward;
        var movementForward = movementRotation * Vector3.forward;

        var angleBetweenHeadAndHips = Vector3.SignedAngle(transformForward, movementForward, Vector3.up);

        footPrintRotation = movementRotation * Quaternion.Euler(0, -angleBetweenHeadAndHips, 0);


        var unsignedAngle = Mathf.Abs(angleBetweenHeadAndHips);
        if (unsignedAngle < 100 && unsignedAngle > 15) //we are strafing
        {
            //while strafing, the foot on the "look" side splays out, and the other one moves straight forward
            if (angleBetweenHeadAndHips > 0)
            {
                //facing left (strafing right). Right foot faces forward, left one faces between forward and transform.forward
                if (!isLeftFoot)
                {
                    footPrintRotation = movementRotation * Quaternion.Euler(0, -angleBetweenHeadAndHips * .5f, 0);
                }
                else
                {
                    footPrintRotation = movementRotation;
                }
            }
            else
            {
                //facing right (strafing left)
                if (isLeftFoot)
                {
                    footPrintRotation = movementRotation * Quaternion.Euler(0, -angleBetweenHeadAndHips * .5f, 0);
                }
                else
                {
                    footPrintRotation = movementRotation;
                }
            }
        }

        return footPrintRotation;
    }
}