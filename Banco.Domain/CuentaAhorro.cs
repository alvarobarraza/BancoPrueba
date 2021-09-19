using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain
{
    public class CuentaAhorro
    {
        public string Numero { get; private set; }//encapsulamiento // guardar la integridad
        public string Nombre { get; private set; }
        public decimal Saldo { get; private set; }

        private List<Movimiento> _movimientos;

        public IReadOnlyCollection<Movimiento> Movimientos => _movimientos.AsReadOnly(); //leer movimientos

        public CuentaAhorro(string numero, string nombre)
        {
            Numero = numero;
            Nombre = nombre;

            _movimientos = new List<Movimiento>();
        }

        public string Consignar(decimal valorConsignacion, DateTime fecha, string ciudad)
        {
            if (valorConsignacion <= 0 && ciudad.Equals(""))
            {
                return "El valor a consignar es incorrecto";
            }
            else if (!_movimientos.Any() && valorConsignacion >= 50000 && ciudad.Equals(""))
            {
                _movimientos.Add(new Movimiento(cuentaAhorro: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;

                return $"Su Nuevo Saldo es de {Saldo:c2} pesos m/c"; //50.000--> 50.000,00
            }
            else if (!_movimientos.Any() && valorConsignacion < 50000 && ciudad.Equals(""))
            {
                return "El valor mínimo de la primera consignación debe ser de $50.000 mil pesos.Su nuevo saldo es $ 0 pesos";
            }
            else if (_movimientos.Any() && ciudad.Equals(""))
            {
                _movimientos.Add(new Movimiento(cuentaAhorro: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += valorConsignacion;
                return $"Su Nuevo Saldo es de {Saldo:c2} pesos1 m/c";
            }
            else if (_movimientos.Any() && ciudad.Equals("Nacional"))
            {
                decimal costoNacional = 10000;
                _movimientos.Add(new Movimiento(cuentaAhorro: this, fecha: fecha, tipo: "CONSIGNACION", valor: valorConsignacion));
                Saldo += (valorConsignacion - costoNacional);
                return $"Su Nuevo Saldo es de {Saldo:c2} pesos2 m/c";
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        public string Retirar(decimal valorRetiro, DateTime fecha)
        {
            var nuevoSaldo = Saldo - valorRetiro;
            var cantidadMovimiento = _movimientos.Where(i => i.Tipo.Equals("Retiro") && i.Fecha.Month==fecha.Month).Count();

            if (Saldo <= 20000)
            {
                return "El valor del saldo es inferior a $20.000,00 pesos";
            }
            else if (valorRetiro > Saldo && Saldo >=20000)
            {
                return "El valor del retiro excede al saldo de tu cuenta";
            }
            else if (valorRetiro < Saldo && cantidadMovimiento <= 3 && Saldo >= 20000)
            {
                _movimientos.Add(new Movimiento(cuentaAhorro: this, fecha: fecha, tipo: "Retiro", valor: valorRetiro));
                Saldo = nuevoSaldo;
                return $"El valor del retiro fue exitoso su nuevo saldo es {Saldo:c2} pesos";
            }
            else if (valorRetiro < Saldo && cantidadMovimiento > 3 && Saldo >= 20000)
            {
                _movimientos.Add(new Movimiento(cuentaAhorro: this, fecha: fecha, tipo: "Retiro", valor: valorRetiro));
                Saldo = nuevoSaldo - 5000;
                return $"El valor del retiro fue exitoso su nuevo saldo es {Saldo:c2} pesos";

            }
            else
            {
                throw new NotImplementedException();
            }
            //return "cantidad de movimientos de retiros " + cantidadMovimiento.ToString(); 
            
        }
    }

    public class Movimiento
    {
        public Movimiento(CuentaAhorro cuentaAhorro, DateTime fecha, string tipo, decimal valor)
        {
            CuentaAhorro = cuentaAhorro;
            Fecha = fecha;
            Tipo = tipo;
            Valor = valor;
        }

        public CuentaAhorro CuentaAhorro { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public decimal Valor { get; private set; }
    }
}
