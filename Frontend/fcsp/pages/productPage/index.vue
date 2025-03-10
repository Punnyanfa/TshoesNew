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
                    <div class="rating mb-2">
                      <span class="star-rating">{{ Array(product.rating + 1).join('★') }}</span>
                      <span class="rating-value"> ({{ product.rating }})</span>
                    </div>
                    <h5 class="text-sneaker-blue mb-3">{{ formatPrice(product.price) }}</h5>
                    <button
                      class="btn btn-sneaker w-100 py-3 fw-bold text-uppercase"
                      @click="goToDetailPage(product.id)"
                    >
                      Add to cart
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
    price: 1399000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nam",
    brand: "Studio",
    color: "Black",
    size: "M",
    rating: 4,
  },
  {
    id: 2,
    name: "Running Shoes X3000",
    description: "Speed and comfort, designed just for you.",
    price: 2399000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nữ",
    brand: "Hastech",
    color: "Red",
    size: "L",
    rating: 5,
  },
  {
    id: 3,
    name: "Casual Canvas Shoes",
    description: "Light, breathable, and uniquely yours.",
    price: 949000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nữ",
    brand: "Quickiin",
    color: "Blue",
    size: "S",
    rating: 3,
  },
  {
    id: 4,
    name: "Sports Sandals",
    description: "Adventure-ready sandals, customized for your feet.",
    price: 1199000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Thể thao",
    brand: "Graphic Corner",
    color: "Green",
    size: "XL",
    rating: 4,
  },
  {
    id: 5,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 3099000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Trẻ em",
    brand: "DevItems",
    color: "Black",
    size: "L",
    rating: 2,
  },
  {
    id: 6,
    name: "Winter Boots",
    description: "Warmth and style, crafted to your specs.",
    price: 1199000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nam",
    brand: "DevItems",
    color: "Black",
    size: "S",
    rating: 3,
  },
  {
    id: 7,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 2399000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Trẻ em",
    brand: "DevItems",
    color: "Black",
    size: "M",
    rating: 5,
  },
  {
    id: 8,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 3099000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nam",
    brand: "DevItems",
    color: "Black",
    size: "S",
    rating: 4,
  },
  {
    id: 9,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 2399000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Thể thao",
    brand: "DevItems",
    color: "Black",
    size: "M",
    rating: 3,
  },
  {
    id: 10,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 2399000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nữ",
    brand: "DevItems",
    color: "Black",
    size: "L",
    rating: 2,
  },
  {
    id: 11,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 3099000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nam",
    brand: "DevItems",
    color: "Black",
    size: "M",
    rating: 5,
  },
  {
    id: 12,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 3099000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nam",
    brand: "DevItems",
    color: "Black",
    size: "M",
    rating: 2,
  },
  {
    id: 13,
    name: "Sports Sandals",
    description: "Warmth and style, crafted to your specs.",
    price: 3099000,
    image: "https://th.bing.com/th/id/OIP.w-ECg912T4CjOEeicqw1iwHaE8?rs=1&pid=ImgDetMain",
    category: "Nữ",
    brand: "DevItems",
    color: "Black",
    size: "S",
    rating: 1,
  },
]);

const filteredProducts = ref(products.value);
const loading = ref(false);
const currentPage = ref(1);
const itemsPerPage = 12;

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

const goToDetailPage = (productId) => router.push(`/product/${productId}`);

const formatPrice = (price) =>
  new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(price);

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
</style>