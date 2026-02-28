import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

/**
 * Utilitario para combinar classes CSS do Tailwind
 * Usado pelos componentes shadcn/ui para merge de classes condicionais
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
