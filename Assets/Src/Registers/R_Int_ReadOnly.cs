using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Read-only/Integer")]
public class R_Int_ReadOnly : R_Default
{
    //Ar_ = arithmetic
    public enum ARITHMETIC { 
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE
    }
    [System.Serializable]
    public struct Ar_RInt {
        public R_Int integerRegister;
        public ARITHMETIC arithmetic;
    }
    public Ar_RInt[] registerSums;

    public int value
    {
        get
        {
            int total = 0;
            foreach (var i in registerSums)
            {
                if (i.integerRegister == null)
                    continue;
                switch (i.arithmetic)
                {
                    case ARITHMETIC.ADD:
                        total += i.integerRegister.integer;
                        break;
                    case ARITHMETIC.SUBTRACT:
                        total -= i.integerRegister.integer;
                        break;
                    case ARITHMETIC.MULTIPLY:
                        total *= i.integerRegister.integer;
                        break;
                    case ARITHMETIC.DIVIDE:
                        total /= i.integerRegister.integer;
                        break;
                }
            }
            return total;
        }
    }
}
