using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class TarjetaCreditos
    {
        public string Numero { get; private set; }
        public string Nombre { get; private set; }
        public decimal Cupo { get; private set; }
        public decimal Saldo { get; private set; }

        private List<TarjetaCreditoMovimiento> _movimientos;

        public IReadOnlyCollection<TarjetaCreditoMovimiento> Movimientos => _movimientos.AsReadOnly();

        public TarjetaCreditos(string numero, string nombre, decimal cupo)
        {
            Numero = numero;
            Nombre = nombre;
            Cupo = cupo;
            
            _movimientos = new List<TarjetaCreditoMovimiento>();
        }

        public string Abono(decimal valorAbono, DateTime fecha)
        {

            if (valorAbono <= 0)
            {
                return $"El valor del Abono No puede ser menor o igual a 0";
            }
            else if (valorAbono >= 0 && valorAbono <= Saldo)
            {
                _movimientos.Add(new TarjetaCreditoMovimiento(cuenta: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorAbono));
                Cupo += valorAbono;
                Saldo -= valorAbono;

                return $"Su nuevo saldo es de {Saldo:c2} pesos y su cupo aumenta a {Cupo:c2} pesos";

            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        public string Retirar(decimal valorRetiro, DateTime fecha)
        {
            if (valorRetiro <= 0)
            {
                return "El valor retirar no puede ser menor igual a cero";
            }
            else if (valorRetiro >= 0 && valorRetiro > Cupo)
            {
                return "El valor excede el cupo en la tarjeta";
            } 
            else if (valorRetiro>=0 && valorRetiro<=Cupo)
            {
                _movimientos.Add(new TarjetaCreditoMovimiento(cuenta: this, fecha: fecha, tipo: "Retirar", valor: valorRetiro));
                var nuevoCupo = Cupo - valorRetiro;
                
                Saldo = nuevoCupo - Cupo;

                Cupo = nuevoCupo;

                return $"Su retiro fue exitoso su nuevo cupo es de {Cupo:c2} y su saldo es de {Saldo:c2}";
            }
            throw new NotImplementedException();
        }
    }

    public class TarjetaCreditoMovimiento
    {
        public TarjetaCreditoMovimiento(TarjetaCreditos cuenta, DateTime fecha, string tipo, decimal valor)
        {
            Cuenta = cuenta;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
        }

        public TarjetaCreditos Cuenta { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
    }
}
