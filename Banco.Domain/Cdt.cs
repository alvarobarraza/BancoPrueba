using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class Cdte
    {
        public string Numero { get; private set; }//encapsulamiento // guardar la integridad
        public string Nombre { get; private set; }

        public decimal Saldo { get; private set; }

        private List<MovimientoCdt> _movimientos;

        public Cdte(string numero, string nombre)
        {
            Numero = numero;
            Nombre = nombre;

            _movimientos = new List<MovimientoCdt>();
        }

        public IReadOnlyCollection<MovimientoCdt> Movimientos => _movimientos.AsReadOnly();


        public string Consignar(decimal valorConsignacion, DateTime fecha, string tasa)
        {
            decimal minConsignacion = 1000000;
            if (valorConsignacion < minConsignacion)
            {
                return "El valor a consignar minimo debe ser de 1 millon de pesos";
            }
            else if (valorConsignacion > minConsignacion && !_movimientos.Any())
            {
                _movimientos.Add(new MovimientoCdt(cdt: this, fecha: fecha, tipo:"Consignacion", valor: valorConsignacion, tasa: tasa));
                Saldo += valorConsignacion;

                return $"Su Nuevo Saldo es de $ {Saldo:c2} pesos m/c";

            }
            else if (valorConsignacion > minConsignacion && _movimientos.Any())
            {
                return "No se puede Consignar dos veces";
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        public string Retirar(decimal valorRetiro, DateTime fecha)
        {
            
            if (valorRetiro <= Saldo)
            {
                //if()
                //{
                //    double tEfectivaA = 0.06;
                //    return "hola";
                //}
                return "hola";
            }
            throw new NotImplementedException();
        }
    }

    public class MovimientoCdt
    {
        public MovimientoCdt(Cdte cdt, DateTime fecha, string tipo, decimal valor, string tasa)
        {
            Cdt = cdt;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
            Tasa = tasa;
        }

        public Cdte Cdt { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
        public string Tasa { get; private set; }
    }

}
