﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : MonoBehaviour
{
    public static float timeBeforeEnabling = 0.01f;
    public bool isCollectable;
    public float quantity;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("MakeCollectable", timeBeforeEnabling);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeCollectable() {
        isCollectable = true;
    }
}
