using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes.BancoMercantil {
    [TestClass]
    public class BancoMercantilTeste {
        private BoletoBancario GerarBoleto() {
            DateTime vencimento = new DateTime(2025, 12, 1);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0320", "0", "03200", "6");

            Boleto boleto = new Boleto(vencimento, 2701.40m, "109", "20063", cedente);

            boleto.NumeroDocumento = "20063";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 389;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void NossoNumero() {
            var boletoBancario = GerarBoleto();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "051090200637";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso número inválido");
        }

        [TestMethod]
        public void LinhaDigitavel() {
            var boletoBancario = GerarBoleto();

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "38993.20059 10902.006377 00000.000026 1 12820000270140";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void CodigoBarra() {
            var boletoBancario = GerarBoleto();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "38991282000027014003200510902006370000000002";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }
    }
}
