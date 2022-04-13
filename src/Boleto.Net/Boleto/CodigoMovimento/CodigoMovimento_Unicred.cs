using System;
using BoletoNet.Enums;
using System.Collections.Generic;
using BoletoNet.Excecoes;

namespace BoletoNet {
    public enum EnumCodigoMovimento_Unicred {
        EntradaConfirmada = 02,                                       //02 Entrada confirmada
        EntradaRejeitada = 03,                                        //03 Entrada rejeitada
        LiquidacaoNormal = 06,                                        //06 Liquidação normal
    }

    public class CodigoMovimento_Unicred : AbstractCodigoMovimento, ICodigoMovimento {
        #region Construtores
        internal CodigoMovimento_Unicred() {
        }

        public CodigoMovimento_Unicred(int codigo) {
            try {
                carregar(codigo);
            } catch (Exception ex) {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        #endregion

        private void carregar(int codigo) {
            try {
                this.Banco = new Banco_Unicred();

                var movimento = (EnumCodigoMovimento_Unicred)codigo;
                Codigo = codigo;
                Descricao = descricoes[movimento];
            } catch (Exception ex) {
                throw new BoletoNetException("Código de movimento é inválido", ex);
            }
        }

        public override TipoOcorrenciaRetorno ObterCorrespondenteFebraban() {
            return ObterCorrespondenteFebraban(correspondentesFebraban, (EnumCodigoMovimento_Unicred)Codigo);
        }

        #region Dicionários
        private Dictionary<EnumCodigoMovimento_Unicred, TipoOcorrenciaRetorno> correspondentesFebraban = new Dictionary<EnumCodigoMovimento_Unicred, TipoOcorrenciaRetorno>()
        {
            { EnumCodigoMovimento_Unicred.EntradaConfirmada                                      ,TipoOcorrenciaRetorno.EntradaConfirmada },
            { EnumCodigoMovimento_Unicred.EntradaRejeitada                                       ,TipoOcorrenciaRetorno.EntradaRejeitada },
            { EnumCodigoMovimento_Unicred.LiquidacaoNormal                                       ,TipoOcorrenciaRetorno.Liquidacao }            
        };

        private Dictionary<EnumCodigoMovimento_Unicred, string> descricoes = new Dictionary<EnumCodigoMovimento_Unicred, string>()
        {
            { EnumCodigoMovimento_Unicred.EntradaConfirmada                                       , "Entrada confirmada"                                             },
            { EnumCodigoMovimento_Unicred.EntradaRejeitada                                        , "Entrada rejeitada"                                              },
            { EnumCodigoMovimento_Unicred.LiquidacaoNormal                                        , "Liquidação normal"                                              }
        };
        #endregion
    }
}