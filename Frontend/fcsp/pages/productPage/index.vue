<template>
  <div>
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <div class="container mt-5">
      <h1 class="text-primary text-center">Product Page</h1>
      
      <!-- Product Details Section -->
      <div class="product-details">
        <img :src="product.image" alt="Product Image" class="img-fluid" />
        <h3 class="mt-3">{{ product.name }}</h3>
        <p class="text-muted">{{ product.category }}</p>
        <p class="lead">{{ product.description }}</p>
        <h4 class="text-success">{{ product.price }} USD</h4>
        
        <!-- Add to Cart Section -->
        <div class="mt-4">
          <button @click="addToCart" class="btn btn-primary w-100">Add to Cart</button>
        </div>
      </div>
    </div>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref } from 'vue';
import Header from '@/components/site-header.vue';
import Footer from '@/components/site-footer.vue';

const product = ref({
  name: 'Sample Product',
  category: 'Category A',
  description: 'This is a detailed description of the product.',
  price: '99.99',
  image: 'https://via.placeholder.com/400',
});

const addToCart = () => {
  alert('Product added to cart!');
};
</script>
