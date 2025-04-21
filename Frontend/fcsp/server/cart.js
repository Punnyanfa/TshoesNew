import { defineStore } from 'pinia';

export const useCartStore = defineStore('cart', {
  state: () => ({
    items: [],
    totalPrice: 0,
    shippingCost: 0,
    totalPayment: 0
  }),

  actions: {
    addToCart(item) {
      const existingItem = this.items.find(i => i.id === item.id && i.selectedSize === item.selectedSize);
      if (existingItem) {
        existingItem.quantity += item.quantity || 1;
      } else {
        this.items.push({ ...item, quantity: item.quantity || 1 });
      }
      this.calculateTotals();
    },

    removeFromCart(itemId, selectedSize) {
      this.items = this.items.filter(i => !(i.id === itemId && i.selectedSize === selectedSize));
      this.calculateTotals();
    },

    updateQuantity(itemId, selectedSize, quantity) {
      const item = this.items.find(i => i.id === itemId && i.selectedSize === selectedSize);
      if (item) {
        item.quantity = quantity;
        this.calculateTotals();
      }
    },

    clearCart() {
      this.items = [];
      this.calculateTotals();
    },

    calculateTotals() {
      this.totalPrice = this.items.reduce((total, item) => total + (item.price * item.quantity), 0);
      // You can implement shipping cost calculation logic here
      this.shippingCost = 30000; // Default shipping cost
      this.totalPayment = this.totalPrice + this.shippingCost;
    }
  },

  getters: {
    cartItemCount: (state) => state.items.length,
    cartTotalItems: (state) => state.items.reduce((total, item) => total + item.quantity, 0)
  }
}); 