using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Interfaces
{
  public interface IEntidad
  {
        List<Cliente> TodosClientes();
        List<Pedido> TodosPedidos();
        List<Servicio> TodosServicios();
        Entidad EntidadDadoId(int id);
  
  }
}
