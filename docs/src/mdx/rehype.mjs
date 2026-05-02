import { slugifyWithCounter } from '@sindresorhus/slugify'
import * as acorn from 'acorn'
import { toString } from 'mdast-util-to-string'
import { mdxAnnotations } from 'mdx-annotations'
import shiki from 'shiki'
import { visit } from 'unist-util-visit'
import rehypeKatex from 'rehype-katex'
import rehypeMermaid from 'rehype-mermaid'

function rehypeParseCodeBlocks() {
  return (tree) => {
    visit(tree, 'element', (node, _nodeIndex, parentNode) => {
      if (node.tagName === 'code') {
        parentNode.properties.language = node.properties.className
          ? node.properties?.className[0]?.replace(/^language-/, '')
          : 'txt'
      }
    })
  }
}

let highlighter

function rehypeShiki() {
  return async (tree) => {
    highlighter =
      highlighter ?? (await shiki.getHighlighter({ theme: 'css-variables' }))

    visit(tree, 'element', (node) => {
      if (node.tagName === 'pre' && node.children[0]?.tagName === 'code') {
        let codeNode = node.children[0]
        let textNode = codeNode.children[0]

        // Skip processing for KaTeX content.
        // Adjust here if dollar signs continue to be an issue in non-KaTeX contexts.
        // For example, adding the condition: node.properties.language !== 'php'.
        const isKaTeX = textNode.value.startsWith('$') && textNode.value.endsWith('$')
        if (isKaTeX) {
          return
        }

        node.properties.code = textNode.value

        if (node.properties.language) {
          let tokens = highlighter.codeToThemedTokens(
            textNode.value,
            node.properties.language,
          )

          textNode.value = shiki.renderToHtml(tokens, {
            elements: {
              pre: ({ children }) => children,
              code: ({ children }) => children,
              line: ({ children }) => `<span>${children}</span>`,
            },
          })
        }
      }
    })
  }
}

function rehypeSlugify() {
  return (tree) => {
    let slugify = slugifyWithCounter()
    visit(tree, 'element', (node) => {
      if (node.tagName === 'h2' && !node.properties.id) {
        node.properties.id = slugify(toString(node))
      }
    })
  }
}

function rehypeAddMDXExports(getExports) {
  return (tree) => {
    let exports = Object.entries(getExports(tree))

    for (let [name, value] of exports) {
      for (let node of tree.children) {
        if (
          node.type === 'mdxjsEsm' &&
          new RegExp(`export\\s+const\\s+${name}\\s*=`).test(node.value)
        ) {
          return
        }
      }

      let exportStr = `export const ${name} = ${value}`

      tree.children.push({
        type: 'mdxjsEsm',
        value: exportStr,
        data: {
          estree: acorn.parse(exportStr, {
            sourceType: 'module',
            ecmaVersion: 'latest',
          }),
        },
      })
    }
  }
}

function getSections(node) {
  let sections = []

  for (let child of node.children ?? []) {
    if (child.type === 'element' && child.tagName === 'h2') {
      sections.push(`{
        title: ${JSON.stringify(toString(child))},
        id: ${JSON.stringify(child.properties.id)},
        ...${child.properties.annotation}
      }`)
    } else if (child.children) {
      sections.push(...getSections(child))
    }
  }

  return sections
}

export const rehypePlugins = [
  [rehypeKatex, {
    displayMode: true,
    strict: true,
    errorColor: '#ff0000',
    trust: true,
    macros: {
      '\\dd': '\\,\\mathrm{d}#1',
      '\\dv': '\\frac{\\mathrm{d}#1}{\\mathrm{d}#2}',
      '\\pdv': '\\frac{\\partial#1}{\\partial#2}',
      '\\atan': '\\mathrm{atan2}\\left(\\frac{#1}{#2}\\right)'
    }
  }],
  [
    rehypeMermaid,
    {
      mermaidConfig: {
        theme: 'base',
        securityLevel: 'strict',
        themeVariables: {
          darkMode: true,
          primaryColor: '#1d1d20',
          edgeLabelBackground: '#1d1d20',
          fontFamily: 'JetBrains Mono',
          fontSize: 12
        },
        themeCSS: `
          .flowchart-link, .marker {
            stroke: var(--tw-prose-body);
          }

          .marker {
            fill: var(--tw-prose-body);
          }
        `
      }
    }
  ],
  mdxAnnotations.rehype,
  rehypeParseCodeBlocks,
  rehypeShiki,
  rehypeSlugify,
  [
    rehypeAddMDXExports,
    (tree) => ({
      sections: `[${getSections(tree).join()}]`,
    }),
  ],
]
