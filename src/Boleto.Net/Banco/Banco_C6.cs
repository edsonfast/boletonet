using BoletoNet.Util;
using System;
using System.Linq;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.336.jpg", "image/jpg")]

namespace BoletoNet {
    internal class Banco_C6 : AbstractBanco, IBanco {
        #region Construtores

        internal Banco_C6() {
            try {
                this.Codigo = 336;
                this.Digito = "0";
                this.Nome = "C6 Bank";
            } catch (Exception ex) {
                throw new ArgumentException("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion Construtores

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do Banco C6
        /// </summary>
        public override void ValidaBoleto(Boleto boleto) {
            var carteirasImplementadas = new int[] { 10, 20, 21, 22, 23, 24 };
            var possiveisModalidadesIdentificadorLayout = new int[] { 3, 4 };

            int carteiraInt;
            int tipoModalidadeInt;

            if (string.IsNullOrEmpty(boleto.Carteira) || !int.TryParse(boleto.Carteira, out carteiraInt))
                throw new ArgumentException("Carteira não informada ou inválida.");

            if (!carteirasImplementadas.Contains(carteiraInt))
                throw new ArgumentException(string.Format("Carteira {0} não implementada (Carteiras disponíveis: {1}).", carteiraInt, string.Join(",", carteirasImplementadas)));

            if (string.IsNullOrEmpty(boleto.TipoModalidade) || !int.TryParse(boleto.TipoModalidade, out tipoModalidadeInt))
                throw new ArgumentException("'TipoModalidade' não informada ou inválida para o boleto (Corresponde ao IdentificadorLayout para o C6 Bank).");

            if (!possiveisModalidadesIdentificadorLayout.Contains(tipoModalidadeInt))
                throw new ArgumentException(string.Format("Modalidade informada {0} é inválida (Modalidades disponíveis: {1}).", tipoModalidadeInt, string.Join(",", possiveisModalidadesIdentificadorLayout)));

            if (boleto.NossoNumero.Length != 10)
                throw new ArgumentException("Nosso número deve possuir 10 posições");

            if (boleto.Cedente.Codigo.Length != 12)
                throw new NotImplementedException("Código do cedente precisa conter 12 dígitos");

            boleto.LocalPagamento = "CANAIS ELETRONICOS, AGENCIAS OU CORRESPONDENTES DE TODO O BRASIL";

            if (boleto.EspecieDocumento.Codigo == "0")
                boleto.EspecieDocumento = new EspecieDocumento_C6(EnumEspecieDocumento_C6.DuplicataMercantil);

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        #endregion Métodos de Instância

        #region Métodos de formatação do boleto

        public override void FormataNossoNumero(Boleto boleto) {
            // Sem necessidade de formatar, considera o valor recebido.
        }

        public override void FormataNumeroDocumento(Boleto boleto) {
            // Sem necessidade de formatar, considera o valor recebido.
        }

        public override void FormataCodigoBarra(Boleto boleto) {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                Utils.FormatCode(Codigo.ToString(), 3),
                boleto.Moeda,
                FatorVencimento(boleto),
                valorBoleto,
                boleto.Cedente.Codigo.PadLeft(12, '0'),
                boleto.NossoNumero.PadLeft(10, '0'),
                boleto.Carteira.PadLeft(2, '0'),
                int.Parse(boleto.TipoModalidade));

            int _dacBoleto = Mod11Peso2a9(boleto.CodigoBarra.Codigo);

            boleto.CodigoBarra.Codigo = boleto.CodigoBarra.Codigo.Substring(0, 4) + _dacBoleto + boleto.CodigoBarra.Codigo.Substring(4, 39);
        }

        public override void FormataLinhaDigitavel(Boleto boleto) {
            string campo1;
            string campo2;
            string campo3;
            string campo4;
            string campo5;
            int digitoMod;

            /*
            Campos 1
            Código do Banco na Câmara de Compensação “336”
            Código da moeda "9" (*)
            Código do Cedente – 5 Posições
            Dígito Verificador Módulo 10 (Campo 1)
             */
            campo1 = String.Concat(Utils.FormatCode(Codigo.ToString(), 3), boleto.Moeda, boleto.Cedente.Codigo.Substring(0, 5));
            digitoMod = Mod10(campo1);
            campo1 += digitoMod.ToString();
            campo1 = campo1.Substring(0, 5) + "." + campo1.Substring(5, 5);

            /*
            Campo 2
            Código do Cedente – 7 Posições
            Nosso número – 3 posições
            Dígito Verificador Módulo 10 (Campo 2)
             */
            campo2 = String.Concat(boleto.Cedente.Codigo.Substring(5, 7), boleto.NossoNumero.Substring(0, 3));
            digitoMod = Mod10(campo2);
            campo2 += digitoMod.ToString();
            campo2 = campo2.Substring(0, 5) + "." + campo2.Substring(5, 6);

            /*
            Campo 3
            Nosso número – 7 posições
            Código da Carteira
            Identificador de Layout
            Dígito Verificador Módulo 10 (Campo 3)
             */
            campo3 = String.Concat(boleto.NossoNumero.Substring(3, 7), boleto.Carteira.PadLeft(2, '0'), boleto.TipoModalidade);
            digitoMod = Mod10(campo3);
            campo3 += digitoMod;
            campo3 = campo3.Substring(0, 5) + "." + campo3.Substring(5, 6);

            /*
            Campo 4
            Dígito Verificador Geral
             */
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do Título (*)
             */
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);
            campo5 = String.Concat(FatorVencimento(boleto).ToString(), valorBoleto);

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        #endregion Métodos de formatação do boleto

        #region Métodos de geração do arquivo remessa - Genéricos


        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            string vMsg = string.Empty;
            mensagem = vMsg;
            return true;
        }

        // 400
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string _ContaComDigito = boleto.Cedente.ContaBancaria.Conta.Trim() + boleto.Cedente.ContaBancaria.DigitoConta.Trim();

                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));                                       // 001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, "02", '0'));                                      // 002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    // 004-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 012, 0, _ContaComDigito, '0'));                           // 018-029
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0030, 008, 0, string.Empty, ' '));                              // 030-037
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, boleto.NumeroDocumento, ' '));                    // 038-062
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 011, 0, boleto.NossoNumero, '0'));                        // 063-073
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0074, 001, 0, Mod11C6(boleto.NossoNumero, 7), '0'));            // 074-074
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0075, 008, 0, string.Empty, ' '));                              // 075-082
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0083, 003, 0, "336", '0'));                                     // 083-085
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0086, 021, 0, string.Empty, ' '));                              // 086-106
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, boleto.Carteira, '0'));                           // 107-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0109, 002, 0, "01", '0'));                                      // 109-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    // 111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     // 121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        // 127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 008, 0, string.Empty, ' '));                              // 140-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0148, 002, 0, "01", '0'));                                      // 148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             // 150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));                      // 151-156
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, "00", '0'));                   // 157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, "00", '0'));                   // 159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));                          // 161-173
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0174, 006, 0, boleto.DataDesconto, ' '));                       // 174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      // 180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0193, 006, 0, boleto.DataMulta, ' '));                          // 193-198
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0199, 007, 0, string.Empty, ' '));                              // 199-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, 0, '0'));                    // 206-218
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, boleto.Sacado.CPFCNPJ.Length <= 11 ? 1 : 2, '0'));           // 219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     // 221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              // 235-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.EndComNumero.ToUpper(), ' ')); // 275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   // 315-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                // 327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   // 335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF, ' '));                 // 350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 030, 0, "", ' '));                      // 352-381

                // D079 - Indicador de Multa 0 = Sem Multa 2 = Percentual
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0382, 001, 0, boleto.PercMulta > 0 ? "2" : "0", ' '));         // 382-382                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0383, 002, 0, boleto.PercMulta, '0'));                          // 383-384
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0385, 001, 0, string.Empty, ' '));                              // 385-385
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0386, 006, 0, boleto.DataJurosMora, ' '));                      // 386-391

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0392, 002, 0, 0, '0'));                       // 392-393

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, string.Empty, ' '));                               // 394-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            // 395-400
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do arquivo de remessa do CNAB400.", ex);
            }
        }

        // 400
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal) {
            try {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "9", '0'));            // 001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   // 002-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); // 395-400
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro) {
            throw new NotImplementedException();
        }

        // 400
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {
            try {

                string _ContaComDigito = cedente.ContaBancaria.Conta.Trim() + cedente.ContaBancaria.DigitoConta.Trim();

                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));                          // 001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));                          // 002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                    // 003-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                         // 010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 008, 0, "COBRANCA", ' '));                   // 012-019
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 007, 0, string.Empty, ' '));                 // 020-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 012, 0, cedente.Codigo, '0'));               // 027-038
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0039, 008, 0, string.Empty, ' '));                 // 039-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));       // 047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 003, 0, "336", '0'));                        // 077-079
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, string.Empty, ' '));                 // 080-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                 // 095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 008, 0, string.Empty, ' '));                 // 101-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0109, 012, 0, _ContaComDigito, '0'));              // 109-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0121, 266, 0, string.Empty, ' '));                 // 121-386
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0387, 008, 0, numeroArquivoRemessa, '0'));         // 387-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, "000001", '0'));                     // 395-400
                reg.CodificarLinha();
                return reg.LinhaRegistro;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos) {
            throw new NotImplementedException();
        }

        #endregion Métodos de geração do arquivo remessa - Genéricos

        #region Retorno 400
        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro) {
            try {
                return new HeaderRetorno(registro) {
                    TipoRegistro = Utils.ToInt32(registro.Substring(0, 1)),
                    CodigoRetorno = Utils.ToInt32(registro.Substring(1, 1)),
                    LiteralRetorno = registro.Substring(002, 7),
                    CodigoServico = Utils.ToInt32(registro.Substring(009, 2)),
                    LiteralServico = registro.Substring(011, 8),

                    CodigoEmpresa = registro.Substring(026, 12),
                    Conta = Utils.ToInt32(registro.Substring(038, 12)),
                    DACConta = Utils.ToInt32(registro.Substring(37, 1)),
                    NomeEmpresa = registro.Substring(046, 30),
                    CodigoBanco = Utils.ToInt32(registro.Substring(076, 3)),
                    NomeBanco = registro.Substring(079, 15),
                    DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##")),
                    Densidade = Utils.ToInt32(registro.Substring(100, 8)),
                    NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(108, 5)),
                    ComplementoRegistro2 = registro.Substring(113, 266),
                    DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(379, 6)).ToString("##-##-##")),
                    ComplementoRegistro3 = registro.Substring(385, 9),
                    NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6))
                };
            } catch (Exception ex) {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro) {
            try {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                // Identificação do registro
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));
                // Tipo de inscrição da empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                // Número de inscrição da empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                
                // Código da empresa
                detalhe.Conta = Utils.ToInt32(registro.Substring(17, 11));
                // DAC da Conta
                detalhe.DACConta = Utils.ToInt32(registro.Substring(28, 1));

                // Nosso número
                detalhe.NossoNumero = registro.Substring(62, 10);
                detalhe.DACNossoNumero = registro.Substring(72, 1);
                detalhe.NossoNumeroComDV = detalhe.NossoNumero + detalhe.DACNossoNumero;
                // Carteira
                detalhe.Carteira = registro.Substring(106, 2);
                
                // Código de ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                // Data de ocorrência
                detalhe.DataOcorrencia = Utils.ToDateTime(Utils.ToInt32(registro.Substring(110, 6)).ToString("##-##-##"));
                // Identificação do título no banco
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                // Identificação do título no banco
                detalhe.IdentificacaoTitulo = registro.Substring(126, 20);
                // Data de vencimento
                detalhe.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(146, 6)).ToString("##-##-##"));
                // Valor do título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                // Banco cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                // Agência cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                // Espécie do título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                // Despesas de cobrança para os códigos de ocorrência
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.ValorDespesa = valorDespesa / 100;
                // Abatimento concedido
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.Abatimentos = valorAbatimento / 100;
                // Desconto concedido
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;
                // Valor pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                // Juros operação em atraso
                decimal valorJuros = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.Juros = valorJuros / 100;

                //// Juros de mora
                //decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                //detalhe.JurosMora = jurosMora / 100;

                //// Outras despesas
                //decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                //detalhe.OutrasDespesas = valorOutrasDespesas / 100;


                //// IOF devido
                //decimal valorIOF = Convert.ToUInt64(registro.Substring(214, 13));
                //detalhe.IOF = valorIOF / 100;

                // 137 Valor Outros Acréscimos 
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                // 140 Data do crédito
                detalhe.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(295, 6)).ToString("##-##-##"));
                // 143 Código de Erro
                detalhe.Erros = registro.Substring(377, 16);
                // 145 Seqüencial
                detalhe.Sequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }
        #endregion

        #region Util
        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo) {
            switch (codigo) {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação normal";
                case "09":
                    return "09-Baixado Automaticamente via Arquivo";
                case "10":
                    return "10-Baixado conforme instruções da Agência";
                case "11":
                    return "11-Em Ser - Arquivo de Títulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Liquidação em Cartório";
                case "17":
                    return "17-Liquidação após baixa ou Título não registrado";
                case "18":
                    return "18-Acerto de Depositária";
                case "19":
                    return "19-Confirmação Recebimento Instrução de Protesto";
                case "20":
                    return "20-Confirmação Recebimento Instrução Sustação de Protesto";
                case "21":
                    return "21-Acerto do Controle do Participante";
                case "23":
                    return "23-Entrada do Título em Cartório";
                case "24":
                    return "24-Entrada rejeitada por CEP Irregular";
                case "27":
                    return "27-Baixa Rejeitada";
                case "28":
                    return "28-Débito de tarifas/custas";
                case "30":
                    return "30-Alteração de Outros Dados Rejeitados";
                case "32":
                    return "32-Instrução Rejeitada";
                case "33":
                    return "33-Confirmação Pedido Alteração Outros Dados";
                case "34":
                    return "34-Retirado de Cartório e Manutenção Carteira";
                case "35":
                    return "35-Desagendamento ) débito automático";
                case "68":
                    return "68-Acerto dos dados ) rateio de Crédito";
                case "69":
                    return "69-Cancelamento dos dados ) rateio";
                default:
                    return "";
            }
        }

        /// <summary>
        /// O cálculo do dígito verificador deve tomar como base 2 dígitos da carteira concatenado ao nosso número com 10 posições
        /// concatenado a um zero a esquerda(0CCNNNNNNNNNN) aplicando a mesma forma de cálculo utilizada pelo Bradesco
        /// (módulo 11 base 7). Mais detalhes sobre como realizar esse cálculo
        /// </summary>
        /// <param name="Sequencial">0000000004</param>
        /// <param name="Base">7</param>
        /// <returns></returns>
        private string Mod11C6(string Sequencial, int Base) {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o cálculo do dígito, será necessário acrescentar o número da carteira à esquerda antes do Nosso Número, 
            e aplicar o módulo 11, com base 7.
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 2, o segundo por 3 e assim sucessivamente.          
             
            Fixo  Carteira   Nosso Numero
            ____    ______   _____________________________________
               0    2    0   0   0   0   0   0   0   0   0   0   4
               x    x    x   x   x   x   x   x   x   x   x   x   x  
               2    7    6   5   4   3   2   7   6   5   4   3   2
               =    =    =   =   =   =   =   =   =   =   =   =   =  
               0    14 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 8 = 22

            O total da soma deverá ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferença entre o divisor e o resto, será o dígito de autoconferência: 11 - 3 = 8 (dígito de auto-conferência)
            
            Se o resto da divisão for “1”, desprezar o cálculo de subtração e considerar o dígito como “P”. 
            Se o resto da divisão for “0”, desprezar o cálculo de subtração e considerar o dígito como “0”.
            */
            #endregion

            string _Fixo = "0";
            string _Carteira = "20";

            string _Linha = _Fixo + _Carteira + Sequencial;

            int _Soma = 0, _Peso = 2;

            for (int i = _Linha.Length; i > 0; i--) {
                _Soma = _Soma + (Convert.ToInt32(_Linha.Mid(i, 1)) * _Peso);
                if (_Peso == Base)
                    _Peso = 2;
                else
                    _Peso = _Peso + 1;
            }

            int Resto = (_Soma % 11);

            if (Resto == 0)
                return "0";
            else if (Resto == 1)
                return "P";
            else
                return (11 - Resto).ToString();
        }
        #endregion
    }
}