<template>
  <div class="container mt-5">
    <div v-if="product" class="row">
      <div class="col-md-6">
        <img :src="product.image" class="img-fluid rounded" alt="Product Image" />
      </div>
      <div class="col-md-6">
        <h2 class="text-primary">{{ product.name }}</h2>
        <p class="text-muted">{{ product.description }}</p>
        <h4 class="text-success">{{ formatPrice(product.price) }}</h4>
        <button class="btn btn-primary mt-3">Buy Now</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const product = ref(null);

onMounted(async () => {
  try {
    const response = await fetch(`https://67b187cc3fc4eef538ea01f6.mockapi.io/api/shoe/shoes/${route.params.id}`);
    product.value = await response.json();
  } catch (error) {
    console.error("Error fetching product details:", error);
  }
});
</script>
