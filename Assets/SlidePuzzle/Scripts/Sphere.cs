using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// sphere to activate game menu function, player only need to touch this.
/// </summary>
public class Sphere : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    /// <summary>
    /// when player touches the sphere
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _gameManager.shuffleBoxes();
        }
    }
}
