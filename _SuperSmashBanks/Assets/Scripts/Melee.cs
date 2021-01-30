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
}
