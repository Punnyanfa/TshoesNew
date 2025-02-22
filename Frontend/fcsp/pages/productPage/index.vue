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
import Sidebar from "@/components/SideBar.vue";

const router = useRouter();
const products = ref([
  {
    id: 1,
    name: "Classic Leather Sneakers",
    description: "Comfortable and stylish leather sneakers for everyday wear.",
    price: 59.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 2,
    name: "Running Shoes X3000",
    description: "High-performance running shoes with advanced cushioning.",
    price: 99.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 3,
    name: "Casual Canvas Shoes",
    description: "Light and breathable canvas shoes perfect for casual outings.",
    price: 39.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 4,
    name: "Sports Sandals",
    description: "Durable sandals designed for outdoor sports and adventures.",
    price: 49.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 5,
    name: "Winter Boots",
    description: "Keep your feet warm and dry with these insulated winter boots.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  }, {
    id: 5,
    name: "Winter Boots",
    description: "Keep your feet warm and dry with these insulated winter boots.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  }
]);
const filteredProducts = ref(products.value);
const loading = ref(false);

const getCurrentProducts = () => {
  return filteredProducts.value;
};

const goToDetailPage = (productId) => router.push(`/product/${productId}`);

const formatPrice = (price) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(price);
</script>
