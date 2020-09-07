using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Puzzle puzzle;
    [SerializeField] private int tileNumber;

    void Start()
    {
        puzzle = GameObject.Find("Puzzle").GetComponent<Puzzle>();    
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        puzzle.MoveTile(tileNumber);
    }
}
