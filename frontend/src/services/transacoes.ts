import api from "./api"
import type { TransacaoInput, TransacaoOutput } from "@/types"

/**
 * Servico de Transacoes
 * Gerencia operacoes de criacao e listagem de transacoes financeiras
 *
 * Regras de negocio implementadas no backend:
 * - Menores de 18 anos so podem ter despesas (nao podem ter receitas)
 * - Tipo da transacao deve ser compativel com a finalidade da categoria
 *
 * Endpoints: GET, POST /api/transacoes
 */

/**
 * Busca todas as transacoes cadastradas
 * @returns Lista de transacoes com dados relacionados (pessoa e categoria)
 */
export async function obterTodas(): Promise<TransacaoOutput[]> {
  const response = await api.get<TransacaoOutput[]>("/transacoes")
  return response.data
}

/**
 * Busca uma transacao pelo ID
 * @param id - Identificador unico da transacao
 * @returns Dados da transacao
 */
export async function obterPorId(id: string): Promise<TransacaoOutput> {
  const response = await api.get<TransacaoOutput>(`/transacoes/${id}`)
  return response.data
}

/**
 * Cria uma nova transacao
 *
 * Validacoes aplicadas pelo backend:
 * - Pessoa e categoria devem existir
 * - Menor de idade (< 18) so pode criar despesas
 * - Categoria deve aceitar o tipo de transacao informado
 *
 * @param data - Dados da transacao
 * @returns Transacao criada com ID gerado
 * @throws Error se validacoes falharem
 */
export async function criar(data: TransacaoInput): Promise<TransacaoOutput> {
  const response = await api.post<TransacaoOutput>("/transacoes", data)
  return response.data
}
