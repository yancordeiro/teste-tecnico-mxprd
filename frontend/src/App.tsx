import { BrowserRouter, Routes, Route } from "react-router-dom"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { MainLayout } from "@/components/layout"
import { Toaster } from "@/components/ui/toaster"
import {
  HomePage,
  PessoasPage,
  CategoriasPage,
  TransacoesPage,
  TotaisPessoaPage,
  TotaisCategoriaPage,
} from "@/pages"

/**
 * Cliente do React Query para gerenciamento de cache e estado do servidor
 * Configurado com defaults para refetch e stale time
 */
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5, // 5 minutos
      retry: 1,
    },
  },
})

/**
 * Componente principal da aplicacao
 *
 * Estrutura:
 * - QueryClientProvider: gerenciamento de estado do servidor
 * - BrowserRouter: roteamento SPA
 * - MainLayout: layout com sidebar de navegacao
 * - Routes: definicao das rotas da aplicacao
 * - Toaster: sistema de notificacoes
 */
function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Routes>
          {/* Rotas dentro do layout principal */}
          <Route element={<MainLayout />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/pessoas" element={<PessoasPage />} />
            <Route path="/categorias" element={<CategoriasPage />} />
            <Route path="/transacoes" element={<TransacoesPage />} />
            <Route path="/totais-pessoa" element={<TotaisPessoaPage />} />
            <Route path="/totais-categoria" element={<TotaisCategoriaPage />} />
          </Route>
        </Routes>
        {/* Sistema de notificacoes toast */}
        <Toaster />
      </BrowserRouter>
    </QueryClientProvider>
  )
}

export default App
