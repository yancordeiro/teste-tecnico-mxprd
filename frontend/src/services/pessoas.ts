import api from "./api"
import type { PessoaInput, PessoaOutput, TotaisGerais } from "@/types"

/**
 * Servico de Pessoas
 * Gerencia todas as operacoes CRUD relacionadas a pessoas
 * Endpoints: GET, POST, PUT, DELETE /api/pessoas
 */

/**
 * Busca todas as pessoas cadastradas
 * @returns Lista de pessoas
 */
export async function obterTodas(): Promise<PessoaOutput[]> {
  const response = await api.get<PessoaOutput[]>("/pessoas")
  return response.data
}

/**
 * Busca uma pessoa pelo ID
 * @param id - Identificador unico da pessoa
 * @returns Dados da pessoa
 */
export async function obterPorId(id: string): Promise<PessoaOutput> {
  const response = await api.get<PessoaOutput>(`/pessoas/${id}`)
  return response.data
}

/**
 * Cria uma nova pessoa
 * @param data - Dados da pessoa (nome e idade)
 * @returns Pessoa criada com ID gerado
 */
export async function criar(data: PessoaInput): Promise<PessoaOutput> {
  const response = await api.post<PessoaOutput>("/pessoas", data)
  return response.data
}

/**
 * Atualiza uma pessoa existente
 * @param id - Identificador da pessoa
 * @param data - Novos dados da pessoa
 * @returns Pessoa atualizada
 */
export async function atualizar(id: string, data: PessoaInput): Promise<PessoaOutput> {
  const response = await api.put<PessoaOutput>(`/pessoas/${id}`, data)
  return response.data
}

/**
 * Remove uma pessoa e todas as suas transacoes
 * ATENCAO: Acao irreversivel que remove a pessoa e todas as transacoes associadas
 * @param id - Identificador da pessoa
 */
export async function remover(id: string): Promise<void> {
  await api.delete(`/pessoas/${id}`)
}

/**
 * Busca os totais financeiros de todas as pessoas
 * Inclui: total de receitas, despesas e saldo de cada pessoa
 * Tambem retorna os totais gerais consolidados
 * @returns Totais por pessoa e totais gerais
 */
export async function obterTotais(): Promise<TotaisGerais> {
  const response = await api.get<TotaisGerais>("/pessoas/totais")
  return response.data
}
