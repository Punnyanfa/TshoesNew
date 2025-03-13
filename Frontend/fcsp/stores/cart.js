import { defineStore } from 'pinia'

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: []
  }),
  
  getters: {
    totalItems: (state) => state.items.length,
    totalAmount: (state) => state.items.reduce((total, item) => {
      return total + (parseFloat(item.price.replace(/[^\d]/g, '')) * item.quantity)
    }, 0)
  },
  
  actions: {
    addItem(item) {
      const existingItem = this.items.find(i => i.id === item.id)
      if (existingItem) {
        existingItem.quantity += item.quantity
      } else {
        this.items.push(item)
      }
    },
    
    removeItem(itemId) {
      const index = this.items.findIndex(item => item.id === itemId)
      if (index > -1) {
        this.items.splice(index, 1)
      }
    },
    
    updateQuantity(itemId, quantity) {
      const item = this.items.find(i => i.id === itemId)
      if (item) {
        item.quantity = quantity
      }
    },
    
    clearCart() {
      this.items = []
    }
  },
  
  persist: true
}) 