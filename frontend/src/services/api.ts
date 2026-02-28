import axios from "axios"

/**
 * Cliente Axios configurado para comunicacao com o backend .NET
 * - Base URL configurada para o proxy do Vite em desenvolvimento
 * - Headers padrao para JSON
 */
const api = axios.create({
  baseURL: "/api",
  headers: {
    "Content-Type": "application/json",
  },
})

/**
 * Interceptor de resposta para tratamento padronizado de erros
 * Extrai mensagem de erro do response da API .NET
 */
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const message =
      error.response?.data?.message ||
      error.response?.data ||
      error.message ||
      "Erro desconhecido"
    return Promise.reject(new Error(typeof message === "string" ? message : JSON.stringify(message)))
  }
)

export default api
