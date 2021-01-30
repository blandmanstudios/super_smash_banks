using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public SpriteRenderer bodyGraphic;
    public Faction faction;
    public bool isAI;
    // false if this is an AI
    // false if this is a dead player
    public bool isBattlerActive;
    public Collider2D ourCollider;
    
    public static float cooldownMelee = 0.5f;
    private float lastUsedMelee;
    
    public static float stunDuration = 1f;
    private float timeLastHit = float.MinValue;

    public Runner_GameScene runner;

    // Stub
    public bool IsAlive => true;

    // TODO: Make more sophisticated
    public static float timeBetweenAIAttacks = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattlerActive) {
            if (IsStunned()) {
                //Debug.Log("Can't do anything b/c stunned");
            } else {
                if (!isAI) {
                    DoPlayerMovement();
                    DoPlayerAttacking();
                }
            }
        }
    }

    // Assumed: Battler is active and not stunned
    public void DoPlayerMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(!Input.GetKey("left") && !Input.GetKey("a") && !Input.GetKey("right") && !Input.GetKey("d")) {
            horizontalInput = 0;
        }

        float verticalInput = Input.GetAxis("Vertical");
        if(!Input.GetKey("up") && !Input.GetKey("w") && !Input.GetKey("down") && !Input.GetKey("s")) {
            verticalInput = 0;
        }

        var inputsSquared = horizontalInput * horizontalInput + verticalInput * verticalInput;
        var multiplier = (inputsSquared > 1) ? Mathf.Sqrt(1.0f/inputsSquared) : 1;
        var normalizedMoveSpeed = Runner_GameScene.playerMoveSpeed * multiplier;

        Vector2 position = transform.position;
        position.x = Mathf.Clamp(
            position.x + normalizedMoveSpeed * horizontalInput * Time.deltaTime,
            Runner_GameScene.playAreaLowerLeft.x + Runner_GameScene.BattlerHalfWidth,
            Runner_GameScene.playAreaUpperRight.x - Runner_GameScene.BattlerHalfWidth
        );
        position.y = Mathf.Clamp(
            position.y + normalizedMoveSpeed * verticalInput * Time.deltaTime,
            Runner_GameScene.playAreaLowerLeft.y + Runner_GameScene.BattlerHalfWidth,
            Runner_GameScene.playAreaUpperRight.y - Runner_GameScene.BattlerHalfWidth
        );
        transform.position = position;
    }

    // Assumed: Battler is active and not stunned
    public void DoPlayerAttacking() {
        if (Input.GetKeyDown(KeyCode.M)) {
            Attack();
        }
    }


    // Cannot currently make assumptions about being active or stunned
    // because AI is currently calling this blindly
    public void DoAIAttacking() {
        if (isBattlerActive) {
            if (!IsStunned()) {
                Attack();
            }
        }
    }

    public void Attack() {
        if ((Time.time - lastUsedMelee) > cooldownMelee) {
            lastUsedMelee = Time.time;
            runner.InstantiateMelee(transform.position, faction);
        }
    }

    // Cannot call in Start() because we first need to know whether we're AI
    public void InitAndStartAIAttacking() {
        var delayBeforeFirstAttack = Random.Range(0.1f, timeBetweenAIAttacks);
        InvokeRepeating("DoAIAttacking", delayBeforeFirstAttack, timeBetweenAIAttacks);
    }

    public bool IsStunned() {
        var timeSinceLastStunBegan = Time.time - timeLastHit;
        return (timeSinceLastStunBegan < stunDuration);
    }

    public void SetStunned() {
        timeLastHit = Time.time;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.LogFormat($"We ({name}) collided with a {other.name}");
        var maybeMelee = other.gameObject.GetComponentInParent<Melee>();
        if (maybeMelee != null) {
            // No friendly fire
            if (maybeMelee.faction != faction) {
                // Dead things don't collide
                if (IsAlive) {
                    // No stunlock
                    if (!IsStunned()) {
                        Debug.Log("Ouch! "+ Time.time);
                        //TODO: Take damage
                        SetStunned();
                    }
                }
            }
        }
    }
}
