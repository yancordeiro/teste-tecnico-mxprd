/**
 * Componente Header
 * Cabecalho da pagina com titulo da secao atual
 */

interface HeaderProps {
  title: string
  description?: string
}

export function Header({ title, description }: HeaderProps) {
  return (
    <header className="border-b bg-card px-6 py-4">
      <h2 className="text-2xl font-bold tracking-tight">{title}</h2>
      {description && (
        <p className="text-muted-foreground">{description}</p>
      )}
    </header>
  )
}
