<template>
  <div>
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <div class="container mt-5">
      <h1 class="text-primary text-center">About Us</h1>
      <p class="lead text-center">
        We specialize in AI-driven shoe customization, providing personalized design solutions to create the perfect pair of shoes for you.
      </p>
      <div class="text-center mt-4">
        <h3>Our Mission</h3>
        <p class="mb-4">
          Our mission is to blend cutting-edge AI technology with personalized design to offer the most comfortable, stylish, and well-fitted shoes for every individual.
        </p>

        <h3>Why Choose Us?</h3>
        <ul class="list-unstyled">
          <li><i class="fas fa-check-circle"></i> Custom Fit: Tailored to your foot shape using advanced AI.</li>
          <li><i class="fas fa-check-circle"></i> Premium Materials: High-quality materials sourced ethically.</li>
          <li><i class="fas fa-check-circle"></i> Sustainable Design: Eco-friendly production processes to reduce environmental impact.</li>
        </ul>

        <div class="mt-4">
          <NuxtLink to="/contact" class="btn btn-outline-primary">Contact Us</NuxtLink>
        </div>
      </div>
    </div>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import Header from '@/components/site-header.vue';
import Footer from '@/components/site-footer.vue';
</script>

<style scoped>
/* Optional: You can add some styling to make the page look more attractive */
ul {
  font-size: 1.1em;
}
i {
  color: #28a745;
}
</style>
