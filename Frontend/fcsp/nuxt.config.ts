export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: true },
  build: {
    transpile: ['swiper', 'element-plus/es', '@popperjs/core']
  },
  css: [
    '~/assets/css/swiper.css',
    '~/assets/css/style.css',
    'element-plus/dist/index.css',
    'bootstrap/dist/css/bootstrap.min.css',
    'bootstrap-icons/font/bootstrap-icons.css'
  ],
  modules: [
    '@pinia/nuxt',
    '@pinia-plugin-persistedstate/nuxt'
  ],
  plugins: [
    '~/plugins/element-plus.ts',
    '~/plugins/bootstrap.client.ts'
  ],
  app: {
    head: {
      script: [
        {
          src: 'https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js',
          defer: true
        },
        {
          src: 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js',
          defer: true
        }
      ]
    }
  },
  vite: {
    optimizeDeps: {
      include: ['element-plus', '@element-plus/icons-vue', '@popperjs/core']
    },
    ssr: {
      noExternal: ['bootstrap']
    }
  }
})
