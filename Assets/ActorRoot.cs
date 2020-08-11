using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ActorRoot : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public IActorEvents ActorEvents { get; private set; }

    public AnimationMaterialHelper AnimationMaterialHelper { get; private set; }
    void Awake()
    {
        Animator= GetComponentInChildren<Animator>();
        ActorEvents = GetComponentInChildren<IActorEvents>();
        AnimationMaterialHelper = GetComponentInChildren<AnimationMaterialHelper>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}