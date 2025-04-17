import { ref, watch } from 'vue';

export const useCart = () => {
  const cartCount = ref(0);

  // Initialize cart count from sessionStorage
  const initializeCartCount = () => {
    try {
      const cart = JSON.parse(sessionStorage.getItem('cart') || '[]');
      cartCount.value = cart.length;
    } catch (error) {
      console.error('Error initializing cart count:', error);
      cartCount.value = 0;
    }
  };

  // Watch for changes in sessionStorage
  if (process.client) {
    window.addEventListener('storage', (e) => {
      if (e.key === 'cart') {
        initializeCartCount();
      }
    });
  }

  // Initialize on creation
  initializeCartCount();

  return {
    cartCount,
    updateCartCount: (count) => {
      cartCount.value = count;
    }
  };
}; 