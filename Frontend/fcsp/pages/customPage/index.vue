<template>
  <div class="product-wrapper">
    <Header />

    <!-- Main Content Section -->
    <main class="container my-5">
      <!-- Filters and Sort -->
      <div class="controls">
        <div class="filters">Filters</div>
        <div class="sort">
          Sort by:
          <select v-model="sortOption" class="sort-select">
            <option value="featured">Featured</option>
            <option value="price-low">Price: Low to High</option>
            <option value="price-high">Price: High to Low</option>
            <option value="name-asc">Name: A to Z</option>
            <option value="name-desc">Name: Z to A</option>
          </select>
        </div>
      </div>

      <!-- Product Grid -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-sneaker-blue" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
      <div v-else>
        <div class="row g-4">
          <div v-for="product in paginatedProducts" :key="product.id" class="col-md-3">
            <div class="card sneaker-card h-100 shadow-lg rounded overflow-hidden" @click="goToDetailPage(product.id)" style="cursor: pointer">
              <div class="position-relative">
                <img
                  :src="product.twoDImageUrl"
                  class="card-img-top sneaker-img"
                  :alt="product.twoDImageUrl"
                />
                <span class="custom-badge">Customizable</span>
              </div>
              <div class="card-body d-flex flex-column">
                <h5 class="card-title text-dark fw-bold">
                  {{ product.name }}
                </h5>
                <p class="text-muted flex-grow-1">{{ product.description }}</p>
                <h5 class="text-sneaker-blue mb-3">{{ formatPrice(product.price) }}</h5>
                <NuxtLink :to="`/customPage/${product.id}`" class="btn btn-sneaker w-100 px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn" @click.stop>
                  Customize Now
                </NuxtLink>
              </div>
            </div>
          </div>
        </div>

        <!-- Pagination -->
        <nav class="mt-5 d-flex justify-content-center" aria-label="Product pagination">
          <ul class="pagination">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <button class="page-link" @click="changePage(currentPage - 1)">Previous</button>
            </li>
            <li
              v-for="page in totalPages"
              :key="page"
              class="page-item"
              :class="{ active: page === currentPage }"
            >
              <button class="page-link" @click="changePage(page)">{{ page }}</button>
            </li>
            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
              <button class="page-link" @click="changePage(currentPage + 1)">Next</button>
            </li>
          </ul>
        </nav>
      </div>
    </main>

    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import { getAllTemplate } from "~/server/custom-service";

const router = useRouter();
const products = ref([]);
const loading = ref(true);
const currentPage = ref(1);
const itemsPerPage = 12;
const sortOption = ref("featured");
const error = ref(null);

// Fetch templates from API
const fetchTemplates = async () => {
  try {
    loading.value = true;
    const response = await getAllTemplate();
    if (response.data) {
      products.value = response.data.map(template => ({
        id: template.id,
        name: template.name,
        description: template.description,
        price: template.price,
        twoDImageUrl: template.twoDImageUrl
      }));
    }
  } catch (err) {
    console.error("Error fetching templates:", err);
    error.value = "Failed to load templates. Please try again later.";
  } finally {
    loading.value = false;
  }
};

// Call fetchTemplates when component mounts
onMounted(() => {
  fetchTemplates();
});

// Computed property for sorted products
const sortedProducts = computed(() => {
  return [...products.value].sort((a, b) => {
    if (sortOption.value === "price-low") return a.price - b.price;
    if (sortOption.value === "price-high") return b.price - a.price;
    if (sortOption.value === "name-asc") return a.name.localeCompare(b.name);
    if (sortOption.value === "name-desc") return b.name.localeCompare(a.name);
    return 0;
  });
});

// Computed property for paginated products
const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return sortedProducts.value.slice(start, end);
});

// Total pages
const totalPages = computed(() => {
  return Math.ceil(sortedProducts.value.length / itemsPerPage);
});

// Change page
const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const goToDetailPage = (id) => {
  router.push({ path: `/customPage/${id}` });
};

const formatPrice = (price) =>
  new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(price);
</script>

<style scoped>
.product-wrapper {
  background: #f5f7fa;
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
}

.text-sneaker-blue {
  color: #AAAAAA;
  height: auto;
}

.sneaker-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: none;
  background: #ffffff;
  border: 1px solid rgba(0, 0, 0, 0.1);
}

.sneaker-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 30px rgba(170, 170, 170, 0.15) !important;
}

.sneaker-img {
  height: 200px;
  object-fit: cover;
  cursor: pointer;
  transition: transform 0.3s ease;
}

.sneaker-img:hover {
  transform: scale(1.05);
}

.custom-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  background: #AAAAAA;
  color: #fff;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: bold;
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.2);
}

.btn-sneaker {
  display: inline-block;
  text-align: center;
  text-decoration: none;
  background: #AAAAAA;
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 12px 24px;
  font-weight: bold;
  text-transform: uppercase;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.2);
}

.btn-sneaker:hover {
  background: #888888;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(170, 170, 170, 0.3);
}

.btn-sneaker:active {
  transform: translateY(0);
  box-shadow: 0 2px 10px rgba(170, 170, 170, 0.2);
}

/* Pagination */
.pagination .page-link {
  color: #AAAAAA;
  border: none;
  padding: 10px 15px;
  transition: all 0.3s ease;
  background: #ffffff;
  border: 1px solid rgba(0, 0, 0, 0.1);
}

.pagination .page-item.active .page-link {
  background: #AAAAAA;
  color: #fff;
  border-radius: 5px;
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.2);
}

.pagination .page-link:hover {
  background: #AAAAAA;
  color: #fff;
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(170, 170, 170, 0.3);
}

/* Controls (Filters and Sort) */
.controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  background: #ffffff;
  padding: 1rem;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  border: 1px solid rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
}

.controls:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.15);
}

.filters {
  font-weight: 600;
  color: #AAAAAA;
  cursor: pointer;
  transition: all 0.3s ease;
}

.filters:hover {
  transform: translateX(5px);
}

.sort {
  font-size: 0.9rem;
  color: #6B7280;
}

.sort-select {
  padding: 0.5rem;
  border: 2px solid #E5E7EB;
  border-radius: 8px;
  background: #ffffff;
  font-size: 0.9rem;
  transition: all 0.3s ease;
}

.sort-select:focus {
  outline: none;
  border-color: #AAAAAA;
  box-shadow: 0 0 0 4px rgba(170, 170, 170, 0.1);
  transform: translateY(-2px);
}

/* Loading spinner */
.spinner-border {
  color: #AAAAAA;
  width: 3rem;
  height: 3rem;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .col-md-3 {
    flex: 0 0 50%;
    max-width: 50%;
  }
  .sneaker-img {
    height: 200px;
  }
  .card-body {
    padding: 15px;
  }
  .pagination .page-link {
    padding: 8px 12px;
  }
  .controls {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }
}

/* Dark mode styles */
.dark-mode .product-wrapper {
  background: #1a1a1a;
}

.dark-mode .sneaker-card {
  background: #2d2d2d;
  border-color: rgba(255, 255, 255, 0.1);
}

.dark-mode .controls {
  background: #2d2d2d;
  border-color: rgba(255, 255, 255, 0.1);
}

.dark-mode .sort-select {
  background: #2d2d2d;
  border-color: rgba(255, 255, 255, 0.1);
  color: #ffffff;
}

.dark-mode .filters {
  color: #ffffff;
}

.dark-mode .sort {
  color: #AAAAAA;
}
</style>