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
import { transacoesService, pessoasService, categoriasService } from "@/services"
import type {
  TransacaoOutput,
  TransacaoInput,
  PessoaOutput,
  CategoriaOutput,
} from "@/types"
import { Finalidade, FinalidadeLabels } from "@/types"
import { Plus, Loader2, ArrowDownCircle, ArrowUpCircle } from "lucide-react"

/**
 * Pagina de Transacoes
 *
 * Funcionalidades implementadas:
 * - Listagem de todas as transacoes cadastradas
 * - Criacao de nova transacao
 *
 * Regras de negocio:
 * - Descricao: maximo 400 caracteres
 * - Valor: deve ser positivo (> 0.01)
 * - Tipo: Despesa ou Receita
 * - Menores de 18 anos so podem ter despesas
 * - Categoria deve aceitar o tipo de transacao
 */
export function TransacoesPage() {
  // Estado da listagem
  const [transacoes, setTransacoes] = useState<TransacaoOutput[]>([])
  const [loading, setLoading] = useState(true)

  // Dados auxiliares para o formulario
  const [pessoas, setPessoas] = useState<PessoaOutput[]>([])
  const [categorias, setCategorias] = useState<CategoriaOutput[]>([])
  const [loadingAux, setLoadingAux] = useState(false)

  // Estado do formulario
  const [dialogOpen, setDialogOpen] = useState(false)
  const [formData, setFormData] = useState<TransacaoInput>({
    descricao: "",
    valor: 0,
    tipo: Finalidade.Despesa,
    categoriaId: "",
    pessoaId: "",
  })
  const [saving, setSaving] = useState(false)

  // Pessoa selecionada (para validacoes)
  const [pessoaSelecionada, setPessoaSelecionada] = useState<PessoaOutput | null>(null)

  const { toast } = useToast()

  /**
   * Carrega a lista de transacoes do backend
   */
  async function carregarTransacoes() {
    try {
      setLoading(true)
      const data = await transacoesService.obterTodas()
      setTransacoes(data)
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao carregar transacoes",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setLoading(false)
    }
  }

  /**
   * Carrega pessoas e categorias para o formulario
   */
  async function carregarDadosAuxiliares() {
    try {
      setLoadingAux(true)
      const [pessoasData, categoriasData] = await Promise.all([
        pessoasService.obterTodas(),
        categoriasService.obterTodas(),
      ])
      setPessoas(pessoasData)
      setCategorias(categoriasData)
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao carregar dados",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setLoadingAux(false)
    }
  }

  useEffect(() => {
    carregarTransacoes()
  }, [])

  /**
   * Abre o dialog para criar nova transacao
   */
  function handleNovaTransacao() {
    setFormData({
      descricao: "",
      valor: 0,
      tipo: Finalidade.Despesa,
      categoriaId: "",
      pessoaId: "",
    })
    setPessoaSelecionada(null)
    carregarDadosAuxiliares()
    setDialogOpen(true)
  }

  /**
   * Atualiza a pessoa selecionada e reseta o tipo se necessario
   */
  function handlePessoaChange(pessoaId: string) {
    const pessoa = pessoas.find((p) => p.id === pessoaId) || null
    setPessoaSelecionada(pessoa)

    // Se menor de idade, forca tipo Despesa
    if (pessoa && pessoa.idade < 18) {
      setFormData({ ...formData, pessoaId, tipo: Finalidade.Despesa })
    } else {
      setFormData({ ...formData, pessoaId })
    }
  }

  /**
   * Filtra categorias compativeis com o tipo de transacao selecionado
   */
  function getCategoriasCompativeis() {
    return categorias.filter((cat) => {
      // Categoria "Ambas" aceita qualquer tipo
      if (cat.finalidadeId === Finalidade.Ambas) return true
      // Categoria especifica deve corresponder ao tipo
      return cat.finalidadeId === formData.tipo
    })
  }

  /**
   * Cria a nova transacao
   */
  async function handleSalvar() {
    // Validacoes basicas
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

    if (formData.valor <= 0) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "O valor deve ser maior que zero",
      })
      return
    }

    if (!formData.pessoaId) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "Selecione uma pessoa",
      })
      return
    }

    if (!formData.categoriaId) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "Selecione uma categoria",
      })
      return
    }

    try {
      setSaving(true)
      await transacoesService.criar(formData)
      toast({
        title: "Transacao criada",
        description: "A transacao foi cadastrada com sucesso.",
      })
      setDialogOpen(false)
      carregarTransacoes()
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao criar transacao",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setSaving(false)
    }
  }

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
        title="Transacoes"
        description="Gerencie as transacoes financeiras"
      />

      <div className="flex-1 p-6">
        {/* Botao para adicionar nova transacao */}
        <div className="flex justify-end mb-4">
          <Button onClick={handleNovaTransacao}>
            <Plus className="h-4 w-4 mr-2" />
            Nova Transacao
          </Button>
        </div>

        {/* Tabela de transacoes */}
        <Card>
          <CardContent className="p-0">
            {loading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
              </div>
            ) : transacoes.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                Nenhuma transacao cadastrada
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-12">Tipo</TableHead>
                    <TableHead>Descricao</TableHead>
                    <TableHead>Pessoa</TableHead>
                    <TableHead>Categoria</TableHead>
                    <TableHead className="text-right">Valor</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {transacoes.map((transacao) => (
                    <TableRow key={transacao.id}>
                      <TableCell>
                        {transacao.tipo === Finalidade.Despesa ? (
                          <ArrowDownCircle className="h-5 w-5 text-red-500" />
                        ) : (
                          <ArrowUpCircle className="h-5 w-5 text-green-500" />
                        )}
                      </TableCell>
                      <TableCell className="font-medium">
                        {transacao.descricao}
                      </TableCell>
                      <TableCell>{transacao.pessoaNome}</TableCell>
                      <TableCell>{transacao.categoriaDescricao}</TableCell>
                      <TableCell
                        className={`text-right font-medium ${
                          transacao.tipo === Finalidade.Despesa
                            ? "text-red-600"
                            : "text-green-600"
                        }`}
                      >
                        {transacao.tipo === Finalidade.Despesa ? "-" : "+"}
                        {formatarValor(transacao.valor)}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      </div>

      {/* Dialog de criar transacao */}
      <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
        <DialogContent className="max-w-md">
          <DialogHeader>
            <DialogTitle>Nova Transacao</DialogTitle>
            <DialogDescription>
              Preencha os dados da nova transacao
            </DialogDescription>
          </DialogHeader>

          {loadingAux ? (
            <div className="flex items-center justify-center py-8">
              <Loader2 className="h-6 w-6 animate-spin" />
            </div>
          ) : (
            <div className="grid gap-4 py-4">
              {/* Pessoa */}
              <div className="grid gap-2">
                <Label htmlFor="pessoa">Pessoa</Label>
                <Select
                  value={formData.pessoaId}
                  onValueChange={handlePessoaChange}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecione a pessoa" />
                  </SelectTrigger>
                  <SelectContent>
                    {pessoas.map((pessoa) => (
                      <SelectItem key={pessoa.id} value={pessoa.id}>
                        {pessoa.nome} ({pessoa.idade} anos)
                        {pessoa.idade < 18 && " - Menor"}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                {pessoaSelecionada && pessoaSelecionada.idade < 18 && (
                  <span className="text-xs text-orange-600">
                    Menores de idade so podem ter despesas
                  </span>
                )}
              </div>

              {/* Tipo */}
              <div className="grid gap-2">
                <Label htmlFor="tipo">Tipo</Label>
                <Select
                  value={formData.tipo.toString()}
                  onValueChange={(value) => {
                    const novoTipo = parseInt(value) as Finalidade
                    // Reseta categoria ao mudar tipo (pois pode ser incompativel)
                    setFormData({ ...formData, tipo: novoTipo, categoriaId: "" })
                  }}
                  disabled={pessoaSelecionada !== null && pessoaSelecionada.idade < 18}
                >
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value={Finalidade.Despesa.toString()}>
                      {FinalidadeLabels[Finalidade.Despesa]} (Saida)
                    </SelectItem>
                    <SelectItem value={Finalidade.Receita.toString()}>
                      {FinalidadeLabels[Finalidade.Receita]} (Entrada)
                    </SelectItem>
                  </SelectContent>
                </Select>
              </div>

              {/* Categoria */}
              <div className="grid gap-2">
                <Label htmlFor="categoria">Categoria</Label>
                <Select
                  value={formData.categoriaId}
                  onValueChange={(value) =>
                    setFormData({ ...formData, categoriaId: value })
                  }
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecione a categoria" />
                  </SelectTrigger>
                  <SelectContent>
                    {getCategoriasCompativeis().map((categoria) => (
                      <SelectItem key={categoria.id} value={categoria.id}>
                        {categoria.descricao}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <span className="text-xs text-muted-foreground">
                  Apenas categorias compativeis com o tipo selecionado
                </span>
              </div>

              {/* Descricao */}
              <div className="grid gap-2">
                <Label htmlFor="descricao">Descricao</Label>
                <Input
                  id="descricao"
                  placeholder="Digite a descricao"
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

              {/* Valor */}
              <div className="grid gap-2">
                <Label htmlFor="valor">Valor (R$)</Label>
                <Input
                  id="valor"
                  type="number"
                  min={0.01}
                  step={0.01}
                  placeholder="0,00"
                  value={formData.valor || ""}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      valor: parseFloat(e.target.value) || 0,
                    })
                  }
                />
              </div>
            </div>
          )}

          <DialogFooter>
            <Button variant="outline" onClick={() => setDialogOpen(false)}>
              Cancelar
            </Button>
            <Button onClick={handleSalvar} disabled={saving || loadingAux}>
              {saving && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
              Criar
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
