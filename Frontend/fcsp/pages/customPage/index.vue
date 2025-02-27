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
          </select>
        </div>
      </div>


      <!-- Product Grid -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-sneaker-orange" role="status">
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
                <h5 class="text-sneaker-orange mb-3">{{ formatPrice(product.price) }}</h5>
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
import { ref, computed } from "vue";
import { useRouter } from "vue-router";


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
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 99.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 3,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 9.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 4,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 49.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 5,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 69.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 6,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 79.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 7,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 19.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 8,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 29.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 9,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 39.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 10,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 89.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 11,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 15.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 12,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 25.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
  {
    id: 13,
    name: "Classic Leather Sneakers",
    description: "Your perfect everyday kicks, tailored to your style.",
    price: 55.99,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain"
  },
]);


const loading = ref(false);
const currentPage = ref(1);
const itemsPerPage = 12; // Adjusted to 8 (2 rows of 4 cards)
const sortOption = ref("featured");


// Computed property for sorted products
const sortedProducts = computed(() => {
  return [...products.value].sort((a, b) => {
    if (sortOption.value === "price-low") return a.price - b.price;
    if (sortOption.value === "price-high") return b.price - a.price;
    return 0; // Default to original order
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
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  min-height: 100vh;
}


.text-sneaker-orange {
  color: #8bc34a;
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
  background: #8bc34a;
  color: #fff;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: bold;
}


.btn-sneaker {
  display: inline-block; /* Để đảm bảo NuxtLink hoạt động giống như một nút */
  text-align: center;
  text-decoration: none; /* Loại bỏ gạch chân của liên kết */
  background: #8bc34a;
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 12px 24px; /* Điều chỉnh padding cho phù hợp */
  font-weight: bold;
  text-transform: uppercase;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: #7cb342; /* Darkened shade for hover effect */
  transform: scale(1.05);
}

.btn-sneaker:active {
  transform: scale(0.95); /* Hiệu ứng nhấn xuống */
}


/* Pagination */
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
  color: #333;
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
  border-color: #007bff;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.3);
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