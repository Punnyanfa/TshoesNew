<template>
  <div class="admin-container">
    <AdminSidebar />
    <div class="content">
      <div class="header">
        <h1 class="text-primary fw-bold">Manage Templates</h1>
        <button class="add-button" @click="showAddModal = true">
          <i class="fas fa-plus"></i> Add New Template
        </button>
      </div>

      <!-- Search and Filter -->
      <div class="search-filter">
        <div class="search-box">
          <input 
            type="text" 
            v-model="searchQuery" 
            placeholder="Search templates..."
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

      <!-- Templates Table -->
      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>Preview</th>
              <th>Template Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="template in filteredTemplates" :key="template.id">
              <td>
                <img :src="template.previewImage" :alt="template.name" class="template-image">
              </td>
              <td>{{ template.name }}</td>
              <td>{{ template.gender }}</td>
              <td>{{ formatPrice(template.basePrice) }}</td>
              <td>
                <div class="status-toggle">
                  <span :class="['status-text', template.isAvailable ? 'active' : 'inactive']">
                    {{ getStatusText(template.isAvailable) }}
                  </span>
                </div>
              </td>
              <td>
                <button 
                  class="delete-btn" 
                  @click="confirmDelete(template)" 
                  title="Delete"
                > 
                  <i class="fas fa-trash-alt"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Add Template Modal -->
      <TemplateModal 
        :show="showAddModal"
        @close="showAddModal = false"
        @template-added="handleTemplateAdded"
      />
    </div>

    <client-only>
      <teleport to="body">
        <transition name="fade">
          <div v-if="showDeleteModal" class="modal-overlay">
            <div class="modal-wrapper delete-modal-wrapper">
              <div class="modal-container delete-modal-container">
                <div class="modal-header delete-modal-header">
                  <span class="delete-icon">
                    <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="#dc3545" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10" fill="#fff"/><path d="M15 9l-6 6M9 9l6 6"/></svg>
                  </span>
                  <h3>Confirm delete shoe model</h3>
                  <button class="modal-close" @click="showDeleteModal = false">×</button>
                </div>
                <div class="modal-body delete-modal-body">
                  <p v-if="selectedTemplate" class="delete-modal-title">Are you sure you want to delete the shoe model<b>"{{ selectedTemplate.name }}"</b>?</p>
                  <p class="text-danger delete-modal-warning">This action cannot be undone.</p>
                </div>
                <div class="modal-footer delete-modal-footer">
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
import TemplateModal from '@/components/TemplateModal.vue'
import { getAllTemplate } from '@/server/custom-service'
import { deleteTemplate } from '@/server/custom-service'

export default {
  name: 'ManageTemplate',
  components: {
    AdminSidebar,
    TemplateModal
  },
  data() {
    return {
      templates: [],
      searchQuery: '',
      categoryFilter: '',
      showAddModal: false,
      showDeleteModal: false,
      selectedTemplate: null
    }
  },
  computed: {
    filteredTemplates() {
      let filtered = this.templates
      
      if (this.searchQuery) {
        filtered = filtered.filter(template => 
          template.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        )
      }
      
      if (this.categoryFilter) {
        filtered = filtered.filter(template => 
          template.isAvailable === (this.categoryFilter === '1')
        )
      }
      
      return filtered
    }
  },
  async mounted() {
    await this.fetchTemplates()
  },
  methods: {
    async fetchTemplates() {
      try {
        const response = await getAllTemplate()
        if (response && response.code === 200 && response.data) {
          this.templates = response.data.map(template => ({
            id: template.id,
            name: template.name,
            description: template.description,
            gender: template.gender || 'Unisex',
            color: template.color || '',
            basePrice: template.price,
            previewImage: template.twoDImageUrl,
            model3DFile: template.threeFileUrl,
            customShoeDesigns: template.customShoeDesigns || [],
            createdAt: template.createdAt,
            updatedAt: template.updatedAt,
            isAvailable: !template.isDeleted
          }))
        }
      } catch (error) {
        console.error('Error fetching templates:', error)
        alert('Failed to fetch templates. Please try again.')
      }
    },
    formatPrice(price) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(price)
    },
    getStatusText(status) {
      return status ? 'Active' : 'Inactive'
    },
    async handleTemplateAdded() {
      await this.fetchTemplates()
    },
    confirmDelete(template) {
      this.selectedTemplate = template;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      try {
        if (!this.selectedTemplate) {
          alert('No shoe models have been selected for deletion.');
          return;
        }
        
        const response = await deleteTemplate(this.selectedTemplate.id);
        if (response && response.code === 200) {
          await this.fetchTemplates();
          this.showDeleteModal = false;
          this.selectedTemplate = null;
        } else {
          throw new Error(response.message || 'Delete failure!');
        }
      } catch (error) {
        console.error('Error deleting template:', error);
        alert('An error occurred while deleting the shoe model.');
      }
    }
  }
}
</script>

<style scoped>
.admin-container {
  display: flex;
  min-height: 100vh;
  background-color: #f5f5f5;
  margin-left: 250px;
  width: calc(100% - 250px);
}

.content {
  flex: 1;
  padding: 20px;
  width: 100%;
  overflow-x: auto;
  background-color: white;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  margin-top: 20px;
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

th:nth-last-child(2), td:nth-last-child(2) {
  width: 120px;
  text-align: center;
}

th {
  background-color: #f8f9fa;
  font-weight: 600;
}

.template-image {
  width: 50px;
  height: 50px;
  object-fit: cover;
  border-radius: 4px;
}

.status-toggle {
  display: flex;
  align-items: center;
  gap: 10px;
  justify-content: center;
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

.add-button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
}

.add-button:hover {
  background-color: #45a049;
}

.add-button i {
  font-size: 14px;
}

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
  background-color: #f44336;
  color: white;
}

.delete-btn:hover {
  background-color: #d32f2f;
}

.delete-btn i {
  font-size: 16px;
}

/* Delete Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100%; height: 100%;
  background-color: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99999;
}

.delete-modal-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100vw;
  height: 100vh;
}

.delete-modal-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(220,53,69,0.15), 0 1.5px 4px rgba(0,0,0,0.08);
  max-width: 380px;
  width: 100%;
  padding: 0;
  overflow: hidden;
  animation: popIn 0.25s cubic-bezier(.4,2,.6,1) both;
}

@keyframes popIn {
  0% { transform: scale(0.8); opacity: 0; }
  100% { transform: scale(1); opacity: 1; }
}

.delete-modal-header {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 20px 24px 10px 24px;
  border-bottom: 1px solid #f1f1f1;
  background: #fff;
}

.delete-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fff0f1;
  border-radius: 50%;
  padding: 8px;
  margin-right: 8px;
}

.delete-modal-header h3 {
  font-size: 20px;
  font-weight: 600;
  color: #dc3545;
  margin: 0;
  flex: 1;
}

.modal-close {
  background: none;
  border: none;
  font-size: 24px;
  color: #888;
  cursor: pointer;
  margin-left: 8px;
  transition: color 0.2s;
}

.modal-close:hover {
  color: #dc3545;
}

.delete-modal-body {
  padding: 18px 24px 0 24px;
  text-align: center;
}

.delete-modal-title {
  font-size: 16px;
  color: #333;
  margin-bottom: 8px;
}

.delete-modal-warning {
  font-size: 14px;
  color: #dc3545;
  margin-bottom: 0;
}

.delete-modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 18px 24px 20px 24px;
  background: #fff;
  border-top: 1px solid #f1f1f1;
}

.btn-cancel {
  background: #f1f1f1;
  color: #333;
  border: none;
  border-radius: 4px;
  padding: 8px 20px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-cancel:hover {
  background: #e0e0e0;
}

.btn-delete {
  background: #dc3545;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 8px 20px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
  box-shadow: 0 2px 8px rgba(220,53,69,0.08);
}

.btn-delete:hover {
  background: #b52a37;
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
