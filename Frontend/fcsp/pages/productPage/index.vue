<template>
  <div>
    <!-- Header -->
    <Header />

    <div class="container my-5">
      <div class="row">
        <!-- Sidebar -->
        <div class="col-md-3">
          <Sidebar @filter-change="handleFilterChange" />
        </div>

        <!-- Product List -->
        <div class="col-md-9">
          <div v-if="loading" class="d-flex justify-content-center py-5">
            <div class="spinner-border text-primary" role="status"></div>
          </div>

          <div v-else>
            <div v-if="filteredProducts.length === 0" class="text-center py-4">
              <h3 class="text-muted">No products found</h3>
              <p>Please try different filters</p>
            </div>
            <div v-else>
              <div class="row">
                <div
                  v-for="product in getCurrentProducts()"
                  :key="product.id"
                  class="col-md-4 mb-4"
                >
                  <div class="card h-100 shadow-sm">
                    <div class="position-relative">
                      <img
                        :src="product.image"
                        class="card-img-top"
                        :alt="product.name"
                        @click="goToDetailPage(product.id)"
                        style="cursor: pointer; height: 250px; object-fit: cover"
                      />
                      <span
                        v-if="product.discount"
                        class="badge bg-danger position-absolute top-0 start-0 m-2"
                      >
                        -{{ product.discount }}%
                      </span>
                    </div>
                    <div class="card-body d-flex flex-column">
                      <h5
                        class="card-title text-primary"
                        @click="goToDetailPage(product.id)"
                        style="cursor: pointer"
                      >
                        {{ product.name }}
                      </h5>
                      <p class="card-text text-muted">
                        {{ product.description }}
                      </p>
                      <p class="mb-1">
                        <strong>Volume:</strong> {{ product.volume }}
                      </p>
                      <p class="mb-1">
                        <strong>Skin Type:</strong> {{ product.skinType }}
                      </p>

                      <div class="mt-auto">
                        <h5 class="text-success">
                          {{ formatPrice(product.price) }}
                          <span
                            v-if="product.originalPrice"
                            class="text-muted text-decoration-line-through ms-2"
                          >
                            {{ formatPrice(product.originalPrice) }}
                          </span>
                        </h5>
                        <button
                          class="btn btn-primary w-100 mt-2"
                          @click="goToDetailPage(product.id)"
                        >
                          Buy Now
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Pagination -->
              <div class="d-flex justify-content-center mt-4">
                <nav>
                  <ul class="pagination">
                    <li
                      class="page-item"
                      :class="{ disabled: currentPage === 1 }"
                      @click="handlePageChange(currentPage - 1)"
                    >
                      <a class="page-link">Previous</a>
                    </li>
                    <li
                      v-for="page in totalPages"
                      :key="page"
                      class="page-item"
                      :class="{ active: page === currentPage }"
                      @click="handlePageChange(page)"
                    >
                      <a class="page-link">{{ page }}</a>
                    </li>
                    <li
                      class="page-item"
                      :class="{ disabled: currentPage === totalPages }"
                      @click="handlePageChange(currentPage + 1)"
                    >
                      <a class="page-link">Next</a>
                    </li>
                  </ul>
                </nav>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Footer -->
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
const currentPage = ref(1);
const pageSize = 9;

onMounted(() => {
  fetchProducts();
});

const fetchProducts = async () => {
  try {
    loading.value = true;
    const response = await fetch(
      "https://6793c6495eae7e5c4d8fd8d4.mockapi.io/api/skincare"
    );
    const data = await response.json();
    products.value = data;
    filteredProducts.value = data;
  } catch (error) {
    console.error("Error fetching products:", error);
  } finally {
    loading.value = false;
  }
};

const handleFilterChange = (filters) => {
  let filtered = [...products.value];

  if (filters.searchTerm) {
    const searchLower = filters.searchTerm.toLowerCase();
    filtered = filtered.filter(
      (product) =>
        product.name.toLowerCase().includes(searchLower) ||
        product.description.toLowerCase().includes(searchLower)
    );
  }

  if (filters.priceRange.min !== "") {
    filtered = filtered.filter(
      (product) => product.price >= parseFloat(filters.priceRange.min)
    );
  }
  if (filters.priceRange.max !== "") {
    filtered = filtered.filter(
      (product) => product.price <= parseFloat(filters.priceRange.max)
    );
  }

  if (filters.brands.length > 0) {
    filtered = filtered.filter((product) =>
      filters.brands.includes(product.brand)
    );
  }

  if (filters.categories.length > 0) {
    filtered = filtered.filter((product) =>
      filters.categories.includes(product.category)
    );
  }

  if (filters.skinTypes.length > 0) {
    filtered = filtered.filter((product) =>
      filters.skinTypes.includes(product.skinType)
    );
  }

  filteredProducts.value = filtered;
};

const goToDetailPage = (productId) => {
  if (productId) router.push(`/product/${productId}`);
};

const formatPrice = (price) => {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  }).format(price);
};

const getCurrentProducts = computed(() => {
  const startIndex = (currentPage.value - 1) * pageSize;
  return filteredProducts.value.slice(startIndex, startIndex + pageSize);
});

const totalPages = computed(() =>
  Math.ceil(filteredProducts.value.length / pageSize)
);

const handlePageChange = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
    window.scrollTo(0, 0);
  }
};
</script>

<style scoped>
.card {
  cursor: pointer;
}
.page-link {
  cursor: pointer;
}
</style>
