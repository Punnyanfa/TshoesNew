import { defineNuxtPlugin } from '#app'

export default defineNuxtPlugin((nuxtApp) => {
  if (process.client) {
    // Dynamically import Bootstrap JS
    import('bootstrap/dist/js/bootstrap.bundle.min.js').then((bootstrap) => {
      // Initialize dropdowns
      const dropdownTriggerList = document.querySelectorAll('.dropdown-toggle')
      dropdownTriggerList.forEach(dropdownToggleEl => {
        new bootstrap.Dropdown(dropdownToggleEl, {
          offset: [0, 10],
          boundary: 'clippingParents'
        })
      })
      console.log('Bootstrap dropdowns initialized')
    }).catch(err => {
      console.error('Failed to load Bootstrap:', err)
    })
  }
}) 