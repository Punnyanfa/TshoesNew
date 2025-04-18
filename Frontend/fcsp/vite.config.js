import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'
import { TresVitePlugin } from '@tresjs/core'

export default defineConfig({
  plugins: [
    vue(),
    TresVitePlugin()
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      'three': 'three',
      '@tresjs/core': '@tresjs/core',
      '@tresjs/cientos': '@tresjs/cientos'
    }
  },
  optimizeDeps: {
    include: ['three', '@tresjs/core', '@tresjs/cientos']
  }
}) 