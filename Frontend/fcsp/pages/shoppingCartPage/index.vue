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
                  <p class="text-muted small mb-0">...</p>
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
                  <span class="fw-bold">{{ (item.price * item.selectedQuantity).toLocaleString('vi-VN') }} đ</span>
                </td>
                <td>
                  <button 
                    class="delete-btn" 
                    @click="confirmDelete(item)" 
                    title="Delete"
                  > 
                    <i class="fas fa-trash-alt"></i>
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
            <span class="fw-bold">{{ selectedSubtotal.toLocaleString('vi-VN') }} đ</span>
          </div>
           <div class="d-flex justify-content-between mb-3">
            <span>Shipping:</span>
            <!-- Add logic for shipping cost based on selected items if needed -->
            <span class="fw-bold">{{ shippingCost.toLocaleString('vi-VN') }} đ</span> 
          </div>
          <hr>
          <div class="d-flex justify-content-between fw-bold fs-5">
            <span>Total:</span>
            <span>{{ selectedTotal.toLocaleString('vi-VN') }} đ</span>
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

    <client-only>
      <teleport to="body">
        <transition name="fade">
          <div v-if="showDeleteModal" class="modal-overlay">
            <div class="modal-wrapper delete-modal-wrapper">
              <div class="modal-container delete-modal-container">
                <div class="modal-header delete-modal-header">
                  <span class="delete-icon">
                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="#dc3545" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10" fill="#fff"/><path d="M15 9l-6 6M9 9l6 6"/></svg>
                  </span>
                  <h3>Confirm product deletion</h3>
                  <button class="modal-close" @click="showDeleteModal = false">×</button>
                </div>
                <div class="modal-body delete-modal-body">
                  <p v-if="selectedItem" class="delete-modal-title">Are you sure you want to delete the product<b>"{{ selectedItem.name }}"</b>?</p>
                  <p class="text-danger delete-modal-warning">This action cannot be undone.</p>
                </div>
                <div class="modal-footer delete-modal-footer">
                  <button class="btn-cancel" @click="showDeleteModal = false">Cancel</button>
                  <button class="btn-delete" @click="handleDelete">Delete</button>
                </div>
              </div>
            </div>
          </div>
        </transition>
      </teleport>
    </client-only>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useCart } from '~/composables/useCart';

const router = useRouter();
const { updateCartCount } = useCart();
const cartItems = ref([]);
const loading = ref(true);
// const shippingCost = ref(10.00);
const shippingCost = ref(30000);

// Use a Set to store unique IDs of selected items (e.g., 'id-size')
const selectedItems = ref(new Set());

const showDeleteModal = ref(false);
const selectedItem = ref(null);

// Fetch cart items from localStorage
onMounted(() => {
  loading.value = true;
  try {
    const userId = localStorage.getItem("userId");
    const storedCart = localStorage.getItem(`cart_${userId}`);
    if (storedCart) {
      cartItems.value = JSON.parse(storedCart);
      console.log('Cart loaded from localStorage:', cartItems.value);
      
      // Cập nhật số lượng sản phẩm trong giỏ hàng
      updateCartCount(cartItems.value.length);
    } else {
      cartItems.value = [];
      updateCartCount(0);
    }
  } catch (error) {
    console.error('Error loading cart from localStorage:', error);
    cartItems.value = []; // Clear cart on error
    updateCartCount(0);
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

// Function to persist cart changes to localStorage
const saveCartToLocal = () => {
  try {
    const userId = localStorage.getItem("userId");
    localStorage.setItem(`cart_${userId}`, JSON.stringify(cartItems.value));
    console.log('Cart saved to local.');
    
    // Cập nhật số lượng sản phẩm trong giỏ hàng
    updateCartCount(cartItems.value.length);
  } catch (error) {
    console.error('Error saving cart to localStorage:', error);
  }
};

const updateQuantity = (item, newQuantity) => {
  const quantity = parseInt(newQuantity);
  if (!isNaN(quantity) && quantity >= 1) {
    item.selectedQuantity = quantity;
    saveCartToLocal(); // Save changes
  } else if (!isNaN(quantity) && quantity <= 0) {
    removeItem(item);
  } else {
    console.warn('Invalid quantity input');
  }
};

const removeItem = (itemToRemove) => {
  // Filter based on a unique identifier (e.g., combining id and size)
  cartItems.value = cartItems.value.filter(item =>
    !(item.id === itemToRemove.id && item.selectedSize === itemToRemove.selectedSize)
  );
  saveCartToLocal(); // Save changes

  // Update cart count
  updateCartCount(cartItems.value.length);
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
  // Check if user is authenticated
  const token = localStorage.getItem('userToken');
  if (!token) {
    // If not authenticated, redirect to login page
    alert('Please login to view product details');
    router.push('/loginPage');
    return;
  }

  console.log('Cart Items before filtering:', cartItems.value);
  
  // Lọc ra các sản phẩm đã được chọn
  const selectedProducts = cartItems.value.filter(item => {
    const itemId = item.id + '-' + item.selectedSize;
    console.log('Checking item:', {
      itemId,
      customShoeDesignId: item.customShoeDesignId,
      isSelected: selectedItems.value.has(itemId)
    });
    return selectedItems.value.has(itemId);
  }).map(item => {
    console.log('Processing selected item:', {
      originalId: item.id,
      customShoeDesignId: item.customShoeDesignId || item.id,
      name: item.name
    });
    return {
      id: item.customShoeDesignId|| item.id,
      name: item.name,
      price: parseFloat(item.price),
      quantity: parseInt(item.selectedQuantity) || 1,
      selectedSize: item.selectedSize,
      image: item.previewImageUrl,
      total: parseFloat(item.price) * (parseInt(item.selectedQuantity) || 1)
    };
  });
  
  console.log('Selected Products after mapping:', selectedProducts);
  
  // Tính toán tổng tiền của các sản phẩm đã chọn
  const orderData = {
    items: selectedProducts,
    totalPrice: selectedSubtotal.value,
    shippingCost: shippingCost.value,
    totalPayment: selectedTotal.value
  };

  console.log('Final Order Data:', orderData);

  // Lưu thông tin đơn hàng vào localStorage
  const userId = localStorage.getItem("userId");
  localStorage.setItem(`orderData_${userId}`, JSON.stringify(orderData));
  
  // Chuyển hướng đến trang order
  router.push('/orderPage');
};

const confirmDelete = (item) => {
  selectedItem.value = item;
  showDeleteModal.value = true;
};

const handleDelete = () => {
  if (selectedItem.value) {
    removeItem(selectedItem.value);
    showDeleteModal.value = false;
    selectedItem.value = null;
  }
};
</script>

<style scoped>
.shopping-cart-page {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
}

.shopping-cart-page::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: radial-gradient(circle, rgba(255, 255, 255, 0.1) 10%, transparent 10%);
  background-size: 30px 30px;
  opacity: 0.5;
  z-index: 0;
}

.container {
  position: relative;
  z-index: 1;
}

h1 {
  font-size: 3.5rem;
  font-weight: 800;
  background: #AAAAAA;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: 1px;
  transition: transform 0.3s ease;
}

.cart-table th {
  font-weight: 600;
  color: #34495e;
  text-transform: uppercase;
  font-size: 0.9rem;
  border-bottom: 2px solid #AAAAAA;
}

.cart-item-img {
  max-width: 80px;
  height: auto;
  border-radius: 8px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
}

.cart-item-img:hover {
  transform: scale(1.05);
}

.quantity-input {
  max-width: 120px;
}

.quantity-input .form-control {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  background: #f9f9f9;
  transition: all 0.3s ease;
}

.quantity-input .form-control:focus {
  border-color: #AAAAAA;
  background: #fff;
  box-shadow: 0 0 10px rgba(170, 170, 170, 0.2);
  outline: none;
}

.quantity-input .btn {
  z-index: 1;
  border: 1px solid #e0e0e0;
  background: #f9f9f9;
  transition: all 0.3s ease;
}

.quantity-input .btn:hover {
  background: #AAAAAA;
  color: white;
  border-color: #AAAAAA;
}

.cart-summary {
  background: #ffffff;
  border-radius: 15px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
  border: 1px solid rgba(0, 0, 0, 0.05);
  animation: fadeInUp 0.5s ease;
}

.btn-primary {
  background-color: #AAAAAA;
  border: none;
  padding: 12px;
  border-radius: 8px;
  font-weight: 600;
  font-size: 1.1rem;
  transition: all 0.3s ease;
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
}

.btn-primary:hover {
  background-color: #888888;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(170, 170, 170, 0.4);
}

.btn-primary:active {
  transform: translateY(0);
  box-shadow: 0 3px 10px rgba(170, 170, 170, 0.2);
}

.btn-outline-danger {
  border: 1px solid #dc3545;
  color: #dc3545;
  transition: all 0.3s ease;
}

.btn-outline-danger:hover {
  background-color: #dc3545;
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(220, 53, 69, 0.3);
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 992px) {
  h1 {
    font-size: 2.5rem;
  }

  .cart-summary {
    padding: 20px;
  }

  .btn-primary {
    font-size: 1rem;
    padding: 10px;
  }
}

.delete-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
  background-color: #f44336;
  color: white;
}

.delete-btn:hover {
  background-color: #d32f2f;
}

.delete-btn i {
  font-size: 16px;
}

/* Delete Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100%; height: 100%;
  background-color: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99999;
}

.delete-modal-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100vw;
  height: 100vh;
}

.delete-modal-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(220,53,69,0.15), 0 1.5px 4px rgba(0,0,0,0.08);
  max-width: 380px;
  width: 100%;
  padding: 0;
  overflow: hidden;
  animation: popIn 0.25s cubic-bezier(.4,2,.6,1) both;
}

@keyframes popIn {
  0% { transform: scale(0.8); opacity: 0; }
  100% { transform: scale(1); opacity: 1; }
}

.delete-modal-header {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 20px 24px 10px 24px;
  border-bottom: 1px solid #f1f1f1;
  background: #fff;
}

.delete-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fff0f1;
  border-radius: 50%;
  padding: 8px;
  margin-right: 8px;
}

.delete-modal-header h3 {
  font-size: 20px;
  font-weight: 600;
  color: #dc3545;
  margin: 0;
  flex: 1;
}

.modal-close {
  background: none;
  border: none;
  font-size: 24px;
  color: #888;
  cursor: pointer;
  margin-left: 8px;
  transition: color 0.2s;
}

.modal-close:hover {
  color: #dc3545;
}

.delete-modal-body {
  padding: 18px 24px 0 24px;
  text-align: center;
}

.delete-modal-title {
  font-size: 16px;
  color: #333;
  margin-bottom: 8px;
}

.delete-modal-warning {
  font-size: 14px;
  color: #dc3545;
  margin-bottom: 0;
}

.delete-modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 18px 24px 20px 24px;
  background: #fff;
  border-top: 1px solid #f1f1f1;
}

.btn-cancel {
  background: #f1f1f1;
  color: #333;
  border: none;
  border-radius: 4px;
  padding: 8px 20px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-cancel:hover {
  background: #e0e0e0;
}

.btn-delete {
  background: #dc3545;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 8px 20px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
  box-shadow: 0 2px 8px rgba(220,53,69,0.08);
}

.btn-delete:hover {
  background: #b52a37;
}

/* Transition animations */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>