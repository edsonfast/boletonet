using BoletoNet.Util;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoletoNet {
    /// <author>  
    /// Iago Rayner Moura (iago.rmoura@hotmail.com)
    /// </author>  
    internal class Banco_Mercantil : AbstractBanco, IBanco {
        private int _dacBoleto = 0;
        private int _dacNossoNumero = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Mercantil.
        /// </summary>
        internal Banco_Mercantil() {
            this.Codigo = 389;
            this.Nome = "Mercantil";
        }

        /// <summary>
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda (9-Real)
        ///    05 a 05 – 1 - Dígito verificador geral do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor do documento
        ///    20 a 44 – 25 - Campo Livre
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto) {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
            FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Util.Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Util.Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, agência do beneficiário, primeiro byte do nosso número e dígito verificador (calculado MOD.10);
        ///      2º campo
        ///          composto pelo nosso número a partir do segundo byte inclusive e dígito verificador (calculado MOD.10);
        ///      3º campo
        ///          composto pelo número do contrato, indicador de desconto (constante e igual a 2) e dígito verificador (calculado MOD.10);
        ///      4º campo
        ///          composto pelo DAC - dígito de auto conferência (calculado MOD.11);
        ///      5º campo
        ///          Composto pelo fator de vencimento e valor nominal do título.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto) {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV

            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
            string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
            string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
            string D1 = Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);

            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            string D2 = Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            string D3 = Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);

            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = _dacBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            string FFFF = FatorVencimento(boleto).ToString();

            string VVVVVVVVVV = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            VVVVVVVVVV = Utils.FormatCode(VVVVVVVVVV, 10);

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Beneficiária (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    24 a 34 - 11 - Nosso Número (Sem o digito verificador)
        ///    35 a 43 -  7 - Código do Beneficiário (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Indicador de Desconto (2 = Sem Desconto, 0 = Com Desconto)            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto) {
            string codCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 9);

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}", boleto.Cedente.ContaBancaria.Agencia, boleto.NossoNumero, codCedente.Substring(0, 9), "2");

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto) {
            throw new NotImplementedException("Função ainda não implementada.");
        }


        public override void FormataNossoNumero(Boleto boleto) {
            boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero.Substring(0, 10), boleto.NossoNumero.Substring(10, 1));
        }

        public override void ValidaBoleto(Boleto boleto) {
            if (boleto.ValorBoleto == 0)
                throw new NotImplementedException("O valor do boleto não pode ser igual a zero");

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
                throw new NotImplementedException("A quantidade de dígitos do nosso número, são 11 números.");
            else if (boleto.NossoNumero.Length < 6)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 6);
            if (boleto.NossoNumero.Length == 6) {
                string nn = string.Format("05{0}{1}", boleto.Carteira, boleto.NossoNumero);
                boleto.NossoNumero = string.Format("{0}{1}", nn, Mod11Mercantil(string.Format("{0}{1}", boleto.Cedente.ContaBancaria.Agencia, nn), 9));
            }

            // Calcula o DAC do Nosso Número a maioria das carteiras
            // agencia/conta/carteira/nosso numero
            if (boleto.Carteira == "112")
                _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.Carteira + boleto.NossoNumero);
            else if (boleto.Carteira != "126" && boleto.Carteira != "131"
                && boleto.Carteira != "146" && boleto.Carteira != "150"
                && boleto.Carteira != "168")
                _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
            else
                // Excessão 126 - 131 - 146 - 150 - 168
                // carteira/nosso numero
                _dacNossoNumero = Mod10(boleto.Carteira + boleto.NossoNumero);

            boleto.DigitoNossoNumero = _dacNossoNumero.ToString();

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4) {
                string agenciaDesconfigurada = boleto.Cedente.ContaBancaria.Agencia;
                boleto.Cedente.ContaBancaria.Agencia = agenciaDesconfigurada.Substring(0, agenciaDesconfigurada.Length - 1);
                boleto.Cedente.ContaBancaria.DigitoAgencia = agenciaDesconfigurada.Substring(agenciaDesconfigurada.Length - 1, 1);
            } else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 9) {
                string conta = boleto.Cedente.ContaBancaria.Conta;

                boleto.Cedente.ContaBancaria.Conta = conta.Substring(0, 9);
                boleto.Cedente.ContaBancaria.DigitoConta = conta.Substring(conta.Length - 1, 1);
            } else if (boleto.Cedente.ContaBancaria.Conta.Length < 9) {
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 9);
            }

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento é valida
            if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            //FormataCodAgenciaConta(boleto);
        }

        //private void FormataCodAgenciaConta(Boleto boleto)
        //{
        //    boleto.AgenciaCodCedente = string.Format("{0}/{1}{2}",
        //       boleto.Cedente.ContaBancaria.Agencia,
        //       Utils.FormatCode(boleto.Cedente.Codigo.Substring(0, boleto.Cedente.Codigo.Length - 1), 8),
        //       boleto.Cedente.Codigo.Substring(boleto.Cedente.Codigo.Length - 1));
        //}
        private string Mod11Mercantil(string seq, int b) {
            /* Variáveis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--) {
                s = s + (Convert.ToInt32(seq.Mid(i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro) {
            try {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(21, 7));
                detalhe.NumeroControle = registro.Substring(37, 25);
                detalhe.NossoNumero = registro.Substring(66, 11);
                detalhe.DACNossoNumero = registro.Substring(76, 1);
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                detalhe.DataOcorrencia = new DateTime(Int32.Parse(registro.Substring(114, 2)) + 2000,
                                                    Int32.Parse(registro.Substring(112, 2)),
                                                    Int32.Parse(registro.Substring(110, 2)), 0, 0, 0);
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                double valorTitulo = Convert.ToInt64(registro.Substring(152, 11));
                detalhe.ValorTitulo = Convert.ToInt64(valorTitulo / 100);
                detalhe.BancoCobrador = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                double tarifaCobranca = Convert.ToInt64(registro.Substring(175, 11));
                detalhe.TarifaCobranca = Convert.ToInt64(tarifaCobranca / 100);
                double outrasDespesas = Convert.ToInt64(registro.Substring(188, 11));
                detalhe.OutrasDespesas = Convert.ToInt64(outrasDespesas / 100);
                double juros = Convert.ToInt64(registro.Substring(201, 11));
                detalhe.Juros = Convert.ToInt64(juros / 100);
                double IOF = Convert.ToInt64(registro.Substring(214, 11));
                detalhe.IOF = Convert.ToInt64(IOF / 100);
                double valorAbatimento = Convert.ToInt64(registro.Substring(227, 11));
                detalhe.ValorAbatimento = Convert.ToInt64(valorAbatimento / 100);
                double descontos = Convert.ToInt64(registro.Substring(240, 11));
                detalhe.Descontos = Convert.ToInt64(descontos / 100);
                double valorPago = Convert.ToInt64(registro.Substring(253, 13));
                detalhe.ValorPago = Convert.ToInt64(valorPago / 100);
                detalhe.ValorPrincipal = detalhe.ValorPago;
                double jurosMora = Convert.ToInt64(registro.Substring(266, 11));
                detalhe.JurosMora = Convert.ToInt64(jurosMora / 100);
                double outrosCreditos = Convert.ToInt64(registro.Substring(279, 11));
                detalhe.OutrosCreditos = Convert.ToInt64(outrosCreditos / 100);
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito);
                detalhe.Instrucao = Utils.ToInt32(registro.Substring(333, 2));
                detalhe.MotivosRejeicao = registro.Substring(377, 10);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.NomeSacado = "";

                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        //public DetalheRetornoCNAB120 LerDetalheRetornoCNA120(string registro)
        //{
        //    throw new NotImplementedException();
        //}


        #region Geração de Arquivo de Remessa CNAB 400
        //public string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cendente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Gera o header do arquivo de remessa CNAB 400
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos) {
            try {
                string header = string.Empty;

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB400:
                        header = GerarHeaderRemessaCNAB400(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    default:
                        throw new Exception("Tipo de arquivo não suportado");
                }

                return header;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar header da remessa", ex);
            }
        }

        /// <summary>
        /// Gera o header do arquivo de remessa CNAB 400
        /// </summary>
        private string GerarHeaderRemessaCNAB400(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa) {
            try {
                string header = string.Empty;

                // Posição 001-001: Código do registro
                header += "0";

                // Posição 002-002: Código de remessa
                header += "1";

                // Posição 003-009: Literal "REMESSA"
                header += Utils.FormatCode("REMESSA", 7);

                // Posição 010-011: Código do serviço
                header += "01";

                // Posição 012-019: Literal "COBRANCA"
                header += Utils.FormatCode("COBRANCA", 8);

                // Posição 020-026: Filler
                header += new string(' ', 7);

                // Posição 027-030: Agência de origem
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, 4);

                // Posição 031-045: Número de inscrição do cedente (CNPJ/CPF)
                string inscricao = cedente.CPFCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                header += Utils.FormatCode(inscricao, 15);

                // Posição 046-046: Filler
                header += " ";

                // Posição 047-076: Nome do cedente
                header += Utils.FormatCode(cedente.Nome.ToUpper(), 30);

                // Posição 077-079: Código do banco
                header += "389";

                // Posição 080-094: Nome do banco
                header += Utils.FormatCode("BANCANTIL", 15);

                // Posição 095-100: Data de gravação (DDMMAA)
                header += DateTime.Now.ToString("ddMMyy");

                // Posição 101-381: Filler
                header += new string(' ', 281);

                // Posição 382-386: Densidade de gravação
                header += "01600";

                // Posição 387-389: BPI de gravação
                header += "   ";

                // Posição 390-394: Número sequencial do arquivo
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), 5);

                // Posição 395-400: Número sequencial do registro
                header += "000001";

                return header;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar header CNAB400", ex);
            }
        }

        /// <summary>
        /// Gera o detalhe do arquivo de remessa CNAB 400
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                string detalhe = string.Empty;

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB400:
                        detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro);
                        break;
                    default:
                        throw new Exception("Tipo de arquivo não suportado");
                }

                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar detalhe da remessa", ex);
            }
        }

        /// <summary>
        /// Gera o detalhe do arquivo de remessa CNAB 400
        /// </summary>
        private string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro) {
            try {
                string detalhe = string.Empty;

                // Posição 001-001: Código do registro
                detalhe += "1";

                // Posição 002-003: Indicador de multa
                detalhe += boleto.PercMulta > 0 ? "09" : "00";

                // Posição 004-004: Código da multa
                detalhe += boleto.PercMulta > 0 ? "2" : "0";

                // Posição 005-017: Valor/Percentual da multa
                if (boleto.PercMulta > 0) {
                    detalhe += Utils.FormatCode((boleto.PercMulta * 100).ToString("f2").Replace(",", "").Replace(".", ""), 13);
                } else {
                    detalhe += new string('0', 13);
                }

                // Posição 018-023: Data da multa
                if (boleto.PercMulta > 0) {
                    detalhe += boleto.DataVencimento.AddDays(1).ToString("ddMMyy");
                } else {
                    detalhe += new string('0', 6);
                }

                // Posição 024-028: Filler
                detalhe += new string(' ', 5);

                // Posição 029-037: Número do contrato
                detalhe += Utils.FormatCode(boleto.Cedente.Convenio.ToString(), 9);

                // Posição 038-062: Uso da empresa
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, 25);

                // Posição 063-066: Agência de origem
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

                // Posição 067-076: Nosso número
                string nossoNumero = boleto.NossoNumero.Replace("-", "");
                if (nossoNumero.Length > 10)
                    nossoNumero = nossoNumero.Substring(0, 10);
                detalhe += Utils.FormatCode(nossoNumero, 10);

                // Posição 077-077: Dígito verificador do nosso número
                detalhe += boleto.DigitoNossoNumero; // CalcularDigitoNossoNumero(nossoNumero);

                // Posição 078-082: Filler
                detalhe += new string(' ', 5);

                // Posição 083-097: CNPJ/CPF do cedente
                string inscricaoCedente = boleto.Cedente.CPFCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                detalhe += Utils.FormatCode(inscricaoCedente, 15);

                // Posição 098-107: Quantidade de moeda
                detalhe += new string('0', 10);

                // Posição 108-108: Código da operação (carteira)
                detalhe += boleto.Carteira;

                // Posição 109-110: Código do movimento
                detalhe += ObterCodigoMovimento(boleto);

                // Posição 111-120: Seu número
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, 10);

                // Posição 121-126: Data de vencimento
                detalhe += boleto.DataVencimento.ToString("ddMMyy");

                // Posição 127-139: Valor do título
                detalhe += Utils.FormatCode(boleto.ValorBoleto.ToString("f2").Replace(",", "").Replace(".", ""), 13);

                // Posição 140-142: Banco cobrador
                detalhe += "389";

                // Posição 143-147: Agência cobradora
                detalhe += new string('0', 5);

                // Posição 148-149: Código da espécie
                detalhe += "01"; //01=DM //ObterCodigoEspecie(boleto.EspecieDocumento);

                // Posição 150-150: Aceite
                detalhe += boleto.Aceite == "S" ? "A" : "N";

                // Posição 151-156: Data de emissão
                detalhe += boleto.DataDocumento.ToString("ddMMyy");

                // Posição 157-158: Primeira instrução
                detalhe += ObterInstrucao1(boleto);

                // Posição 159-160: Segunda instrução
                detalhe += ObterInstrucao2(boleto);

                // Posição 161-173: Juros de mora por dia
                if (boleto.JurosMora > 0) {
                    detalhe += Utils.FormatCode(boleto.JurosMora.ToString("f2").Replace(",", "").Replace(".", ""), 13);
                } else {
                    detalhe += new string('0', 13);
                }

                // Posição 174-179: Data limite para desconto
                if (boleto.ValorDesconto > 0 && boleto.DataDesconto != DateTime.MinValue) {
                    detalhe += boleto.DataDesconto.ToString("ddMMyy");
                } else {
                    detalhe += new string('0', 6);
                }

                // Posição 180-192: Valor do desconto
                if (boleto.ValorDesconto > 0) {
                    detalhe += Utils.FormatCode(boleto.ValorDesconto.ToString("f2").Replace(",", "").Replace(".", ""), 13);
                } else {
                    detalhe += new string('0', 13);
                }

                // Posição 193-205: Valor do IOF
                detalhe += new string('0', 13);

                // Posição 206-218: Valor do abatimento
                //if (boleto.ValorAbatimento > 0) {
                //    detalhe += Utils.FormatCode(boleto.ValorAbatimento.ToString("f2").Replace(",", "").Replace(".", ""), 13);
                //} else {
                detalhe += new string('0', 13);
                //}

                // Posição 219-220: Código de inscrição do sacado
                string tipoInscricaoSacado = boleto.Sacado.CPFCNPJ.Replace(".", "").Replace("/", "").Replace("-", "").Length == 14 ? "02" : "01";
                detalhe += tipoInscricaoSacado;

                // Posição 221-234: CNPJ/CPF do sacado
                string inscricaoSacado = boleto.Sacado.CPFCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                detalhe += Utils.FormatCode(inscricaoSacado, 14);

                // Posição 235-274: Nome do sacado
                detalhe += Utils.FormatCode(boleto.Sacado.Nome.ToUpper(), 40);

                // Posição 275-314: Endereço do sacado
                string endereco = $"{boleto.Sacado.Endereco.End} {boleto.Sacado.Endereco.Numero}";
                if (!string.IsNullOrEmpty(boleto.Sacado.Endereco.Complemento))
                    endereco += $" {boleto.Sacado.Endereco.Complemento}";
                detalhe += Utils.FormatCode(endereco.ToUpper(), 40);

                // Posição 315-326: Bairro do sacado
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Bairro.ToUpper(), 12);

                // Posição 327-331: Prefixo do CEP
                string cep = boleto.Sacado.Endereco.CEP.Replace("-", "");
                if (cep.Length >= 5) {
                    detalhe += cep.Substring(0, 5);
                } else {
                    detalhe += Utils.FormatCode(cep, 5);
                }

                // Posição 332-334: Sufixo do CEP
                if (cep.Length >= 8) {
                    detalhe += cep.Substring(5, 3);
                } else {
                    detalhe += new string('0', 3);
                }

                // Posição 335-349: Cidade do sacado
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.Cidade.ToUpper(), 15);

                // Posição 350-351: Estado do sacado
                detalhe += Utils.FormatCode(boleto.Sacado.Endereco.UF.ToUpper(), 2);

                // Posição 352-381: Sacador/Avalista
                if (boleto.Avalista != null && !string.IsNullOrEmpty(boleto.Avalista.Nome)) {
                    detalhe += Utils.FormatCode(boleto.Avalista.Nome.ToUpper(), 30);
                } else {
                    detalhe += new string(' ', 30);
                }

                // Posição 382-393: Filler
                detalhe += new string(' ', 12);

                // Posição 394-394: Código da moeda
                detalhe += "1";

                // Posição 395-400: Número sequencial do registro
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), 6);

                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar detalhe CNAB400", ex);
            }
        }

        /// <summary>
        /// Gera o trailer do arquivo de remessa CNAB 400
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal) {
            try {
                string trailer = string.Empty;

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB400:
                        trailer = GerarTrailerRemessaCNAB400(numeroRegistro);
                        break;
                    default:
                        throw new Exception("Tipo de arquivo não suportado");
                }

                return trailer;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar trailer da remessa", ex);
            }
        }

        /// <summary>
        /// Gera o trailer do arquivo de remessa CNAB 400
        /// </summary>
        private string GerarTrailerRemessaCNAB400(int numeroRegistro) {
            try {
                string trailer = string.Empty;

                // Posição 001-001: Código do registro
                trailer += "9";

                // Posição 002-394: Filler
                trailer += new string(' ', 393);

                // Posição 395-400: Número sequencial do registro
                trailer += Utils.FormatCode(numeroRegistro.ToString(), 6);

                return trailer;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar trailer CNAB400", ex);
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Obtém o código de movimento baseado no tipo de operação
        /// </summary>
        private string ObterCodigoMovimento(Boleto boleto) {
            // Por padrão, entrada de títulos
            return "01";
        }

        /// <summary>
        /// Obtém o código da espécie do documento
        /// </summary>
        //private string ObterCodigoEspecie(EspecieDocumento especie) {
        //    switch (especie) {
        //        case EspecieDocumento.DM:
        //            return "01"; // Duplicata Mercantil
        //        case EspecieDocumento.NP:
        //            return "02"; // Nota Promissória
        //        case EspecieDocumento.RC:
        //            return "03"; // Recibo
        //        case EspecieDocumento.NS:
        //            return "05"; // Nota de Seguro
        //        case EspecieDocumento.DS:
        //            return "06"; // Duplicata de Prestação de Serviços
        //        default:
        //            return "07"; // Outros Documentos
        //    }
        //}

        /// <summary>
        /// Obtém a primeira instrução
        /// </summary>
        private string ObterInstrucao1(Boleto boleto) {
            // Verifica se deve protestar

            if (boleto.Instrucoes[0] != null) {
                if (boleto.Instrucoes[0].QuantidadeDias > 0) {
                    if (boleto.Instrucoes[0].QuantidadeDias <= 3)
                        return "19"; // Protestar em 03 dias úteis
                    else if (boleto.Instrucoes[0].QuantidadeDias <= 5)
                        return "20"; // Protestar em 05 dias úteis
                    else if (boleto.Instrucoes[0].QuantidadeDias <= 10)
                        return "21"; // Protestar em 10 dias úteis
                    else if (boleto.Instrucoes[0].QuantidadeDias <= 15)
                        return "22"; // Protestar em 15 dias úteis
                    else
                        return "55"; // Protestar em 30 dias úteis
                }
            }

            // Verifica se deve devolver após vencimento
            //if (boleto.DiasDevolver > 0) {
            //    if (boleto.DiasDevolver <= 5)
            //        return "23"; // Devolver após 05 dias
            //    else if (boleto.DiasDevolver <= 15)
            //        return "24"; // Devolver após 15 dias
            //    else
            //        return "25"; // Devolver após 30 dias
            //}

            return "27"; // Não protestar
        }

        /// <summary>
        /// Obtém a segunda instrução
        /// </summary>
        private string ObterInstrucao2(Boleto boleto) {
            if (boleto.JurosMora > 0)
                return "26"; // Não dispensar juros de mora
            else
                return "11"; // Dispensar juros de mora
        }

        /// <summary>
        /// Calcula o dígito verificador do nosso número
        /// </summary>
        /// <param name="nossoNumero">Nosso número sem o dígito verificador</param>
        /// <returns>Dígito verificador</returns>
        public string CalcularDigitoNossoNumero(string nossoNumero) {
            try {
                // Remove caracteres não numéricos
                string numero = Utils.FormatCode(nossoNumero, 10);

                // Sequência de multiplicadores (módulo 11)
                int[] multiplicadores = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3 };

                int soma = 0;
                for (int i = 0; i < numero.Length && i < multiplicadores.Length; i++) {
                    soma += int.Parse(numero[i].ToString()) * multiplicadores[i];
                }

                int resto = soma % 11;
                int dv = resto < 2 ? 0 : 11 - resto;

                return dv.ToString();
            } catch (Exception ex) {
                throw new Exception("Erro ao calcular dígito verificador do nosso número", ex);
            }
        }
        #endregion
    }
}
