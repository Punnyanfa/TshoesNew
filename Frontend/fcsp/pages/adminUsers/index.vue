<template>
  <div class="container mt-5">
    <h1 class="text-primary">Manage Users</h1>

    <table class="table mt-4">
      <thead>
        <tr>
          <th>User ID</th>
          <th>Name</th>
          <th>Email</th>
          <th>Role</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td>{{ user.id }}</td>
          <td>{{ user.name }}</td>
          <td>{{ user.email }}</td>
          <td>{{ user.role }}</td>
          <td>
            <button class="btn btn-warning btn-sm">Edit</button>
            <button class="btn btn-danger btn-sm ms-2">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const users = ref([
  { id: 1, name: "Admin", email: "admin@example.com", role: "Admin" },
  { id: 2, name: "User One", email: "user1@example.com", role: "Customer" }
]);
</script>
