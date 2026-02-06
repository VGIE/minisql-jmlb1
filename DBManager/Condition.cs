using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public const string MayorQue = ">";
        public const string MenorQue = "<";
        public const string IgualQue = "=";

        public string ColumnName { get; private set; }
        public string Operator { get; private set; } // >/>/=
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables
            this.ColumnName = column;
            this.Operator = op;
            this.LiteralValue = literalValue;
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd" --> se compara la primera letra solo
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)
            //int.parse() --> string en int
            //double.parse() --> string en double

            //si el tipo de columna es entero
            if(type == ColumnDefinition.DataType.Int)
            {
                //los valores de string --> entero
                int value1 = int.Parse(value);
                int value2 = int.Parse(LiteralValue);

                if(Operator == MayorQue)
                {
                    return value1 > value2;
                }

                if (Operator == MenorQue)
                {
                    return value1 < value2;
                }

                if (Operator == IgualQue)
                {
                    return value1 == value2;
                }
            }
            if (type == ColumnDefinition.DataType.Double)
            {
                // string --> decimal
                double value1 = double.Parse(value);
                double value2 = double.Parse(LiteralValue);

                if (Operator == MayorQue)
                {
                    return value1 > value2;
                }

                if (Operator == MenorQue)
                {
                    return value1 < value2;
                }

                if (Operator == IgualQue)
                {
                    return value1 == value2;
                }

            }
            if(type == ColumnDefinition.DataType.String)
            {
                //la comparación devuelve un número
                int comparacion = string.Compare(value, LiteralValue);
                // comparacion = 0 --> los dos valores son iguales
                // comparacion > 0 --> value es mayor que literalValue
                // comparación < 0 --> value es mas pequeñeo que literalValue

                if (Operator == MayorQue)
                {
                    //si el primer valor es más grande que el segundo devolverá 'true'
                    //en los demás casos devuelve 'false'
                    return comparacion > 0;
                }

                if (Operator == MenorQue)
                {
                    return comparacion < 0;
                }

                if (Operator == IgualQue)
                {
                    return comparacion == 0;
                }
            }

            return false;
            
        }
    }
}