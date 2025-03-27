<template>
  <div class="product-wrapper">
    <Header />

    <!-- Breadcrumb -->
    <nav class="container py-3" aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <NuxtLink to="/homePage" class="text-decoration-none">Trang Chủ</NuxtLink>
        </li>
        <li class="breadcrumb-item active" aria-current="page">Tùy Chỉnh Giày</li>
      </ol>
    </nav>

    <!-- Main Content Section -->
    <main class="container my-5">
      <!-- Filters and Sort -->
      <div class="controls mb-4">
        <div class="filters d-flex gap-3">
          <select v-model="selectedCategory" class="form-select">
            <option value="">Tất Cả Danh Mục</option>
            <option value="running">Giày Chạy Bộ</option>
            <option value="casual">Giày Thời Trang</option>
            <option value="sport">Giày Thể Thao</option>
          </select>
          <select v-model="priceRange" class="form-select">
            <option value="">Tất Cả Giá</option>
            <option value="0-50">Dưới 1.000.000₫</option>
            <option value="50-100">1.000.000₫ - 2.000.000₫</option>
            <option value="100+">Trên 2.000.000₫</option>
          </select>
        </div>
        <div class="sort">
          <select v-model="sortOption" class="form-select">
            <option value="featured">Nổi Bật</option>
            <option value="price-low">Giá: Thấp đến Cao</option>
            <option value="price-high">Giá: Cao đến Thấp</option>
            <option value="newest">Mới Nhất</option>
          </select>
        </div>
      </div>

      <!-- Product Grid -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-sneaker-blue" role="status">
          <span class="visually-hidden">Đang tải...</span>
        </div>
      </div>
      
      <div v-else-if="filteredProducts.length === 0" class="text-center py-5">
        <div class="no-results">
          <i class="bi bi-search display-1 text-muted"></i>
          <h3 class="mt-3">Không Tìm Thấy Sản Phẩm</h3>
          <p class="text-muted">Vui lòng thử lại với bộ lọc khác</p>
        </div>
      </div>
      
      <div v-else>
        <div class="row g-4">
          <div v-for="product in paginatedProducts" :key="product.id" class="col-md-3">
            <div class="card sneaker-card h-100">
              <div class="position-relative">
                <img
                  :src="product.image"
                  class="card-img-top sneaker-img"
                  :alt="product.name"
                  @load="handleImageLoad(product.id)"
                  :class="{ 'image-loaded': imageLoaded[product.id] }"
                />
                <div class="image-skeleton" v-if="!imageLoaded[product.id]"></div>
                <span class="custom-badge">
                  <i class="bi bi-brush-fill me-1"></i>Tùy Chỉnh
                </span>
              </div>
              <div class="card-body d-flex flex-column">
                <h5 class="card-title fw-bold mb-2">{{ product.name }}</h5>
                <p class="text-muted flex-grow-1">{{ product.description }}</p>
                <div class="d-flex justify-content-between align-items-center mb-3">
                  <span class="price">{{ formatPrice(product.price) }}</span>
                  <span class="rating">
                    <i class="bi bi-star-fill"></i>
                    <span class="ms-1">{{ product.rating }} ({{ product.reviews }})</span>
                  </span>
                </div>
                <NuxtLink 
                  :to="`/customdetailPage/${product.id}`" 
                  class="btn btn-sneaker w-100"
                >
                  Tùy Chỉnh Ngay
                </NuxtLink>
              </div>
            </div>
          </div>
        </div>

        <!-- Pagination -->
        <nav class="mt-5" aria-label="Product pagination">
          <ul class="pagination justify-content-center">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <button class="page-link" @click="changePage(currentPage - 1)">
                <i class="bi bi-chevron-left"></i>
              </button>
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
              <button class="page-link" @click="changePage(currentPage + 1)">
                <i class="bi bi-chevron-right"></i>
              </button>
            </li>
          </ul>
        </nav>
      </div>
    </main>

    <Footer />
  </div>
</template>

<script setup>
import { ref, computed } from "vue";

// State
const loading = ref(false);
const currentPage = ref(1);
const itemsPerPage = 12;
const sortOption = ref("featured");
const selectedCategory = ref("");
const priceRange = ref("");
const imageLoaded = ref({});

// Products data
const products = ref([
  {
    id: 1,
    name: "Nike Air Max Custom",
    description: "Giày chạy bộ cao cấp với công nghệ Air Max",
    price: 2999000,
    rating: 4.8,
    reviews: 120,
    category: "running",
    image: "https://example.com/image1.jpg"
  },
  // Thêm nhiều sản phẩm khác...
]);

// Computed
const filteredProducts = computed(() => {
  let result = [...products.value];

  // Lọc theo danh mục
  if (selectedCategory.value) {
    result = result.filter(p => p.category === selectedCategory.value);
  }

  // Lọc theo giá
  if (priceRange.value) {
    const [min, max] = priceRange.value.split('-').map(Number);
    result = result.filter(p => {
      const price = p.price / 1000000; // Chuyển đổi sang triệu
      if (max) {
        return price >= min && price <= max;
      }
      return price >= min;
    });
  }

  // Sắp xếp
  return result.sort((a, b) => {
    if (sortOption.value === "price-low") return a.price - b.price;
    if (sortOption.value === "price-high") return b.price - a.price;
    if (sortOption.value === "newest") return b.id - a.id;
    return 0;
  });
});

const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filteredProducts.value.slice(start, end);
});

const totalPages = computed(() => {
  return Math.ceil(filteredProducts.value.length / itemsPerPage);
});

// Methods
const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const handleImageLoad = (productId) => {
  imageLoaded.value[productId] = true;
};

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price);
};
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

/* Thêm styles mới */
.form-select {
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  padding: 8px 12px;
  font-size: 0.95rem;
  background-color: white;
  min-width: 180px;
}

.form-select:focus {
  border-color: #3498db;
  box-shadow: 0 0 0 2px rgba(52, 152, 219, 0.2);
}

.no-results {
  padding: 40px;
  color: #718096;
}

.image-skeleton {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: loading 1.5s infinite;
}

@keyframes loading {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}

/* Responsive */
@media (max-width: 768px) {
  .controls {
    flex-direction: column;
    gap: 1rem;
  }
  
  .filters {
    flex-direction: column;
  }
  
  .form-select {
    width: 100%;
  }
}
</style>