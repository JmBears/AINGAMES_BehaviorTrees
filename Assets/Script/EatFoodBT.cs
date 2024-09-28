using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFoodBT : MonoBehaviour
{
    // Step 1: Declare references to other scripts
    [SerializeField] private Awareness awareness;
    [SerializeField] private Hunger hunger;
    [SerializeField] private Inventory inventory;

    // Step 2: Declare all the nodes. The root node is a Sequence
    private Sequence rootNode;

    private ActionNode an_checkHunger;
    private ActionNode an_checkInventoryFood;
    private ActionNode an_checkEnemy;
    private ActionNode an_eatFood;

    private Selector selector_checkInventory;

    private Inverter inverter_enemyAround;

    private void Awake()
    {
        // Step 3: Get Components of all action-related nodes
        awareness = GetComponent<Awareness>();
        hunger = GetComponent<Hunger>();
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        // Step 4: Make the behavior tree. Use the constructors to
        // build up instances each node. It is better to build up
        // the tree from the children nodes all the way up to the
        // parent nodes

        //  Action Nodes
        an_checkHunger = new ActionNode(CheckHunger);
        an_checkInventoryFood = new ActionNode(inventory.CheckInventoryForFood);    //  Don't understand this part
        an_checkEnemy = new ActionNode(CheckEnemy);
        an_eatFood = new ActionNode(EatFood);

        //  List of Action Nodes for the Selector
        List<Node> selectorNodes = new();
        selectorNodes.Add(an_checkInventoryFood);
        selector_checkInventory = new Selector(selectorNodes);

        //  Inverts the node inside the Inverter(parameter)
        inverter_enemyAround = new Inverter(an_checkEnemy);

        // Step 5: Store all nodes as children of the root node
        List<Node> rootNodes = new();
        rootNodes.Add(an_checkHunger);
        rootNodes.Add(selector_checkInventory);
        rootNodes.Add(inverter_enemyAround);
        rootNodes.Add(an_eatFood);
        rootNode = new Sequence(rootNodes);
    }

    private void Update()
    {
        // Step 6: Simply call the Evaluate function of the root node
        rootNode.Evaluate();
    }

    // You can declare all action node functions here
    // Example only:

    private NodeState CheckHunger()
    {
        return hunger.IsHungry() ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private NodeState CheckEnemy()
    {
        return awareness.IsEnemyAround() ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private NodeState EatFood()
    {
        Debug.Log("FOOD EATEN");
        hunger.CurrentHunger += 100f;
        return NodeState.SUCCESS;
    }
}
