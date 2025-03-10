<template>
  <div class="container mt-5">
    <h1 class="text-center text-primary">Your Shopping Cart</h1>

    <div v-if="cart.length > 0">
      <!-- Giỏ hàng có sản phẩm -->
      <ul class="list-group mt-4">
        <li v-for="item in cart" :key="item.id" class="list-group-item d-flex justify-content-between align-items-center cart-item">
          <span>{{ item.name }} - ${{ item.price }}</span>
          <button class="btn btn-danger btn-sm" @click="removeFromCart(item.id)">Remove</button>
        </li>
      </ul>

      <!-- Tổng tiền và nút thanh toán -->
      <div class="d-flex justify-content-between align-items-center mt-3 cart-footer">
        <h4>Total: <strong>${{ totalPrice }}</strong></h4>
        <NuxtLink to="/checkout" class="btn btn-success checkout-btn">Proceed to Checkout</NuxtLink>
      </div>
    </div>

    <div v-else class="text-center mt-5 empty-cart">
      <h4>Your cart is empty!</h4>
      <NuxtLink to="/custom" class="btn btn-primary mt-3">Start Customizing</NuxtLink>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';

// 🛒 Dữ liệu giỏ hàng giả
const fakeCartData = [
  { id: 1, name: 'Sneaker A', price: 59.99 },
  { id: 2, name: 'Sneaker B', price: 79.99 },
  { id: 3, name: 'Sneaker C', price: 89.99 }
];

// 🛒 Load cart từ localStorage hoặc sử dụng dữ liệu giả
const cart = ref([]);

// Tính tổng tiền
const totalPrice = computed(() => cart.value.reduce((sum, item) => sum + item.price, 0));

// 🗑 Xóa sản phẩm khỏi giỏ hàng
const removeFromCart = (id) => {
  cart.value = cart.value.filter(item => item.id !== id);
};

// 🏗 Lưu giỏ hàng vào localStorage khi thay đổi
watch(cart, () => {
  localStorage.setItem('cart', JSON.stringify(cart.value));
}, { deep: true });

// 🔄 Khởi tạo cart từ localStorage hoặc dữ liệu giả
onMounted(() => {
  const savedCart = localStorage.getItem('cart');
  if (savedCart) {
    cart.value = JSON.parse(savedCart);
  } else {
    // Sử dụng dữ liệu giả nếu không có dữ liệu trong localStorage
    cart.value = fakeCartData;
  }
});
</script>

<style scoped>
/* 🌟 Cấu trúc và màu sắc */
.container {
  max-width: 800px;
  margin: 0 auto;
}

h1 {
  font-size: 2.5rem;
  font-weight: bold;
  color: #007bff;
  margin-bottom: 20px;
}

/* 🛒 Giỏ hàng */
.list-group-item {
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  margin-bottom: 10px;
  background-color: #fff;
  transition: background-color 0.3s ease;
}

.list-group-item:hover {
  background-color: #f1f1f1;
}

.cart-item span {
  font-size: 1rem;
  color: #333;
}

.cart-item button {
  transition: background-color 0.3s ease;
}

.cart-item button:hover {
  background-color: #e74c3c;
}

/* 📦 Phần footer giỏ hàng */
.cart-footer {
  background-color: #f8f9fa;
  padding: 15px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.cart-footer h4 {
  font-size: 1.25rem;
  font-weight: bold;
  color: #333;
}

.checkout-btn {
  background-color: #28a745;
  font-weight: bold;
  color: white;
  padding: 10px 20px;
  border-radius: 25px;
  transition: background-color 0.3s ease;
}

.checkout-btn:hover {
  background-color: #218838;
}

/* 🛒 Trường hợp giỏ hàng trống */
.empty-cart {
  color: #555;
}

.empty-cart h4 {
  font-size: 1.5rem;
  font-weight: bold;
  color: #e74c3c;
}

.empty-cart .btn {
  background-color: #3498db;
  color: white;
  border-radius: 25px;
  padding: 10px 20px;
  text-transform: uppercase;
  font-weight: bold;
  transition: background-color 0.3s ease;
}

.empty-cart .btn:hover {
  background-color: #2980b9;
}
</style>
