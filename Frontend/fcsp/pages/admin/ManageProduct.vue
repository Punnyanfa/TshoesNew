<template>
  <div class="admin-container">
    <AdminSidebar />
    <div class="content">
      <!-- <div class="header">
        <h1>Manage Products</h1>
        <button class="add-btn" @click="showAddModal = true">
          <i class="fas fa-plus"></i> Add Product
        </button>
      </div> -->

      <!-- Search and Filter -->
      <div class="search-filter">
        <div class="search-box">
          <input 
            type="text" 
            v-model="searchQuery" 
            placeholder="Search products..."
          />
          <i class="fas fa-search"></i>
        </div>
        <div class="filter-box">
          <select v-model="categoryFilter">
            <option value="">All Status</option>
            <option value="1">Active</option>
            <option value="2">Inactive</option>
          </select>
        </div>
      </div>

      <!-- Products Table -->
      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Image</th>
              <th>Product Name</th>
              <th>Price</th>
              <th>Rating</th>
              <th>Rating Count</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="product in paginatedProducts" :key="product.id">
              <td>{{ product.id }}</td>
              <td>
                <img :src="product.image" :alt="product.name" class="product-image">
              </td>
              <td>{{ product.name }}</td>
              <td>{{ formatPrice(product.price) }}</td>
              <td>{{ product.rating }}/5</td>
              <td>{{ product.ratingCount }}</td>
              <td>
                <div class="status-toggle">
                  <label class="switch">
                    <input 
                      type="checkbox" 
                      :checked="product.status === 1"
                      @change="toggleStatus(product)"
                    >
                    <span class="slider round"></span>
                  </label>
                  <span :class="['status-text', product.status === 1 ? 'active' : 'inactive']">
                    {{ getStatusText(product.status) }}
                  </span>
                </div>
              </td>
              <td class="actions">
                <button class="edit-btn" @click="editProduct(product)" title="Edit">
                  <i class="fas fa-edit"></i>
                </button>
                <button 
                  class="delete-btn" 
                  @click="confirmDelete(product)" 
                  title="Delete"
                > 
                  <i class="fas fa-trash-alt"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <div class="pagination" v-if="totalPages > 1">
        <button 
          class="page-btn nav-btn" 
          @click="previousPage" 
          :disabled="currentPage === 1"
        >
          <i class="fas fa-chevron-left"></i>
        </button>

        <button 
          v-if="displayedPages[0] > 1" 
          class="page-btn" 
          @click="changePage(1)"
        >
          1
        </button>

        <span v-if="displayedPages[0] > 2" class="page-ellipsis">...</span>

        <button 
          v-for="page in displayedPages" 
          :key="page"
          :class="['page-btn', currentPage === page ? 'active' : '']"
          @click="changePage(page)"
        >
          {{ page }}
        </button>

        <span v-if="displayedPages[displayedPages.length - 1] < totalPages - 1" class="page-ellipsis">...</span>

        <button 
          v-if="displayedPages[displayedPages.length - 1] < totalPages" 
          class="page-btn" 
          @click="changePage(totalPages)"
        >
          {{ totalPages }}
        </button>

        <button 
          class="page-btn nav-btn" 
          @click="nextPage" 
          :disabled="currentPage === totalPages"
        >
          <i class="fas fa-chevron-right"></i>
        </button>
      </div>

      <!-- Add/Edit Product Modal -->
      <div v-if="showAddModal || showEditModal" class="modal">
        <div class="modal-content">
          <span class="close" @click="closeModal">&times;</span>
          <h2>{{ showAddModal ? 'Add New Product' : 'Edit Product' }}</h2>
          <form @submit.prevent="showAddModal ? addProduct() : updateProduct()">
            <div class="form-group">
              <label>Product Name</label>
              <input type="text" v-model="productForm.name" required>
            </div>
            <div class="form-group">
              <label>Price</label>
              <input type="number" v-model="productForm.price" required>
            </div>
            <div class="form-group">
              <label>Description</label>
              <textarea v-model="productForm.description" required></textarea>
            </div>
            <div class="form-group">
              <label>Image</label>
              <input type="file" @change="handleImageUpload" accept="image/*">
              <img v-if="productForm.previewImageUrl" :src="productForm.previewImageUrl" class="preview-image">
            </div>
            <div class="form-group">
              <label>Status</label>
              <select v-model="productForm.status">
                <option :value="1">Active</option>
                <option :value="2">Inactive</option>
              </select>
            </div>
            <div class="form-actions">
              <button type="submit" class="submit-btn">
                {{ showAddModal ? 'Add' : 'Update' }}
              </button>
              <button type="button" class="cancel-btn" @click="closeModal">Cancel</button>
            </div>
          </form>
        </div>
      </div>
    </div>
    
    <client-only>
      <teleport to="body">
        <transition name="fade">
          <div v-if="showDeleteModal" class="modal-overlay">
            <div class="modal-wrapper">
              <div class="modal-container">
                <div class="modal-header">
                  <h3>Delete Product</h3>
                  <button class="modal-close" @click="showDeleteModal = false">×</button>
                </div>
                <div class="modal-body">
                  <p v-if="selectedProduct">Are you sure you want to delete "{{ selectedProduct.name }}"?</p>
                  <p class="text-danger">This action cannot be undone.</p>
                </div>
                <div class="modal-footer">
                  <button class="btn-cancel" @click="showDeleteModal = false">Cancel</button>
                  <button class="btn-delete" @click="handleDelete">Delete</button>
                </div>
              </div>
            </div>
          </div>
        </transition>
      </teleport>
    </client-only>
  </div>
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue'
import { getAllProducts, updateProductStatus, deleteProduct } from '@/server/product-service'

export default {
  name: 'ManageProduct',
  components: {
    AdminSidebar
  },
  data() {
    return {
      products: [],
      categories: [],
      searchQuery: '',
      categoryFilter: '',
      currentPage: 1,
      itemsPerPage: 9,
      maxVisiblePages: 5,
      showAddModal: false,
      showEditModal: false,
      showDeleteModal: false,
      productForm: {
        name: '',
        previewImageUrl: '',
        rating: 0,
        ratingCount: 0,
        price: 0,
        description: '',
        status: 1,
        gender: null
      },
      selectedProduct: null
    }
  },
  computed: {
    filteredProducts() {
      let filtered = this.products
      
      if (this.searchQuery) {
        filtered = filtered.filter(product => 
          product.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        )
      }
      
      if (this.categoryFilter) {
        filtered = filtered.filter(product => 
          product.status === parseInt(this.categoryFilter)
        )
      }
      
      return filtered
    },
    paginatedProducts() {
      const start = (this.currentPage - 1) * this.itemsPerPage
      const end = start + this.itemsPerPage
      return this.filteredProducts.slice(start, end)
    },
    totalPages() {
      return Math.ceil(this.filteredProducts.length / this.itemsPerPage)
    },
    displayedPages() {
      const half = Math.floor(this.maxVisiblePages / 2)
      let start = this.currentPage - half
      let end = this.currentPage + half

      if (start < 1) {
        start = 1
        end = Math.min(this.totalPages, this.maxVisiblePages)
      }

      if (end > this.totalPages) {
        end = this.totalPages
        start = Math.max(1, end - this.maxVisiblePages + 1)
      }

      return Array.from({ length: end - start + 1 }, (_, i) => start + i)
    }
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await getAllProducts()
        if (response && response.code === 200 && response.data && response.data.designs) {
          this.products = response.data.designs.map(design => ({
            id: design.id,
            name: design.name,
            image: design.previewImageUrl,
            price: design.price || 0,
            status: 1, 
            description: design.description || '',
            rating: design.rating || 0,
            ratingCount: design.ratingCount || 0
          }))
        }
      } catch (error) {
        console.error('Error fetching products:', error)
      }
    },
    formatPrice(price) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(price)
    },
    getStatusText(status) {
      return status === 1 ? 'Active' : 'Inactive'
    },
    handleImageUpload(event) {
      const file = event.target.files[0]
      if (file) {
        this.productForm.previewImageUrl = URL.createObjectURL(file)
      }
    },
    editProduct(product) {
      this.selectedProduct = product
      this.productForm = {
        name: product.name,
        previewImageUrl: product.image,
        price: product.price,
        description: product.description,
        status: product.status,
        rating: product.rating,
        ratingCount: product.ratingCount,
        gender: product.gender
      }
      this.showEditModal = true
    },
    async updateProduct() {
      try {
        if (this.selectedProduct) {
          // Update the product status
          const response = await updateProductStatus(this.selectedProduct.id, this.productForm.status);
          
          if (response && response.code === 200) {
            // Show success message or notification
            alert('Product status updated successfully!');
            this.closeModal();
            await this.fetchProducts(); // Refresh the product list
          } else {
            throw new Error('Failed to update product status');
          }
        }
      } catch (error) {
        console.error('Error updating product:', error);
        alert('An error occurred while updating the product!');
      }
    },
    confirmDelete(product) {
      console.log('Confirming delete for product:', product);
      this.selectedProduct = product;
      this.showDeleteModal = true;
      console.log('Modal state:', this.showDeleteModal);
    },
    async handleDelete() {
      try {
        if (!this.selectedProduct) {
          alert('No product selected for deletion');
          return;
        }
        
        console.log('Attempting to delete product:', this.selectedProduct.id);
        await deleteProduct(this.selectedProduct.id);
        
        // If we reach here, deletion was successful
        alert('Product deleted successfully!');
        this.showDeleteModal = false;
        this.selectedProduct = null;
        await this.fetchProducts(); // Refresh the list
        
      } catch (error) {
        console.error('Error deleting product:', error);
        alert(error.message || 'An error occurred while deleting the product');
      } finally {
        this.showDeleteModal = false;
      }
    },
    closeModal() {
      this.showAddModal = false
      this.showEditModal = false
      this.productForm = {
        name: '',
        previewImageUrl: '',
        rating: 0,
        ratingCount: 0,
        price: 0,
        description: '',
        status: 1,
        gender: null
      }
    },
    changePage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page
      }
    },
    previousPage() {
      if (this.currentPage > 1) {
        this.currentPage--
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++
      }
    },
    async toggleStatus(product) {
      try {
        const newStatus = product.status === 1 ? 2 : 1;
        const response = await updateProductStatus(product.id, newStatus);
        
        if (response && response.code === 200) {
          // Update the local product status
          product.status = newStatus;
          // Show success message
          alert('Product status updated successfully!');
        } else {
          throw new Error('Failed to update product status');
        }
      } catch (error) {
        console.error('Error updating product status:', error);
        alert('An error occurred while updating product status!');
        // Revert the checkbox state
        product.status = product.status === 1 ? 2 : 1;
      }
    }
  },
  mounted() {
    this.fetchProducts()
  }
}
</script>

<style scoped>
.admin-container {
  display: flex;
  min-height: 100vh;
  background-color: #f5f5f5;
  margin-left: 250px; /* Add margin for sidebar */
  width: calc(100% - 250px); /* Adjust width to account for sidebar */
}

.content {
  flex: 1;
  padding: 20px;
  width: 100%;
  overflow-x: auto;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.add-btn {
  background-color: #4CAF50;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
}

.search-filter {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
}

.search-box, .filter-box {
  position: relative;
}

.search-box input, .filter-box select {
  padding: 10px 40px 10px 15px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 300px;
}

.search-box i {
  position: absolute;
  right: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: #666;
}

.table-container {
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
}

th, td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #ddd;
  vertical-align: middle;
}

th:last-child, td:last-child {
  width: 120px;
  text-align: center;
}

th {
  background-color: #f8f9fa;
  font-weight: 600;
}

.product-image {
  width: 50px;
  height: 50px;
  object-fit: cover;
  border-radius: 4px;
}

.status-toggle {
  display: flex;
  align-items: center;
  gap: 10px;
}

.switch {
  position: relative;
  display: inline-block;
  width: 50px;
  height: 24px;
}

.switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 16px;
  width: 16px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  transition: .4s;
}

input:checked + .slider {
  background-color: #4CAF50;
}

input:focus + .slider {
  box-shadow: 0 0 1px #4CAF50;
}

input:checked + .slider:before {
  transform: translateX(26px);
}

.slider.round {
  border-radius: 24px;
}

.slider.round:before {
  border-radius: 50%;
}

.status-text {
  font-size: 14px;
  font-weight: 500;
}

.status-text.active {
  color: #4CAF50;
}

.status-text.inactive {
  color: #f44336;
}

.actions {
  display: flex;
  gap: 8px;
  justify-content: center;
  align-items: center;
}

.edit-btn, .delete-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.edit-btn {
  background-color: #2196F3;
  color: white;
}

.edit-btn:hover {
  background-color: #1976D2;
}

.delete-btn {
  background-color: #f44336;
  color: white;
}

.delete-btn:hover {
  background-color: #d32f2f;
}

.edit-btn i, .delete-btn i {
  font-size: 16px;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  margin-top: 20px;
}

.page-btn {
  min-width: 32px;
  height: 32px;
  padding: 0 6px;
  border: 1px solid #ddd;
  background-color: white;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
  color: #333;
  transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
  background-color: #f5f5f5;
  border-color: #ccc;
}

.page-btn.active {
  background-color: #2196F3;
  color: white;
  border-color: #2196F3;
}

.page-btn:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}

.nav-btn {
  background-color: #f5f5f5;
}

.page-ellipsis {
  color: #666;
  padding: 0 4px;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99999;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
  width: 90%;
  max-width: 400px;
  position: relative;
  z-index: 100000;
}

.delete-modal {
  text-align: center;
  margin: auto;
}

.delete-modal h2 {
  margin-bottom: 1rem;
  color: #333;
}

.delete-modal p {
  margin-bottom: 1rem;
  color: #666;
}

.text-danger {
  color: #dc3545;
  font-weight: 500;
}

.delete-actions {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-top: 1.5rem;
}

.btn-cancel,
.btn-delete {
  padding: 0.5rem 1.5rem;
  border: none;
  border-radius: 4px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-cancel {
  background-color: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background-color: #5a6268;
}

.btn-delete {
  background-color: #dc3545;
  color: white;
}

.btn-delete:hover {
  background-color: #c82333;
}

/* Action buttons */
.actions {
  display: flex;
  gap: 8px;
  justify-content: center;
  align-items: center;
}

.edit-btn,
.delete-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.edit-btn {
  background-color: #2196F3;
  color: white;
}

.edit-btn:hover {
  background-color: #1976D2;
}

.delete-btn {
  background-color: #f44336;
  color: white;
}

.delete-btn:hover {
  background-color: #d32f2f;
}

.edit-btn i,
.delete-btn i {
  font-size: 16px;
}

.modal-overlay {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.modal-container {
  width: 400px;
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
}

.modal-header {
  padding: 15px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
}

.modal-header h3 {
  margin: 0;
  color: #333;
  font-size: 1.25rem;
}

.modal-close {
  border: none;
  background: none;
  font-size: 24px;
  font-weight: bold;
  color: #666;
  cursor: pointer;
}

.modal-close:hover {
  color: #333;
}

.modal-body {
  padding: 20px;
  text-align: center;
}

.modal-footer {
  padding: 15px 20px;
  display: flex;
  justify-content: center;
  gap: 10px;
  border-top: 1px solid #eee;
}

.btn-cancel,
.btn-delete {
  padding: 8px 20px;
  border: none;
  border-radius: 4px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-cancel {
  background-color: #6c757d;
  color: white;
}

.btn-cancel:hover {
  background-color: #5a6268;
}

.btn-delete {
  background-color: #dc3545;
  color: white;
}

.btn-delete:hover {
  background-color: #c82333;
}

/* Transition animations */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
