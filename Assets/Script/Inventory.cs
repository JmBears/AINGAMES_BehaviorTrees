using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<string> items = new List<string>();

    private void Awake()
    {
        ActionNode actionNode = new(CheckInventoryForFood);
    }
    public bool ContainsItem(string id)
    {
        return (items.Contains(id));
    }

    //  Wouldn't it be better if it's just CheckInventoryForFood and have an if(items.Contains("Meat")||if(items.Contains("Vegetable")||if(items.Contains("Fruit"))?
    public NodeState CheckInventoryForFood()
    {
        if (items.Contains("Meat")||(items.Contains("Vegetable")||(items.Contains("Fruit"))))
            return NodeState.SUCCESS;
        else
            return NodeState.FAILURE;
    }

    /*public NodeState CheckInventoryForVegetable()
    {
        if (items.Contains("Vegetable"))
            return NodeState.SUCCESS;
        else
            return NodeState.FAILURE;
    }

    public NodeState CheckInventoryForFruit()
    {
        if (items.Contains("Fruit"))
            return NodeState.SUCCESS;
        else
            return NodeState.FAILURE;
    }*/
}
