﻿using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Shapes> requiredShapes = new List<Shapes>();
    [SerializeField] private ShapeDirectory shapeDirectory = null;
    
    private Stack<Shapes> remainingShapesRequired = null;
    private Stack<Shapes> currentShapes = new Stack<Shapes>();

    private void Awake()
    {
        remainingShapesRequired = new Stack<Shapes>(requiredShapes);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (remainingShapesRequired.Count <= 0)
        {
            return;
        }
        
        if (other.CompareTag("Shape"))
        {
            Shapes shapeType = other.GetComponent<Shape>().ShapeType;
            if (shapeType == remainingShapesRequired.Peek())
            {
                currentShapes.Push(remainingShapesRequired.Pop());
                Destroy(other.gameObject);
                Debug.Log("fits!");
            }
            else
            {
                Debug.Log("no fits!");
            }
        }
    }

    #region Interactions

    public Transform InteractionHandler { get; set; }
    public void OnInteractionBegin(Interact interactor)
    {
        
        if (currentShapes.Count <= 0)
        {
            return;
        }
        Shapes toSpawn = currentShapes.Pop();
        remainingShapesRequired.Push(toSpawn);
        Instantiate(shapeDirectory.GetShapePrefab(toSpawn),
            this.transform.position,
            Quaternion.identity);
    }

    public void OnInteractionTick(Interact interactor)
    {
        interactor.EndInteraction();
    }

    public void OnInteractionEnd(Interact interactor)
    {
        Debug.Log("Interaction with pedestal done!");
    }

    #endregion

}