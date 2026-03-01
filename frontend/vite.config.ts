import path from "path"
import react from "@vitejs/plugin-react"
import { defineConfig } from "vite"

/**
 * Configuracao do Vite para o projeto de Gastos Residenciais
 * - Plugin React para suporte a JSX
 * - Alias @ configurado para a pasta src (padrao shadcn/ui)
 * - Proxy para o backend .NET em desenvolvimento
 */
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  server: {
    port: 5173,
    proxy: {
      "/api": {
        target: process.env.VITE_API_URL || "http://localhost:5000",
        changeOrigin: true,
      },
    },
  },
})
