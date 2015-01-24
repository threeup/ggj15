using System.Collections.Generic;
using UnityEngine;
//General
[System.Serializable]
public class BasicState 
{
    public object enumValue = null;
    public string enumName = "UNDEFINED";
    
    public int idx;
    public delegate void OnChangeDelegate();
    public delegate bool CanEnterDelegate();
    public delegate void OnUpdateDelegate(float deltaTime);
 
    public OnChangeDelegate OnEnter;
    public OnChangeDelegate OnExit;
    public CanEnterDelegate CanEnter;
    public OnUpdateDelegate OnUpdate;
 
    public BasicState(int i, object eVal, string eName)
    {
        idx = i;
        OnEnter = null;
        OnExit = null;
        OnUpdate = null;
        CanEnter = DefaultEnter;
        enumValue = eVal;
        enumName = eName;
    }
 
    public void Noop()
    {
        return;
    }
 
    public bool DefaultEnter()
    {
        return true;
    }
}
 
public class BasicMachine<T> where T : struct
{
 
    public delegate void OnChangeDelegate(int prev, int next);
    public OnChangeDelegate OnChange;
 
    public BasicState currentState;
    public BasicState previousState;
    public List<BasicState> stateList;
    public bool isInitialized = false;
    protected System.Type enumType;
    
    public virtual void Initialize(System.Type eType)
    {
        enumType = eType;
        System.Array eVals = System.Enum.GetValues(enumType);
        int count = eVals.Length;
        stateList = new List<BasicState>(count);
        
        for (int i=0; i < count; ++i)
        {
            object enumValue = eVals.GetValue(i);
            stateList.Add(new BasicState(i, enumValue, System.Enum.GetName(enumType, enumValue)));
        }
        currentState = stateList[0];
        isInitialized = true;
    }
 
    public BasicState this[int i]
    {
        get { return stateList[i]; }
        set {}
    }
 
    public int GetActiveState()
    {
        return currentState.idx;
    }
    public BasicState GetStateByType(T type)
    {
        return stateList[(int)(System.ValueType)type];
    }
 
    public bool IsState(T tValue)
    {
        return currentState.enumValue.Equals(tValue);
    }
 
    public bool? SetState(T type)
    {
        return SetState(GetStateByType(type));
    }
 
    public bool? SetState(BasicState nextState)
    {
        if (nextState == currentState)
        {
            return null;
        }
 
        if (nextState.CanEnter())
        {
            previousState = currentState; 
            currentState = nextState;
            
            if (previousState.OnExit != null) { previousState.OnExit(); }
            if (OnChange != null) { OnChange(previousState.idx, nextState.idx); }
            if (nextState.OnEnter != null) { nextState.OnEnter(); }
            return true;
        }
        return false;
    }
 
    public override string ToString()
    {
        return currentState.enumName;
    }
 
    public void DoUpdate(float deltaTime)
    {
        if (currentState.OnUpdate != null)
        {
            currentState.OnUpdate(deltaTime);
        }
    }
}