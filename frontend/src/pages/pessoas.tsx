import { useState, useEffect } from "react"
import { Header } from "@/components/layout"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
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
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog"
import { Card, CardContent } from "@/components/ui/card"
import { useToast } from "@/hooks/use-toast"
import { pessoasService } from "@/services"
import type { PessoaOutput, PessoaInput } from "@/types"
import { Plus, Pencil, Trash2, Loader2 } from "lucide-react"

/**
 * Pagina de Pessoas
 *
 * Funcionalidades implementadas:
 * - Listagem de todas as pessoas cadastradas
 * - Criacao de nova pessoa (nome e idade)
 * - Edicao de pessoa existente
 * - Exclusao de pessoa (com confirmacao)
 *
 * Regras de negocio:
 * - Nome: maximo 200 caracteres
 * - Idade: 0 a 150 anos
 * - Ao deletar pessoa, todas as transacoes sao removidas
 */
export function PessoasPage() {
  // Estado da listagem
  const [pessoas, setPessoas] = useState<PessoaOutput[]>([])
  const [loading, setLoading] = useState(true)

  // Estado do formulario (criar/editar)
  const [dialogOpen, setDialogOpen] = useState(false)
  const [editingPessoa, setEditingPessoa] = useState<PessoaOutput | null>(null)
  const [formData, setFormData] = useState<PessoaInput>({ nome: "", idade: 0 })
  const [saving, setSaving] = useState(false)

  // Estado da confirmacao de exclusao
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false)
  const [deletingPessoa, setDeletingPessoa] = useState<PessoaOutput | null>(null)
  const [deleting, setDeleting] = useState(false)

  const { toast } = useToast()

  /**
   * Carrega a lista de pessoas do backend
   */
  async function carregarPessoas() {
    try {
      setLoading(true)
      const data = await pessoasService.obterTodas()
      setPessoas(data)
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao carregar pessoas",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setLoading(false)
    }
  }

  // Carrega pessoas ao montar o componente
  useEffect(() => {
    carregarPessoas()
  }, [])

  /**
   * Abre o dialog para criar nova pessoa
   */
  function handleNovaPessoa() {
    setEditingPessoa(null)
    setFormData({ nome: "", idade: 0 })
    setDialogOpen(true)
  }

  /**
   * Abre o dialog para editar pessoa existente
   */
  function handleEditarPessoa(pessoa: PessoaOutput) {
    setEditingPessoa(pessoa)
    setFormData({ nome: pessoa.nome, idade: pessoa.idade })
    setDialogOpen(true)
  }

  /**
   * Abre o dialog de confirmacao para excluir pessoa
   */
  function handleConfirmarExclusao(pessoa: PessoaOutput) {
    setDeletingPessoa(pessoa)
    setDeleteDialogOpen(true)
  }

  /**
   * Salva a pessoa (cria ou atualiza)
   */
  async function handleSalvar() {
    // Validacao basica no frontend
    if (!formData.nome.trim()) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "O nome e obrigatorio",
      })
      return
    }

    if (formData.nome.length > 200) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "O nome deve ter no maximo 200 caracteres",
      })
      return
    }

    if (formData.idade < 0 || formData.idade > 150) {
      toast({
        variant: "destructive",
        title: "Erro de validacao",
        description: "A idade deve estar entre 0 e 150 anos",
      })
      return
    }

    try {
      setSaving(true)

      if (editingPessoa) {
        // Atualizar pessoa existente
        await pessoasService.atualizar(editingPessoa.id, formData)
        toast({
          title: "Pessoa atualizada",
          description: `${formData.nome} foi atualizado(a) com sucesso.`,
        })
      } else {
        // Criar nova pessoa
        await pessoasService.criar(formData)
        toast({
          title: "Pessoa criada",
          description: `${formData.nome} foi cadastrado(a) com sucesso.`,
        })
      }

      setDialogOpen(false)
      carregarPessoas()
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao salvar",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setSaving(false)
    }
  }

  /**
   * Exclui a pessoa selecionada
   */
  async function handleExcluir() {
    if (!deletingPessoa) return

    try {
      setDeleting(true)
      await pessoasService.remover(deletingPessoa.id)
      toast({
        title: "Pessoa excluida",
        description: `${deletingPessoa.nome} e todas as suas transacoes foram removidos.`,
      })
      setDeleteDialogOpen(false)
      setDeletingPessoa(null)
      carregarPessoas()
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Erro ao excluir",
        description: error instanceof Error ? error.message : "Erro desconhecido",
      })
    } finally {
      setDeleting(false)
    }
  }

  return (
    <div className="flex flex-col h-full">
      <Header
        title="Pessoas"
        description="Gerencie as pessoas cadastradas no sistema"
      />

      <div className="flex-1 p-6">
        {/* Botao para adicionar nova pessoa */}
        <div className="flex justify-end mb-4">
          <Button onClick={handleNovaPessoa}>
            <Plus className="h-4 w-4 mr-2" />
            Nova Pessoa
          </Button>
        </div>

        {/* Tabela de pessoas */}
        <Card>
          <CardContent className="p-0">
            {loading ? (
              <div className="flex items-center justify-center py-8">
                <Loader2 className="h-6 w-6 animate-spin text-muted-foreground" />
              </div>
            ) : pessoas.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                Nenhuma pessoa cadastrada
              </div>
            ) : (
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Nome</TableHead>
                    <TableHead className="w-24">Idade</TableHead>
                    <TableHead className="w-24">Tipo</TableHead>
                    <TableHead className="w-32 text-right">Acoes</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {pessoas.map((pessoa) => (
                    <TableRow key={pessoa.id}>
                      <TableCell className="font-medium">{pessoa.nome}</TableCell>
                      <TableCell>{pessoa.idade} anos</TableCell>
                      <TableCell>
                        {pessoa.idade < 18 ? (
                          <span className="text-orange-600 text-sm">Menor</span>
                        ) : (
                          <span className="text-green-600 text-sm">Adulto</span>
                        )}
                      </TableCell>
                      <TableCell className="text-right">
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => handleEditarPessoa(pessoa)}
                          title="Editar"
                        >
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => handleConfirmarExclusao(pessoa)}
                          title="Excluir"
                        >
                          <Trash2 className="h-4 w-4 text-destructive" />
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>
        </Card>
      </div>

      {/* Dialog de criar/editar pessoa */}
      <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>
              {editingPessoa ? "Editar Pessoa" : "Nova Pessoa"}
            </DialogTitle>
            <DialogDescription>
              {editingPessoa
                ? "Altere os dados da pessoa abaixo"
                : "Preencha os dados da nova pessoa"}
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="nome">Nome</Label>
              <Input
                id="nome"
                placeholder="Digite o nome"
                maxLength={200}
                value={formData.nome}
                onChange={(e) =>
                  setFormData({ ...formData, nome: e.target.value })
                }
              />
              <span className="text-xs text-muted-foreground">
                {formData.nome.length}/200 caracteres
              </span>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="idade">Idade</Label>
              <Input
                id="idade"
                type="number"
                min={0}
                max={150}
                value={formData.idade}
                onChange={(e) =>
                  setFormData({ ...formData, idade: parseInt(e.target.value) || 0 })
                }
              />
              {formData.idade < 18 && (
                <span className="text-xs text-orange-600">
                  Menores de 18 anos so podem ter despesas
                </span>
              )}
            </div>
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setDialogOpen(false)}>
              Cancelar
            </Button>
            <Button onClick={handleSalvar} disabled={saving}>
              {saving && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
              {editingPessoa ? "Salvar" : "Criar"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Dialog de confirmacao de exclusao */}
      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Confirmar exclusao</AlertDialogTitle>
            <AlertDialogDescription>
              Tem certeza que deseja excluir <strong>{deletingPessoa?.nome}</strong>?
              <br />
              <br />
              <span className="text-destructive font-medium">
                Atencao: Todas as transacoes dessa pessoa tambem serao excluidas.
                Esta acao nao pode ser desfeita.
              </span>
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancelar</AlertDialogCancel>
            <AlertDialogAction
              onClick={handleExcluir}
              disabled={deleting}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              {deleting && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
              Excluir
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  )
}
