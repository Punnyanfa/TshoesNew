<template>
  <div class="container mt-5">
    <h1 class="text-center text-primary">Checkout</h1>

    <form class="mt-4">
      <div class="mb-3">
        <label class="form-label">Shipping Address</label>
        <input type="text" class="form-control" v-model="address" required />
      </div>

      <div class="mb-3">
        <label class="form-label">Payment Method</label>
        <select class="form-select" v-model="paymentMethod">
          <option value="credit">Credit Card</option>
          <option value="paypal">PayPal</option>
          <option value="cod">Cash on Delivery</option>
        </select>
      </div>

      <button type="button" class="btn btn-primary w-100" @click="placeOrder">Place Order</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const address = ref("");
const paymentMethod = ref("credit");

const placeOrder = () => {
  alert("Order placed successfully!");
};
</script>
