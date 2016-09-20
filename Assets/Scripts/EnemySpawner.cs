﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    public List<EnemyBehavior>Enemies;
    public Collider2D FlyZone;
    public Collider2D GroundZone;
        
    public float delay = 1f;
    private float timer;
    private bool spawn = true;
	// Use this for initialization
	void Start () {
        ResetTimer();
    }

    private void ResetTimer()
    {
        timer = Time.time + delay;
    }

    public void Enable()
    {
        spawn = true;
        ResetTimer();
    }

    public void Disable()
    {
        spawn = false;
    }

	// Update is called once per frame
	void Update () {
        if (timer < Time.time && spawn)
        {
            int i = Random.Range(0, Enemies.Count);
            EnemyBehavior enemy = Instantiate(Enemies[i]);
            EnemyBehavior b = enemy.GetComponent<EnemyBehavior>();

            Bounds spawnBounds = new Bounds();
            Vector2 iuc = Random.insideUnitCircle;
            switch (b.PositionType)
            {
                case EnemyPositionType.FLYING:
                    spawnBounds = FlyZone.bounds;
                    break;

                case EnemyPositionType.GROUND:
                    spawnBounds = GroundZone.bounds;
                    break;

                case EnemyPositionType.BOTH:
                    int r = Random.Range(0, 2);
                    if (r == 0)
                    {
                        spawnBounds = GroundZone.bounds;
                    }
                    else if(r == 1)
                    {
                        spawnBounds = FlyZone.bounds;
                    }
                    break;
            }

            enemy.transform.position = new Vector3(spawnBounds.center.x + iuc.x * spawnBounds.extents.x,
                spawnBounds.center.y + iuc.y * spawnBounds.extents.y, 0.21f);
            ResetTimer();
        }
    }
}