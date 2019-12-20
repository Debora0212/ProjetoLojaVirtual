using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Texto
{
    public class Mascara
    {
        public static string Remover(string valor)
        {
            return valor.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("R$", "").Replace(",", "").Replace(" ", "");
        }
        /*
         * PagarMe
         * - O PagarMe recebe o valor no seguinte formato: 3310, que representa R$ 33,10
         */
        public static int ConverterValorPagarMe(decimal valor)
        {
            //33.1
            string valorString = valor.ToString("C");
            //R$33,10
            valorString = Remover(valorString);
            //3310
            int valorInt = int.Parse(valorString);

            return valorInt;
        }
    }
}
