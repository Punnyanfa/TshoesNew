<template>
  <div class="product-wrapper">
    <Header />

    <!-- Main Content Section -->
    <section class="container my-5">
      <div class="row g-5">
        <!-- Sidebar -->
        <div class="col-md-3">
          <Sidebar @filter-change="handleFilterChange" />
        </div>

        <!-- Product Grid -->
        <div class="col-md-9">
          <div v-if="loading" class="row g-4">
            <div v-for="n in 12" :key="n" class="col-md-4">
              <div class="card sneaker-card h-100 shadow-lg rounded overflow-hidden">
                <div class="skeleton-img"></div>
                <div class="card-body d-flex flex-column">
                  <div class="skeleton-text mb-2"></div>
                  <div class="skeleton-text mb-3"></div>
                  <div class="skeleton-text mb-3"></div>
                  <div class="skeleton-button"></div>
                </div>
              </div>
            </div>
          </div>
          <div v-else>
            <div class="row g-4">
              <div v-for="product in paginatedProducts" :key="product.id" class="col-md-4">
                <div 
                  class="card sneaker-card h-100 shadow-lg rounded overflow-hidden clickable-card"
                  @click="goToDetailPage(product.id)"
                >
                  <div class="position-relative">
                    <img
                      :src="product.previewImageUrl"
                      :alt="product.name"
                      class="card-img-top sneaker-img"
                      loading="lazy"
                    />
                  </div>
                  <div class="card-body d-flex flex-column">
                    <h5
                      class="card-title text-dark fw-bold"
                      style="cursor: pointer"
                    >
                      {{ product.name }}
                    </h5>
                    <div class="rating mb-2">
                      <div class="stars">
                        <i 
                          v-for="index in 5" 
                          :key="index"
                          class="bi"
                          :class="getStarClass(product.rating, index)"
                          aria-hidden="true"
                        ></i>
                      </div>
                      <span class="rating-value">({{ product.rating }} / {{ product.ratingCount }} reviews)</span>
                    </div>
                    <h5 class="text-sneaker-blue mb-3">${{ formatPrice(product.price) }}</h5>
                    <button
                      class="btn btn-sneaker w-100 py-3 fw-bold text-uppercase"
                      @click.stop="goToDetailPage(product.id)"
                      aria-label="View product details"
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
                  <button 
                    class="page-link" 
                    @click="changePage(currentPage - 1)"
                    :disabled="currentPage === 1"
                    aria-label="Previous page"
                  >
                    Previous
                  </button>
                </li>
                <li
                  v-for="page in totalPages"
                  :key="page"
                  class="page-item"
                  :class="{ active: page === currentPage }"
                >
                  <button 
                    class="page-link" 
                    @click="changePage(page)"
                    :aria-current="page === currentPage ? 'page' : null"
                  >
                    {{ page }}
                  </button>
                </li>
                <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                  <button 
                    class="page-link" 
                    @click="changePage(currentPage + 1)"
                    :disabled="currentPage === totalPages"
                    aria-label="Next page"
                  >
                    Next
                  </button>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Newsletter Section -->
    <div class="newsletter-section">
      <img 
        src="https://i.pinimg.com/736x/87/88/95/878895303d804635464c69ca3b932995.jpg" 
        alt="Newsletter Visual" 
        class="newsletter-bg-img"
        loading="lazy"
      >
      <div class="newsletter-content">
        <div class="newsletter-text">
          <div class="newsletter-label">Bản tin</div>
          <h4 class="newsletter-title">Nhận cập nhật hàng tuần</h4>
          <form class="newsletter-form">
            <input 
              type="email" 
              class="newsletter-input" 
              placeholder="example@gmail.com" 
              required
            />
            <button class="newsletter-button" type="submit">Đăng ký</button>
          </form>
          <div class="newsletter-website">www.DownloadNewThemes.com</div>
        </div>
      </div>
    </div>

    <!-- Services Section -->
    <div class="services-section">
      <div class="services-container">
        <div class="service-item">
          <i class="bi bi-truck"></i>
          <div class="service-title">Giao hàng nhanh & an toàn</div>
          <div class="service-description">Tốc độ giao hàng tuyệt vời</div>
        </div>
        <div class="service-item">
          <i class="bi bi-arrow-counterclockwise"></i>
          <div class="service-title">Đảm bảo hoàn tiền</div>
          <div class="service-description">Hoàn tiền trong 10 ngày</div>
        </div>
        <div class="service-item">
          <i class="bi bi-clock-history"></i>
          <div class="service-title">Chính sách đổi trả 24h</div>
          <div class="service-description">Không hỏi lý do</div>
        </div>
        <div class="service-item">
          <i class="bi bi-headset"></i>
          <div class="service-title">Hỗ trợ chất lượng</div>
          <div class="service-description">Hỗ trợ 24/7</div>
        </div>
      </div>
    </div>

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
    const response = await getAllProducts();

    if (response.data?.designs?.length) {
      const activeProducts = response.data.designs.filter(product => product.status === 1);
      products.value = activeProducts;
      filteredProducts.value = activeProducts;
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

const totalPages = computed(() => Math.ceil(filteredProducts.value.length / itemsPerPage));

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
};

const goToDetailPage = (productId) => {
  router.push(`/productPage/${productId}`);
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

const formatPrice = (price) => price.toFixed(2);

const handleFilterChange = (filters) => {
  filteredProducts.value = products.value.filter((product) => {
    const isActive = product.status === 1;
    const matchesSearch = !filters.searchTerm || product.name.toLowerCase().includes(filters.searchTerm.toLowerCase());
    const matchesPrice = product.price <= filters.priceRange.max;
    const matchesCategory = filters.categories.length === 0 || filters.categories.includes(product.category);
    const matchesBrand = filters.brand.length === 0 || filters.brand.includes(product.brand);
    const matchesColor = filters.color.length === 0 || filters.color.includes(product.color);
    const matchesSize = filters.size.length === 0 || filters.size.includes(product.size);
    const matchesRating = filters.rating.length === 0 || filters.rating.includes(product.rating);

    return isActive && matchesSearch && matchesPrice && matchesCategory && matchesBrand && matchesColor && matchesSize && matchesRating;
  });
  currentPage.value = 1;
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

/* Card Styles */
.sneaker-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: none;
  overflow: hidden;
}

.clickable-card {
  cursor: pointer;
}

.clickable-card:hover {
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

/* Rating Styles */
.rating {
  display: flex;
  align-items: center;
  gap: 5px;
}

.stars {
  display: inline-flex;
  gap: 2px;
  margin-right: 8px;
}

.rating-value {
  color: #666;
  font-size: 0.9rem;
}

/* Button Styles */
.btn-sneaker {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
  border: none;
  border-radius: 10px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: linear-gradient(45deg, #2c3e50 20%, #2980b9);
  transform: scale(1.05);
}

/* Pagination Styles */
.pagination .page-link {
  color: #3498db;
  border: none;
  padding: 10px 15px;
  transition: background 0.3s ease, color 0.3s ease;
}

.pagination .page-item.active .page-link {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
  border-radius: 5px;
}

.pagination .page-link:hover {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
}

.pagination .page-item.disabled .page-link {
  color: #ccc;
  cursor: not-allowed;
}

/* Skeleton Loading */
.skeleton-img {
  height: 200px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: loading 1.5s infinite;
}

.skeleton-text {
  height: 20px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: loading 1.5s infinite;
  border-radius: 4px;
  margin-bottom: 10px;
}

.skeleton-button {
  height: 44px;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: loading 1.5s infinite;
  border-radius: 10px;
}

@keyframes loading {
  0% {
    background-position: 200% 0;
  }
  100% {
    background-position: -200% 0;
  }
}

/* Newsletter Section */
.newsletter-section {
  position: relative;
  margin: 50px auto;
  border-radius: 24px;
  overflow: hidden;
  width: 100%;
  max-width: 1300px;
}

.newsletter-bg-img {
  width: 100%;
  height: 320px;
  object-fit: cover;
}

.newsletter-content {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgba(0, 0, 0, 0.5);
}

.newsletter-text {
  text-align: center;
  color: white;
  padding: 20px;
}

.newsletter-label {
  font-size: 1rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.newsletter-title {
  font-size: 1.8rem;
  font-weight: bold;
  margin-bottom: 20px;
}

.newsletter-form {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 20px;
  max-width: 420px;
  margin: 0 auto 20px;
}

.newsletter-input {
  flex: 1;
  padding: 10px 20px;
  border-radius: 30px;
  border: none;
  height: 44px;
}

.newsletter-button {
  padding: 10px 30px;
  border-radius: 30px;
  background-color: #333;
  color: white;
  border: none;
  cursor: pointer;
}

.newsletter-website {
  font-weight: bold;
  font-size: 1.05rem;
}

/* Services Section */
.services-section {
  padding: 50px 0;
}

.services-container {
  display: flex;
  justify-content: center;
  gap: 30px;
  flex-wrap: wrap;
  max-width: 1200px;
  margin: 0 auto;
}

.service-item {
  text-align: center;
  padding: 20px;
  flex: 1;
  min-width: 200px;
}

.service-item i {
  font-size: 2rem;
  color: #007bff;
  margin-bottom: 15px;
}

.service-title {
  font-weight: bold;
  margin-bottom: 10px;
}

.service-description {
  color: #666;
  font-size: 0.9rem;
}

/* Responsive Styles */
@media (max-width: 768px) {
  .sneaker-img {
    height: 180px;
  }
  
  .card-body {
    padding: 15px;
  }
  
  .pagination .page-link {
    padding: 8px 12px;
  }
  
  .newsletter-section {
    width: 90%;
    margin: 30px auto;
  }
  
  .newsletter-section .newsletter-bg-img { 
    border-radius: 12px; 
  }

  .hero-section {
    height: 400px;
  }

  .payment-hero-content {
    padding: 50px 20px;
  }

  .payment-hero-title {
    font-size: 2rem;
  }

  .stats-section {
    flex-wrap: wrap;
    padding: 30px 0;
  }

  .stat-item {
    width: 50%;
    margin-bottom: 20px;
  }

  .stat-divider {
    display: none;
  }

  .services-container {
    flex-direction: column;
    align-items: center;
  }

  .service-item {
    width: 100%;
    max-width: 300px;
  }
}

@media (max-width: 576px) {
  .sneaker-img {
    height: 160px;
  }

  .newsletter-section {
    width: 95%;
    margin: 20px auto;
  }
}
</style>