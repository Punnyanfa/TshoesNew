<template>
  <div class="card p-3 shadow-sm">
    <!-- Search -->
    <div class="mb-3">
      <input type="text" class="form-control" placeholder="Tìm kiếm sản phẩm..." v-model="searchTerm" @input="applyFilters" />
    </div>

    <!-- Reset Filters -->
    <button class="btn btn-danger w-100 mb-3" @click="resetFilters">Reset Filters</button>

    <!-- Price Range -->
    <div class="mb-3">
      <h5>Price Range (VND)</h5>
      <div class="d-flex">
        <input type="number" class="form-control me-2" placeholder="Min" v-model.number="priceRange.min" @input="applyFilters" />
        <input type="number" class="form-control" placeholder="Max" v-model.number="priceRange.max" @input="applyFilters" />
      </div>
    </div>

    <!-- Checkbox Filters -->
    <div v-for="(options, type) in filterOptions" :key="type" class="mb-3">
      <h5>{{ options.label }}</h5>
      <div class="form-check" v-for="option in options.items" :key="option">
        <input class="form-check-input" type="checkbox" :id="option" :value="option" v-model="selectedFilters[type]" @change="applyFilters" />
        <label class="form-check-label" :for="option">{{ option }}</label>
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
    const priceRange = reactive({ min: "", max: "" });
    const selectedFilters = reactive({
      categories: [],
      brand: [],
      color: [],
      size: [],
    });

    const filterOptions = {
      categories: { label: "Categories", items: ["Mens", "Womens", "Kids", "Sports"] },
      brand: { label: "Brand", items: ["Studio", "Hastech", "Quickiin", "Graphic Corner", "DevItems"] },
      color: { label: "Color", items: ["Green", "Black", "Red", "Blue", "Pink"] },
      size: { label: "Size", items: ["S", "M", "L", "XL"] },
    };

    const applyFilters = () => {
      props.onFilterChange({ searchTerm: searchTerm.value, priceRange, ...selectedFilters });
    };

    const resetFilters = () => {
      searchTerm.value = "";
      priceRange.min = "";
      priceRange.max = "";
      Object.keys(selectedFilters).forEach((key) => (selectedFilters[key] = []));
      applyFilters();
    };

    return { searchTerm, priceRange, selectedFilters, filterOptions, applyFilters, resetFilters };
  },
};
</script>

<style scoped>
.card { max-width: 300px; }
</style>