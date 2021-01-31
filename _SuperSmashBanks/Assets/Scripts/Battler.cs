using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public static float initialNetWorth = 1000000f;
    public static float damageOnHit     =  100000f;

    private float shares;
    public float Shares => shares;
    public float netWorth => Runner_GameScene.SharesToDollars(shares);

    public GameObject graphicsObject;
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

    [SerializeField] Rigidbody2D rb;

    bool aiVelocityStoredForRecovery;
    Vector2 lastKnownAIVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAI) {
            // Cheat - Bankrupt the player. For testing purposes only
            if (Input.GetKeyDown(KeyCode.B)) {
                shares = 0;
                Die();
            }
        }

        // Movement and attacking
        if (isBattlerActive) {
            if (IsStunned()) {
                //Debug.Log("Can't do anything b/c stunned");
                if (!aiVelocityStoredForRecovery) {
                    // Do things on your first frame of being stunned
                    lastKnownAIVelocity = rb.velocity;
                    aiVelocityStoredForRecovery = true;
                    rb.velocity = Vector2.zero;
                }
            } else {
                if (isAI) {
                    if (aiVelocityStoredForRecovery) {
                        // Do things on your first frame of being unstunned
                        rb.velocity = lastKnownAIVelocity;
                        aiVelocityStoredForRecovery = false;
                    }
                    // Actually right now all this does is bounce off walls.
                    DoAIMovement();
                    // Facing should come after movement. You need to know which way you're moving before you can face in that direction.
                    DoAIFacing();
                } else {
                    if (Input.GetKeyDown(KeyCode.G)) {
                        GetOut();
                    } else {
                        DoPlayerMovementAndFacing();
                        DoPlayerAttacking();
                    }
                }
            }
        }

        // Reporting
        if (!isAI) {
            runner.uiController.UpdatePlayerStatsDisplay(this);
        }

        Animate();
    }

    void Animate() {
        if (isBattlerActive && !IsStunned()) {
            if (!isAI) {
                bodyGraphic.sprite = CharSpriteMgr.blackSprites[CharSpriteMgr.animState];
            } else if (faction == Faction.Shorts) {
                bodyGraphic.sprite = CharSpriteMgr.blueSprites[CharSpriteMgr.animState];
            } else {
                bodyGraphic.sprite = CharSpriteMgr.redSprites[CharSpriteMgr.animState];
            }
        }
    }

    // Assumed: Battler is active and not stunned
    public void DoPlayerMovementAndFacing() {
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

        // Player Facing
        var playerVelocity = new Vector2(horizontalInput, verticalInput);
        if (playerVelocity.sqrMagnitude > 0.0001) {
            var angle = Vector2.SignedAngle(Vector2.right, playerVelocity);
            graphicsObject.transform.eulerAngles = new Vector3(0,0,angle - 90);
        }
    }

    // Assumed: Battler is active and not stunned
    public void DoAIMovement() {
        if (transform.position.y >= Runner_GameScene.playAreaUpperRight.y - Runner_GameScene.BattlerHalfWidth) {
            rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y));
        } else if (transform.position.y <= Runner_GameScene.playAreaLowerLeft.y + Runner_GameScene.BattlerHalfWidth) {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
        }

        if (transform.position.x <= Runner_GameScene.playAreaLowerLeft.x + Runner_GameScene.BattlerHalfWidth) {
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
        } else if (transform.position.x >= Runner_GameScene.playAreaUpperRight.x - Runner_GameScene.BattlerHalfWidth) {
            rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
        }
    }

    public void DoAIFacing() {
        var angle = Vector2.SignedAngle(Vector2.right, rb.velocity);
        graphicsObject.transform.eulerAngles = new Vector3(0,0,angle - 90);
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

    public void InitStartingValues() {
        shares = Runner_GameScene.DollarsToShares(initialNetWorth);
    }

    public void InitAndStartAIMovement() {
        var randomMotionAngle = Random.Range(-180.0f, 180.0f);

        float angleInRads = randomMotionAngle * Mathf.Deg2Rad;
        Vector2 unitVector = new Vector2(Mathf.Cos(angleInRads), Mathf.Sin(angleInRads));
        Vector2 velocityVector = unitVector * Runner_GameScene.aiMoveSpeed;
        rb.velocity = velocityVector;
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
                        //Debug.Log("Ouch! "+ Time.time);
                        if (isAI) {
                            runner.soundMgr.PickAiHurtSound().Play();
                        } else {
                            runner.soundMgr.soundPlayerHurt.Play();
                        }
                        SetStunned();
                        var newStock = runner.InstantiateStock(transform.position);
                        var idealSharesLost = Runner_GameScene.DollarsToShares(damageOnHit);
                        var actualSharesLost = Mathf.Min(idealSharesLost, shares);
                        newStock.quantity = actualSharesLost;
                        if (idealSharesLost > actualSharesLost) {
                            shares = 0;
                        } else {
                            shares -= actualSharesLost;
                        }
                        if (shares <= 0) {
                            Die();
                        }
                    }
                }
            }
        }

        var maybeStock = other.gameObject.GetComponentInParent<Stock>();
        if (maybeStock != null) {
            // Dead things don't collide
            if (IsAlive) {
                // Stunned things don't pick up Stock
                if (!IsStunned()) {
                    if (maybeStock.isCollectable) {
                        if (isAI) {
                            runner.soundMgr.soundAiAcquiresStock.Play();
                        } else {
                            runner.soundMgr.soundPlayerAcquiresStock.Play();
                        }
                        shares += maybeStock.quantity;
                        Destroy(maybeStock.gameObject);
                    }
                }
            }
        }
    }

    public void GetOut() {
        if (!isAI) {
            runner.uiController.UpdatePlayerStatsDisplay(this);
            runner.uiController.ShowGotOutPanel(this.netWorth);
            switch (faction) {
                case Faction.Shorts:
                    runner.shorts.Remove(this);
                    break;
                case Faction.Longs:
                    runner.longs.Remove(this);
                    break;
                default:
                    throw new System.Exception($"Unknown faction {faction}");
            }
            Destroy(gameObject);
        }
    }

    public void Die() {
        if (!isAI) {
            runner.uiController.UpdatePlayerStatsDisplay(this);
            runner.uiController.ShowYouLosePanel(true);
            runner.soundMgr.soundBadGameOver.Play();
        }
        switch (faction) {
            case Faction.Shorts:
                runner.shorts.Remove(this);
                break;
            case Faction.Longs:
                runner.longs.Remove(this);
                break;
            default:
                throw new System.Exception($"Unknown faction {faction}");
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        CancelInvoke();
    }
}
