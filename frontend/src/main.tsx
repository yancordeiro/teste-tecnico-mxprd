import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import "./index.css"
import App from "./App.tsx"

/**
 * Ponto de entrada da aplicacao React
 * Renderiza o componente App dentro do StrictMode para detectar problemas
 */
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>
)
