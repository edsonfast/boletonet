using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BoletoNet;

namespace Boleto.Net.Testes.BancoUnicred {
    [TestClass]
    public class BancoUnicredTeste {
        #region Carteira 09

        private BoletoBancario GerarBoletoCarteira09() {
            DateTime vencimento = new DateTime(2021, 6, 8);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 7620, "09", "299621", cedente);

            boleto.NumeroDocumento = "299621";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 136;

            boletoBancario.Boleto = boleto;

            return boletoBancario;
        }

        private BoletoBancario GerarBoletoComValorCobradoCarteira09() {
            var vencimento = new DateTime(2012, 6, 8);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0539", "8", "0032463", "9");

            var boleto = new BoletoNet.Boleto(vencimento, 5000, "09", "18194", cedente);
            boleto.NumeroDocumento = "18194";
            boleto.ValorMulta = 100;
            boleto.ValorCobrado = 5100;

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 136;
            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }

        [TestMethod]
        public void Unicred_Carteira_09_NossoNumero() {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string nossoNumeroValido = "0000299621-9";

            Assert.AreEqual(nossoNumeroValido, boletoBancario.Boleto.NossoNumero, "Nosso número inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_09_LinhaDigitavel() {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();
            // 13690.53903 00003.246303 00029.962198 5 86450000762000
            // 23790.53909 90000.001819 94003.246306 3 53580000762000
            string linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 3 53580000762000";

            Assert.AreEqual(linhaDigitavelValida, boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, "Linha digitável inválida");
        }

        [TestMethod]
        public void Unicred_Carteira_09_LinhaDigitavel_ValorCobrado() {
            var boletoBancario = GerarBoletoComValorCobradoCarteira09();

            boletoBancario.Boleto.Valida();

            var linhaDigitavelValida = "23790.53909 90000.001819 94003.246306 1 53580000510000";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida, "Linha digitável inválida");
        }

        [TestMethod]
        public void Unicred_Carteira_09_CodigoBarra() {
            var boletoBancario = GerarBoletoCarteira09();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "23793535800007620000539090000001819400324630";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_09_CodigoBarra_ValorCobrado() {
            var boletoBancario = GerarBoletoComValorCobradoCarteira09();

            boletoBancario.Boleto.Valida();

            var codigoBarraValida = "23791535800005100000539090000001819400324630";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_09_ArquivoRemessa() {
            Cedente objCEDENTE = new Cedente(
               "12345678000155",
               "TESTE",
               "1111",
               "11234",
               "1"
               );
            objCEDENTE.Codigo = "123456";
            objCEDENTE.Convenio = 9;

            //Inst�ncia de Boleto
            BoletoNet.Boleto objBOLETO = new BoletoNet.Boleto();
            //O nosso-numero deve ser de 11 posi��es
            objBOLETO.EspecieDocumento = new EspecieDocumento(237, "12");
            objBOLETO.DataVencimento = DateTime.Now.AddDays(10);
            objBOLETO.ValorBoleto = 90;
            objBOLETO.Carteira = "09";
            objBOLETO.NossoNumero = ("00000012345");
            objBOLETO.Cedente = objCEDENTE;
            //O n� do documento deve ser de 10 posi��es
            objBOLETO.NumeroDocumento = "1234567890";
            objBOLETO.NumeroControle = "100";
            //A data do documento � a data de emiss�o do boleto
            objBOLETO.DataDocumento = DateTime.Now;
            //A data de processamento � a data em que foi processado o documento, portanto � da data de emiss�o do boleto
            objBOLETO.DataProcessamento = DateTime.Now;
            objBOLETO.Sacado = new Sacado("12345678000255", "TESTE SACADO");
            objBOLETO.Sacado.Endereco.End = "END SACADO";
            objBOLETO.Sacado.Endereco.Bairro = "BAIRRO SACADO";
            objBOLETO.Sacado.Endereco.Cidade = "CIDADE SACADO";
            objBOLETO.Sacado.Endereco.CEP = "CEP SACADO";
            objBOLETO.Sacado.Endereco.UF = "RR";

            objBOLETO.PercMulta = 10;
            objBOLETO.JurosMora = 5;

            // nao precisa desta parte no boleto do brasdesco.
            /*objBOLETO.Remessa = new Remessa()
            {
                Ambiente = Remessa.TipoAmbiemte.Producao,
                CodigoOcorrencia = "01",
            };*/

            Boletos objBOLETOS = new Boletos();
            objBOLETOS.Add(objBOLETO);

            var mem = new MemoryStream();
            var objREMESSA = new ArquivoRemessa(TipoArquivo.CNAB400);
            objREMESSA.GerarArquivoRemessa("09", new Banco(136), objCEDENTE, objBOLETOS, mem, 1000);


        }

        [TestMethod]
        public void Unicred_Carteira_09_ArquivoRetorno400() {
            ArquivoRetornoCNAB400 ret = new ArquivoRetornoCNAB400();

            var arquivo = string.Empty; //@"C:\Temp\2017-Julho-CB180701_009_CB_09_433920_20170718041844_00152068-7.RET";

            if (arquivo != string.Empty) {
                using (FileStream fs = new FileStream(arquivo, FileMode.Open, FileAccess.Read)) {
                    ret.LerArquivoRetorno(new Banco(136), fs);
                }

                foreach (var item in ret.ListaDetalhe) {
                    Console.WriteLine(item.ToString());
                }
            }

        }
        #endregion
    }
}
