<template>
  <div class="container mt-5">
    <h1 class="text-primary">Manage Orders</h1>

    <table class="table mt-4">
      <thead>
        <tr>
          <th>Order ID</th>
          <th>Customer</th>
          <th>Total</th>
          <th>Status</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.id }}</td>
          <td>{{ order.customer }}</td>
          <td>${{ order.total }}</td>
          <td>{{ order.status }}</td>
          <td>
            <button class="btn btn-primary btn-sm">Update</button>
            <button class="btn btn-danger btn-sm ms-2">Cancel</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const orders = ref([
  { id: "12345", customer: "John Doe", total: 150, status: "Shipped" },
  { id: "67890", customer: "Jane Smith", total: 200, status: "Pending" }
]);
</script>