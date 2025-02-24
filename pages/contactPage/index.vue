<!-- Contact Us Template -->
<template>
  <div>
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <div class="container mt-5">
      <h1 class="text-primary text-center">Contact Us</h1>
      <form @submit.prevent="sendMessage">
        <div class="mb-3">
          <label class="form-label">Name</label>
          <input v-model="form.name" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Email</label>
          <input v-model="form.email" type="email" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Message</label>
          <textarea v-model="form.message" class="form-control" rows="4" required></textarea>
        </div>
        <button type="submit" class="btn btn-primary w-100">Send</button>
      </form>
    </div>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref } from 'vue';
import Header from '@/components/site-header.vue';
import Footer from '@/components/site-footer.vue';

const form = ref({ name: '', email: '', message: '' });

const sendMessage = () => {
  alert('Message Sent!');
  form.value = { name: '', email: '', message: '' };
};
</script>
