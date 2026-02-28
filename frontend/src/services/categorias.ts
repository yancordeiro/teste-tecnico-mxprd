import api from "./api"
import type { CategoriaInput, CategoriaOutput, TotaisCategorias } from "@/types"

/**
 * Servico de Categorias
 * Gerencia operacoes de criacao e listagem de categorias
 * Categorias definem a finalidade: Despesa, Receita ou Ambas
 * Endpoints: GET, POST /api/categorias
 */

/**
 * Busca todas as categorias cadastradas
 * @returns Lista de categorias
 */
export async function obterTodas(): Promise<CategoriaOutput[]> {
  const response = await api.get<CategoriaOutput[]>("/categorias")
  return response.data
}

/**
 * Busca uma categoria pelo ID
 * @param id - Identificador unico da categoria
 * @returns Dados da categoria
 */
export async function obterPorId(id: string): Promise<CategoriaOutput> {
  const response = await api.get<CategoriaOutput>(`/categorias/${id}`)
  return response.data
}

/**
 * Cria uma nova categoria
 * @param data - Dados da categoria (descricao e finalidade)
 * @returns Categoria criada com ID gerado
 */
export async function criar(data: CategoriaInput): Promise<CategoriaOutput> {
  const response = await api.post<CategoriaOutput>("/categorias", data)
  return response.data
}

/**
 * Busca os totais financeiros de todas as categorias
 * Inclui: total de receitas, despesas e saldo de cada categoria
 * Tambem retorna os totais gerais consolidados
 * @returns Totais por categoria e totais gerais
 */
export async function obterTotais(): Promise<TotaisCategorias> {
  const response = await api.get<TotaisCategorias>("/categorias/totais")
  return response.data
}
