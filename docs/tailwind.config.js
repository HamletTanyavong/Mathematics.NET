/** @type {import('tailwindcss').Config} */
export const corePlugins = {
  preflight: false,
  container: false
};
export const content = ['./src/**/*.{tsx,html}'];
export const darkMode = ['class', '[data-theme="dark"]'];
export const theme = {
  extend: {}
};
export const plugins = [];
