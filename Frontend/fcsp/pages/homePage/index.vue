<template>
  <div class="container mt-5 text-center">
    <h1 class="text-primary">Welcome to Shoe Customizer</h1>
    <p class="lead">Design your own shoes with our AI-powered 3D customizer.</p>

    <NuxtLink to="/custom" class="btn btn-primary mt-3">Start Designing</NuxtLink>

    <div class="row mt-5">
      <div class="col-md-4" v-for="shoe in shoes" :key="shoe.id">
        <div class="card">
          <img :src="shoe.image" class="card-img-top" alt="Shoe" />
          <div class="card-body">
            <h5 class="card-title">{{ shoe.name }}</h5>
            <p class="card-text">${{ shoe.price }}</p>
            <NuxtLink :to="`/products/${shoe.id}`" class="btn btn-outline-primary">View Details</NuxtLink>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const shoes = ref([
  { id: 1, name: "Air Max Custom", price: 120, image: "/images/shoe1.jpg" },
  { id: 2, name: "Classic Sneakers", price: 95, image: "/images/shoe2.jpg" },
  { id: 3, name: "High-Top Design", price: 110, image: "/images/shoe3.jpg" }
]);
</script>
