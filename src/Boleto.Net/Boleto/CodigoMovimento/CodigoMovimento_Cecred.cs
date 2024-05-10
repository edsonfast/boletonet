using System;
using System.Collections.Generic;
using BoletoNet.Enums;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Cecred
    {
        EntradaConfirmada = 2,
        EntradaRejeitada = 3,
        TransferenciaCarteiraEntrada = 4,
        TransferenciaCarteiraBaixa = 5,
        Liquidacao = 6,
        ConfirmacaoRecebimentoDesconto = 7,
        ConfirmacaoRecebimentoCancelamentoDesconto = 8,
        Baixa = 9,
        TitulosCarteira = 11,
        ConfirmacaoRecebimentoInstrucaoAbatimento = 12,
        ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento = 13,
        ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento = 14,
        LiquidacaoAposBaixa = 17,
        ConfirmacaoRecebimentoInstrucaoProtesto = 19,
        ConfirmacaoRecebimentoInstrucaoSustacaoProtesto = 20,
        RemessaCartorio = 23,
        RetiradaCartorioManutencaoCarteira = 24,
        ProtestadoBaixado = 25,
        InstrucaoRejeitada = 26,
        ConfirmaçãoPedidoAlteracaoOutrosDados = 27,
        DebitoTarifas = 28,
        AlteracaoDadosRejeitada = 30,
        AlteracaoDadosSacado = 42,
        InstrucaoCancelarProtesto = 46,
        /// <summary>
        /// ‘51’ = Título DDA reconhecido pelo Pagador (quando o pagador aceitar o boleto depois de ter recusado)
        /// </summary>
        TituloDDAReconhecidoPeloPagador = 51,

        /// <summary>
        /// ‘52’ = Título DDA não reconhecido pelo Pagador
        /// </summary>
        TituloDDANaoReconhecidoPeloPagador = 52,

        /// <summary>
        /// ‘76’ = Liquidação CEE (boleto emitido na modalidade Cooperativa Emite e Expede)
        /// </summary>
        LiquidacaoCCE = 76,

        /// <summary>
        /// ‘77’ = Liquidação após Baixa ou Liquidação Título Não Registrado CEE (boleto emitido na modalidade Cooperativa Emite e Expede)
        /// </summary>
        LiquidacaoAposBaixaOuNaoRegistrado = 77,

        /// <summary>
        /// ‘89’ = Rejeição cartorária (Visualizar motivo na última página deste manual)
        /// </summary>
        RejeicaoCartoraria = 89,

        /// <summary>
        /// ‘91’ = Título em aberto não enviado ao pagador
        /// </summary>
        TituloEmAbertoNaoEnviado = 91,

        /// <summary>
        /// ‘92’ = Inconsistência Negativação Serasa
        /// </summary>
        InconsistenciaNegativacaoSerasa = 92,

        /// <summary>
        /// ‘93’ = Incluir Serasa
        /// </summary>
        IncluirSerasa = 93,

        /// <summary>
        /// ‘94’ = Excluir Serasa
        /// </summary>
        ExcluirSerasa = 94,

        /// <summary>
        /// ‘95’ = Instrução de SMS
        /// </summary>
        InstrucaoSMS = 95,

        /// <summary>
        /// ‘96’ = Cancelamento Instrução SMS
        /// </summary>
        CancelamentoInstrucaoSMS = 96,

        /// <summary>
        /// ‘97’ = Confirmação de instrução automática de protesto
        /// </summary>
        ConfirmacaoDeInstrucaoAutoProtesto = 97,

        /// <summary>
        /// ‘98’ = Excluir Protesto com carta de anuência
        /// </summary>
        ExcluirProtestoComCartaAnuencia = 98,
    }

    #endregion 

    public class CodigoMovimento_Cecred : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores 

        public CodigoMovimento_Cecred()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Cecred(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        #endregion

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch ((EnumCodigoMovimento_Cecred)codigo)
                {
                    case EnumCodigoMovimento_Cecred.EntradaConfirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Cecred.EntradaRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_Cecred.Liquidacao:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Liquidacao;
                        this.Descricao = "Liquidação";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento da Instrução de Desconto";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento do Cancelamento do Desconto";
                        break;
                    case EnumCodigoMovimento_Cecred.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Cecred.TitulosCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TitulosCarteira;
                        this.Descricao = "Títulos em carteira";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação Após Baixa ou Liquidação Título Não Registrado";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case EnumCodigoMovimento_Cecred.RemessaCartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case EnumCodigoMovimento_Cecred.ProtestadoBaixado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimento_Cecred.InstrucaoRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case EnumCodigoMovimento_Cecred.DebitoTarifas:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case EnumCodigoMovimento_Cecred.AlteracaoDadosSacado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosSacado;
                        this.Descricao = "Confirmação da alteração dos dados do Sacado";
                        break;
                    case EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto;
                        this.Descricao = "Instrução para cancelar protesto confirmada";
                        break;
                    case EnumCodigoMovimento_Cecred.TituloDDAReconhecidoPeloPagador:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloDDAReconhecidoPeloPagador;
                        this.Descricao = "Título DDA reconhecido pelo Pagador (quando o pagador aceitar o boleto depois de ter recusado)";
                        break;
                    case EnumCodigoMovimento_Cecred.TituloDDANaoReconhecidoPeloPagador:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloDDANaoReconhecidoPeloPagador;
                        this.Descricao = "Título DDA não reconhecido pelo Pagador";
                        break;
                    case EnumCodigoMovimento_Cecred.LiquidacaoCCE:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoCCE;
                        this.Descricao = "Liquidação CEE (boleto emitido na modalidade Cooperativa Emite e Expede)";
                        break;
                    case EnumCodigoMovimento_Cecred.LiquidacaoAposBaixaOuNaoRegistrado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixaOuNaoRegistrado;
                        this.Descricao = "Liquidação após Baixa ou Liquidação Título Não Registrado CEE (boleto emitido na modalidade Cooperativa Emite e Expede)";
                        break;
                    case EnumCodigoMovimento_Cecred.RejeicaoCartoraria:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RejeicaoCartoraria;
                        this.Descricao = "Rejeição cartorária (Visualizar motivo na última página deste manual)";
                        break;
                    case EnumCodigoMovimento_Cecred.TituloEmAbertoNaoEnviado:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloEmAbertoNaoEnviado;
                        this.Descricao = "Título em aberto não enviado ao pagador";
                        break;
                    case EnumCodigoMovimento_Cecred.InconsistenciaNegativacaoSerasa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InconsistenciaNegativacaoSerasa;
                        this.Descricao = "Inconsistência Negativação Serasa";
                        break;
                    case EnumCodigoMovimento_Cecred.IncluirSerasa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.IncluirSerasa;
                        this.Descricao = "Incluir Serasa";
                        break;
                    case EnumCodigoMovimento_Cecred.ExcluirSerasa:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ExcluirSerasa;
                        this.Descricao = "Excluir Serasa";
                        break;
                    case EnumCodigoMovimento_Cecred.InstrucaoSMS:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoSMS;
                        this.Descricao = "Instrução de SMS";
                        break;
                    case EnumCodigoMovimento_Cecred.CancelamentoInstrucaoSMS:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.CancelamentoInstrucaoSMS;
                        this.Descricao = "Cancelamento Instrução SMS";
                        break;
                    case EnumCodigoMovimento_Cecred.ConfirmacaoDeInstrucaoAutoProtesto:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoDeInstrucaoAutoProtesto;
                        this.Descricao = "Confirmação de instrução automática de protesto";
                        break;
                    case EnumCodigoMovimento_Cecred.ExcluirProtestoComCartaAnuencia:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ExcluirProtestoComCartaAnuencia;
                        this.Descricao = "Excluir Protesto com carta de anuência";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaConfirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.EntradaRejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa;
                        this.Descricao = "Transferência de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Liquidacao;
                        this.Descricao = "Liquidação";
                        break;
                    case 7:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto;
                        this.Descricao = "Confirmação do Recebimento da Instrução de Desconto";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto;
                        this.Descricao = "Confirmação do Recebimento do Cancelamento do Desconto";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TitulosCarteira;
                        this.Descricao = "Títulos em carteira em ser";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa;
                        this.Descricao = "Liquidação após baixa";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto;
                        this.Descricao = "Confirmação de recebimento de instrução de sustação de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RemessaCartorio;
                        this.Descricao = "Remessa a cartório/aponte em cartório";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira;
                        this.Descricao = "Retirada de cartório e manutenção em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ProtestadoBaixado;
                        this.Descricao = "Protestado e baixado/baixa por ter sido protestado";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoRejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dados";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.DebitoTarifas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case 42:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.AlteracaoDadosSacado;
                        this.Descricao = "Confirmação da alteração dos dados do Sacado";
                        break;
                    case 46:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto;
                        this.Descricao = "Instrução para cancelar protesto confirmada";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloDDAReconhecidoPeloPagador;
                        this.Descricao = "Título DDA reconhecido pelo Pagador (quando o pagador aceitar o boleto depois de ter recusado)";
                        break;
                    case 52:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloDDANaoReconhecidoPeloPagador;
                        this.Descricao = "Título DDA não reconhecido pelo Pagador";
                        break;
                    case 76:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoCCE;
                        this.Descricao = "Liquidação CEE (boleto emitido na modalidade Cooperativa Emite e Expede)";
                        break;
                    case 77:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.LiquidacaoAposBaixaOuNaoRegistrado;
                        this.Descricao = "Liquidação após Baixa ou Liquidação Título Não Registrado CEE (boleto emitido na modalidade Cooperativa Emite e Expede)";
                        break;
                    case 89:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.RejeicaoCartoraria;
                        this.Descricao = "Rejeição cartorária (Visualizar motivo na última página deste manual)";
                        break;
                    case 91:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.TituloEmAbertoNaoEnviado;
                        this.Descricao = "Título em aberto não enviado ao pagador";
                        break;
                    case 92:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InconsistenciaNegativacaoSerasa;
                        this.Descricao = "Inconsistência Negativação Serasa";
                        break;
                    case 93:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.IncluirSerasa;
                        this.Descricao = "Incluir Serasa";
                        break;
                    case 94:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ExcluirSerasa;
                        this.Descricao = "Excluir Serasa";
                        break;
                    case 95:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.InstrucaoSMS;
                        this.Descricao = "Instrução de SMS";
                        break;
                    case 96:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.CancelamentoInstrucaoSMS;
                        this.Descricao = "Cancelamento Instrução SMS";
                        break;
                    case 97:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ConfirmacaoDeInstrucaoAutoProtesto;
                        this.Descricao = "Confirmação de instrução automática de protesto";
                        break;
                    case 98:
                        this.Codigo = (int)EnumCodigoMovimento_Cecred.ExcluirProtestoComCartaAnuencia;
                        this.Descricao = "Excluir Protesto com carta de anuência";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban()
        {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Cecred)Codigo);
        }

        private Dictionary<EnumCodigoMovimento_Cecred, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Cecred, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Cecred.EntradaConfirmada                                     ,TipoOcorrenciaRetorno.EntradaConfirmada                                      },
            { EnumCodigoMovimento_Cecred.EntradaRejeitada                                      ,TipoOcorrenciaRetorno.EntradaRejeitada                                       },
            { EnumCodigoMovimento_Cecred.TransferenciaCarteiraEntrada                          ,TipoOcorrenciaRetorno.TransferenciaDeCarteiraEntrada                           },
            { EnumCodigoMovimento_Cecred.TransferenciaCarteiraBaixa                            ,TipoOcorrenciaRetorno.TransferenciaDeCarteiraBaixa                             },
            { EnumCodigoMovimento_Cecred.Liquidacao                                            ,TipoOcorrenciaRetorno.Liquidacao                                             },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoDesconto                        ,TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoCancelamentoDesconto            ,TipoOcorrenciaRetorno.ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto             },
            { EnumCodigoMovimento_Cecred.Baixa                                                 ,TipoOcorrenciaRetorno.Baixa                                                  },
            { EnumCodigoMovimento_Cecred.TitulosCarteira                                       ,TipoOcorrenciaRetorno.TitulosEmCarteira                                        },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAbatimento             ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento              },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoCancelamentoAbatimento ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento  },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoAlteracaoVencimento    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento     },
            { EnumCodigoMovimento_Cecred.LiquidacaoAposBaixa                                   ,TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoProtesto               ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto                },
            { EnumCodigoMovimento_Cecred.ConfirmacaoRecebimentoInstrucaoSustacaoProtesto       ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto        },
            { EnumCodigoMovimento_Cecred.RemessaCartorio                                       ,TipoOcorrenciaRetorno.RemessaACartorio                                        },
            { EnumCodigoMovimento_Cecred.RetiradaCartorioManutencaoCarteira                    ,TipoOcorrenciaRetorno.RetiradaDeCartorioEManutencaoEmCarteira                     },
            { EnumCodigoMovimento_Cecred.ProtestadoBaixado                                     ,TipoOcorrenciaRetorno.ProtestadoEBaixado                                      },
            { EnumCodigoMovimento_Cecred.InstrucaoRejeitada                                    ,TipoOcorrenciaRetorno.InstrucaoRejeitada                                     },
            { EnumCodigoMovimento_Cecred.ConfirmaçãoPedidoAlteracaoOutrosDados                 ,TipoOcorrenciaRetorno.ConfirmacaoDoPedidoDeAlteracaoDeOutrosDados                  },
            { EnumCodigoMovimento_Cecred.DebitoTarifas                                         ,TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Cecred.AlteracaoDadosRejeitada                               ,TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada                                },
            { EnumCodigoMovimento_Cecred.AlteracaoDadosSacado                                  ,TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosdadosDoPagador },
            { EnumCodigoMovimento_Cecred.InstrucaoCancelarProtesto                             ,TipoOcorrenciaRetorno.InstrucaoParacancelarProtestoConfirmada },
            { EnumCodigoMovimento_Cecred.TituloDDAReconhecidoPeloPagador                       ,TipoOcorrenciaRetorno.TituloDDAreconhecidoPeloPagador },
            { EnumCodigoMovimento_Cecred.TituloDDANaoReconhecidoPeloPagador                    ,TipoOcorrenciaRetorno.TituloDDANaoReconhecidoPeloPagador },
        };
    }
}
