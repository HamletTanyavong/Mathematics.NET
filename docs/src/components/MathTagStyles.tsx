'use client'

import { useEffect } from 'react'
import { debounce } from 'lodash'

export default function MathTagStyles() {
  useMathTagStyles()
  return null
}

function useMathTagStyles() {
  useEffect(() => {
    if (typeof window === 'undefined') return
    const debouncedHandleResize = debounce(handleResize, 100)
    debouncedHandleResize()
    window.addEventListener('resize', debouncedHandleResize)
    return () => window.removeEventListener('resize', debouncedHandleResize)
  }, [])
}

const handleResize = () => {
  const math = document.querySelectorAll('.katex-html')
  math.forEach((parent) => {
    // Not all equations have numbers; skip if they do not;
    const tag = parent.querySelector('.tag')
    if (!tag) return

    const container = parent.querySelector('.math-container')
    if (!container) return

    const parentWidth = parent.getBoundingClientRect().width;
    const containerWidth = container.getBoundingClientRect().width;
    const tagWidth = tag.getBoundingClientRect().width;

    const emSize = parseFloat(
      window.getComputedStyle(container).fontSize
    );

    if (containerWidth + tagWidth + 2 * emSize > parentWidth) {
      tag.classList.add('wide-math')
    } else {
      tag.classList.remove('wide-math')
    }
  })
}
