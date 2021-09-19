using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banco.Domain.Test.Cdt
{
    public class CdtTest
    {
        /*
         HU 7.
            Como Usuario quiero realizar consignar mi dinero a mi CDT para ahorrar el dinero sin tener
            acceso al de acuerdo al término definido.
            Criterios de Aceptación
            7.1 El valor de consignación inicial debe ser de mínimo 1 millón de pesos.
            7.2 Sólo se podrá realizar una sola consignación. 
        */

        [Test]
        public void NopuedeConsigarMenosDeMillon()
        {
            var cuentaCdt = new Cdte(numero: "10001", nombre: "Cuenta Ejemplo");

            decimal valorConsignacion = 500000;

            string respuesta = cuentaCdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), tasa: "Semestral");
            Assert.AreEqual("El valor a consignar minimo debe ser de 1 millon de pesos", respuesta);
        }

        [Test]
        public void PrimeraConsignacion()
        {
            var cuentaCdt = new Cdte(numero: "10001", nombre: "Cuenta Ejemplo");

            decimal valorConsignacion = 1500000;
            string respuesta = cuentaCdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), tasa: "Semestral");
            Assert.AreEqual(1, cuentaCdt.Movimientos.Count);
            Assert.AreEqual("Su Nuevo Saldo es de $ 1500000 pesos m/c", respuesta);
        }

        [Test]
        public void NoSePuedeConsignarXsegundaVez()
        {
            var cuentaCdt = new Cdte(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaCdt.Consignar(valorConsignacion: 1500000, fecha: new DateTime(2020, 2, 1), tasa: "Semestral");

            decimal valorConsignacion = 1500000;
            string respuesta = cuentaCdt.Consignar(valorConsignacion: valorConsignacion, fecha: new DateTime(2020, 2, 1), tasa: "Semestral");
           
            Assert.AreEqual("No se puede Consignar dos veces", respuesta);
        }


        /*-----------------------------Retiro-------------------------------*/
        /*
         HU 8.
            Como Usuario quiero el retiro de mi dinero de mi CDT al finalizar el Término establecido para
            recuperar el dinero depositado.
            Criterios de Aceptación
            8.1 Los retiros sólo se podrán realizar una vez haya finalizado el término del CDT.
            8.2 Al realizar el retiro se le liquidará un interés de acuerdo a la tasa definida y plazo de termino.
            8.3 El valor a retirar se reduce del saldo del CDT.
         */

        [Test]
        public void NoPuedeRetirarNoHaFinalizadoCdt()
        {
            var cuentaCdt = new Cdte(numero: "10001", nombre: "Cuenta Ejemplo");
            cuentaCdt.Consignar(valorConsignacion: 1500000, fecha: new DateTime(2021, 09, 19), tasa: "Trimestral");

            decimal valorRetiro = 1000000;
            string respuesta = cuentaCdt.Retirar(valorRetiro: valorRetiro, fecha: new DateTime(2021, 12, 20));

            Assert.AreEqual("No puede Retirar Porque no ha finalizado el termino", respuesta);

        }
    }

}
