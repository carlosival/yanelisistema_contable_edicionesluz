using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdicionesLUZ.Modelo;

namespace EdicionesLUZ.Interfaces
{
    public interface IServicio
    {
        double PrecioDadoTipoPapel(string tipoPapel, int canthojas, string tipoImpresion, string colorImpresion);
        double PrecioDadoManoObra(string manoObra);
        double PrecioEncuadernadoDadoTipoPresillas(string tipoPresillas);
        Servicio ServicioDadoId(int id);
        double CostoDadoMaterial(string material);


        List<Servicio> ServiciosDadoIdPedido(int p);
    }
}
