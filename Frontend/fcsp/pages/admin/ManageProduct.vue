<template>
  <div class="admin-container">
    <AdminSidebar />
    <div class="content">
      
      <!-- <div class="header">
        <h1>Manage Products</h1>
        <button class="add-btn" @click="showAddModal = true">
          <i class="fas fa-plus"></i> Add Product
        </button>
      </div>

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
                <span :class="['status-text', getStatusClass(product.status)]">
                  {{ getStatusText(product.status) }}
                </span>
              </td>
              <td class="actions">
                <button 
                  class="btn btn-sm btn-outline-success me-1" 
                  data-bs-toggle="tooltip" 
                  title="Edit Status"
                  @click="openStatusModal(product)"
                >
                  <i class="bi bi-pencil"></i>
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

      <!-- Modal Update Status (Bootstrap style) -->
      <div class="modal fade" id="statusModal" tabindex="-1" ref="statusModal">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header bg-primary text-white">
              <h5 class="modal-title">Update Product Status</h5>
              <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label for="status-select" class="form-label">Select new status:</label>
                <select id="status-select" class="form-select" v-model="selectedStatus">
                  <option :value="1">Public</option>
                  <!-- <option :value="2">Private</option> -->
                  <option :value="3">Archived</option>
                  <option :value="4">Pending</option>
                </select>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="button" class="btn btn-success" @click="confirmUpdateStatus">Update</button>
            </div>
          </div>
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
import { getAllProducts, deleteProduct } from '@/server/product-service'
import { updateStatus } from '@/server/designUp-service'

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
      showStatusModal: false,
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
      selectedProduct: null,
      selectedStatus: null,
      statusModal: null
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
        console.log(response)
        if (response && response.code === 200 && response.data && response.data.designs) {
          this.products = response.data.designs.map(design => ({
            id: design.id,
            name: design.name,
            image: design.previewImageUrl,
            price: design.total || 0,
            status: design.status,
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
      switch (status) {
        case 1: return 'Public';
        
        case 3: return 'Archived';
        case 4: return 'Pending';
        default: return 'Unknown';
      }
    },
    getStatusClass(status) {
      switch (status) {
        case 1: return 'public';
        case 3: return 'archived';
        case 4: return 'pending';
        default: return '';
      }
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
          const response = await updateStatus(this.selectedProduct.id, this.productForm.status);
          
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
    openStatusModal(product) {
      if (!this.statusModal && window.bootstrap) {
        this.statusModal = new window.bootstrap.Modal(this.$refs.statusModal);
      }
      this.selectedProduct = product;
      this.selectedStatus = product.status;
      this.statusModal.show();
    },
    closeStatusModal() {
      if (this.statusModal) this.statusModal.hide();
      this.selectedProduct = null;
      this.selectedStatus = null;
    },
    async confirmUpdateStatus() {
      if (!this.selectedProduct) return;
      try {
        const response = await updateStatus(this.selectedProduct.id, this.selectedStatus);
        if (response && response.code === 200) {
          this.selectedProduct.status = this.selectedStatus;
          alert('Status updated successfully!');
          this.closeStatusModal();
        } else {
          throw new Error('Failed to update status');
        }
      } catch (error) {
        alert('Error updating status!');
      }
    }
  },
  mounted() {
    this.fetchProducts();
    // Ensure Bootstrap JS is available
    if (typeof window !== 'undefined' && !window.bootstrap) {
      import('bootstrap/dist/js/bootstrap.bundle.min.js').then((mod) => {
        window.bootstrap = mod;
      });
    }
  }
}
</script>

<style scoped>
.admin-container {
  display: flex;
  min-height: 100vh;
  background-color: white;
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

.status-text {
  font-size: 14px;
  font-weight: 500;
}

.status-text.public {
  color: #4CAF50;
}

.status-text.private {
  color: #f44336;
}

.status-text.archived {
  color: #2196F3;
}

.status-text.pending {
  color: #FF9800;
}

.status-update-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  color: #666;
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
