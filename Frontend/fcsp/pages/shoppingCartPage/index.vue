<template>
  <div class="shopping-cart-page">
    <Header />
    <section class="container my-5">
      <h1 class="text-center mb-4">Shopping Cart</h1>

      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>

      <div v-else-if="cartItems.length === 0" class="text-center py-5">
        <p>Your shopping cart is empty.</p>
        <NuxtLink to="/productPage" class="btn btn-primary">Continue Shopping</NuxtLink>
      </div>

      <div v-else class="cart-content">
        <div class="table-responsive">
          <table class="table align-middle cart-table">
            <thead>
              <tr>
                <th scope="col" style="width: 5%;">
                  <input class="form-check-input" type="checkbox" 
                         :checked="isAllSelected" 
                         @change="toggleSelectAll">
                </th>
                <th scope="col" style="width: 15%;">Image</th>
                <th scope="col" style="width: 30%;">Product</th>
                <th scope="col" style="width: 10%;">Size</th>
                <th scope="col" style="width: 15%;">Quantity</th>
                <th scope="col" style="width: 15%;">Price</th>
                <th scope="col" style="width: 10%;">Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in cartItems" :key="item.id + '-' + item.selectedSize || index">
                 <td>
                  <input class="form-check-input" type="checkbox" 
                         :checked="isSelected(item)" 
                         @change="toggleItemSelection(item)">
                </td>
                <td>
                  <img :src="item.previewImageUrl || '/placeholder.png'" 
                       :alt="item.name" 
                       class="img-fluid cart-item-img">
                </td>
                 <td>
                  <h5 class="mb-1">{{ item.name }}</h5>
                  <p class="text-muted small mb-0">{{ item.description?.substring(0, 50) }}...</p>
                </td>
                <td>
                  <span class="badge bg-secondary">{{ item.selectedSize }}</span>
                </td>
                 <td>
                  <div class="input-group quantity-input">
                    <button class="btn btn-outline-secondary btn-sm" type="button" @click="updateQuantity(item, item.selectedQuantity - 1)">-</button>
                    <input type="number" class="form-control form-control-sm text-center" 
                           :value="item.selectedQuantity" 
                           @change="updateQuantity(item, parseInt($event.target.value))" 
                           min="1">
                    <button class="btn btn-outline-secondary btn-sm" type="button" @click="updateQuantity(item, item.selectedQuantity + 1)">+</button>
                  </div>
                </td>
                 <td>
                  <span class="fw-bold">${{ (item.price * item.selectedQuantity).toFixed(2) }}</span>
                </td>
                <td>
                  <button class="btn btn-sm btn-outline-danger" @click="removeItem(item)">
                    <i class="bi bi-trash">xóa</i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="cart-summary mt-4 p-4 bg-light rounded">
          <h4 class="mb-3">Summary ({{ selectedItems.size }} item{{ selectedItems.size !== 1 ? 's' : '' }} selected)</h4>
          <div class="d-flex justify-content-between mb-2">
            <span>Subtotal:</span>
            <span class="fw-bold">${{ selectedSubtotal.toFixed(2) }}</span>
          </div>
           <div class="d-flex justify-content-between mb-3">
            <span>Shipping:</span>
            <!-- Add logic for shipping cost based on selected items if needed -->
            <span class="fw-bold">${{ shippingCost.toFixed(2) }}</span> 
          </div>
          <hr>
          <div class="d-flex justify-content-between fw-bold fs-5">
            <span>Total:</span>
            <span>${{ selectedTotal.toFixed(2) }}</span>
          </div>
          <div class="d-grid mt-4">
            <button class="btn btn-primary btn-lg" 
                    :disabled="selectedItems.size === 0" 
                    @click="proceedToCheckout">
              Proceed to Checkout
            </button>
          </div>
        </div>
      </div>
    </section>
    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const cartItems = ref([]);
const loading = ref(true);
const shippingCost = ref(10.00);

// Use a Set to store unique IDs of selected items (e.g., 'id-size')
const selectedItems = ref(new Set());

// Fetch cart items from sessionStorage
onMounted(() => {
  loading.value = true;
  try {
    const storedCart = sessionStorage.getItem('cart');
    cartItems.value = storedCart ? JSON.parse(storedCart) : [];
    console.log('Cart loaded from session:', cartItems.value);
  } catch (error) {
    console.error('Error loading cart from sessionStorage:', error);
    cartItems.value = []; // Clear cart on error
  } finally {
    loading.value = false;
  }
});

const cartSubtotal = computed(() => {
  return cartItems.value.reduce((sum, item) => {
    // Ensure price and quantity are numbers
    const price = parseFloat(item.price) || 0;
    const quantity = parseInt(item.selectedQuantity) || 0;
    return sum + price * quantity;
  }, 0);
});

const cartTotal = computed(() => {
  return cartSubtotal.value + shippingCost.value;
});

// Function to persist cart changes to sessionStorage
const saveCartToSession = () => {
  try {
    sessionStorage.setItem('cart', JSON.stringify(cartItems.value));
    console.log('Cart saved to session.');
  } catch (error) {
    console.error('Error saving cart to sessionStorage:', error);
  }
};

const updateQuantity = (item, newQuantity) => {
  const quantity = parseInt(newQuantity);
  if (!isNaN(quantity) && quantity >= 1) {
    item.selectedQuantity = quantity;
    saveCartToSession(); // Save changes
  } else if (!isNaN(quantity) && quantity <= 0) {
    // Optionally remove item if quantity is 0 or less, or just set minimum to 1
    removeItem(item);
  } else {
     // Handle invalid input if needed
     console.warn('Invalid quantity input');
  }
};

const removeItem = (itemToRemove) => {
  // Filter based on a unique identifier (e.g., combining id and size)
  cartItems.value = cartItems.value.filter(item =>
    !(item.id === itemToRemove.id && item.selectedSize === itemToRemove.selectedSize)
  );
  saveCartToSession(); // Save changes
};

// Computed property to check if all items are selected
const isAllSelected = computed(() => {
  return cartItems.value.length > 0 && selectedItems.value.size === cartItems.value.length;
});

// Check if a specific item is selected
const isSelected = (item) => {
  const itemId = item.id + '-' + item.selectedSize; // Create unique key
  return selectedItems.value.has(itemId);
};

// Toggle selection of all items
const toggleSelectAll = () => {
  const allSelected = isAllSelected.value;
  selectedItems.value.clear(); // Clear first
  if (!allSelected) {
    cartItems.value.forEach(item => {
      selectedItems.value.add(item.id + '-' + item.selectedSize);
    });
  }
  // No need to explicitly set isAllSelected, computed property handles it
};

// Toggle selection of a single item
const toggleItemSelection = (item) => {
  const itemId = item.id + '-' + item.selectedSize;
  if (selectedItems.value.has(itemId)) {
    selectedItems.value.delete(itemId);
  } else {
    selectedItems.value.add(itemId);
  }
};

// Computed property for subtotal of selected items
const selectedSubtotal = computed(() => {
  return cartItems.value.reduce((sum, item) => {
    const itemId = item.id + '-' + item.selectedSize;
    if (selectedItems.value.has(itemId)) {
      const price = parseFloat(item.price) || 0;
      const quantity = parseInt(item.selectedQuantity) || 0;
      return sum + price * quantity;
    }
    return sum;
  }, 0);
});

// Computed property for total of selected items
const selectedTotal = computed(() => {
  // Adjust shipping cost based on selected items if necessary
  return selectedItems.value.size > 0 ? selectedSubtotal.value + shippingCost.value : 0;
});

// Function to handle proceeding to checkout
const proceedToCheckout = () => {
  // Lọc ra các sản phẩm đã được chọn
  const selectedProducts = cartItems.value.filter(item => {
    const itemId = item.id + '-' + item.selectedSize;
    return selectedItems.value.has(itemId);
  }).map(item => ({
    id: item.id,
    name: item.name,
    price: parseFloat(item.price),
    quantity: parseInt(item.selectedQuantity) || 1,
    selectedSize: item.selectedSize,
    image: item.previewImageUrl,
    total: parseFloat(item.price) * (parseInt(item.selectedQuantity) || 1)
  }));
  
  // Tính toán tổng tiền của các sản phẩm đã chọn
  const orderData = {
    items: selectedProducts,
    totalPrice: selectedSubtotal.value,
    shippingCost: shippingCost.value,
    totalPayment: selectedTotal.value
  };

  // Lưu thông tin đơn hàng vào sessionStorage
  sessionStorage.setItem('orderData', JSON.stringify(orderData));
  
  // Chuyển hướng đến trang order
  router.push('/orderPage');
};
</script>

<style scoped>
.shopping-cart-page {
  background: #f8f9fa;
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
}

.cart-table th {
  font-weight: 600;
  color: #495057;
  text-transform: uppercase;
  font-size: 0.9rem;
  background-color: #e9ecef;
}

.cart-item-img {
  max-width: 80px;
  height: auto;
  border-radius: 4px;
}

.quantity-input {
  max-width: 120px;
}

.quantity-input .form-control {
  border-left: none;
  border-right: none;
}

.quantity-input .btn {
  z-index: 1;
}

.cart-summary {
  border: 1px solid #dee2e6;
}
</style>
