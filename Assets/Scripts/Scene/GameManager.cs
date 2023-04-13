using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Scene references")]
    public Transform _player;
    public Transform _directionModifier;

    [Header("Values")]
    public int _maxCrowd = 4;
    public float closeDist;

    [Header("Misc")]
    public bool inTesting;

    public static Transform player;
    public static Transform directionModifier;
    public static int maxCrowd;
    public static int nearPlayer;

    private void Awake()
    {
        player = _player;
        maxCrowd = _maxCrowd;
        directionModifier = _directionModifier;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTesting)
        {
            if (Input.GetKeyDown(KeyCode.T)) Debug.Log(nearPlayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_player.position, closeDist);
        }
    }
}
