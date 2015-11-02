using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Interfaces;

using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Managers
{
    class ServicioManager
    {
        IServicio iservicio;
        Servicio servicio;
        public Servicio Servicio
        {
            get 
            {
                if (servicio == null)
                    servicio = new Servicio();
                    return servicio;
            }
        }

        public ServicioManager()
        {
            //iservicio = new ServicioMock();
        }
    }
}
