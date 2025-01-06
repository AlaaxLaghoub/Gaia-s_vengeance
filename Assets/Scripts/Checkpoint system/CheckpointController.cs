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

        // Update RespawnManager with the new checkpoint
        if (RespawnManager.instance != null)
        {
            RespawnManager.instance.SetCheckpoint(spawnPoint);
        }
        else
        {
            Debug.LogError("RespawnManager instance not found! Ensure it exists in the scene.");
        }
    }

    public void Respawn()
    {
        if (RespawnManager.instance != null)
        {
            RespawnManager.instance.RespawnPlayer(); // Call the no-argument RespawnPlayer method
        }
        else
        {
            Debug.LogError("RespawnManager instance not found! Ensure it exists in the scene.");
        }
    }
}
