import { ref, watch } from 'vue';

export const useCart = () => {
  const cartCount = ref(0);

  // Initialize cart count from localStorage
  const initializeCartCount = () => {
    try {
      const userId = localStorage.getItem("userId");
      const cart = JSON.parse(localStorage.getItem(`cart_${userId}`) || '[]');
      cartCount.value = cart.length;
    } catch (error) {
      console.error('Error initializing cart count:', error);
      cartCount.value = 0;
    }
  };

  // Watch for changes in localStorage
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