<template>
  <div class="container mt-5">
    <h1 class="text-primary text-center">Customer Support</h1>
    <p class="text-center">Need help? Fill out the form below and we'll get back to you.</p>

    <form @submit.prevent="submitSupport" class="mt-4">
      <div class="mb-3">
        <label class="form-label">Full Name</label>
        <input type="text" class="form-control" v-model="form.name" required />
      </div>

      <div class="mb-3">
        <label class="form-label">Email</label>
        <input type="email" class="form-control" v-model="form.email" required />
      </div>

      <div class="mb-3">
        <label class="form-label">Message</label>
        <textarea class="form-control" v-model="form.message" rows="4" required></textarea>
      </div>

      <button type="submit" class="btn btn-primary w-100">Submit Request</button>
    </form>

    <p v-if="submitted" class="text-success mt-3">Your request has been submitted successfully!</p>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const form = ref({ name: '', email: '', message: '' });
const submitted = ref(false);

const submitSupport = () => {
  console.log("Support Request:", form.value);
  submitted.value = true;
  setTimeout(() => {
    form.value = { name: '', email: '', message: '' };
    submitted.value = false;
  }, 3000);
};
</script>
