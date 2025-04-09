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
    <section class="container my-5">
      <div class="row g-5">
        <!-- Sidebar -->
        <div class="col-md-3">
          <Sidebar @filter-change="handleFilterChange" />
        </div>

        <!-- Product Grid -->
        <div class="col-md-9">
          <div v-if="loading" class="text-center py-5">
            <div class="spinner-border text-sneaker-blue" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <div class="row g-4">
              <div v-for="product in paginatedProducts" :key="product.id" class="col-md-4">
                <div class="card sneaker-card h-100 shadow-lg rounded overflow-hidden">
                  <div class="position-relative">
                    <img
                      :src="product.previewImageUrl"
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
                      {{ product.name }} <!-- Tên sản phẩm -->
                    </h5>
                    <div class="rating mb-2">
                      <div class="stars">
                        <i v-for="index in 5" 
                           :key="index"
                           class="bi"
                           :class="getStarClass(product.rating, index)">
                        </i>
                      </div>
                      <span class="rating-value">({{ product.rating }} / {{ product.ratingCount }} reviews)</span>
                    </div>
                    <h5 class="text-sneaker-blue mb-3">${{ formatPrice(product.price) }}</h5> <!-- Giá sản phẩm -->
                    <button
                      class="btn btn-sneaker w-100 py-3 fw-bold text-uppercase"
                      @click="goToDetailPage(product.id)"
                    >
                      View Details
                    </button>
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
        </div>
      </div>
    </section>

    <Footer />
  </div>
</template>


<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import Sidebar from "~/components/sidebar.vue";
import { getAllProducts } from "~/server/product-service";

const router = useRouter();
const products = ref([]);
const filteredProducts = ref([]);
const loading = ref(true);
const currentPage = ref(1);
const itemsPerPage = 12;

// Fetch products from API
const fetchProducts = async () => {
  try {
    loading.value = true;
    const response = await getAllProducts();  // Lấy dữ liệu từ getAllProducts

    // Kiểm tra nếu response.data và response.data.designs tồn tại
    if (response.data && Array.isArray(response.data.designs)) {
      products.value = response.data.designs;  // Truy cập vào designs
      filteredProducts.value = response.data.designs;
    } else {
      products.value = [];
      filteredProducts.value = [];
    }
    
  } catch (error) {
    console.error('Error fetching products:', error);
    products.value = [];
    filteredProducts.value = [];
  } finally {
    loading.value = false;
  }
};

const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filteredProducts.value.slice(start, end);
});

const totalPages = computed(() => {
  return Math.ceil(filteredProducts.value.length / itemsPerPage);
});

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const goToDetailPage = (productId) => {
  console.log('Navigating to product detail:', productId);
  router.push(`/productPage/${productId}`);  // Điều hướng đến trang chi tiết sản phẩm
};

const getStarClass = (rating, position) => {
  const roundedRating = Math.round(rating * 2) / 2;
  if (position <= Math.floor(roundedRating)) {
    return 'bi-star-fill text-warning';
  } else if (position === Math.ceil(roundedRating) && !Number.isInteger(roundedRating)) {
    return 'bi-star-half text-warning';
  }
  return 'bi-star text-warning';
};

const formatPrice = (price) => {
  return price.toFixed(2);
};

const handleFilterChange = (filters) => {
  filteredProducts.value = products.value.filter((product) => {
    const matchesSearch = !filters.searchTerm || product.name.toLowerCase().includes(filters.searchTerm.toLowerCase());
    const matchesPrice = product.price <= filters.priceRange.max;
    const matchesCategory =
      filters.categories.length === 0 || filters.categories.includes(product.category);
    const matchesBrand = filters.brand.length === 0 || filters.brand.includes(product.brand);
    const matchesColor = filters.color.length === 0 || filters.color.includes(product.color);
    const matchesSize = filters.size.length === 0 || filters.size.includes(product.size);
    const matchesRating = filters.rating.length === 0 || filters.rating.includes(product.rating);

    return matchesSearch && matchesPrice && matchesCategory && matchesBrand && matchesColor && matchesSize && matchesRating;
  });
  currentPage.value = 1; // Reset về trang 1 khi lọc
};

onMounted(async () => {
  await fetchProducts();
});
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
  height: 36px;
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

.rating {
  display: flex;
  align-items: center;
  gap: 5px;
}

.star-rating {
  color: #f1c40f;
  font-size: 1rem;
}

.rating-value {
  color: #555;
  font-size: 0.9rem;
}

.btn-sneaker {
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay từ #8bc34a */
  color: #fff;
  border: none;
  border-radius: 10px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: linear-gradient(45deg, #2c3e50 20%, #2980b9); /* Tối hơn một chút từ #3498db */
  transform: scale(1.05);
}

/* Pagination Styling */
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

.pagination .page-item.disabled .page-link {
  color: #ccc;
  cursor: not-allowed;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .sneaker-img {
    height: 200px;
  }
  .card-body {
    padding: 15px;
  }
  .pagination .page-link {
    padding: 8px 12px;
  }
}

.stars {
  display: inline-flex;
  gap: 2px;
  margin-right: 8px;
}

.rating {
  display: flex;
  align-items: center;
  gap: 5px;
  font-size: 0.9rem;
}

.rating-value {
  color: #666;
}
</style>