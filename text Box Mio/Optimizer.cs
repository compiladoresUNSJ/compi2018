using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Compi
{
    class Optimizer
    {
        public enum EType { COMP, NCOMP, REDUC }
        public class ExpressionTable
        {
            public int pos;
            public String expression;
            public String result;
            public EType type;

            public string toString()
            {
                return pos.ToString() + " - " + expression + " - " + result;
            }

        }

        public static ExpressionTable getBetween(string strSource, string strStart, string strEnd, int desplazamiento)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, desplazamiento) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);

                string expression = strSource.Substring(Start, End - Start);
                string auxexpr = expression.Replace(" ", string.Empty);
                try
                {

                    DataTable dt = new DataTable();
                    int answer = (int)dt.Compute(auxexpr, "");
                    ExpressionTable ET = new ExpressionTable();
                    ET.result = answer.ToString();
                    ET.expression = expression;
                    ET.pos = End + 1;
                    ET.type = EType.COMP;
                    return ET;
                }
                catch (EvaluateException e)
                {
                    ExpressionTable ET = new ExpressionTable();
                    ET.expression = auxexpr;
                    ET.pos = End + 1;
                    ET.result = "Expresion";
                    ET.type = EType.REDUC;
                    return ET;
                }
                catch (SyntaxErrorException e)
                {
                    ExpressionTable ET = new ExpressionTable();
                    ET.expression = auxexpr;
                    ET.pos = End + 1;
                    ET.result = "No Computable";
                    ET.type = EType.NCOMP;
                    return ET;
                }

            }
            else
            {
                return null;
            }
        }
    }
}
