using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banco.Domain.Test.CuentasAhorro
{
    public class CuentaAhorroTest
    {

        /*
        Escenario: Valor de consignación -1
        H1: COMO Cajero del Banco QUIERO realizar consignaciones a una cuenta de ahorro PARA salvaguardar el dinero.
        Criterio de Aceptación:
        1.2 El valor de la consignación no puede ser menor o igual a 0.
        //El ejemplo o escenario
        Dado El cliente tiene una cuenta de ahorro 
        Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
        Cuando Va a consignar un valor -1
        Entonces El sistema presentará el mensaje. “El valor a consignar es incorrecto”
         */
        [Test]
        public void NoPuedeConsignarValorDeMenosUno()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");

            decimal valorConsignacion = -1;

            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020,2,1), ciudad: "");
            Assert.AreEqual("El valor a consignar es incorrecto", respuesta);
        }

        /*
          Escenario: Consignación Inicial Correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el 
            dinero.
            Criterio de Aceptación:
           
            1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
            1.3 El valor de la consignación se le adicionará al valor del saldo aumentará

            Dado El cliente tiene una cuenta de ahorro 
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
            Cuando Va a consignar el valor inicial de 50 mil pesos 
            Entonces El sistema registrará la consignación
            AND presentará el mensaje. “Su Nuevo Saldo es de $50.000,00 pesos m/c”.
         */
        [Test]
        public void PuedeHacerConsignacionInicialCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");

            decimal valorConsignacion = 50000;

            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "");

            Assert.AreEqual(1, cuentaAhorro.Movimientos.Count);//Criterio general
            Assert.AreEqual("Su Nuevo Saldo es de $ 50.000,00 pesos m/c", respuesta);
        }

        /*
         Escenario: Consignación Inicial Incorrecta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el
            dinero.
            Criterio de Aceptación:
            1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
            Dado El cliente tiene una cuenta de ahorro con
            Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
            Cuando Va a consignar el valor inicial de $49.950 pesos
            Entonces El sistema no registrará la consignación
            AND presentará el mensaje. “El valor mínimo de la primera consignación debe ser
            de $50.000 mil pesos. Su nuevo saldo es $0 pesos”.
         */

        [Test]
        public void ConsignaciónInicialIncorrecta()
        {
            
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            
            decimal valorConsignacion = 49950;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "");

            Assert.AreEqual("El valor mínimo de la primera consignación debe ser de $50.000 mil pesos.Su nuevo saldo es $ 0 pesos", respuesta);
        }

        /*
         * Escenario: Consignación posterior a la inicial correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el
            dinero.
            Criterio de Aceptación:
            1.3 El valor de la consignación se le adicionará al valor del saldo aumentará
            Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000
            Cuando Va a consignar el valor de $49.950 pesos
            Entonces El sistema registrará la consignación
            AND presentará el mensaje. “Su Nuevo Saldo es de $79.950,00 pesos m/c”
          */
        [Test]
        public void ConsignaciónPosteriorALaInicialCorrecta()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "");
            cuentaAhorro.Retirar(valorRetiro: 20000, fecha: new DateTime(2020, 2, 1));

            decimal valorConsignacion = 49950;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: "");
            Assert.AreEqual(3, cuentaAhorro.Movimientos.Count);
            Assert.AreEqual("Su Nuevo Saldo es de $ 79.950,00 pesos1 m/c",respuesta);
        }

        /*
         Escenario: Consignación posterior a la inicial correcta
            HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el
            dinero.
            Criterio de Aceptación:
            1.4 La consignación nacional (a una cuenta de otra ciudad) tendrá un costo de $10 mil pesos.
            Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000 perteneciente a una
            sucursal de la ciudad de Bogotá y se realizará una consignación desde una sucursal
            de la Valledupar.
            Cuando Va a consignar el valor inicial de $49.950 pesos.
            Entonces El sistema registrará la consignación restando el valor a consignar los 10 mil pesos.
            AND presentará el mensaje. “Su Nuevo Saldo es de $69.950,00 pesos m/c”.
         */
        [Test]
        public void ConsignaciónPosteriorALaInicialCorrectaConCiudad()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "");
            cuentaAhorro.Retirar(valorRetiro: 20000, fecha: new DateTime(2020, 2, 1));


            decimal valorConsignacion = 49950;
            string respuesta = cuentaAhorro.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), ciudad: new string("Nacional"));
            Assert.AreEqual(3, cuentaAhorro.Movimientos.Count);
            Assert.AreEqual("Su Nuevo Saldo es de $ 69.950,00 pesos2 m/c", respuesta);
        }


        /*------------------------------Retirar------------------------*/

        [Test]
        public void NoPuedeRetirarSaldoMenosAVeinteMilpesos()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            decimal valorRetiro = 10000;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("El valor del saldo es inferior a $20.000,00 pesos", respuesta);
        }


        [Test]
        public void NoPuedeRetirarMasDelSaldo()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "");
            decimal valorRetiro = 60000;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));
            Assert.AreEqual("El valor del retiro excede al saldo de tu cuenta", respuesta);
        }

        [Test]
        public void PuedeRetirarLosPrimeros3NoTendraCosto()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaAhorro.Consignar(valorConsignacion: 50000, fecha: new DateTime(2020, 2, 1), ciudad: "");

            decimal valorRetiro = 10000;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(1, cuentaAhorro.Movimientos.Where(i => i.Tipo.Equals("Retiro")).Count());
            Assert.AreEqual("El valor del retiro fue exitoso su nuevo saldo es $ 40.000,00 pesos", respuesta);
        }

        [Test]
        public void PuedeRetirarDespuesDe3TendraCosto()
        {
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaAhorro.Consignar(valorConsignacion: 100000, fecha: new DateTime(2020, 2, 1), ciudad: "");
            cuentaAhorro.Retirar(valorRetiro: 10000, fecha: new DateTime(2020, 2, 1));
            cuentaAhorro.Retirar(valorRetiro: 10000, fecha: new DateTime(2020, 2, 1));
            cuentaAhorro.Retirar(valorRetiro: 10000, fecha: new DateTime(2020, 2, 1));
            cuentaAhorro.Retirar(valorRetiro: 10000, fecha: new DateTime(2020, 2, 1));

            decimal valorRetiro = 10000;
            string respuesta = cuentaAhorro.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2020, 2, 1));

            Assert.AreEqual(5, cuentaAhorro.Movimientos.Where(i => i.Tipo.Equals("Retiro")).Count()); //criterio general

            Assert.AreEqual("El valor del retiro fue exitoso su nuevo saldo es $ 45.000,00 pesos", respuesta);
        }
    }
}
