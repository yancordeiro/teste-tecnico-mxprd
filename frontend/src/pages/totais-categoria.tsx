import { useState, useEffect } from "react"
import { Header } from "@/components/layout"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
  TableFooter,
} from "@/components/ui/table"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { useToast } from "@/hooks/use-toast"
import { categoriasService } from "@/services"
import type { TotaisCategorias } from "@/types"
import { Loader2, TrendingUp, TrendingDown, Wallet } from "lucide-react"

/**
 * Pagina de Totais por Categoria (Opcional)
 *
 * Exibe um relatorio consolidado com:
 * - Total de receitas de cada categoria
 * - Total de despesas de cada categoria
 * - Saldo (receita - despesa) de cada categoria
 * - Totais gerais de todas as categorias
 *
 * Util para identificar onde o dinheiro esta sendo gasto/recebido
 */
export function TotaisCategoriaPage() {
  const [totais, setTotais] = useState<TotaisCategorias | null>(null)
  const [loading, setLoading] = useState(true)
  const { toast } = useToast()

  /**
   * Carrega os totais financeiros por categoria
   */
  async function carregarTotais() {
    try {
      setLoading(true)
      const data = await categoriasService.obterTotais()
      setTotais(data)
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao carregar totais",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    carregarTotais()
  }, [])

  /**
   * Formata valor para exibicao em BRL
   */
  function formatarValor(valor: number) {
    return valor.toLocaleString("pt-BR", {
      style: "currency",
      currency: "BRL",
    })
  }

  return (
    <div className="flex flex-col h-full">
      <Header
        title="Totais por Categoria"
        description="Resumo financeiro de todas as categorias cadastradas"
      />

      <div className="flex-1 p-6">
        {loading ? (
          <div className="flex items-center justify-center py-8">
            <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
          </div>
        ) : !totais ? (
          <div className="text-center py-8 text-muted-foreground">
            Nao foi possivel carregar os totais
          </div>
        ) : (
          <>
            {/* Cards de resumo geral */}
            <div className="grid gap-4 md:grid-cols-3 mb-6">
              <Card>
                <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                  <CardTitle className="text-sm font-medium">
                    Total de Receitas
                  </CardTitle>
                  <TrendingUp className="h-4 w-4 text-green-500" />
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold text-green-600">
                    {formatarValor(totais.totalGeralReceitas)}
                  </div>
                  <p className="text-xs text-muted-foreground">
                    Soma de todas as entradas
                  </p>
                </CardContent>
              </Card>

              <Card>
                <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                  <CardTitle className="text-sm font-medium">
                    Total de Despesas
                  </CardTitle>
                  <TrendingDown className="h-4 w-4 text-red-500" />
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold text-red-600">
                    {formatarValor(totais.totalGeralDespesas)}
                  </div>
                  <p className="text-xs text-muted-foreground">
                    Soma de todos os gastos
                  </p>
                </CardContent>
              </Card>

              <Card>
                <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                  <CardTitle className="text-sm font-medium">
                    Saldo Liquido
                  </CardTitle>
                  <Wallet className="h-4 w-4 text-blue-500" />
                </CardHeader>
                <CardContent>
                  <div
                    className={`text-2xl font-bold ${
                      totais.saldoLiquido >= 0 ? "text-green-600" : "text-red-600"
                    }`}
                  >
                    {formatarValor(totais.saldoLiquido)}
                  </div>
                  <p className="text-xs text-muted-foreground">
                    Receitas - Despesas
                  </p>
                </CardContent>
              </Card>
            </div>

            {/* Tabela detalhada por categoria */}
            <Card>
              <CardHeader>
                <CardTitle>Detalhamento por Categoria</CardTitle>
              </CardHeader>
              <CardContent className="p-0">
                {totais.categorias.length === 0 ? (
                  <div className="text-center py-8 text-muted-foreground">
                    Nenhuma categoria cadastrada
                  </div>
                ) : (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Categoria</TableHead>
                        <TableHead className="text-right">Receitas</TableHead>
                        <TableHead className="text-right">Despesas</TableHead>
                        <TableHead className="text-right">Saldo</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {totais.categorias.map((categoria) => (
                        <TableRow key={categoria.id}>
                          <TableCell className="font-medium">
                            {categoria.descricao}
                          </TableCell>
                          <TableCell className="text-right text-green-600">
                            {formatarValor(categoria.totalReceitas)}
                          </TableCell>
                          <TableCell className="text-right text-red-600">
                            {formatarValor(categoria.totalDespesas)}
                          </TableCell>
                          <TableCell
                            className={`text-right font-medium ${
                              categoria.saldo >= 0 ? "text-green-600" : "text-red-600"
                            }`}
                          >
                            {formatarValor(categoria.saldo)}
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                    <TableFooter>
                      <TableRow>
                        <TableCell className="font-bold">TOTAL GERAL</TableCell>
                        <TableCell className="text-right font-bold text-green-600">
                          {formatarValor(totais.totalGeralReceitas)}
                        </TableCell>
                        <TableCell className="text-right font-bold text-red-600">
                          {formatarValor(totais.totalGeralDespesas)}
                        </TableCell>
                        <TableCell
                          className={`text-right font-bold ${
                            totais.saldoLiquido >= 0
                              ? "text-green-600"
                              : "text-red-600"
                          }`}
                        >
                          {formatarValor(totais.saldoLiquido)}
                        </TableCell>
                      </TableRow>
                    </TableFooter>
                  </Table>
                )}
              </CardContent>
            </Card>
          </>
        )}
      </div>
    </div>
  )
}
