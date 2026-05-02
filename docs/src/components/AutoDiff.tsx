import { Button } from '@/components/Button'
import { Heading } from '@/components/Heading'

const topics = [
  {
    href: '/autodiff/reverse-mode-autodiff',
    name: 'Reverse-Mode AutoDiff',
    description: 'Learn how about reverse mode, automatic differentiation.',
  },
  {
    href: '/autodiff/forward-mode-autodiff',
    name: 'Forward-Mode AutoDiff',
    description: 'Learn how about forward mode, automatic differentiation.',
  },
]

export function AutoDiff() {
  return (
    <div className="my-16 xl:max-w-none">
      <Heading level={2} id="autodiff-topics">
        Topics
      </Heading>
      <div className="not-prose mt-4 grid grid-cols-1 gap-8 border-t border-zinc-900/5 pt-10 sm:grid-cols-2 xl:grid-cols-4 dark:border-white/5">
        {topics.map((topic) => (
          <div key={topic.href}>
            <h3 className="text-sm font-semibold text-zinc-900 dark:text-white">
              {topic.name}
            </h3>
            <p className="mt-1 text-sm text-zinc-600 dark:text-zinc-400">
              {topic.description}
            </p>
            <p className="mt-4">
              <Button href={topic.href} variant="text" arrow="right">
                Read more
              </Button>
            </p>
          </div>
        ))}
      </div>
    </div>
  )
}
