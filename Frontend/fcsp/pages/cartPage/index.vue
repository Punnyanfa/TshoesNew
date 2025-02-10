<template>
  <div class="container mt-5">
    <h1 class="text-center text-primary">Your Shopping Cart</h1>

    <div v-if="cart.length > 0">
      <ul class="list-group mt-4">
        <li v-for="item in cart" :key="item.id" class="list-group-item d-flex justify-content-between align-items-center">
          <span>{{ item.name }} - ${{ item.price }}</span>
          <button class="btn btn-danger btn-sm" @click="removeFromCart(item.id)">Remove</button>
        </li>
      </ul>

      <div class="d-flex justify-content-between align-items-center mt-3">
        <h4>Total: <strong>${{ totalPrice }}</strong></h4>
        <NuxtLink to="/checkout" class="btn btn-success">Proceed to Checkout</NuxtLink>
      </div>
    </div>

    <div v-else class="text-center mt-5">
      <h4>Your cart is empty!</h4>
      <NuxtLink to="/custom" class="btn btn-primary mt-3">Start Customizing</NuxtLink>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';

// 🛒 Load cart từ localStorage
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

// 🔄 Khởi tạo cart từ localStorage
onMounted(() => {
  const savedCart = localStorage.getItem('cart');
  if (savedCart) {
    cart.value = JSON.parse(savedCart);
  }
});
</script>
