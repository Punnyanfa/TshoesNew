<template>
  <div class="container mt-5">
    <h1 class="text-primary text-center">Forgot Password</h1>

    <form @submit.prevent="resetPassword" class="mt-4">
      <div class="mb-3">
        <label class="form-label">Email Address</label>
        <input type="email" class="form-control" v-model="email" required />
      </div>

      <button type="submit" class="btn btn-primary w-100">Reset Password</button>
    </form>

    <p v-if="message" class="text-success mt-3">{{ message }}</p>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const email = ref("");
const message = ref("");

const resetPassword = () => {
  console.log("Password reset requested for:", email.value);
  message.value = "A password reset link has been sent to your email.";
};
</script>
