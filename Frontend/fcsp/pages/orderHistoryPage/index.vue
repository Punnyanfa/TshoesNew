<template>
  <div class="container">
    <h1 class="text-center text-primary">Order History</h1>
    <table class="table table-bordered">
      <thead>
        <tr>
          <th>Order ID</th>
          <th>Date</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.id }}</td>
          <td>{{ order.date }}</td>
          <td>{{ order.status }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
const orders = [
  { id: 1, date: '2024-02-01', status: 'Shipped' },
  { id: 2, date: '2024-02-02', status: 'Processing' },
];
</script>
