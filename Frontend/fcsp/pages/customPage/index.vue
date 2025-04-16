<template>
  <div class="product-wrapper">
    <Header />

    <!-- Breadcrumb -->
    <nav class="container py-3" aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <NuxtLink to="/homePage" class="text-decoration-none">Home</NuxtLink>
        </li>
        <li class="breadcrumb-item active" aria-current="page">Products</li>
      </ol>
    </nav>

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
            <div class="card sneaker-card h-100 shadow-lg rounded overflow-hidden">
              <div class="position-relative">
                <img
                  :src="product.image"
                  class="card-img-top sneaker-img"
                  :alt="product.name"
                  @click="goToDetailPage(product.id)"
                />
                <span class="custom-badge">Customizable</span>
              </div>
              <div class="card-body d-flex flex-column">
                <h5
                  class="card-title text-dark fw-bold"
                  @click="goToDetailPage(product.id)"
                  style="cursor: pointer"
                >
                  {{ product.name }}
                </h5>
                <p class="text-muted flex-grow-1">{{ product.description }}</p>
                <h5 class="text-sneaker-blue mb-3">{{ formatPrice(product.price) }}</h5>
                <NuxtLink to="/customdetailPage" class="btn btn-sneaker w-100 px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
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
        image: template.twoImageUrl
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

const goToDetailPage = (productId) => router.push(`/product/${productId}`);

const formatPrice = (price) =>
  new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(price);
</script>

<style scoped>
.product-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
}

/* Breadcrumb Styles */
.breadcrumb {
  background: transparent;
  padding: 0;
  margin-bottom: 0;
}

.breadcrumb-item a {
  color: #3498db; /* Thay từ #8bc34a */
  text-decoration: none;
  transition: color 0.3s ease;
}

.breadcrumb-item a:hover {
  color: #2c3e50; /* Thay từ #7cb342 */
  text-decoration: underline;
}

.breadcrumb-item.active {
  color: #1e293b; /* Đồng bộ với About Page */
  font-weight: 500;
}

/* Các style hiện có chỉnh sửa màu */
.text-sneaker-blue {
  color: #3498db; /* Thay từ #8bc34a */
  height: auto;
}

.sneaker-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: none;
}

.sneaker-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 30px rgba(0, 0, 0, 0.1) !important;
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay từ #8bc34a */
  color: #fff;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: bold;
}

.btn-sneaker {
  display: inline-block;
  text-align: center;
  text-decoration: none;
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay từ #8bc34a */
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 12px 24px;
  font-weight: bold;
  text-transform: uppercase;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: linear-gradient(45deg, #2c3e50 20%, #2980b9); /* Tối hơn một chút từ #3498db */
  transform: scale(1.05);
}

.btn-sneaker:active {
  transform: scale(0.95);
}

/* Pagination */
.pagination .page-link {
  color: #3498db; /* Thay từ #8bc34a */
  border: none;
  padding: 10px 15px;
  transition: background 0.3s ease, color 0.3s ease;
}

.pagination .page-item.active .page-link {
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay từ #8bc34a */
  color: #fff;
  border-radius: 5px;
}

.pagination .page-link:hover {
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay từ #8bc34a */
  color: #fff;
}

/* Controls (Filters and Sort) */
.controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  background: #fff;
  padding: 1rem;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.filters {
  font-weight: 600;
  color: #1e293b; /* Đồng bộ với About Page */
  cursor: pointer;
}

.sort {
  font-size: 0.9rem;
  color: #666;
}

.sort-select {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 4px;
  background: #fff;
  font-size: 0.9rem;
}

.sort-select:focus {
  outline: none;
  border-color: #3498db; /* Thay từ #007bff */
  box-shadow: 0 0 5px rgba(52, 152, 219, 0.3); /* Thay từ rgba(0, 123, 255, 0.3) */
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
</style>