using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet.EDI.Banco {

    /// <summary>
	/// Classe de Integração Unicred
	/// </summary>
    public class TRegistroEDI_Unicred_Retorno : TRegistroEDI {

        #region Atributos e Propriedades
        public string Fixo1 { get; set; }
        public string TipoDeInscricaoDaEmpresa { get; set; }
        public string NumeroDeInscricaoDaEmpresa { get; set; }
        public string NumeroDaAgencia { get; set; }
        public string DigitoVerificadorDaAgencia { get; set; }
        public string ContaCorrenteDoBeneficiario { get; set; }
        public string DigitoVerificadorDaContaDoBeneficiario { get; set; }
        public string CodigoDoBeneficiario { get; set; }
        public string NossoNumero { get; set; }
        public string Fixo2 { get; set; }
        public string Fixo3 { get; set; }
        public string Fixo4 { get; set; }
        public string Fixo5 { get; set; }
        public string Fixo6 { get; set; }
        public string Fixo7 { get; set; }
        public string Fixo8 { get; set; }
        /// <summary>
        /// 02 – Instrução Confirmada 03 – Instrução Rejeitada 06 - LIQUIDAÇÃO NORMAL
        /// </summary>
        public string CodigoDeMovimento { get; set; }
        public string DataLiquidacao { get; set; }
        public string Fixo9 { get; set; }
        public string DataDeVencimento { get; set; }
        public string ValorDoTitulo { get; set; }
        public string CodigoDoBancoRecebedor { get; set; }
        public string PrefixoDaAgenciaRecebedora { get; set; }
        public string DVPrefixoAgenciaRecebedora { get; set; }
        public string Fixo10 { get; set; }
        public string DataProgramadaParaRepasse { get; set; }
        public string ValorDaTarifa { get; set; }
        public string Fixo11 { get; set; }
        public string ValorAbatimento { get; set; }
        public string ValorDesconto { get; set; }
        public string ValorRecebido { get; set; }
        public string JurosDeMora { get; set; }
        public string SeuNumero { get; set; }
        public string ValorPago { get; set; }
        public string ComplementoDoMovimento { get; set; }
        public string TipoDeInstrucaoOrigem { get; set; }
        public string Fixo12 { get; set; }
        public string SequencialDoRegistro { get; set; }
        #endregion

        public TRegistroEDI_Unicred_Retorno() {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0022, 001, 0, string.Empty, ' ')); //022-022
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0023, 008, 0, string.Empty, ' ')); //023-030
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 014, 0, string.Empty, ' ')); //032-045
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0046, 017, 0, string.Empty, ' ')); //046-062
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 011, 0, string.Empty, ' ')); //063-073
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 001, 0, string.Empty, ' ')); //074-074
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0075, 001, 0, string.Empty, ' ')); //075-075
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0076, 010, 0, string.Empty, ' ')); //076-085
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0086, 003, 0, string.Empty, ' ')); //086-088
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0089, 018, 0, string.Empty, ' ')); //089-106
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0107, 002, 0, string.Empty, ' ')); //107-108
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 030, 0, string.Empty, ' ')); //117-146
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 006, 0, string.Empty, ' ')); //176-181
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0182, 007, 0, string.Empty, ' ')); //182-188
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 039, 0, string.Empty, ' ')); //189-227
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 026, 0, string.Empty, ' ')); //280-305
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0306, 013, 0, string.Empty, ' ')); //306-318
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0319, 008, 0, string.Empty, ' ')); //319-326
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0327, 002, 0, string.Empty, ' ')); //327-328
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0329, 066, 0, string.Empty, ' ')); //329-394
            this._CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400
            #endregion
        }

        /// <summary>
        /// Aqui iremos atribuir os valores das propriedades em cada campo correspondente do Registro EDI
        /// e codificaremos a linha para obter uma string formatada com o nosso layout.
        /// Repare que declarei as propriedades em uma ordem tal que a adição dos objetos TCampoRegistroEDI na propriedade
        /// _CamposEDI siga a mesma ordem. Portanto, utilizarei o índice na atribuição.
        /// </summary>
        public override void CodificarLinha() {
            #region Todos os Campos
            //PARA LEITURA DO RETORNO BANCÁRIO NÃO PRECISAMOS IMPLEMENTAR ESSE MÉTODO           
            #endregion
            //
            base.CodificarLinha(); //Aqui que eu chamo efetivamente a rotina de codificação; o resultado será exibido na propriedade LinhaRegistro.
        }

        /// <summary>
        /// Agora, faço o inverso da codificação. Decodifico o valor da propriedade LinhaRegistro e separo em cada campo.
        /// Cada campo é separado na propriedade ValorNatural de cada item da prop. _CamposEDI. Como esta é do tipo object, para atribuir
        /// nas propriedades do registro é necessário fazer um cast para o tipo de dado adequado. Caso ocorra algum erro na decodificação,
        /// uma exceção será disparada, provavelmente por causa de impossibilidade de fazer um cast na classe pai. Portanto, o layout deve estar
        /// correto!
        /// </summary>
        public override void DecodificarLinha() {
            base.DecodificarLinha();

            Fixo1 = (string)this._CamposEDI[00].ValorNatural;
            TipoDeInscricaoDaEmpresa = (string)this._CamposEDI[01].ValorNatural;
            NumeroDeInscricaoDaEmpresa = (string)this._CamposEDI[02].ValorNatural;
            NumeroDaAgencia = (string)this._CamposEDI[03].ValorNatural;
            DigitoVerificadorDaAgencia = (string)this._CamposEDI[04].ValorNatural;
            ContaCorrenteDoBeneficiario = (string)this._CamposEDI[05].ValorNatural;
            DigitoVerificadorDaContaDoBeneficiario = (string)this._CamposEDI[06].ValorNatural;
            CodigoDoBeneficiario = (string)this._CamposEDI[07].ValorNatural;
            NossoNumero = (string)this._CamposEDI[08].ValorNatural;
            Fixo2 = (string)this._CamposEDI[09].ValorNatural;
            Fixo3 = (string)this._CamposEDI[10].ValorNatural;
            Fixo4 = (string)this._CamposEDI[11].ValorNatural;
            Fixo5 = (string)this._CamposEDI[12].ValorNatural;
            Fixo6 = (string)this._CamposEDI[13].ValorNatural;
            Fixo7 = (string)this._CamposEDI[14].ValorNatural;
            Fixo8 = (string)this._CamposEDI[15].ValorNatural;
            CodigoDeMovimento = (string)this._CamposEDI[16].ValorNatural;
            DataLiquidacao = (string)this._CamposEDI[17].ValorNatural;
            Fixo9 = (string)this._CamposEDI[18].ValorNatural;
            DataDeVencimento = (string)this._CamposEDI[19].ValorNatural;
            ValorDoTitulo = (string)this._CamposEDI[20].ValorNatural;
            CodigoDoBancoRecebedor = (string)this._CamposEDI[21].ValorNatural;
            PrefixoDaAgenciaRecebedora = (string)this._CamposEDI[22].ValorNatural;
            DVPrefixoAgenciaRecebedora = (string)this._CamposEDI[23].ValorNatural;
            Fixo10 = (string)this._CamposEDI[24].ValorNatural;
            DataProgramadaParaRepasse = (string)this._CamposEDI[25].ValorNatural;
            ValorDaTarifa = (string)this._CamposEDI[26].ValorNatural;
            Fixo11 = (string)this._CamposEDI[27].ValorNatural;
            ValorAbatimento = (string)this._CamposEDI[28].ValorNatural;
            ValorDesconto = (string)this._CamposEDI[29].ValorNatural;
            ValorRecebido = (string)this._CamposEDI[30].ValorNatural;
            JurosDeMora = (string)this._CamposEDI[31].ValorNatural;
            SeuNumero = (string)this._CamposEDI[32].ValorNatural;
            ValorPago = (string)this._CamposEDI[33].ValorNatural;
            ComplementoDoMovimento = (string)this._CamposEDI[34].ValorNatural;
            TipoDeInstrucaoOrigem = (string)this._CamposEDI[35].ValorNatural;
            Fixo12 = (string)this._CamposEDI[36].ValorNatural;
            SequencialDoRegistro = (string)this._CamposEDI[37].ValorNatural;
        }
    }

    /// <summary>
    /// Classe que irá representar o arquivo EDI em si
    /// </summary>
    public class TArquivoUnicredRetorno_EDI : TEDIFile {
        /*
		 * De modo geral, apenas preciso sobreescrever o método de decodificação de linhas,
		 * pois preciso adicionar um objeto do tipo registro na coleção do arquivo, passar a linha que vem do arquivo
		 * neste objeto novo, e decodificá-lo para separar nos campos.
		 * O DecodeLine é chamado a partir do método LoadFromFile() (ou Stream) da classe base.
		 */
        protected override void DecodeLine(string Line) {
            base.DecodeLine(Line);
            Lines.Add(new TRegistroEDI_Unicred_Retorno()); //Adiciono a linha a ser decodificada
            Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
        }
    }
}