import { defineNuxtPlugin } from '#app'

export default defineNuxtPlugin(async (nuxtApp) => {
  if (process.client) {
    try {
      // Import Popper.js correctly first
      const popperjs = await import('@popperjs/core')
      const { createPopper } = popperjs.default || popperjs

      // Then import Bootstrap
      const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js')
      
      // Initialize dropdowns
      const dropdownTriggerList = document.querySelectorAll('.dropdown-toggle')
      dropdownTriggerList.forEach(dropdownToggleEl => {
        new bootstrap.Dropdown(dropdownToggleEl, {
          offset: [0, 10],
          boundary: 'clippingParents'
        })
      })
      console.log('Bootstrap dropdowns initialized')
    } catch (err) {
      console.error('Failed to load Bootstrap:', err)
    }
  }
}) 