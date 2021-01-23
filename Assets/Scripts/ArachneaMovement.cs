using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArachneaMovement : EnemyMovement
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int AnimatorGoingUp = Animator.StringToHash("GoingUp");
    private static readonly int AnimatorGoingDown = Animator.StringToHash("GoingDown");
    private static readonly int AnimatorGrounded = Animator.StringToHash("Grounded");

    [SerializeField] private LineRenderer m_Line;
    [SerializeField] private BoxCollider m_TeleportArea;

    private LineRenderer Line => m_Line;
    public override bool ShouldStop => base.ShouldStop || !Grounded || GoingUp || GoingDown;
    private bool GoingUp { get; set; }
    private bool GoingDown { get; set; }

    private bool Grounded { get; set; } = true;

    private float delayBetweenJumps = 5f;
    private float ceilingTime = 2f;
    private Vector3 LineStartPosition { get; set; }

    private float timer { get; set; }

    protected override void Start()
    {
        base.Start();
        LineStartPosition = Line.GetPosition(0);
        ActorRoot.ActorEvents.OnAggroEvent += OnAggro;
        
    }

    private void OnAggro(object sender, OnAggroEventArgs args)
    {
        StartCoroutine(UpDownLoop());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ActorRoot.ActorEvents.OnAggroEvent -= OnAggro;
    }


    private IEnumerator UpDownLoop()
    {
        while (true)
        {
            if (!Health.IsAlive)
            {
                yield break;
            }

            timer += Time.deltaTime;
            if (Grounded && timer > delayBetweenJumps)
            {
                yield return GoUp();
                Teleport();

            }


            if (!Grounded && timer > ceilingTime)
            {
                yield return GoDown();
            }

            yield return null;
        }
    }

    private void Teleport()
    {
        var bounds = m_TeleportArea.bounds;
        float posX = Random.Range(bounds.min.x, bounds.max.x);
        float posY = Random.Range(bounds.min.y, bounds.max.y);
        float posZ = Random.Range(bounds.min.z, bounds.max.z);
        Agent.Warp(new Vector3(posX, posY, posZ));
    }

    private IEnumerator GoDown()
    {
        timer = 0;
        float currentHeight = transformToMove.localPosition.y;
        float time = currentHeight / AscendSpeed;
        float animationDuration = 1f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            transformToMove.localPosition = new Vector3(0, currentHeight - (timer * AscendSpeed), 0);
            if (!GoingDown && timer > time - animationDuration)
            {
                GoingDown = true;
                StartCoroutine(RetractWeb());
            }

            yield return null;
        }

        GoingDown = false;
        Grounded = true;

        transformToMove.localPosition = new Vector3(0, 0, 0);
        timer = 0;
    }

    private float CeilingHeight { get; } = 20;
    private float AscendSpeed { get; } = 10;
    [SerializeField] private Transform transformToMove;

    private IEnumerator GoUp()
    {
        timer = 0;
        float time = CeilingHeight / AscendSpeed;
        GoingUp = true;
        yield return ExtendWeb();
        GoingUp = false;
        Grounded = false;

        while (timer < time)
        {
            if (!Health.IsAlive)
            {
                yield return GoDown();
                yield break;
            }

            timer += Time.deltaTime;
            transformToMove.localPosition = new Vector3(0, timer * AscendSpeed, 0);
            yield return null;
        }

        transformToMove.localPosition = new Vector3(0, CeilingHeight, 0);
        timer = 0;
    }

    private IEnumerator ExtendWeb()
    {
        float delayForAnimationHack = .75f;
        float t = 0;
        Line.SetPosition(1, LineStartPosition);

        while (t < 1f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        t = 0;
        while (t < delayForAnimationHack)
        {
            t += Time.deltaTime;
            Line.SetPosition(1, LineStartPosition + (Vector3.up * (t / delayForAnimationHack) * CeilingHeight));
            yield return null;
        }

        Line.SetPosition(1, Vector3.up * CeilingHeight);
    }

    private IEnumerator RetractWeb()
    {
        float delayForAnimationHack = 1f;
        float startPosition = Line.GetPosition(1).y;
        float t = 0;
        float factor = 0;
        while (t < delayForAnimationHack)
        {
            t += Time.deltaTime;
            factor = 1 - (t / delayForAnimationHack);
            Line.SetPosition(1, Vector3.up * EasingFunction.Spring(LineStartPosition.y, startPosition, factor));
            yield return null;
        }

        Line.SetPosition(1, Vector3.up * CeilingHeight);
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimationStates();
    }

    private void UpdateAnimationStates()
    {
        _animator.SetBool(AnimatorGoingDown, GoingDown);
        _animator.SetBool(AnimatorGoingUp, GoingUp);
        _animator.SetBool(AnimatorGrounded, Grounded);
        Line.enabled = GoingDown || GoingUp || !Grounded;
    }
}