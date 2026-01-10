import clsx from 'clsx'

const variantStyles = {
  small: '',
  medium: 'rounded-lg px-1.5 ring-1 ring-inset',
}

const colorStyles = {
  yellow: {
    small: 'text-yellow-500 dark:text-yellow-400',
    medium:
      'ring-yellow-300 dark:ring-yellow-400/30 bg-yellow-400/10 text-yellow-500 dark:text-yellow-400',
  },
  blue: {
    small: 'text-blue-500',
    medium:
      'ring-blue-300 bg-blue-400/10 text-blue-500 dark:ring-blue-400/30 dark:bg-blue-400/10 dark:text-blue-400',
  },
  rose: {
    small: 'text-red-500 dark:text-rose-500',
    medium:
      'ring-rose-200 bg-rose-50 text-red-500 dark:ring-rose-500/20 dark:bg-rose-400/10 dark:text-rose-400',
  },
  zinc: {
    small: 'text-zinc-400 dark:text-zinc-500',
    medium:
      'ring-zinc-200 bg-zinc-50 text-zinc-500 dark:ring-zinc-500/20 dark:bg-zinc-400/10 dark:text-zinc-400',
  },
}

const valueColorMap = {
  PROPERTY: 'yellow',
  FUNCTION: 'blue',
  OPERATOR: 'rose',
} as Record<string, keyof typeof colorStyles>

export function Tag({
  children,
  variant = 'medium',
  color = valueColorMap[children] ?? 'yellow',
}: {
  children: keyof typeof valueColorMap & (string | {})
  variant?: keyof typeof variantStyles
  color?: keyof typeof colorStyles
}) {
  return (
    <span
      className={clsx(
        'font-mono text-[0.625rem]/6 font-semibold',
        variantStyles[variant],
        colorStyles[color][variant],
      )}
    >
      {children}
    </span>
  )
}
