import { useState, useEffect } from "react"
import { Header } from "@/components/layout"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Card, CardContent } from "@/components/ui/card"
import { useToast } from "@/hooks/use-toast"
import { categoriasService } from "@/services"
import type { CategoriaOutput, CategoriaInput } from "@/types"
import { Finalidade, FinalidadeLabels } from "@/types"
import { Plus, Loader2 } from "lucide-react"

/**
 * Pagina de Categorias
 *
 * Funcionalidades implementadas:
 * - Listagem de todas as categorias cadastradas
 * - Criacao de nova categoria (descricao e finalidade)
 *
 * Regras de negocio:
 * - Descricao: maximo 400 caracteres
 * - Finalidade: Despesa, Receita ou Ambas
 * - Finalidade define quais tipos de transacao podem usar a categoria
 */
export function CategoriasPage() {
  // Estado da listagem
  const [categorias, setCategorias] = useState<CategoriaOutput[]>([])
  const [loading, setLoading] = useState(true)

  // Estado do formulario
  const [dialogOpen, setDialogOpen] = useState(false)
  const [formData, setFormData] = useState<CategoriaInput>({
    descricao: "",
    finalidade: Finalidade.Ambas,
  })
  const [saving, setSaving] = useState(false)

  const { toast } = useToast()

  /**
   * Carrega a lista de categorias do backend
   */
  async function carregarCategorias() {
    try {
      setLoading(true)
      const data = await categoriasService.obterTodas()
      setCategorias(data)
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao carregar categorias",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    carregarCategorias()
  }, [])

  /**
   * Abre o dialog para criar nova categoria
   */
  function handleNovaCategoria() {
    setFormData({ descricao: "", finalidade: Finalidade.Ambas })
    setDialogOpen(true)
  }

  /**
   * Cria a nova categoria
   */
  async function handleSalvar() {
    // Validacao basica
    if (!formData.descricao.trim()) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "A descricao e obrigatoria",
      })
      return
    }

    if (formData.descricao.length > 400) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "A descricao deve ter no maximo 400 caracteres",
      })
      return
    }

    try {
      setSaving(true)
      await categoriasService.criar(formData)
      toast({
        title: "Categoria criada",
        description: `${formData.descricao} foi cadastrada com sucesso.`,
      })
      setDialogOpen(false)
      carregarCategorias()
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao criar categoria",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setSaving(false)
    }
  }

  /**
   * Retorna a cor do badge baseada na finalidade
   */
  function getFinalidadeStyle(finalidadeId: number) {
    switch (finalidadeId) {
      case Finalidade.Despesa:
        return "bg-red-100 text-red-800"
      case Finalidade.Receita:
        return "bg-green-100 text-green-800"
      case Finalidade.Ambas:
        return "bg-blue-100 text-blue-800"
      default:
        return "bg-gray-100 text-gray-800"
    }
  }

  return (
    <div className="flex flex-col h-full">
      <Header
        title="Categorias"
        description="Gerencie as categorias de transacoes"
      />

      <div className="flex-1 p-6">
        {/* Botao para adicionar nova categoria */}
        <div className="flex justify-end mb-4">
          <Button onClick={handleNovaCategoria}>
            <Plus className="h-4 w-4 mr-2" />
            Nova Categoria
          </Button>
        </div>

        {/* Tabela de categorias */}
        <Card>
          <CardContent className="p-0">
            {loading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
              </div>
            ) : categorias.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                Nenhuma categoria cadastrada
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Descricao</TableHead>
                    <TableHead className="w-32">Finalidade</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {categorias.map((categoria) => (
                    <TableRow key={categoria.id}>
                      <TableCell className="font-medium">
                        {categoria.descricao}
                      </TableCell>
                      <TableCell>
                        <span
                          className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${getFinalidadeStyle(
                            categoria.finalidadeId
                          )}`}
                        >
                          {categoria.finalidadeDescricao}
                        </span>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      </div>

      {/* Dialog de criar categoria */}
      <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Nova Categoria</DialogTitle>
            <DialogDescription>
              Preencha os dados da nova categoria
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="descricao">Descricao</Label>
              <Input
                id="descricao"
                placeholder="Digite a descricao da categoria"
                maxLength={400}
                value={formData.descricao}
                onChange={(e) =>
                  setFormData({ ...formData, descricao: e.target.value })
                }
              />
              <span className="text-xs text-muted-foreground">
                {formData.descricao.length}/400 caracteres
              </span>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="finalidade">Finalidade</Label>
              <Select
                value={formData.finalidade.toString()}
                onValueChange={(value) =>
                  setFormData({ ...formData, finalidade: parseInt(value) as Finalidade })
                }
              >
                <SelectTrigger>
                  <SelectValue placeholder="Selecione a finalidade" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value={Finalidade.Despesa.toString()}>
                    {FinalidadeLabels[Finalidade.Despesa]} - Apenas gastos
                  </SelectItem>
                  <SelectItem value={Finalidade.Receita.toString()}>
                    {FinalidadeLabels[Finalidade.Receita]} - Apenas entradas
                  </SelectItem>
                  <SelectItem value={Finalidade.Ambas.toString()}>
                    {FinalidadeLabels[Finalidade.Ambas]} - Gastos e entradas
                  </SelectItem>
                </SelectContent>
              </Select>
              <span className="text-xs text-muted-foreground">
                Define quais tipos de transacao podem usar esta categoria
              </span>
            </div>
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setDialogOpen(false)}>
              Cancelar
            </Button>
            <Button onClick={handleSalvar} disabled={saving}>
              {saving && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
              Criar
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
