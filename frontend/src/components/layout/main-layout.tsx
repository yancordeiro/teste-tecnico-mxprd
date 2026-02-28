import { Outlet } from "react-router-dom"
import { Sidebar } from "./sidebar"

/**
 * Layout principal da aplicacao
 * Estrutura com sidebar fixa a esquerda e area de conteudo principal
 * Usa o Outlet do React Router para renderizar as paginas filhas
 */
export function MainLayout() {
  return (
    <div className="flex min-h-screen">
      {/* Sidebar de navegacao */}
      <Sidebar />

      {/* Area principal de conteudo */}
      <main className="flex-1 overflow-auto">
        <Outlet />
      </main>
    </div>
  )
}
