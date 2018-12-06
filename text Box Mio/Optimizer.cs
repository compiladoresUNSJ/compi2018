using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Compi
{
    class Optimizer
    {

        //Tipos de expresiones
        public enum EType { COMP, NCOMP, REDUC }
        //TDA Tabla de expresiones
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
        /// <summary>
        /// Metodo para obtener las expresiones en el codigo.
        /// </summary>
        /// <param name="strSource">Codigo a optimizar</param>
        /// <param name="strStart">Substring de inicio '=' </param>
        /// <param name="strEnd">Substring de fin ';'</param>
        /// <param name="desplazamiento">Posicion desde donde inspeccionar el stirng, 0.</param>
        /// <returns>Expresion detectada o null si ya no hay mas expresiones.</returns>
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

        /// <summary>
        /// Funcion que optimiza el codigo.
        /// </summary>
        /// <param name="code">Codigo a optimizar</param>
        /// <returns>Codigo optimizado</returns>
        public static string run(string code)
        {
            int despl = 0;
            List<ExpressionTable> list = new List<ExpressionTable>();
            string remainingCode = code;
            
            //Paso 1: Deteccion y computacion de expresiones.
            while (true)
            {
                //Encontramos las expresiones.
                ExpressionTable data = Optimizer.getBetween(remainingCode, "=", ";", 0);

                //Si no hay expresiones restantes entonces termina.
                if (data == null)
                    break;
                else
                {
                    //Si obtenemos una expresion la agregamos a la lista
                    //Y actualizamos el desplazamiento para obtener el codigo remanente a optimizar.
                    list.Add(data);
                    despl = data.pos;
                    remainingCode = remainingCode.Substring(despl);
                }

            }

            //Paso 2: Reemplazo de las expresiones
            foreach (ExpressionTable item in list)
            {
                switch (item.type)
                {
                    case EType.COMP:
                        //Reemplazo de expresion por constante.
                        code = code.Replace(item.expression, " " + item.result);
                        break;

                    case EType.REDUC:
                        //A futuro: Implemental reduccion.
                        break;

                    case EType.NCOMP:
                        //A futuro: Extender optimizacion.
                        break;
                }
                    
            }
            return code;
        }
    }
}
