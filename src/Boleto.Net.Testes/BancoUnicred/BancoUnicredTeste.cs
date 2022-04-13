using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BoletoNet;

namespace Boleto.Net.Testes.BancoUnicred {
    [TestClass]
    public class BancoUnicredTeste {
        #region Carteira 021

        private BoletoBancario GerarBoletoCarteira021() {
            DateTime vencimento = new DateTime(2021, 3, 11);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "1757", "4", "161851", "2");
            cedente.Endereco = new Endereco() { Bairro = "Bairro", CEP = "88337-000", End = "Rua x", Numero = "S/N", Cidade = "Cidade", Complemento = "Compl.", Email = "email@email.com.br", UF = "UF" };
            cedente.MostrarCNPJnoBoleto = true;

            BoletoNet.Boleto boleto = new BoletoNet.Boleto(vencimento, 23, "021", "17", cedente);

            boleto.NumeroDocumento = "17";

            var boletoBancario = new BoletoBancario();

            boletoBancario.CodigoBanco = 136;

            boleto.Sacado = new Sacado("15.299.525.0001/47", "Nome Teste", new Endereco() {
                End = "Rua Teste",
                Bairro = "Bairro Teste",
                CEP = "88337000",
                Cidade = "Cidade Teste",
                Complemento = "Compl. teste",
                Email = "suporte@sysvalley.com.br",
                Numero = "123",
                UF = "SC",
            });
            boletoBancario.MostrarEnderecoCedente = true;
            
            boletoBancario.Boleto = boleto;

            var inst = new BoletoNet.Instrucao(136);


            return boletoBancario;
        }

        private BoletoBancario GerarBoletoComValorCobradoCarteira021() {
            var vencimento = new DateTime(2021, 4, 9);
            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "1757", "4", "161851", "2");

            var boleto = new BoletoNet.Boleto(vencimento, 23, "021", "17", cedente);
            boleto.NumeroDocumento = "17";
            boleto.ValorMulta = 100;
            boleto.ValorCobrado = 5100;

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 136;

            boleto.Sacado = new Sacado("15.299.525.0001/47", "Nome Teste", new Endereco() {
                End = "Rua Teste",
                Bairro = "Bairro Teste",
                CEP = "88337000",
                Cidade = "Cidade Teste",
                Complemento = "Compl. teste",
                Email = "suporte@sysvalley.com.br",
                Numero = "123",
                UF = "SC",
            });

            boletoBancario.Boleto = boleto;
            return boletoBancario;
        }

        [TestMethod]
        public void Unicred_Carteira_021_NossoNumero() {
            var boletoBancario = GerarBoletoCarteira021();

            boletoBancario.Boleto.Valida();

            boletoBancario.MontaHtml();

            // 0000000002-7
            // 0000299621-9
            // 0000000501-0
            // 0000000017-5
            string nossoNumeroValido = "0000000017-5";//"0000000017-5";

            Assert.AreEqual(nossoNumeroValido, boletoBancario.Boleto.NossoNumero, "Nosso número inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_021_LinhaDigitavel() {
            var boletoBancario = GerarBoletoCarteira021();

            boletoBancario.Boleto.Valida();
            string linhaDigitavelValida = "13691.75706 00161.851209 00000.001750 5 85850000002300";

            Assert.AreEqual(linhaDigitavelValida, boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, "Linha digitável inválida");
        }

        [TestMethod]
        public void Unicred_Carteira_021_LinhaDigitavel_ValorCobrado() {
            var boletoBancario = GerarBoletoComValorCobradoCarteira021();

            boletoBancario.Boleto.Valida();

            var linhaDigitavelValida = "13691.75706 00161.851209 00000.001750 5 85850000002300";

            Assert.AreEqual(linhaDigitavelValida, boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, "Linha digitável inválida");
        }

        [TestMethod]
        public void Unicred_Carteira_021_CodigoBarra() {
            var boletoBancario = GerarBoletoCarteira021();

            boletoBancario.Boleto.Valida();

            string codigoBarraValida = "13695858500000023001757000161851200000000175";

            Assert.AreEqual(codigoBarraValida, boletoBancario.Boleto.CodigoBarra.Codigo, "Código de Barra inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_021_CodigoBarra_ValorCobrado() {
            var boletoBancario = GerarBoletoComValorCobradoCarteira021();

            boletoBancario.Boleto.Valida();

            var codigoBarraValida = "13695858500000023001757000161851200000000175";

            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.Codigo, codigoBarraValida, "Código de Barra inválido");
        }

        [TestMethod]
        public void Unicred_Carteira_021_ArquivoRemessa() {
            Cedente objCEDENTE = new Cedente(
               "12345678000155",
               "TESTE",
               "1757",
               "161851",
               "2"
               );
            objCEDENTE.Codigo = "123456";
            objCEDENTE.Convenio = 80014090;
            objCEDENTE.ContaBancaria.DigitoAgencia = "4";

            //Inst�ncia de Boleto
            BoletoNet.Boleto objBOLETO = new BoletoNet.Boleto();
            //O nosso-numero deve ser de 11 posições
            objBOLETO.EspecieDocumento = new EspecieDocumento(136, "12");
            objBOLETO.DataVencimento = DateTime.Now.AddDays(10);
            objBOLETO.ValorBoleto = 23;
            objBOLETO.Carteira = "021";
            objBOLETO.NossoNumero = ("0000000017");
            objBOLETO.Cedente = objCEDENTE;
            //O n� do documento deve ser de 10 posições
            objBOLETO.NumeroDocumento = "123";
            objBOLETO.NumeroControle = "456";
            //A data do documento � a data de emissão do boleto
            objBOLETO.DataDocumento = DateTime.Now;
            //A data de processamento é a data em que foi processado o documento, portanto é da data de emissão do boleto
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
            objREMESSA.GerarArquivoRemessa("021", new Banco(136), objCEDENTE, objBOLETOS, mem, 1000);

            using (Stream stream = File.Create(@"c:\temp\unicred\remessa\rem1.txt")) {
                objREMESSA.GerarArquivoRemessa("80014090", new Banco(136), objCEDENTE, objBOLETOS, stream, 1000);
            }
        }

        [TestMethod]
        public void Unicred_Carteira_09_ArquivoRetorno400() {
            ArquivoRetornoCNAB400 ret = new ArquivoRetornoCNAB400();

            var arquivo = @"C:\Temp\UNICRED.RET";

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
