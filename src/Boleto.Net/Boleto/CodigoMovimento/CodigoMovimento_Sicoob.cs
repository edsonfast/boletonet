using System;
using BoletoNet.Enums;
using System.Collections.Generic;
using BoletoNet.Excecoes;

namespace BoletoNet
{
    public enum EnumCodigoMovimento_Sicoob
    {
        /*
         * "Código de Movimento de Retorno
        '02' = Entrada Confirmada
        '03' = Entrada Rejeitada
        '04' = Transferência de Carteira/Entrada
        '05' = Transferência de Carteira/Baixa
        '06' = Liquidação
        '07' = Confirmação do Recebimento da Instrução de Desconto
        '08' = Confirmação do Recebimento do Cancelamento do Desconto
        '09' = Baixa
        '11' = Títulos em Carteira (Em Ser)
        '12' = Confirmação Recebimento Instrução de Abatimento
        '13' = Confirmação Recebimento Instrução de Cancelamento Abatimento
        '14' = Confirmação Recebimento Instrução Alteração de Vencimento
        '15' = Franco de Pagamento
        '17' = Liquidação Após Baixa ou Liquidação Título Não Registrado
        '19' = Confirmação Recebimento Instrução de Protesto
        '20' = Confirmação Recebimento Instrução de Sustação/Cancelamento de Protesto
        '23' = Remessa a Cartório (Aponte em Cartório)
        '24' = Retirada de Cartório e Manutenção em Carteira
        '25' = Protestado e Baixado (Baixa por Ter Sido Protestado)
        '26' = Instrução Rejeitada
        '27' = Confirmação do Pedido de Alteração de Outros Dados
        '28' = Débito de Tarifas/Custas
        '29' = Ocorrências do Pagador
        '30' = Alteração de Dados Rejeitada
        '33' = Confirmação da Alteração dos Dados do Rateio de Crédito
        '34' = Confirmação do Cancelamento dos Dados do Rateio de Crédito
        '35' = Confirmação do Desagendamento do Débito Automático
        ‘36’ = Confirmação de envio de e-mail/SMS
        ‘37’ = Envio de e-mail/SMS rejeitado
        ‘38’ = Confirmação de alteração do Prazo Limite de Recebimento (a data deve ser
        ‘39’ = Confirmação de Dispensa de Prazo Limite de Recebimento
        ‘40’ = Confirmação da alteração do número do título dado pelo Beneficiário
        ‘41’ = Confirmação da alteração do número controle do Participante
        ‘42’ = Confirmação da alteração dos dados do Pagador
        ‘43’ = Confirmação da alteração dos dados do Pagadorr/Avalista
        ‘44’ = Título pago com cheque devolvido
        ‘45’ = Título pago com cheque compensado
        ‘46’ = Instrução para cancelar protesto confirmada
        ‘47’ = Instrução para protesto para fins falimentares confirmada
        ‘48’ = Confirmação de instrução de transferência de carteira/modalidade de cobrança
        ‘49’ = Alteração de contrato de cobrança
        ‘50’ = Título pago com cheque pendente de liquidação
        ‘51’ = Título DDA reconhecido pelo Pagador
        ‘52’ = Título DDA não reconhecido pelo Pagador
        ‘53’ = Título DDA recusado pela CIP
        '54' - Confirmação da Instrução de Baixa/Cancelamento de Título Negativado sem Protesto
        ‘55’ = Confirmação de Pedido de Dispensa de Multa
        ‘56’ = Confirmação do Pedido de Cobrança de Multa
        ‘57’ = Confirmação do Pedido de Alteração de Cobrança de Juros
        ‘58’ = Confirmação do Pedido de Alteração do Valor/Data de Desconto
        ‘59’ = Confirmação do Pedido de Alteração do Beneficiário do Título
        ‘60’ = Confirmação do Pedido de Dispensa de Juros de Mora
        '80' - Confirmação da instrução de negativação
        '85' = Confirmação de Desistência de Protesto
        '86' = Confirmação de cancelamento do Protesto"
         */

        EntradaConfirmada = 02,                                             //02 Entrada confirmada
        EntradaRejeitada = 03,                                              //03 Entrada rejeitada
        TransferenciaDeCarteiraEntrada = 04,                                //04 Transferência de Carteira/Entrada
        LiquidacaoNormal = 06,                                              //06 Liquidação normal
        Baixado = 09,                                                       //09 Baixado
        BaixadoConformeInstrucoesDaCooperativaDeCredito = 10,               //10 Baixado conforme instruções da cooperativa de crédito
        TituloEmSer = 11,                                                   //11 Títulso em Ser
        AbatimentoConcedido = 12,                                           //12 Abatimento concedido
        AbatimentoCancelado = 13,                                           //13 Abatimento cancelado
        VencimentoAlterado = 14,                                            //14 Vencimento alterado
        LiquidacaoEmCartorio = 15,                                          //15 Liquidação em cartório
        LiquidacaoAposBaixa = 17,                                           //17 Liquidação após baixa
        ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto = 19,                 //19 Confirmação de recebimento de instrução de protesto
        ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto = 20,       //20 Confirmação de recebimento de instrução de sustação de protesto
        EntradaDeTituloEmCartorio = 23,                                     //23 Entrada de título em cartório        
        EntradaRejeitadaPorCEPIrregular = 24,                               //24 Entrada rejeitada por CEP irregular
        ProtestadoEBaixado = 25,                                            //25 Protestado e Baixado (Baixa por Ter Sido Protestado)
        InstrucaoRejeitada = 26,                                            //26 instrucao rejeitada
        BaixaRejeitada = 27,                                                //27 Baixa rejeitada
        Tarifa = 28,                                                        //28 Tarifa
        RejeicaoDoPagador = 29,                                             //29 Rejeição do pagador
        AlteracaoRejeitada = 30,                                            //30 Alteração rejeitada
        ConfirmacaoDePedidoDeAlteracaoDeOutrosDados = 33,                   //33 Confirmação de pedido de alteração de outros dados
        RetiradoDeCartorioEManutencaoEmCarteira = 34,                       //34 Retirado de cartório e manutenção em carteira
        AceiteDoPagador = 35,                                               //35 Aceite do pagador
        ConfirmacaoDaAlteracaoDoNumeroDoTituloDadoPeloBeneficiario = 40,    //40 Confirmação da alteração do número do título dado pelo Beneficiário
        ConfirmacaoDeInstrucaoDeTransferenciaDeCarteira = 48,               //48 Confirmação de instrução de transferência de carteira/modalidade de cobrança
        ConfirmacaoDaInstrucaoDeBaixaCancelamentoDeTituloNegativadoSemProtesto = 54, // 54 Confirmação da Instrução de Baixa/Cancelamento de Título Negativado sem Protesto 
        ConfirmacaoDaInstrucaoDeNegativacao = 80,                           //80 Confirmação da instrução de negativação
        ConfirmacaoDeDesistenciaDeProtesto = 85,                            //85 Confirmação de Desistência de Protesto
        ConfirmacaoDeCancelamentoDoProtesto = 86,                           //86 Confirmação de cancelamento do Protesto
    }

    public class CodigoMovimento_Sicoob : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores
        internal CodigoMovimento_Sicoob()
        {
        }

        public CodigoMovimento_Sicoob(int codigo)
        {
            try
            {
                carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        #endregion

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                var movimento = (EnumCodigoMovimento_Sicoob)codigo;
                Codigo = codigo;
                Descricao = descricoes[movimento];
            }
            catch (Exception ex)
            {
                throw new BoletoNetException("Código de movimento é inválido", ex);
            }
        }

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban()
        {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Sicoob)Codigo);
        }

        #region Dicionários
        private readonly Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Sicoob, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                      ,TipoOcorrenciaRetorno.EntradaConfirmada },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                       ,TipoOcorrenciaRetorno.EntradaRejeitada },
            { EnumCodigoMovimento_Sicoob.TransferenciaDeCarteiraEntrada                         ,TipoOcorrenciaRetorno.TransferenciaDeCarteiraEntrada },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                       ,TipoOcorrenciaRetorno.Liquidacao },
            { EnumCodigoMovimento_Sicoob.Baixado                                                ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito        ,TipoOcorrenciaRetorno.Baixa },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeAbatimento },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                    ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                     ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                    ,TipoOcorrenciaRetorno.LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto          ,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeProtesto },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto,TipoOcorrenciaRetorno.ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto },
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                              ,TipoOcorrenciaRetorno.RemessaACartorio },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                 ,TipoOcorrenciaRetorno.DebitoDeTarifasCustas },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                      ,TipoOcorrenciaRetorno.OcorrenciasDoPagador },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                     ,TipoOcorrenciaRetorno.AlteracaoDeDadosRejeitada },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                     ,TipoOcorrenciaRetorno.InstrucaoRejeitada },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados            ,TipoOcorrenciaRetorno.ConfirmacaoDaAlteracaoDosDadosDoRateioDeCredito },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                ,TipoOcorrenciaRetorno.ConfirmacaoDoCancelamentoDosDadosDoRateioDeCredito },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                            ,TipoOcorrenciaRetorno.TitulosEmCarteira },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                     ,TipoOcorrenciaRetorno.ProtestadoEBaixado},
        };

        private readonly Dictionary<EnumCodigoMovimento_Sicoob, string> descricoes = new Dictionary<EnumCodigoMovimento_Sicoob, string>()
        {
            { EnumCodigoMovimento_Sicoob.EntradaConfirmada                                       , "Entrada confirmada"                                             },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitada                                        , "Entrada rejeitada"                                              },
            { EnumCodigoMovimento_Sicoob.TransferenciaDeCarteiraEntrada                          , "Transferência de Carteira/Entrada"                              },
            { EnumCodigoMovimento_Sicoob.LiquidacaoNormal                                        , "Liquidação normal"                                              },
            { EnumCodigoMovimento_Sicoob.Baixado                                                 , "Baixa de Título"                                                },
            { EnumCodigoMovimento_Sicoob.BaixadoConformeInstrucoesDaCooperativaDeCredito         , "Baixado conforme instruções da cooperativa de crédito"          },
            { EnumCodigoMovimento_Sicoob.AbatimentoConcedido                                     , "Abatimento concedido"                                           },
            { EnumCodigoMovimento_Sicoob.AbatimentoCancelado                                     , "Abatimento cancelado"                                           },
            { EnumCodigoMovimento_Sicoob.VencimentoAlterado                                      , "Vencimento alterado"                                            },
            { EnumCodigoMovimento_Sicoob.LiquidacaoEmCartorio                                    , "Liquidação em cartório"                                         },
            { EnumCodigoMovimento_Sicoob.LiquidacaoAposBaixa                                     , "Liquidação após baixa"                                          },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeProtesto           , "Confirmação de recebimento de instrução de protesto"            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeRecebimentoDeInstrucaoDeSustacaoDeProtesto , "Confirmação de recebimento de instrução de sustação de protesto"},
            { EnumCodigoMovimento_Sicoob.EntradaDeTituloEmCartorio                               , "Entrada de título em cartório"                                  },
            { EnumCodigoMovimento_Sicoob.EntradaRejeitadaPorCEPIrregular                         , "Entrada rejeitada por CEP irregular"                            },
            { EnumCodigoMovimento_Sicoob.ProtestadoEBaixado                                      , "Protestado e Baixado (Baixa por Ter Sido Protestado)"           },
            { EnumCodigoMovimento_Sicoob.BaixaRejeitada                                          , "Baixa rejeitada"                                                },
            { EnumCodigoMovimento_Sicoob.Tarifa                                                  , "Tarifa"                                                         },
            { EnumCodigoMovimento_Sicoob.RejeicaoDoPagador                                       , "Rejeição do pagador"                                            },
            { EnumCodigoMovimento_Sicoob.AlteracaoRejeitada                                      , "Alteração rejeitada"                                            },
            { EnumCodigoMovimento_Sicoob.InstrucaoRejeitada                                      , "Instrução rejeitada"                                            },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDePedidoDeAlteracaoDeOutrosDados             , "Confirmação de pedido de alteração de outros dados"             },
            { EnumCodigoMovimento_Sicoob.RetiradoDeCartorioEManutencaoEmCarteira                 , "Retirado de cartório e manutenção em carteira"                  },
            { EnumCodigoMovimento_Sicoob.AceiteDoPagador                                         , "Aceite do pagador" },
            { EnumCodigoMovimento_Sicoob.TituloEmSer                                             , "Título em Ser" },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeInstrucaoDeTransferenciaDeCarteira         , "Confirmação de instrução de transferência de carteira/modalidade de cobrança" },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaInstrucaoDeBaixaCancelamentoDeTituloNegativadoSemProtesto, "Confirmação da Instrução de Baixa/Cancelamento de Título Negativado sem Protesto"                        },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDaInstrucaoDeNegativacao                     , "Confirmação da instrução de negativação"                        },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeDesistenciaDeProtesto                      , "Confirmação de Desistência de Protesto"                         },
            { EnumCodigoMovimento_Sicoob.ConfirmacaoDeCancelamentoDoProtesto                     , "Confirmação de Cancelamento do Protesto"                        },
        };
        #endregion
    }
}