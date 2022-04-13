using BoletoNet.EDI.Banco;
using BoletoNet.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.136.jpg", "image/jpg")]
namespace BoletoNet {
    /// <Author>
    /// Ivan Teles (ivan@idevweb.com.br)- Unicred
    /// </Author>
    internal class Banco_Unicred : AbstractBanco, IBanco {
        private HeaderRetorno header;

        /// <author>
        /// Classe responsavel em criar os campos do Banco Unicred.
        /// </author>
        internal Banco_Unicred() {
            this.Codigo = 136;
            this.Digito = "8";
            this.Nome = "Banco Unicred";
        }

        public override void ValidaBoleto(Boleto boleto) {
            //Formata o tamanho do numero da agencia
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Formata o tamanho do numero da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 5)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

            //Atribui o nome do banco ao local de pagamento

            boleto.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA/CORRESPONDENTE BANCÁRIO";

            //Verifica se data do processamento eh valida
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento Ã© valida
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            string infoFormatoCodigoCedente = "formato AAAAPPCCCCC, onde: AAAA = Numero da agencia, PP = Posto do beneficiario, CCCCC = Codigo do beneficiario";


            var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 11);

            if (string.IsNullOrEmpty(codigoCedente))
                throw new BoletoNetException("Codigo do cedente deve ser informado, " + infoFormatoCodigoCedente);

            var conta = boleto.Cedente.ContaBancaria.Conta;
            if (boleto.Cedente.ContaBancaria != null &&
                (!codigoCedente.StartsWith(boleto.Cedente.ContaBancaria.Agencia) ||
                 !(codigoCedente.EndsWith(conta) || codigoCedente.EndsWith(conta.Substring(0, conta.Length - 1)))))

                //throw new BoletoNetException("Codigo do cedente deve estar no " + infoFormatoCodigoCedente);
                boleto.Cedente.Codigo = string.Format("{0}{1}{2}", boleto.Cedente.ContaBancaria.Agencia, boleto.Cedente.ContaBancaria.OperacaConta, boleto.Cedente.Codigo);


            //Verifica se o nosso numero eh valido
            var Length_NN = boleto.NossoNumero.Length;
            if (Length_NN > 10) throw new NotImplementedException("Nosso numero invalido maior que 10");
            if (Length_NN < 10) {
                boleto.NossoNumero = boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
            }

            FormataCodigoBarra(boleto);
            //if (boleto.CodigoBarra.Codigo.Length != 44)
            //    throw new BoletoNetException("Codigo de barras eh invalido");


            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        public override void FormataNossoNumero(Boleto boleto) {
            string nossoNumero = boleto.NossoNumero;

            if (nossoNumero == null || nossoNumero.Length != 10) {

                throw new Exception("Erro ao tentar formatar nosso numero, verifique o tamanho do campo: " + nossoNumero.Length);

            }

            try {
                boleto.NossoNumero = string.Format("{0}-{1}", nossoNumero, Mod11UniCred(nossoNumero, false));
            } catch (Exception ex) {

                throw new Exception("Erro ao formatar nosso numero", ex);
            }


        }

        public override void FormataNumeroDocumento(Boleto boleto) {
            throw new BoletoNetException("Nao implantado");
        }
        public override void FormataLinhaDigitavel(Boleto boleto) {
            string campo1 = "1369" + boleto.CodigoBarra.Codigo.Substring(19, 5);
            int d1 = Mod10Unicred(campo1);
            campo1 = FormataCampoLD(campo1) + d1.ToString();

            string campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            int d2 = Mod10Unicred(campo2);
            campo2 = FormataCampoLD(campo2) + d2.ToString();

            string campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            int d3 = Mod10Unicred(campo3);
            campo3 = FormataCampoLD(campo3) + d3.ToString();

            string campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            string campo5 = boleto.CodigoBarra.Codigo.Substring(5, 14);

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }
        private string FormataCampoLD(string campo) {
            return string.Format("{0}.{1}", campo.Substring(0, 5), campo.Substring(5));
        }

        public override void FormataCodigoBarra(Boleto boleto) {
            string valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            var nossoNumero = string.Format("{0}{1}", boleto.NossoNumero, Mod11UniCred(boleto.NossoNumero, true));
            string cmp_livre = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4) +
                                                Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta, 10) +
                                                Utils.FormatCode(nossoNumero, 11);

            //string dv_cmpLivre = DigUnicred(cmp_livre).ToString();

            var codigoTemp = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, string.Empty);

            boleto.CodigoBarra.CampoLivre = cmp_livre;
            boleto.CodigoBarra.FatorVencimento = FatorVencimento(boleto);
            boleto.CodigoBarra.Moeda = 9;
            boleto.CodigoBarra.ValorDocumento = valorBoleto;

            int _dacBoleto = DigUnicred(codigoTemp);

            if (_dacBoleto == 0 || _dacBoleto > 9)
                _dacBoleto = 1;

            //Estava gerando com 46 digitos ao invés de 44, então tirei o dv_cmpLivre para corrigir.
            //boleto.CodigoBarra.Codigo = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, dv_cmpLivre, _dacBoleto);
            boleto.CodigoBarra.Codigo = GerarCodigoDeBarras(boleto, valorBoleto, cmp_livre, string.Empty, _dacBoleto);
        }

        private string GerarCodigoDeBarras(Boleto boleto, string valorBoleto, string cmp_livre, string dv_cmpLivre, int? dv_geral = null) {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}",
                Utils.FormatCode(Codigo.ToString(), 3),
                boleto.Moeda,
                dv_geral.HasValue ? dv_geral.Value.ToString() : string.Empty,
                FatorVencimento(boleto),
                valorBoleto,
                cmp_livre,
                dv_cmpLivre);
        }

        #region REMESSA

        #region HEADER
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos) {
            throw new BoletoNetException("Nao implantado");
        }
        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {

            throw new BoletoNetException("Nao implantado");
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa) {
            try {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo) {

                    case TipoArquivo.CNAB240:
                        //_header = GerarHeaderRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo) {
            throw new BoletoNetException("Nao implantado");
        }
        #endregion

        #region DETALHE
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240();
                        break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #region TRAILER
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal) {
            try {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo) {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            } catch (Exception ex) {
                throw new Exception("", ex);
            }
        }
        #endregion

        #region REMESSA 400

        #region HEADER
        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa) {
            try {
                string complemento = new string(' ', 277);
                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += Utils.FitStringLength(cedente.Codigo.ToString(), 20, 20, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "136";
                _header += "UNICRED        ";
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "       ";
                _header += "000";
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);
                _header += complemento;
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            try {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                // USO DO BANCO - Identificação da operação no Banco (posição 87 a 107)
                string identificaOperacaoBanco = new string(' ', 10);
                string nrDeControle = Utils.FitStringLength(boleto.NumeroDocumento.TrimStart(' '), 25, 25, ' ', 0, true, true, false);
                string usoBanco = new string(' ', 10);
                string _detalhe;

                //detalhe                           (tamanho,tipo) A= Alfanumerico, N= Numerico
                _detalhe = "1"; //Identificação do Registro         (1, N)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); //N da agencia(5, N)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, false); //Digito da agencia(1, N)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true); //Conta Corrente(12, N)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//Digito da conta(1, A)
                _detalhe += "0";
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); // Codigo da carteira (3)
                _detalhe += new string('0', 13);

                _detalhe += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);
                _detalhe += "136";
                _detalhe += "00";
                _detalhe += new string(' ', 25);
                _detalhe += "0"; // 93 Filler

                if (boleto.PercMulta > 0) {
                    _detalhe += "2"; // 94 (1, A) ‘1’ = Valor Fixo (R$) ‘2’ = Taxa(%) ‘3’ = Isento
                    _detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 10, 10, '0', 0, true, true, true); // 95-104 (10, A) Percentual Multa (10, A)
                } else {
                    _detalhe += "0"; // 94 (1, A) ‘1’ = Valor Fixo (R$) ‘2’ = Taxa(%) ‘3’ = Isento
                    _detalhe += new string('0', 10); // 95-104 (10, A)
                }

                // 105 (1, A) Código adotado pela FEBRABAN para identificação do tipo de pagamento de mora de juros.
                /*
                 * ‘1’ = Valor Diário (R$)
                 * ‘2’ = Taxa Mensal (%)
                 * ‘3’ = Valor Mensal (R$) *
                 * ‘4’ = Taxa diária (%)
                 * ‘5’ = Isento
                 */
                _detalhe += boleto.CodJurosMora;


                _detalhe += "N"; // 106 Identificação de Título Descontável (1, A)
                _detalhe += "  ";

                //Identificação ocorrência(2, N)
                /*
                01..Remessa
                02..Pedido de baixa
                04..Concessão de abatimento
                05..Cancelamento de abatimento concedido
                06..Alteração de vencimento
                07..Alteração do controle do participante
                08..Alteração de seu número
                09..Pedido de protesto
                18..Sustar protesto e baixar Título
                19..Sustar protesto e manter em carteira
                31..Alteração de outros dados
                35..Desagendamento do débito automático
                68..Acerto nos dados do rateio de Crédito
                69..Cancelamento do rateio de crédito.
                */
                if (boleto.Remessa == null || string.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia.Trim())) {
                    _detalhe += "01";
                } else {
                    _detalhe += boleto.Remessa.CodigoOcorrencia.PadLeft(2, '0');
                }

                // 111 a 120 Nº do Documento (Seu número) 010
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false); // Utils.Right(boleto.NumeroDocumento, 10, ' ', true);

                // 121 a 126 Data de vencimento do Título 006
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");


                //127 a 139 Valor do Título (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                _detalhe += "0000000000";

                // 150 a 150 Código do desconto (1, A) 0 = Isento  1 = Valor Fixo
                if (boleto.ValorDesconto > 0) {
                    _detalhe += "1";
                } else {
                    _detalhe += "0";
                }
                
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy"); //Data da emissão do Título (6, N) DDMMAA
                _detalhe += "0";

                _detalhe += "0"; // 158 a 158 Código para Protesto
                _detalhe += "00"; // 159 a 160 Número de Dias para Protesto

                // 161 a 173 Valor a ser cobrado por Dia de Atraso (13, N)
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                // 174 a 179 Data Limite P/Concessão de Desconto (06, N)
                if (boleto.DataDesconto == DateTime.MinValue) {
                    _detalhe += "000000"; // Caso não tenha data de vencimento
                } else {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //180 a 192 Valor do Desconto (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                var _NossoNumero = Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true);
                var _NossoNumeroDigito = Mod11UniCred(_NossoNumero, false);

                _detalhe += _NossoNumero + _NossoNumeroDigito; // 193 a 203 Nosso Numero (11, A)
                _detalhe += "00";

                //206 a 218 Valor do Abatimento a ser concedido ou cancelado (13, N)
                _detalhe += Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                /*219 a 220 Identificação do Tipo de Inscrição do Sacado (02, N)
                *01-CPF
                02-CNPJ
                03-PIS/PASEP
                98-Não tem
                99-Outros 
                00-Outros 
                */
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //Nº Inscrição do Sacado (14, N)
                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpf_Cnpj, 14, 14, '0', 0, true, true, true);

                //235 a 274 Nome do Sacado (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //275 a 314 Endereço Completo (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumero.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //315 a 326 Bairro do Pagador
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();

                //327 a 334 CEP (8, N) + Sufixo do CEP (3, N) Total (8, N)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true);

                // 335 a 354 Cidade do Pagador (20, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 20, 20, ' ', 0, true, true, false).ToUpper();

                // 355 a 356 UF do pagador (2, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF.TrimStart(' '), 2, 2, ' ', 0, true, true, false).ToUpper();


                // 357 a 394 Pagador/Avalista (38, A)
                _detalhe += new string(' ', 38); ;

                //Nº Seqüencial do Registro (06, N)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa400(int numeroRegistro) {
            try {
                string complemento = new string(' ', 393);
                string _trailer;

                _trailer = "9";
                _trailer += complemento;
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            } catch (Exception ex) {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #endregion

        #region REMESSA 240

        #region HEADER
        public string GerarHeaderRemessaCNAB240(Cedente cedente) {
            throw new BoletoNetException("Nao implantado");
        }
        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa) {
            try {
                return GerarHeaderRemessaCNAB240(cedente);
            } catch (Exception e) {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }
        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo) {
            throw new BoletoNetException("Nao implantado");

        }
        public string GerarDetalheRemessaCNAB240() {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa240() {
            throw new NotImplementedException("Função não implementada.");
        }
        public string GerarTrailerRemessa240(int numeroRegistro) {
            throw new BoletoNetException("Nao implantado");
        }
        #endregion

        #endregion

        /// <summary>
        /// Efetua as Validacoes dentro da classe Boleto, para garantir a geracao da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem) {
            bool vRetorno = true;
            string vMsg = string.Empty;
            /// TODO - IMPLEMENTACAO PENDENTE
            mensagem = vMsg;
            return vRetorno;
        }

        #endregion

        #region RETORNO
        private string LerMotivoRejeicao(string codigorejeicao) {
            var rejeicao = String.Empty;

            if (codigorejeicao.Length >= 2) {
                #region LISTA DE MOTIVOS
                List<String> ocorrencias = new List<string>();

                ocorrencias.Add("00 - Sem Complemento a informar");
                ocorrencias.Add("01 - Código do Banco Inválido");
                ocorrencias.Add("04 - Código de Movimento não permitido para a carteira");
                ocorrencias.Add("05 - Código de Movimento Inválido");
                ocorrencias.Add("06 - Número de Inscrição do Beneficiário Inválido");
                ocorrencias.Add("07 - Agência - Conta Inválida");
                ocorrencias.Add("08 - Nosso Número Inválido");
                ocorrencias.Add("09 - Nosso Número Duplicado");
                ocorrencias.Add("10 - Carteira inválida");
                ocorrencias.Add("12 - Tipo de Documento Inválido");
                ocorrencias.Add("16 - Data de Vencimento Inválida");
                ocorrencias.Add("17 - Data de Vencimento Anterior à Data de Emissão");
                ocorrencias.Add("18 - Vencimento fora do Prazo de Operação");
                ocorrencias.Add("20 - Valor do Título Inválido");
                ocorrencias.Add("24 - Data de Emissão Inválida");
                ocorrencias.Add("25 - Data de Emissão Posterior à data de Entrega");
                ocorrencias.Add("26 - Código de juros inválido");
                ocorrencias.Add("27 – Valor de juros inválido");
                ocorrencias.Add("28 – Código de Desconto inválido");
                ocorrencias.Add("29 – Valor de Desconto inválido");
                ocorrencias.Add("30 - Alteração de Dados Rejeitada");
                ocorrencias.Add("33 - Valor de Abatimento Inválido");
                ocorrencias.Add("34 - Valor do Abatimento Maior ou Igual ao Valor do título");
                ocorrencias.Add("45 - Nome do Pagador não informado");
                ocorrencias.Add("46 - Número de Inscrição do Pagador Inválido");
                ocorrencias.Add("47 - Endereço do Pagador Não Informado");
                ocorrencias.Add("48 - CEP Inválido");
                ocorrencias.Add("52 - Unidade Federativa Inválida");
                ocorrencias.Add("57 – Código de Multa inválido");
                ocorrencias.Add("58 – Data de Multa inválido");
                ocorrencias.Add("59 – Valor / percentual de Multa inválido");
                ocorrencias.Add("60 - Movimento para Título não Cadastrado");
                ocorrencias.Add("63 - Entrada para Título já Cadastrado");
                ocorrencias.Add("79 – Data de Juros inválida");
                ocorrencias.Add("80 – Data de Desconto inválida");
                ocorrencias.Add("86 - Seu Número Inválido");
                ocorrencias.Add("A5 - Título Liquidado");
                ocorrencias.Add("A8 - Valor do Abatimento Inválido para Cancelamento");
                ocorrencias.Add("C0 – Sistema Intermitente – Entre em contato com sua Cooperativa");
                ocorrencias.Add("C1 - Situação do título Aberto");
                ocorrencias.Add("C3 - Status do Borderô Inválido");
                ocorrencias.Add("C4 - Nome do Beneficiário Inválido");
                ocorrencias.Add("C5 - Documento Inválido");
                ocorrencias.Add("C6 - Instrução não Atualiza Cadastro do Título");
                
                #endregion

                var ocorrencia = (from s in ocorrencias where s.Substring(0, 2) == codigorejeicao.Substring(0, 2) select s).FirstOrDefault();

                if (ocorrencia != null)
                    rejeicao = ocorrencia;
            }

            return rejeicao;
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro) {
            try {
                EDI.Banco.TRegistroEDI_Unicred_Retorno reg = new TRegistroEDI_Unicred_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(reg.Fixo1);
                //TipoCobranca
                //CodigoPagadorAgenciaBeneficiario
                //detalhe.NomeSacado = reg.p;
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV do Nosso Numero
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.CodigoDeMovimento);
                int dataOcorrencia = Utils.ToInt32(reg.DataLiquidacao);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = new CodigoMovimento(136, detalhe.CodigoOcorrencia).Descricao;

                detalhe.NumeroDocumento = reg.SeuNumero;

                if (!String.IsNullOrEmpty(reg.DataDeVencimento)) {
                    int dataVencimento = Utils.ToInt32(reg.DataDeVencimento);
                    detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                }
                decimal valorTitulo = Convert.ToInt64(reg.ValorDoTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;
              
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.ValorAbatimento);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.ValorDesconto);
                detalhe.Descontos = valorDesconto / 100;
                
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorRecebido);
                detalhe.ValorPago = valorPago / 100;
                
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.JurosDeMora);
                detalhe.JurosMora = jurosMora / 100;

                decimal tarifa = Convert.ToUInt64(reg.ValorDaTarifa);
                detalhe.TarifaCobranca = tarifa / 100;

                int dataCredito = Utils.ToInt32(reg.DataProgramadaParaRepasse);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("####-##-##"));

                detalhe.NumeroSequencial = Utils.ToInt32(reg.SequencialDoRegistro);

                //detalhe.Especie = reg.TipoDocumento; //Verificar Espécie de Documentos...
                detalhe.OutrosCreditos = 0;
                detalhe.OrigemPagamento = String.Empty;
                //detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //
                detalhe.IOF = 0;
                //Motivos das Rejeições para os Códigos de Ocorrência
                if (detalhe.CodigoOcorrencia == 3) {
                    detalhe.MotivosRejeicao = LerMotivoRejeicao(reg.ComplementoDoMovimento);
                }

                //Número do Cartório
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;

                detalhe.CodigoInscricao = 0;
                detalhe.NumeroInscricao = string.Empty;
                detalhe.Agencia = 0;
                detalhe.Conta = header.Conta;
                detalhe.DACConta = header.DACConta;

                detalhe.NumeroControle = string.Empty;
                detalhe.IdentificacaoTitulo = string.Empty;
                //Banco Cobrador
                detalhe.CodigoBanco = 0;
                //Agência Cobradora
                detalhe.AgenciaCobradora = 0;
                //
                return detalhe;
            } catch (Exception ex) {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro) {
            try {
                header = new HeaderRetorno(registro);
                header.TipoRegistro = Utils.ToInt32(registro.Substring(000, 1));
                header.CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1));
                header.LiteralRetorno = registro.Substring(002, 7);
                header.CodigoServico = Utils.ToInt32(registro.Substring(009, 2));
                header.LiteralServico = registro.Substring(011, 8);

                header.Agencia = Utils.ToInt32(registro.Substring(026, 5));

                string _conta = registro.Substring(031, 9);
                header.Conta = Utils.ToInt32(_conta.Substring(0, _conta.Length - 1));
                header.DACConta = Utils.ToInt32(_conta.Substring(_conta.Length - 1));

                //header.ComplementoRegistro2 = registro.Substring(031, 14);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##"));
                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(100, 7));
                //header.Versao = registro.Substring(390, 5);
                //header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return header;
            } catch (Exception ex) {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }
        #endregion


        #region UTIL
        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto) {
            return Mod11UniCred(Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true), false).ToString();
        }

        public int Mod10Unicred(string seq) {
            /* Variaveis
             * -------------
             * d - Digito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 2, r;

            for (int i = seq.Length - 1; i >= 0; i--) {

                r = (Convert.ToInt32(seq.Substring(i, 1)) * p);
                if (r > 9)
                    r = SomaDezena(r);
                s = s + r;
                if (p < b)
                    p++;
                else
                    p--;
            }

            d = Multiplo10(s);
            return d;
        }

        public int SomaDezena(int dezena) {
            string d = dezena.ToString();
            int d1 = Convert.ToInt32(d.Substring(0, 1));
            int d2 = Convert.ToInt32(d.Substring(1));
            return d1 + d2;
        }

        protected static int Mod11UniCred(string seq, bool ehBarcode) {
            /* Variaveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */
            int[] mult = new[] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int d, s = 0, i = 0;

            foreach (char c in seq) {
                var mul = mult[i];
                s = s + (int.Parse(c.ToString()) * mul);
                i++;
            }

            d = 11 - (s % 11);
            //if (ehBarcode) {
            //    if (d == 0 || d >= 10)
            //        d = 1;
            //} else {
                if (d > 9)
                    d = 0;
            //}

            return d;
        }

        public int DigUnicred(string seq) {
            /* Variaveis
              * -------------
              * d - Dígito
              * s - Soma
              * p - Peso
              * b - Base
              * r - Resto
              */
            int[] mult = seq.Length == 25 ? new[] { 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 } :
                new[] { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7 };

            int d, s = 0, i = 0;

            foreach (char c in seq) {
                if (seq.Length > mult.Length) {
                    throw new BoletoNetException("Tamanho da sequencia maior que o limite");
                }
                var mul = mult[i];
                s += (int.Parse(c.ToString()) * mul);
                i++;
            }

            d = 11 - (s % 11);
            if (d == 0 || d == 1 || d == 10)
                d = 0;

            return d;
        }
        #endregion



    }
}