declare module '*.css'

declare module '*.scss'

declare module '*.sass'

declare module '@/mdx/search.mjs' {
  export type Result = {
    url: string
    title: string
    pageTitle?: string
  }

  export function search(
    query: string,
    options?: import('flexsearch').SearchOptions,
  ): Array<Result>
}
