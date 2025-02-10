<template>
  <div class="container">
    <h1 class="text-center text-primary">Login</h1>
    <form @submit.prevent="login">
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input v-model="email" type="email" class="form-control" required />
      </div>
      <div class="mb-3">
        <label class="form-label">Password</label>
        <input v-model="password" type="password" class="form-control" required />
      </div>
      <button type="submit" class="btn btn-primary w-100">Login</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const email = ref('');
const password = ref('');

const login = () => {
  console.log(`Logging in with ${email.value}`);
  // Handle login logic
};
</script>
