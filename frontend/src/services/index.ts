/**
 * Re-exporta todos os servicos para facilitar imports
 * Uso: import { pessoasService, categoriasService } from "@/services"
 */
import * as pessoasService from "./pessoas"
import * as categoriasService from "./categorias"
import * as transacoesService from "./transacoes"

export { pessoasService, categoriasService, transacoesService }
