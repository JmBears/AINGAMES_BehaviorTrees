using UnityEngine;
using System;
using System.Data.Common;
using Panda;
public class Hunger : MonoBehaviour
{
    [SerializeField]
    private float maxHunger = 100f;
    [SerializeField]
    private float hungerDecreaseRate = 1.5f;

    private float _hunger;

  
    // For general purposes you can use System.Action
    //Limitations: only void return type, up to 4 parameters only
    //System.Action is a variable that takes in a VOID function, NO paremeter
    public event System.Action OnHungerDepleted;
    // This is an event that takes in a VOID function that has a float parameter
    public event System.Action<float> OnHungerUpdated;

    // Define the function signature, this is the definition part
    // Use a delegate for very specific function calls
    public delegate bool CheckHungerDelegate(float currentValue, float maxValue, float decreaseRate);
    private CheckHungerDelegate CheckHungerFunction;
    public event CheckHungerDelegate OnCheckHunger;

    // Both can be added as an "event" if you want to add multiple functions and call them at the same time
    // += and -= only works with Actions/Delegates that are set as event
    // Without adding "event" it can only store one function

    public float CurrentHunger
    {
        get
        {
            //Debug.Log(_hunger);
            if (_hunger <= 0)
            {
                // Call the function assigned to the Action
                // Always add ?. for safety purposes
                OnHungerDepleted?.Invoke();
                // Same as
                //if(OnHungerDepleted != null)
                //{
                //    OnHungerDepleted.Invoke();
                //}
            }
            return _hunger;
        }
        set
        {
            // Clamp the value so it doesnt go lower than minimum (0)
            // or higher than maximum (maxHunger)
            _hunger = Mathf.Clamp(value, 0, maxHunger);
            // Update the hunger value
            OnHungerUpdated?.Invoke(_hunger);
            CheckHungerFunction?.Invoke(CurrentHunger, maxHunger, hungerDecreaseRate);
        }
    }

    private void Start()
    {
        // set the value to max
        CurrentHunger = maxHunger;
    }

    public void Update()
    {
        DecreaseHungerOverTime();
    }

    private void DecreaseHungerOverTime()
    {
        CurrentHunger -= Time.deltaTime * hungerDecreaseRate;
    }

    public void SetHunger(float value)
    {
        CurrentHunger = value;
    }

    public void SetCheckHunger(CheckHungerDelegate function)
    {
        CheckHungerFunction = function;
    }

    public void HungerSample(System.Action OnHungerFull, System.Action OnHungerDepleted)
    {
        if(CurrentHunger == maxHunger)
        {
            OnHungerFull();
        }
        if(CurrentHunger == 0)
        {
            OnHungerDepleted();
        }
    }

    public bool IsHungry()
    {
        return CurrentHunger <= maxHunger/2;
    }

    public void Eat(int value)
    {
        CurrentHunger += value;
    }

    [Task]
    public void CheckHunger()
    {
        if (IsHungry())
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    /*
    public void SetHungerDepletedCallback(System.Action callback)
    {
        OnHungerDepleted = callback;
    }*/
}
