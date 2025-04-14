<template>
  <div class="manufacturer-layout">
    <div class="main-content">
      <div class="container-fluid mt-4">
        <!-- Profile Section -->
        <div class="card mb-4">
          <div class="card-header bg-primary text-white">
            <h4>Manufacturer Profile</h4>
          </div>
          <div class="card-body">
            <form @submit.prevent="saveProfile">
              <div class="row mb-3">
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <label for="manufacturerName">Company Name</label>
                    <input 
                      type="text" 
                      class="form-control" 
                      id="manufacturerName" 
                      v-model="profile.name"
                      placeholder="Enter company name"
                      required
                    >
                  </div>
                  
                  <div class="form-group mb-3">
                    <label for="contactPerson">Contact Person</label>
                    <input 
                      type="text" 
                      class="form-control" 
                      id="contactPerson" 
                      v-model="profile.contactPerson"
                      placeholder="Enter contact person name"
                      required
                    >
                  </div>
                  
                  <div class="form-group mb-3">
                    <label for="email">Email</label>
                    <input 
                      type="email" 
                      class="form-control" 
                      id="email" 
                      v-model="profile.email"
                      placeholder="Enter email address"
                      required
                    >
                  </div>
                </div>
                
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <label for="phone">Phone</label>
                    <input 
                      type="tel" 
                      class="form-control" 
                      id="phone" 
                      v-model="profile.phone"
                      placeholder="Enter phone number"
                      required
                    >
                  </div>
                  
                  <div class="form-group mb-3">
                    <label for="address">Address</label>
                    <textarea 
                      class="form-control" 
                      id="address" 
                      v-model="profile.address"
                      placeholder="Enter address"
                      rows="2"
                      required
                    ></textarea>
                  </div>
                </div>
              </div>
              
              <div class="d-flex justify-content-end">
                <button type="submit" class="btn btn-primary">Save Profile</button>
              </div>
            </form>
          </div>
        </div>

        <!-- Services Section -->
        <div class="card">
          <div class="card-header bg-success text-white d-flex justify-content-between align-items-center">
            <h4 class="mb-0">My Services</h4>
            <button class="btn btn-light" @click="showAddServiceModal">
              <i class="bi bi-plus-lg"></i> Add New Service
            </button>
          </div>
          <div class="card-body">
            <div class="table-responsive">
              <table class="table table-striped table-hover">
                <thead>
                  <tr>
                    <th>Service Name</th>
                    <th>Description</th>
                    <th>Price</th>
                    <th>Status</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="service in services" :key="service.id">
                    <td>{{ service.name }}</td>
                    <td>{{ service.description }}</td>
                    <td>{{ formatCurrency(service.price) }}</td>
                    <td>
                      <span 
                        :class="{
                          'badge bg-success': service.status === 1,
                          'badge bg-danger': service.status === 0
                        }"
                      >
                        {{ getStatusText(service.status) }}
                      </span>
                    </td>
                    <td>
                      <button class="btn btn-sm btn-info me-1" @click="editService(service)">
                        <i class="bi bi-pencil"></i>
                      </button>
                      <button class="btn btn-sm btn-danger" @click="deleteService(service.id)">
                        <i class="bi bi-trash"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="services.length === 0">
                    <td colspan="5" class="text-center">No services found</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Add/Edit Service Modal -->
      <div v-if="showServiceModal" class="modal-overlay">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header" :class="isEditing ? 'bg-info' : 'bg-success'">
              <h5 class="modal-title text-white">{{ isEditing ? 'Edit Service' : 'Add New Service' }}</h5>
              <button type="button" class="btn-close btn-close-white" @click="hideServiceModal"></button>
            </div>
            <div class="modal-body">
              <form @submit.prevent="saveService">
                <div class="form-group mb-3">
                  <label for="serviceName">Service Name</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="serviceName" 
                    v-model="currentService.name"
                    placeholder="Enter service name"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="serviceDescription">Description</label>
                  <textarea 
                    class="form-control" 
                    id="serviceDescription" 
                    v-model="currentService.description"
                    placeholder="Enter service description"
                    rows="2"
                  ></textarea>
                </div>
                
                <div class="form-group mb-3">
                  <label for="servicePrice">Price (VND)</label>
                  <input 
                    type="number" 
                    class="form-control" 
                    id="servicePrice" 
                    v-model="currentService.price"
                    placeholder="Enter price"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="serviceStatus">Status</label>
                  <select 
                    class="form-select" 
                    id="serviceStatus" 
                    v-model="currentService.status"
                    required
                  >
                    <option value="1">Active</option>
                    <option value="0">Inactive</option>
                  </select>
                </div>
                
                <div class="d-flex justify-content-end">
                  <button type="button" class="btn btn-secondary me-2" @click="hideServiceModal">Cancel</button>
                  <button type="submit" class="btn" :class="isEditing ? 'btn-info' : 'btn-success'">
                    {{ isEditing ? 'Update' : 'Add' }} Service
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ManufacturerPage',
  data() {
    return {
      profile: {
        name: 'Nike',
        contactPerson: 'John Doe',
        email: 'john.doe@nike.com',
        phone: '123-456-7890',
        address: '123 Nike Street, Portland, OR'
      },
      services: [
        { id: 1, name: 'Custom Design', description: 'Create custom shoe designs', price: 150, status: 1 },
        { id: 2, name: 'Bulk Order', description: 'Order in bulk quantities', price: 100, status: 1 }
      ],
      showServiceModal: false,
      isEditing: false,
      currentService: {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1
      }
    }
  },
  methods: {
    saveProfile() {
      // TODO: Implement API call to save profile
      alert('Profile saved successfully!');
    },
    getStatusText(status) {
      switch (status) {
        case 0: return 'Inactive';
        case 1: return 'Active';
        default: return 'Unknown';
      }
    },
    formatCurrency(value) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(value);
    },
    showAddServiceModal() {
      this.isEditing = false;
      this.currentService = {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1
      };
      this.showServiceModal = true;
    },
    editService(service) {
      this.isEditing = true;
      this.currentService = { ...service };
      this.showServiceModal = true;
    },
    hideServiceModal() {
      this.showServiceModal = false;
      this.currentService = {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1
      };
    },
    saveService() {
      if (this.isEditing) {
        // Update existing service
        const index = this.services.findIndex(s => s.id === this.currentService.id);
        if (index !== -1) {
          this.services[index] = { ...this.currentService };
        }
      } else {
        // Add new service
        const newService = {
          ...this.currentService,
          id: this.services.length > 0 ? Math.max(...this.services.map(s => s.id)) + 1 : 1
        };
        this.services.push(newService);
      }
      
      this.hideServiceModal();
    },
    deleteService(serviceId) {
      if (confirm('Are you sure you want to delete this service?')) {
        this.services = this.services.filter(s => s.id !== serviceId);
      }
    }
  }
}
</script>

<style>
.manufacturer-layout {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  padding: 20px;
}

.card {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border: none;
  margin-bottom: 1.5rem;
}

.card-header {
  border-bottom: 0;
}

.table th {
  background-color: #f8f9fa;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-dialog {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  margin: 1.75rem auto;
  max-width: 500px;
  position: relative;
  width: 100%;
}

.modal-content {
  position: relative;
  display: flex;
  flex-direction: column;
  width: 100%;
  pointer-events: auto;
  background-color: #fff;
  background-clip: padding-box;
  border: 1px solid rgba(0, 0, 0, 0.2);
  border-radius: 0.3rem;
  outline: 0;
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem;
  border-bottom: 1px solid #dee2e6;
  border-top-left-radius: 0.3rem;
  border-top-right-radius: 0.3rem;
}

.modal-body {
  position: relative;
  flex: 1 1 auto;
  padding: 1rem;
}

.btn-close {
  background: transparent;
  border: 0;
  padding: 0.5rem;
  cursor: pointer;
}

.btn-close-white {
  filter: invert(1) grayscale(100%) brightness(200%);
}
</style>
