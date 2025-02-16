<template>
  <div>
    <Header />
    <div class="container my-5">
      <div class="row">
        <div class="col-md-3">
          <Sidebar @filter-change="handleFilterChange" />
        </div>
        <div class="col-md-9">
          <div v-if="loading" class="text-center py-5">
            <div class="spinner-border text-primary" role="status"></div>
          </div>
          <div v-else>
            <div class="row">
              <div v-for="product in getCurrentProducts()" :key="product.id" class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                  <img :src="product.image" class="card-img-top" :alt="product.name" @click="goToDetailPage(product.id)" style="cursor: pointer; height: 250px; object-fit: cover" />
                  <div class="card-body">
                    <h5 class="text-primary" @click="goToDetailPage(product.id)" style="cursor: pointer">{{ product.name }}</h5>
                    <p class="text-muted">{{ product.description }}</p>
                    <h5 class="text-success">{{ formatPrice(product.price) }}</h5>
                    <button class="btn btn-primary w-100" @click="goToDetailPage(product.id)">Buy Now</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import Sidebar from "@/components/Sidebar.vue";
import Header from "@/components/site-header.vue";
import Footer from "@/components/site-footer.vue";

const router = useRouter();
const products = ref([]);
const filteredProducts = ref([]);
const loading = ref(true);

onMounted(async () => {
  try {
    loading.value = true;
    const response = await fetch("https://67b187cc3fc4eef538ea01f6.mockapi.io/api/shoe/shoes");
    products.value = await response.json();
    filteredProducts.value = products.value;
  } catch (error) {
    console.error("Fetch error:", error);
  } finally {
    loading.value = false;
  }
});

const goToDetailPage = (productId) => router.push(`/product/${productId}`);
const formatPrice = (price) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(price);
</script>