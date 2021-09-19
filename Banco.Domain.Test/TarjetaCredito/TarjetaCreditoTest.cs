using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain.Test.TarjetaCredito
{
    public class TarjetaCreditoTest
    {
        /*
         HU 5.
            Como Usuario quiero realizar consignaciones (abonos) a una Tarjeta Crédito para abonar al saldo
            del servicio.
            Criterios de Aceptación
            5.1 El valor a abono no puede ser menor o igual a 0.
            5.2 El abono podrá ser máximo el valor del saldo de la tarjeta de crédito.
            5.3 Al realizar un abono el cupo disponible aumentará con el mismo valor que el valor del abono
            y reducirá de manera equivalente el saldo. 
         */

        [Test]
        public void ValorDelAbonoNoPuedeSerMenorIgualaCero()
        {
            var tarjetaCredito = new TarjetaCreditos(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 1000000);
            tarjetaCredito.Retirar(valorRetiro: 300000, fecha: new DateTime(2020, 2, 1));
            decimal valorAbono = -1000;

            string respuesta = tarjetaCredito.Abono(valorAbono: valorAbono, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, tarjetaCredito.Movimientos.Count);//Criterio general
            Assert.AreEqual("El valor del Abono No puede ser menor o igual a 0", respuesta);
        }

        [Test]
        public void PodraSerMaximoElSaldoDeLaTarjeta()
        {
            var tarjetaCredito = new TarjetaCreditos(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 1000000);
            decimal valorAbono = 500000;

            string respuesta = tarjetaCredito.Abono(valorAbono: valorAbono, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, tarjetaCredito.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su nuevo saldo es de $ 500.000,00 pesos y su cupo aumenta a $ 1.500.000,00 pesos", respuesta);

        }

        /*-----------------------------RETIRAR-----------------------------------*/

        /*
         HU 6.
            Como Usuario quiero realizar retiros (avances) a una cuenta de crédito para retirar dinero en
            forma de avances del servicio de crédito.
            Criterios de Aceptación
            6.1 El valor del avance debe ser mayor a 0. ya
            6.2 Al realizar un avance se debe reducir el valor disponible del cupo con el valor del avance.
            6.3 Un avance no podrá ser mayor al valor disponible del cupo.
         */
        [Test]
        public void RetirarMayoraCero()
        {
            var tarjetaCredito = new TarjetaCreditos(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 1000000);

            decimal ValorRetiro = -10000;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: ValorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual("El valor retirar no puede ser menor igual a cero", respuesta);
        }

        [Test]
        public void NoPodraRetirarMayorAvance()
        {
            var tarjetaCredito = new TarjetaCreditos(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 1000000);

            decimal ValorRetiro = 2000000;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: ValorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual("El valor excede el cupo en la tarjeta", respuesta);
        }

        [Test]
        public void PodraRetirarAvance()
        {
            var tarjetaCredito = new TarjetaCreditos(numero: "10001", nombre: "Cuenta Ejemplo", cupo: 1000000);

            decimal ValorRetiro = 250000;
            string respuesta = tarjetaCredito.Retirar(valorRetiro: ValorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, tarjetaCredito.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su retiro fue exitoso su nuevo cupo es de $ 750.000,00 y su saldo es de -$ 250.000,00", respuesta);
        }
    }
}
