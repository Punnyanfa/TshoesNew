<template>
  <div class="admin-container">
    <AdminSidebar />
    <div class="content">
      <div class="header">
        <h1>Manage Templates</h1>
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
              <th>ID</th>
              <th>Preview</th>
              <th>Template Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="template in filteredTemplates" :key="template.id">
              <td>{{ template.id }}</td>
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
  </div>
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue'
import TemplateModal from '@/components/TemplateModal.vue'
import { getAllTemplate } from '@/server/custom-service'

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
      showAddModal: false
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
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
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
</style>
