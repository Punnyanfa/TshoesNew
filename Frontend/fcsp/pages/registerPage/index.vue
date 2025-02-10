<template>
  <div class="container">
    <h1 class="text-center text-primary">Register</h1>
    <form @submit.prevent="register">
      <div class="mb-3">
        <label class="form-label">Name</label>
        <input v-model="name" class="form-control" required />
      </div>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input v-model="email" type="email" class="form-control" required />
      </div>
      <div class="mb-3">
        <label class="form-label">Password</label>
        <input v-model="password" type="password" class="form-control" required />
      </div>
      <button type="submit" class="btn btn-success w-100">Sign Up</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const name = ref('');
const email = ref('');
const password = ref('');

const register = () => {
  console.log(`Registering ${name.value}`);
};
</script>
