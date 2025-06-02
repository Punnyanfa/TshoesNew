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
    // Theo dõi thay đổi của cart trong localStorage
    window.addEventListener('storage', (e) => {
      const userId = localStorage.getItem("userId");
      if (e.key === `cart_${userId}`) {
        initializeCartCount();
      }
    });

    // Theo dõi thay đổi trực tiếp của localStorage
    const originalSetItem = localStorage.setItem;
    localStorage.setItem = function(key, value) {
      const userId = localStorage.getItem("userId");
      if (key === `cart_${userId}`) {
        const cart = JSON.parse(value || '[]');
        cartCount.value = cart.length;
      }
      originalSetItem.apply(this, arguments);
    };
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