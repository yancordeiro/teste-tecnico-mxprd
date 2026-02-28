import { Link, useLocation } from "react-router-dom"
import { cn } from "@/lib/utils"
import {
  Users,
  FolderTree,
  ArrowLeftRight,
  BarChart3,
  PieChart,
  Home,
} from "lucide-react"

/**
 * Item de navegacao da sidebar
 */
interface NavItem {
  title: string
  href: string
  icon: React.ComponentType<{ className?: string }>
}

/**
 * Lista de itens de navegacao do sistema
 * Cada item representa uma funcionalidade do teste tecnico
 */
const navItems: NavItem[] = [
  {
    title: "Inicio",
    href: "/",
    icon: Home,
  },
  {
    title: "Pessoas",
    href: "/pessoas",
    icon: Users,
  },
  {
    title: "Categorias",
    href: "/categorias",
    icon: FolderTree,
  },
  {
    title: "Transacoes",
    href: "/transacoes",
    icon: ArrowLeftRight,
  },
  {
    title: "Totais por Pessoa",
    href: "/totais-pessoa",
    icon: BarChart3,
  },
  {
    title: "Totais por Categoria",
    href: "/totais-categoria",
    icon: PieChart,
  },
]

/**
 * Componente Sidebar
 * Barra lateral de navegacao com links para todas as funcionalidades do sistema
 * Destaca o item ativo baseado na rota atual
 */
export function Sidebar() {
  const location = useLocation()

  return (
    <aside className="w-64 border-r bg-card min-h-screen p-4">
      {/* Logo/Titulo do sistema */}
      <div className="mb-8">
        <h1 className="text-xl font-bold text-primary">Gastos Residenciais</h1>
        <p className="text-sm text-muted-foreground">Controle financeiro</p>
      </div>

      {/* Links de navegacao */}
      <nav className="space-y-2">
        {navItems.map((item) => {
          const isActive = location.pathname === item.href
          const Icon = item.icon

          return (
            <Link
              key={item.href}
              to={item.href}
              className={cn(
                "flex items-center gap-3 rounded-lg px-3 py-2 text-sm transition-colors",
                isActive
                  ? "bg-primary text-primary-foreground"
                  : "text-muted-foreground hover:bg-accent hover:text-accent-foreground"
              )}
            >
              <Icon className="h-4 w-4" />
              {item.title}
            </Link>
          )
        })}
      </nav>
    </aside>
  )
}
