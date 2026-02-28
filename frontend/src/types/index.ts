/**
 * Tipos TypeScript para o sistema de Gastos Residenciais
 * Espelham os DTOs do backend .NET para garantir consistencia
 */

// ============================================
// ENUMS
// ============================================

/**
 * Enum de Finalidade - define o tipo de categoria/transacao
 * Valores correspondentes ao backend:
 * - Despesa = 1: Gastos/saidas de dinheiro
 * - Receita = 2: Entradas de dinheiro
 * - Ambas = 3: Categoria aceita ambos os tipos
 */
export enum Finalidade {
  Despesa = 1,
  Receita = 2,
  Ambas = 3,
}

/**
 * Mapeamento de Finalidade para texto legivel
 */
export const FinalidadeLabels: Record<Finalidade, string> = {
  [Finalidade.Despesa]: "Despesa",
  [Finalidade.Receita]: "Receita",
  [Finalidade.Ambas]: "Ambas",
}

// ============================================
// PESSOA
// ============================================

/**
 * DTO de entrada para criar/atualizar pessoa
 */
export interface PessoaInput {
  nome: string
  idade: number
}

/**
 * DTO de saida - dados da pessoa retornados pela API
 */
export interface PessoaOutput {
  id: string
  nome: string
  idade: number
}

/**
 * DTO com totais financeiros de uma pessoa
 */
export interface PessoaTotais {
  id: string
  nome: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

/**
 * DTO com totais gerais de todas as pessoas
 */
export interface TotaisGerais {
  pessoas: PessoaTotais[]
  totalGeralReceitas: number
  totalGeralDespesas: number
  saldoLiquido: number
}

// ============================================
// CATEGORIA
// ============================================

/**
 * DTO de entrada para criar categoria
 */
export interface CategoriaInput {
  descricao: string
  finalidade: Finalidade
}

/**
 * DTO de saida - dados da categoria retornados pela API
 */
export interface CategoriaOutput {
  id: string
  descricao: string
  finalidadeId: number
  finalidadeDescricao: string
}

/**
 * DTO com totais financeiros de uma categoria
 */
export interface CategoriaTotais {
  id: string
  descricao: string
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

/**
 * DTO com totais gerais de todas as categorias
 */
export interface TotaisCategorias {
  categorias: CategoriaTotais[]
  totalGeralReceitas: number
  totalGeralDespesas: number
  saldoLiquido: number
}

// ============================================
// TRANSACAO
// ============================================

/**
 * DTO de entrada para criar transacao
 */
export interface TransacaoInput {
  descricao: string
  valor: number
  tipo: Finalidade
  categoriaId: string
  pessoaId: string
}

/**
 * DTO de saida - dados da transacao retornados pela API
 */
export interface TransacaoOutput {
  id: string
  descricao: string
  valor: number
  tipo: Finalidade
  tipoDescricao: string
  categoriaId: string
  categoriaDescricao: string
  pessoaId: string
  pessoaNome: string
}
