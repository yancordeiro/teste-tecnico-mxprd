import { Link } from "react-router-dom"
import { Header } from "@/components/layout"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import {
  Users,
  FolderTree,
  ArrowLeftRight,
  BarChart3,
  PieChart,
  ArrowRight,
} from "lucide-react"

/**
 * Pagina Inicial
 *
 * Dashboard com links rapidos para todas as funcionalidades do sistema
 * Apresenta uma visao geral das opcoes disponiveis
 */

interface QuickLinkCardProps {
  title: string
  description: string
  href: string
  icon: React.ComponentType<{ className?: string }>
}

function QuickLinkCard({ title, description, href, icon: Icon }: QuickLinkCardProps) {
  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardHeader>
        <div className="flex items-center gap-3">
          <div className="p-2 rounded-lg bg-primary/10">
            <Icon className="h-6 w-6 text-primary" />
          </div>
          <div>
            <CardTitle className="text-lg">{title}</CardTitle>
            <CardDescription>{description}</CardDescription>
          </div>
        </div>
      </CardHeader>
      <CardContent>
        <Link to={href}>
          <Button variant="outline" className="w-full">
            Acessar
            <ArrowRight className="h-4 w-4 ml-2" />
          </Button>
        </Link>
      </CardContent>
    </Card>
  )
}

export function HomePage() {
  return (
    <div className="flex flex-col h-full">
      <Header
        title="Bem-vindo ao Sistema de Gastos Residenciais"
        description="Gerencie suas financas de forma simples e eficiente"
      />

      <div className="flex-1 p-6">
        {/* Introducao */}
        <div className="mb-8">
          <p className="text-muted-foreground max-w-2xl">
            Este sistema permite o controle completo de gastos residenciais,
            incluindo cadastro de pessoas, categorias e transacoes financeiras.
            Utilize o menu lateral ou os cards abaixo para navegar entre as funcionalidades.
          </p>
        </div>

        {/* Grid de links rapidos */}
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          <QuickLinkCard
            title="Pessoas"
            description="Cadastre, edite e remova pessoas"
            href="/pessoas"
            icon={Users}
          />
          <QuickLinkCard
            title="Categorias"
            description="Gerencie categorias de transacoes"
            href="/categorias"
            icon={FolderTree}
          />
          <QuickLinkCard
            title="Transacoes"
            description="Registre receitas e despesas"
            href="/transacoes"
            icon={ArrowLeftRight}
          />
          <QuickLinkCard
            title="Totais por Pessoa"
            description="Veja o resumo financeiro por pessoa"
            href="/totais-pessoa"
            icon={BarChart3}
          />
          <QuickLinkCard
            title="Totais por Categoria"
            description="Analise gastos por categoria"
            href="/totais-categoria"
            icon={PieChart}
          />
        </div>

        {/* Informacoes adicionais */}
        <div className="mt-8 p-4 bg-muted rounded-lg">
          <h3 className="font-semibold mb-2">Regras do Sistema</h3>
          <ul className="text-sm text-muted-foreground space-y-1">
            <li>• Menores de 18 anos so podem ter transacoes de despesa</li>
            <li>• Ao excluir uma pessoa, todas as suas transacoes sao removidas</li>
            <li>• Categorias definem quais tipos de transacao podem utiliza-las</li>
            <li>• Valores de transacao devem ser sempre positivos</li>
          </ul>
        </div>
      </div>
    </div>
  )
}
