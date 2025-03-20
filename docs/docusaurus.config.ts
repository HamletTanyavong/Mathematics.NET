import { themes as prismThemes } from 'prism-react-renderer';
import type { Config } from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import remarkMath from 'remark-math';
import rehypeKatex from 'rehype-katex';
import tailwindPlugin from './plugins/tailwind-plugin.cjs';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'Mathematics.NET',
  tagline: 'Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems.',
  favicon: 'img/mathematics.net.ico',

  url: 'https://mathematics.hamlettanyavong.com',
  baseUrl: '/',

  organizationName: 'HamletTanyavong',
  projectName: 'Mathematics.NET',
  trailingSlash: false,

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  i18n: {
    defaultLocale: 'en',
    locales: ['en']
  },

  plugins: [tailwindPlugin],

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
          editUrl: 'https://github.com/HamletTanyavong/Mathematics.NET/docs/docs',
          remarkPlugins: [remarkMath],
          rehypePlugins: [rehypeKatex]
        },
        blog: {
          showReadingTime: true,
          feedOptions: {
            type: ['rss', 'atom'],
            xslt: true
          },
          onInlineTags: 'warn',
          onInlineAuthors: 'warn',
          onUntruncatedBlogPosts: 'warn'
        },
        theme: {
          customCss: './src/css/custom.css'
        }
      } satisfies Preset.Options
    ]
  ],

  stylesheets: [
    {
      href: 'https://cdn.jsdelivr.net/npm/katex@0.13.24/dist/katex.min.css',
      type: 'text/css',
      integrity: 'sha384-odtC+0UGzzFL/6PNoE8rX/SPcQDXBJ+uRepguP4QkPCm2LBxH3FA3y+fKSiJ+AmM',
      crossorigin: 'anonymous'
    }
  ],

  themeConfig: {
    image: 'img/mathematics.net-social-card.png',
    navbar: {
      title: 'Mathematics.NET',
      logo: {
        alt: 'Mathematics.NET Logo',
        src: 'img/mathematics.net.svg'
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'docsSidebar',
          position: 'left',
          label: 'Docs'
        },
        { label: 'Blog', to: '/blog', position: 'left' },
        {
          href: 'https://github.com/HamletTanyavong/Mathematics.NET',
          label: 'GitHub',
          position: 'right'
        }
      ],
      hideOnScroll: true
    },
    tableOfContents: {
      minHeadingLevel: 2,
      maxHeadingLevel: 4
    },
    docs: {
      sidebar: {
        hideable: true,
        autoCollapseCategories: true
      }
    },
    footer: {
      links: [
        {
          title: 'Docs',
          items: [
            {
              label: 'Introduction',
              to: '/docs/introduction'
            }
          ]
        },
        {
          title: 'Community',
          items: [
            {
              label: 'Discord',
              href: 'https://discordapp.com/invite/haqS9TVK8B'
            }
          ]
        },
        {
          title: 'More',
          items: [
            {
              label: 'Blog',
              to: '/blog'
            },
            {
              label: 'GitHub',
              href: 'https://github.com/HamletTanyavong/Mathematics.NET'
            }
          ]
        }
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Hamlet Tanyavong`
    },

    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.vsDark,
      additionalLanguages: ['csharp']
    },

    mermaid: {
      theme: {
        light: 'default',
        dark: 'dark'
      }
    }
  } satisfies Preset.ThemeConfig,

  markdown: {
    mermaid: true
  },
  themes: ['@docusaurus/theme-mermaid']
};

export default config;
