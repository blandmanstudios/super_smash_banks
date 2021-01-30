using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
 
    public SpriteRenderer bodyGraphic;
    public Faction faction;
    public static float lifetime = 0.1f;

    public Collider2D ourCollider;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }

//    void OnTriggerEnter2D(Collider2D other)
    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.LogFormat($"We ({name}) collided with a {other.name}");
        var maybeBattler = other.gameObject.GetComponentInParent<Battler>();
        if (maybeBattler != null) {
            // No friendly fire
            if (maybeBattler.faction != faction) {
                // Dead things don't collide
                if (maybeBattler.IsAlive) {
                    // No stunlock
                    if (!maybeBattler.IsStunned()) {
                        //TODO: Damage the battler
                        Debug.Log("Haha, take that! "+ Time.time);
                        maybeBattler.SetStunned();
                    }
                }
            }
        }
    }
}
