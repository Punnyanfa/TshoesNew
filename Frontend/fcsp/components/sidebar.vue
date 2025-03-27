<template>
  <div class="card p-4 shadow-lg sidebar">
    <!-- Search -->
    <div class="mb-4 search-container">
      <div class="input-group">
        <span class="input-group-text"><i class="bi bi-search"></i></span>
        <input
          type="text"
          class="form-control"
          placeholder="Tìm kiếm sản phẩm..."
          v-model="searchTerm"
          @input="applyFilters"
        />
      </div>
    </div>

    <!-- Reset Filters -->
    <button class="btn btn-outline-danger w-100 mb-4 reset-btn" @click="resetFilters">
      <i class="bi bi-arrow-clockwise me-2"></i>Reset Filters
    </button>

    <!-- Price Range -->
    <div class="mb-4 price-range">
      <h5 class="filter-title">Price Range (VND)</h5>
      <input
        type="range"
        class="form-range"
        :min="0"
        :max="10000000"
        step="10000"
        v-model.number="priceRange.max"
        @input="applyFilters"
      />
      <div class="d-flex justify-content-between text-muted">
        <span>{{ formatPrice(0) }}</span>
        <span>{{ formatPrice(priceRange.max) }}</span>
      </div>
    </div>

    <!-- Checkbox Filters -->
    <div v-for="(options, type) in filterOptions" :key="type" class="mb-4 filter-group">
      <h5 class="filter-title">{{ options.label }}</h5>
      <div class="form-check" v-for="option in options.items" :key="option">
        <input
          class="form-check-input"
          type="checkbox"
          :id="`${type}-${option}`"
          :value="option"
          v-model="selectedFilters[type]"
          @change="applyFilters"
        />
        <label class="form-check-label" :for="`${type}-${option}`">
          <span v-if="type === 'rating'" class="star-rating">
            {{ Array(option + 1).join('★') }} {{ option === 5 ? '' : '' }}
          </span>
          <span v-else>{{ option }}</span>
        </label>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, reactive } from "vue";

export default {
  props: {
    onFilterChange: Function,
  },
  setup(props) {
    const searchTerm = ref("");
    const priceRange = reactive({ max: 5000000 }); // Giá trị mặc định
    const selectedFilters = reactive({
      categories: [],
      brand: [],
      color: [],
      size: [],
      rating: [], // Thêm rating vào selectedFilters
    });

    const filterOptions = {
      categories: { label: "Danh mục", items: ["Nam", "Nữ", "Trẻ em", "Thể thao"] },
      brand: {
        label: "Thương hiệu",
        items: ["Studio", "Hastech", "Quickiin", "Graphic Corner", "DevItems"],
      },
      color: { label: "Màu sắc", items: ["Xanh", "Đen", "Đỏ", "Xanh dương", "Hồng"] },
      size: { label: "Kích cỡ", items: ["S", "M", "L", "XL"] },
      rating: { label: "Đánh giá", items: [1, 2, 3, 4, 5] },
    };

    const applyFilters = () => {
      props.onFilterChange({ searchTerm: searchTerm.value, priceRange, ...selectedFilters });
    };

    const resetFilters = () => {
      searchTerm.value = "";
      priceRange.max = 5000000;
      Object.keys(selectedFilters).forEach((key) => (selectedFilters[key] = []));
      applyFilters();
    };

    const formatPrice = (value) => {
      return new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(value);
    };

    return { searchTerm, priceRange, selectedFilters, filterOptions, applyFilters, resetFilters, formatPrice };
  },
};
</script>

<style scoped>
.sidebar {
  max-width: 320px;
  border-radius: 12px;
  background: linear-gradient(145deg, #ffffff, #f8f9fa);
  border: none;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.sidebar:hover {
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
}

.filter-title {
  font-size: 1.15rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 12px;
  padding-bottom: 6px;
  border-bottom: 2px solid #3498db;
}

.search-container .form-control {
  border-radius: 8px;
  border: 1px solid #ced4da;
  padding: 10px;
  transition: all 0.3s ease;
}

.search-container .form-control:focus {
  border-color: #3498db;
  box-shadow: 0 0 8px rgba(52, 152, 219, 0.2);
}

.input-group-text {
  background: #3498db;
  color: white;
  border: none;
  border-radius: 8px 0 0 8px;
}

.form-check {
  margin-bottom: 10px;
  padding-left: 2rem;
  transition: all 0.2s ease;
}

.form-check:hover {
  transform: translateX(4px);
}

.form-check-input {
  cursor: pointer;
  border-color: #3498db;
}

.form-check-input:checked {
  background-color: #3498db;
  border-color: #3498db;
}

.form-check-label {
  cursor: pointer;
  color: #34495e;
  font-weight: 500;
  display: flex;
  align-items: center;
}

.star-rating {
  color: #f1c40f;
  font-size: 1.1rem;
}

.reset-btn {
  border-radius: 8px;
  padding: 10px 16px;
  font-weight: 600;
  border-width: 2px;
  transition: all 0.3s ease;
}

.reset-btn:hover {
  background: #e74c3c;
  color: white;
  border-color: #e74c3c;
}

.price-range .form-range {
  accent-color: #3498db;
}

.price-range .text-muted {
  font-size: 0.9rem;
}

.filter-group {
  padding: 0 10px;
}

@media (max-width: 768px) {
  .sidebar {
    max-width: 100%;
    margin: 0 10px;
    padding: 15px;
  }

  .filter-title {
    font-size: 1rem;
  }
}
</style>