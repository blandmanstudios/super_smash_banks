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
    public bool isPlayerMovementAllowed;
    public float cooldownMelee = 0.5f;

    float lastUsedMelee;

    public Runner_GameScene runner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoPlayerMovement();
        DoPlayerAttacking();
    }


    public void DoPlayerMovement() {
        if (isPlayerMovementAllowed) {
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
    }

    public void DoPlayerAttacking() {
        if (isPlayerMovementAllowed) {
            if (Input.GetKeyDown(KeyCode.M)) {
                if (Time.time - lastUsedMelee > cooldownMelee) {
                    lastUsedMelee = Time.time;
                    runner.InstantiateMelee(transform.position, faction);
                }
            }
        }
    }
}
