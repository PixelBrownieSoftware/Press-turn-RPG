using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class S_Stats
{
    public int strength;
    public int vitality;
    public int agility;
    public int dexterity;
    public int magicPow;
    public int luck;

    public S_Stats()
    {
    }
    public S_Stats(S_Stats stats)
    {
        strength = stats.strength;
        vitality = stats.vitality;
        agility = stats.agility;
        dexterity = stats.dexterity;
        magicPow = stats.magicPow;
        luck = stats.luck;
    }
    public S_Stats(int strength, int vitality, int agility, int magicPow, int luck, int dexterity)
    {
        this.strength = strength;
        this.vitality = vitality;
        this.agility = agility;
        this.dexterity = dexterity;
        this.magicPow = magicPow;
        this.luck = luck;
    }
    public static S_Stats operator +(S_Stats b, S_Stats c)
    {
        S_Stats st = new S_Stats();
        st.strength = b.strength + c.strength;
        st.luck = b.luck + c.luck;
        st.magicPow = b.magicPow + c.magicPow;
        st.vitality = b.vitality + c.vitality;
        st.agility = b.agility + c.agility;
        st.dexterity = b.dexterity + c.dexterity;
        return st;
    }
    public static S_Stats operator +(S_Stats b, int c)
    {
        S_Stats st = new S_Stats();
        st.strength = b.strength + c;
        st.luck = b.luck + c;
        st.magicPow = b.magicPow + c;
        st.vitality = b.vitality + c;
        st.agility = b.agility + c;
        st.dexterity = b.dexterity + c;
        return st;
    }
    public static bool operator >(S_Stats lhs, S_Stats rhs)
    {
        bool status = false;
        if (lhs.strength > rhs.strength &&
            lhs.agility > rhs.agility &&
            lhs.vitality > rhs.vitality &&
            lhs.magicPow > rhs.magicPow &&
            lhs.dexterity > rhs.dexterity &&
            lhs.luck > rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator <(S_Stats lhs, S_Stats rhs)
    {
        bool status = false;
        if (lhs.strength < rhs.strength &&
            lhs.agility < rhs.agility &&
            lhs.vitality < rhs.vitality &&
            lhs.magicPow < rhs.magicPow &&
            lhs.dexterity < rhs.dexterity &&
            lhs.luck < rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator >=(S_Stats lhs, S_Stats rhs)
    {
        bool status = false;
        if (lhs.strength >= rhs.strength &&
            lhs.agility >= rhs.agility &&
            lhs.vitality >= rhs.vitality &&
            lhs.magicPow >= rhs.magicPow &&
            lhs.dexterity >= rhs.dexterity &&
            lhs.luck >= rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator <=(S_Stats lhs, S_Stats rhs)
    {
        bool status = false;
        if (lhs.strength <= rhs.strength &&
            lhs.agility <= rhs.agility &&
            lhs.vitality <= rhs.vitality &&
            lhs.magicPow <= rhs.magicPow &&
            lhs.dexterity <= rhs.dexterity &&
            lhs.luck <= rhs.luck)
        {
            status = true;
        }
        return status;
    }

}
[System.Serializable]
public class S_Stats_Float
{
    public float strength;
    public float vitality;
    public float agility;
    public float magicPow;
    public float dexterity;
    public float luck;
    public static S_Stats_Float operator +(S_Stats_Float b, S_Stats_Float c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength + c.strength;
        st.luck = b.luck + c.luck;
        st.magicPow = b.magicPow + c.magicPow;
        st.dexterity = b.dexterity + c.dexterity;
        st.vitality = b.vitality + c.vitality;
        st.agility = b.agility + c.agility;
        return st;
    }
    public float GetFloats(S_Stats c)
    {
        float st = 
            (strength * c.strength) +
            (luck * c.luck) + 
            (magicPow * c.magicPow) +
            (vitality * c.vitality) +
            (dexterity * c.dexterity) +
            (agility * c.agility);
        return st;
    }
    public static S_Stats_Float operator *(S_Stats_Float b, S_Stats c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength * c.strength;
        st.luck = b.luck * c.luck;
        st.magicPow = b.magicPow * c.magicPow;
        st.dexterity = b.dexterity * c.dexterity;
        st.vitality = b.vitality * c.vitality;
        st.agility = b.agility * c.agility;
        return st;
    }
    public static S_Stats_Float operator *(S_Stats b, S_Stats_Float c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength * c.strength;
        st.luck = b.luck * c.luck;
        st.magicPow = b.magicPow * c.magicPow;
        st.dexterity = b.dexterity * c.dexterity;
        st.vitality = b.vitality * c.vitality;
        st.agility = b.agility * c.agility;
        return st;
    }
    public static S_Stats_Float operator +(S_Stats b, S_Stats_Float c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength + c.strength;
        st.luck = b.luck + c.luck;
        st.magicPow = b.magicPow + c.magicPow;
        st.vitality = b.vitality + c.vitality;
        st.dexterity = b.dexterity + c.dexterity;
        st.agility = b.agility + c.agility;
        return st;
    }
    public static S_Stats_Float operator +(S_Stats_Float b, S_Stats c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength + c.strength;
        st.luck = b.luck + c.luck;
        st.magicPow = b.magicPow + c.magicPow;
        st.vitality = b.vitality + c.vitality;
        st.dexterity = b.dexterity + c.dexterity;
        st.agility = b.agility + c.agility;
        return st;
    }
    public static S_Stats_Float operator +(S_Stats_Float b, float c)
    {
        S_Stats_Float st = new S_Stats_Float();
        st.strength = b.strength + c;
        st.luck = b.luck + c;
        st.magicPow = b.magicPow + c;
        st.dexterity = b.dexterity + c;
        st.vitality = b.vitality + c;
        st.agility = b.agility + c;
        return st;
    }
    public static bool operator >(S_Stats_Float lhs, S_Stats_Float rhs)
    {
        bool status = false;
        if (lhs.strength > rhs.strength &&
            lhs.agility > rhs.agility &&
            lhs.vitality > rhs.vitality &&
            lhs.magicPow > rhs.magicPow &&
            lhs.dexterity > rhs.dexterity &&
            lhs.luck > rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator <(S_Stats_Float lhs, S_Stats_Float rhs)
    {
        bool status = false;
        if (lhs.strength < rhs.strength &&
            lhs.agility < rhs.agility &&
            lhs.vitality < rhs.vitality &&
            lhs.magicPow < rhs.magicPow &&
            lhs.dexterity < rhs.dexterity &&
            lhs.luck < rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator >=(S_Stats_Float lhs, S_Stats_Float rhs)
    {
        bool status = false;
        if (lhs.strength >= rhs.strength &&
            lhs.agility >= rhs.agility &&
            lhs.vitality >= rhs.vitality &&
            lhs.magicPow >= rhs.magicPow &&
            lhs.dexterity >= rhs.dexterity &&
            lhs.luck >= rhs.luck)
        {
            status = true;
        }
        return status;
    }
    public static bool operator <=(S_Stats_Float lhs, S_Stats_Float rhs)
    {
        bool status = false;
        if (lhs.strength <= rhs.strength &&
            lhs.agility <= rhs.agility &&
            lhs.vitality <= rhs.vitality &&
            lhs.magicPow <= rhs.magicPow &&
            lhs.dexterity <= rhs.dexterity &&
            lhs.luck <= rhs.luck)
        {
            status = true;
        }
        return status;
    }

}
[System.Serializable]
public class S_Stats_Util
{
    public int health;
    public int maxHealth;
    public int stamina;
    public int maxStamina = 10; 

    public S_Stats_Util(int health, int maxHealth, int stamina, int maxStamina) { 
        this.health = health;
        this.stamina = stamina;
        this.maxHealth = maxHealth;
        this.maxStamina = maxStamina;
    }
    public S_Stats_Util(S_Stats_Util stats)
    {
        this.maxHealth = this.health = stats.maxHealth;
        this.maxStamina = this.stamina = stats.maxStamina;
    }
    public S_Stats_Util(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.maxStamina = this.stamina = 10;
    }
}

