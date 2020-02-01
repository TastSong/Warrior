/***************************************************************
 * Class Name : WorldState
 * Function   : Represents the world state to the GOAP controller in the game
 * Created by : Marek R.
 *
 **************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PropType
{
    E_INVALID = -1,
    E_BOOL,
    E_INT,
    E_FLOAT,
    E_VECTOR,
    E_AGENT,
    E_EVENT,
    E_ORDER,
}

public enum E_PropKey
{
    E_INVALID = -1,
    E_IDLING,
    E_ORDER,
    E_AT_TARGET_POS,
    E_IN_DODGE,
    E_ALERTED,
    E_ATTACK_TARGET,
    E_LOOKING_AT_TARGET,
    E_IN_WEAPONS_RANGE,
    E_WEAPON_IN_HANDS,
    E_USE_WORLD_OBJECT,
    E_PLAY_ANIM,
    E_EVENT,
    E_IN_BLOCK,
    E_IN_COMBAT_RANGE,
    E_AHEAD_OF_ENEMY,
    E_BEHIND_ENEMY,
    MoveToRight,
    MoveToLeft,
    BossIsNear,
    E_TELEPORT,
    E_COUNT
}


public enum RETURN_TYPES
{
    INVALID = -1,
    FALSE_RETURN,
    TRUE_RETURN
}

public class Value
{

}


public class ValueVector : Value
{
    public UnityEngine.Vector3 Vector;

    public ValueVector(UnityEngine.Vector3 vector) { Vector = vector; }

    public override string ToString() { return Vector.ToString(); }
}

public class ValueAgent : Value
{
    public Agent Agent;

    public ValueAgent(Agent a) { Agent = a; }

    public override string ToString() { return Agent.name; }
}

public class ValueBool : Value
{
    public bool Bool;

    public ValueBool(bool b) { Bool = b; }

    public override string ToString() { return Bool.ToString(); }
}

public class ValueFloat : Value
{
    public float Float;

    public ValueFloat(float f) { Float = f; }

    public override string ToString() { return Float.ToString(); }
}

public class ValueInt : Value
{
    public int Int;

    public ValueInt(int i) { Int = i; }

    public override string ToString() { return Int.ToString(); }
}

public class ValueEvent : Value
{
    public E_EventTypes Event;

    public ValueEvent(E_EventTypes eventType) { Event = eventType; }

    public override string ToString() { return Event.ToString(); }
}

public class ValueOrder : Value
{
    public AgentOrder.E_OrderType Order;

    public ValueOrder(AgentOrder.E_OrderType order) { Order = order; }

    public override string ToString() { return Order.ToString(); }
}


[System.Serializable]
public class WorldStateProp
{
    public E_PropKey PropKey;// { get {return PropKey;} set {PropKey = value;} }
    public string PropName { get { return PropKey.ToString(); } }
    public System.Object PropValue;// { get { return PropState; } set {PropState = value;} }
    public E_PropType PropType;
    public float Time;

    public WorldStateProp(bool state) { PropValue = new ValueBool(state); PropType = E_PropType.E_BOOL; }
    public WorldStateProp(int state) { PropValue = new ValueInt(state); PropType = E_PropType.E_INT; }
    public WorldStateProp(float state) { PropValue = new ValueFloat(state); PropType = E_PropType.E_FLOAT; }
    public WorldStateProp(Agent state) { PropValue = new ValueAgent(state); PropType = E_PropType.E_AGENT; }
    public WorldStateProp(UnityEngine.Vector3 vector) { PropValue = new ValueVector(vector); PropType = E_PropType.E_VECTOR; }
    public WorldStateProp(E_EventTypes eventType) { PropValue = new ValueEvent(eventType); PropType = E_PropType.E_EVENT; }
    public WorldStateProp(AgentOrder.E_OrderType order) { PropValue = new ValueOrder(order); PropType = E_PropType.E_ORDER; }

    //public static implicit operator WorldStateProp(bool state) { return new WorldStateProp(state);}

    public bool GetBool() { ValueBool b = PropValue as ValueBool; return b != null ? b.Bool : false; }
    public int GetInt() { ValueInt v = PropValue as ValueInt; return v != null ? v.Int : 0; }
    public float GetFloat() { ValueFloat v = PropValue as ValueFloat; return v != null ? v.Float : 0.0f; }
    public UnityEngine.Vector3 GetVector() { ValueVector v = PropValue as ValueVector; return v != null ? v.Vector : Vector3.zero; }
    public Agent GetAgent() { ValueAgent v = PropValue as ValueAgent; return v != null ? v.Agent : null; }
    public E_EventTypes GetEvent() { ValueEvent v = PropValue as ValueEvent; return v != null ? v.Event : E_EventTypes.None; }
    public AgentOrder.E_OrderType GetOrder() { ValueOrder v = PropValue as ValueOrder; return v != null ? v.Order : AgentOrder.E_OrderType.E_NONE; }

    public override bool Equals(System.Object o)
    {
        WorldStateProp otherProp = o as WorldStateProp;
        if (otherProp != null)
        {
            if (this.PropType != otherProp.PropType)
                return false;// different typs of values

            switch (this.PropType)
            {
                case E_PropType.E_BOOL:
                    return (this.PropValue as ValueBool).Bool == (otherProp.PropValue as ValueBool).Bool;
                case E_PropType.E_INT:
                    return (this.PropValue as ValueInt).Int == (otherProp.PropValue as ValueInt).Int;
                case E_PropType.E_FLOAT:
                    return (this.PropValue as ValueFloat).Float == (otherProp.PropValue as ValueFloat).Float;
                case E_PropType.E_VECTOR:
                    return (this.PropValue as ValueVector).Vector == (otherProp.PropValue as ValueVector).Vector;
                case E_PropType.E_AGENT:
                    return (this.PropValue as ValueAgent).Agent == (otherProp.PropValue as ValueAgent).Agent;
                case E_PropType.E_EVENT:
                    return (this.PropValue as ValueEvent).Event == (otherProp.PropValue as ValueEvent).Event;
                case E_PropType.E_ORDER:
                    return (this.PropValue as ValueOrder).Order == (otherProp.PropValue as ValueOrder).Order;
                default:
                    return false;
            }
        }

        return false;
    }

    public override int GetHashCode()
    {
        return (this as object).GetHashCode();
    }

    static public bool operator ==(WorldStateProp prop, WorldStateProp other)
    {
        if ((prop as object) == null)
            return (other as object) == null;

        return prop.Equals(other as object);
    }

    static public bool operator !=(WorldStateProp prop, WorldStateProp other)
    {
        return !(prop == other);
    }

    public override string ToString()
    {
        return PropName + ": " + PropValue.ToString();
    }

}
[System.Serializable]
public class WorldState
{
    private WorldStateProp[] m_PropState = new WorldStateProp[(int)E_PropKey.E_COUNT];
    private BitArray m_PropBitSet = new BitArray((int)E_PropKey.E_COUNT);

    public WorldStateProp GetWSProperty(E_PropKey key) { return m_PropState[(int)key]; }
    public bool IsWSPropertySet(E_PropKey key) { return m_PropBitSet.Get((int)key); }

    public E_PropType GetWSPropertyType(E_PropType key) { return E_PropType.E_BOOL; } // only bool now



    public void SetWSProperty(E_PropKey key, bool value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, float value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, int value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, Agent value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, UnityEngine.Vector3 value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, E_EventTypes value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }

    public void SetWSProperty(E_PropKey key, AgentOrder.E_OrderType value)
    {
        int index = (int)key;
        if (m_PropState[index] != null)
            WorldStatePropFactory.Return(m_PropState[index]);

        m_PropState[index] = WorldStatePropFactory.Create(key, value);
        m_PropBitSet.Set(index, true); // set info that key is set
    }


    public void SetWSProperty(WorldStateProp other)
    {   if (other == null)
            return;

        switch (other.PropType)
        {
            case E_PropType.E_BOOL:
                SetWSProperty(other.PropKey, other.GetBool());
                break;
            case E_PropType.E_INT:
                SetWSProperty(other.PropKey, other.GetInt());
                break;
            case E_PropType.E_FLOAT:
                SetWSProperty(other.PropKey, other.GetFloat());
                break;
            case E_PropType.E_VECTOR:
                SetWSProperty(other.PropKey, other.GetVector());
                break;
            case E_PropType.E_AGENT:
                SetWSProperty(other.PropKey, other.GetAgent());
                break;
            case E_PropType.E_EVENT:
                SetWSProperty(other.PropKey, other.GetEvent());
                break;
            case E_PropType.E_ORDER:
                SetWSProperty(other.PropKey, other.GetEvent());
                break;
            default:
                Debug.LogError("error in SetWSProperty " + other.PropKey.ToString());
                break;
        }
    }

    public void ResetWSProperty(E_PropKey key)
    {
        //Debug.Log("Reset WS property " + key.ToString());
        int i = (int)key;
        if (m_PropState[i] != null)
        {
            WorldStatePropFactory.Return(m_PropState[i]);
            m_PropState[i] = null;
            m_PropBitSet.Set(i, false);
        }
    }

    public void Reset()
    {
        //Debug.Log("Worldstate reset");

        for (int i = 0; i < (int)E_PropKey.E_COUNT; i++)
        {
            if (m_PropState[i] != null)
            {
                WorldStatePropFactory.Return(m_PropState[i]);
                m_PropState[i] = null;
            }
        }

        m_PropBitSet.SetAll(false);
    }

    //拷贝其他世界状态到自身
    public void CopyWorldState(WorldState otherState)
    {
        Reset();
        for (E_PropKey i = 0; i < E_PropKey.E_COUNT; i++)
        {
            if (otherState.GetPropBitSet().Get((int)i) == true)
                SetWSProperty(otherState.GetWSProperty(i));
        }
    }

    /**
    * Returns the number of world state properties that are different from the world state input
    * @param the other world state to check against the current world state
    */
    //获取两个世界状态不同的属性数量
    public int GetNumWorldStateDifferences(WorldState otherState)
    {
        int count = 0;
        for (int i = 0; i < (int)E_PropKey.E_COUNT; i++)
        {
            if (otherState.GetPropBitSet().Get(i) && GetPropBitSet().Get(i))
            {
                if (!(GetWSProperty((E_PropKey)i) == otherState.GetWSProperty((E_PropKey)i)))
                    count++;

            }
            else if (otherState.GetPropBitSet().Get(i) || GetPropBitSet().Get(i))
                count++;
        }
        return count;
    }

    /**
    * Returns the number of world state properties that are different from the world state input
    * @param the other world state to check against the current world state
    */
    //获取未满足的属性数量
    public int GetNumUnsatisfiedWorldStateProps(WorldState otherState)
    {
        int count = 0;
        for (E_PropKey i = 0; i < E_PropKey.E_COUNT; i++)
        {
						//自身该属性没有设置跳出
            if (IsWSPropertySet(i) == false)
                continue;
						//其他角色该属性没有设置，自身设置了，count++
            if (!otherState.IsWSPropertySet(i))
                count++;

						//自身和其他角色该属性不相等，count++
            if (!(GetWSProperty(i) == otherState.GetWSProperty(i))) //test 
                count++;
        }
        return count;
    }


    public BitArray GetPropBitSet() { return m_PropBitSet; }

    public override string ToString()
    {
        string s = "World state : ";

        for (E_PropKey i = E_PropKey.E_ORDER; i < E_PropKey.E_COUNT; i++)
        {
            if (IsWSPropertySet(i))
                s += " " + GetWSProperty(i).ToString();
        }

        return s;
    }
}
