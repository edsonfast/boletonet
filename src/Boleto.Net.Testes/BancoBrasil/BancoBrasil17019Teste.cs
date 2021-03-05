using System;
using BoletoNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boleto.Net.Testes.BancoBrasil
{
    [TestClass]
    public class BancoBrasil17019Teste
    {
        #region Carteira 17-019

        private BoletoBancario GerarBoletoCarteira17019()
        {
            DateTime vencimento = new DateTime(2012, 6, 14);

            // Cedente
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0131", "7", "00059127", "0");

            // Sacado
            var endereco = new Endereco();
            endereco.End = "Rua";
            endereco.Numero = "S/N";
            endereco.Bairro = "Bairro";
            endereco.Cidade = "Cidade";
            endereco.CEP = "00000-000";
            endereco.Complemento = "Complemento";

            var sacado = new BoletoNet.Sacado("00.000.000/0000-00", "Empresa Teste", endereco);


            // Boleto
            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 1700, "17-019", "18204", cedente);

            var inst = new Instrucao_BancoBrasil(81, 5);
            boleto.Instrucoes.Add(inst);

            boleto.Sacado = sacado;
            boleto.NumeroDocumento = "18204";

            


            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 1;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_4Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379;

            boletoBancario.Boleto.Valida();

            Console.WriteLine(boletoBancario.MontaHtml());

            string nossoNumeroValido = "17/23790018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso n�mero inv�lido para 4 posi��es");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_6Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 237966;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796618204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso n�mero inv�lido para 6 posi��es");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_NossoNumero_ComCodigoConvenio_7Posicoes()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "17/23796610000018204";

            Assert.AreEqual(boletoBancario.Boleto.NossoNumero, nossoNumeroValido, "Nosso n�mero inv�lido para 7 posi��es");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string linhaDigitavelValida = "00190.00009 02379.661008 00018.204172 9 53640000170000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digit�vel inv�lida");
        }

        [TestMethod]
        public void BancoDoBrasil_Carteira_17019_CodigoBarra()
        {
            var boletoBancario = GerarBoletoCarteira17019();

            boletoBancario.Cedente.Convenio = 2379661;

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "00199536400001700000000002379661000001820417";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "C�digo de Barra inv�lido");
        }

        #endregion  Carteira 17-019
    }
}
