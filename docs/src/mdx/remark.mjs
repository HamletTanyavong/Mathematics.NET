import { mdxAnnotations } from 'mdx-annotations'
import remarkGfm from 'remark-gfm'
import remarkMath from 'remark-math'

export const remarkPlugins = [
  remarkMath,
  mdxAnnotations.remark,
  remarkGfm,
]
