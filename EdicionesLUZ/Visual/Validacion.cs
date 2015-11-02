using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EdicionesLUZ.Visual
{
    public class Validacion
    {
        //Metodos de validacion 
        public static void ValidarSoloLetras(KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
                e.Handled = false;
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }

            else
                e.Handled = true;
        }

        public static void ValidarSoloNumerosConCaracteres(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }

            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }

            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void ValidarSoloNumeros(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        public static bool ValidarConcordanciaFechas(DateTime fecha1, DateTime fecha2) 
        {
            /*
             *  si el entero es menor que cero entonces la fecha 2 es menor que la fecha 1
             *  si el entero es cero entonces las fechas son iguales
             *  si el entero es mayor que cero entonces la fecha 2 es mayor que la fecha 1
             */

            int estado = 0; 
            estado= fecha2.Date.CompareTo(fecha1);
            if (estado < 0 )
                return false;
            if (estado > 0 || estado == 0)
                return true;
            else return false;
        }
    }
}
