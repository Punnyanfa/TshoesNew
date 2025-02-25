<!-- Product Page Template with Pagination - Custom Sneaker Theme -->
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
          <div v-if="loading" class="text-center py-5">
            <div class="spinner-border text-sneaker-orange" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
          <div v-else>
            <h2 class="fw-bold text-sneaker-orange mb-4 animate__animated animate__fadeIn">Customize Your Kicks</h2>
            <div class="row g-4">
              <div v-for="product in paginatedProducts" :key="product.id" class="col-md-4">
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
                    <h5 class="text-sneaker-orange mb-3">{{ formatPrice(product.price) }}</h5>
                    <button 
                      class="btn btn-sneaker w-100 py-2 fw-bold text-uppercase" 
                      @click="goToDetailPage(product.id)"
                    >
                      Design Now
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
import Sidebar from "@/components/SideBar.vue";

const router = useRouter();
const products = ref([
  {
    id: 1,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 59.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 2,
    name: "Running Shoes X3000",
    description: "Speed and comfort, designed just for you.",
    price: 99.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 3,
    name: "Casual Canvas Shoes",
    description: "Light, breathable, and uniquely yours.",
    price: 39.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 4,
    name: "Sports Sandals",
    description: "Adventure-ready sandals, customized for your feet.",
    price: 49.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 5,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
    {
    id: 6,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
    {
    id: 7,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
    {
    id: 8,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
    {
    id: 9,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
    {
    id: 10,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 129.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
]);

const filteredProducts = ref(products.value);
const loading = ref(false);
const currentPage = ref(1);
const itemsPerPage = 6; // Số sản phẩm mỗi trang

// Tính toán sản phẩm hiển thị trên trang hiện tại
const paginatedProducts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filteredProducts.value.slice(start, end);
});

// Tổng số trang
const totalPages = computed(() => {
  return Math.ceil(filteredProducts.value.length / itemsPerPage);
});

// Chuyển trang
const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const goToDetailPage = (productId) => router.push(`/product/${productId}`);

const formatPrice = (price) => new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(price);

const handleFilterChange = (filters) => {
  filteredProducts.value = products.value.filter(product => 
    (!filters.price || product.price <= filters.price) &&
    (!filters.category || product.name.includes(filters.category))
  );
  currentPage.value = 1; // Reset về trang 1 khi lọc
};
</script>

<style scoped>
.product-wrapper {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  min-height: 100vh;
}

.text-sneaker-orange {
  color: #8bc34a;
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
  height: 250px;
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
  background: #8bc34a;
  color: #fff;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: bold;
}

.btn-sneaker {
  background: #8bc34a;
  color: #fff;
  border: none;
  border-radius: 10px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: #8bc34a;
  transform: scale(1.05);
}

/* Pagination Styling */
.pagination .page-link {
  color: #8bc34a;
  border: none;
  padding: 10px 15px;
  transition: background 0.3s ease, color 0.3s ease;
}

.pagination .page-item.active .page-link {
  background: #8bc34a;
  color: #fff;
  border-radius: 5px;
}

.pagination .page-link:hover {
  background: #8bc34a;
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
</style>