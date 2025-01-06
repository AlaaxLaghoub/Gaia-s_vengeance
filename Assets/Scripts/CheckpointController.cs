using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;
    private Checkpoint[] checkpoints;
    public Vector3 spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void DeactivateCheckpoint()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.resetChecpoint();
        }
    }

    public void setSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void Respawn()
    {
        RespawnManager.instance.RespawnPlayer(spawnPoint);
    }
}
