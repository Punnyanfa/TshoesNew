<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="manage-shipping-container">
        <div class="container py-4">
          <h2 class="mb-4">Shipping Management</h2>
          
          <!-- Dashboard Stats -->
          <div class="row mb-4">
            <div class="col-md-3 mb-3">
              <div class="card h-100">
                <div class="card-body text-center">
                  <h5 class="card-title">Active Methods</h5>
                  <p class="card-text display-4">{{ activeShippingMethods.length }}</p>
                </div>
              </div>
            </div>
            <div class="col-md-3 mb-3">
              <div class="card h-100">
                <div class="card-body text-center">
                  <h5 class="card-title">Shipping Zones</h5>
                  <p class="card-text display-4">{{ shippingZones.length }}</p>
                </div>
              </div>
            </div>
            <div class="col-md-3 mb-3">
              <div class="card h-100">
                <div class="card-body text-center">
                  <h5 class="card-title">Avg. Shipping Cost</h5>
                  <p class="card-text display-4">${{ averageShippingCost.toFixed(2) }}</p>
                </div>
              </div>
            </div>
            <div class="col-md-3 mb-3">
              <div class="card h-100">
                <div class="card-body text-center">
                  <h5 class="card-title">Free Shipping Orders</h5>
                  <p class="card-text display-4">{{ freeShippingPercentage }}%</p>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Tabs for different sections -->
          <ul class="nav nav-tabs mb-4">
            <li class="nav-item">
              <a 
                class="nav-link" 
                :class="{ active: activeTab === 'methods' }" 
                href="#" 
                @click.prevent="activeTab = 'methods'"
              >
                Shipping Methods
              </a>
            </li>
            <li class="nav-item">
              <a 
                class="nav-link" 
                :class="{ active: activeTab === 'zones' }" 
                href="#" 
                @click.prevent="activeTab = 'zones'"
              >
                Shipping Zones
              </a>
            </li>
            <li class="nav-item">
              <a 
                class="nav-link" 
                :class="{ active: activeTab === 'settings' }" 
                href="#" 
                @click.prevent="activeTab = 'settings'"
              >
                Settings
              </a>
            </li>
          </ul>
          
          <!-- Shipping Methods Tab -->
          <div v-if="activeTab === 'methods'">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <h3 class="mb-0">Shipping Methods</h3>
              <button class="btn btn-primary" @click="openMethodForm()">
                Add New Method
              </button>
            </div>
            
            <!-- Shipping Methods Table -->
            <div class="card">
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table table-hover">
                    <thead>
                      <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Base Cost</th>
                        <th>Estimated Delivery</th>
                        <th>Status</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="method in shippingMethods" :key="method.id">
                        <td>{{ method.name }}</td>
                        <td>{{ method.description }}</td>
                        <td>${{ method.baseCost.toFixed(2) }}</td>
                        <td>{{ method.estimatedDelivery }}</td>
                        <td>
                          <span 
                            class="badge" 
                            :class="method.isActive ? 'bg-success' : 'bg-danger'"
                          >
                            {{ method.isActive ? 'Active' : 'Inactive' }}
                          </span>
                        </td>
                        <td>
                          <div class="btn-group">
                            <button 
                              class="btn btn-sm btn-outline-primary" 
                              @click="openMethodForm(method)"
                            >
                              Edit
                            </button>
                            <button 
                              class="btn btn-sm btn-outline-danger" 
                              @click="deleteShippingMethod(method.id)"
                            >
                              Delete
                            </button>
                          </div>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            
            <!-- Add/Edit Shipping Method Modal -->
            <div class="modal fade" id="shippingMethodModal" tabindex="-1" aria-hidden="true" ref="methodModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title">{{ isEditingMethod ? 'Edit Shipping Method' : 'Add Shipping Method' }}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <form @submit.prevent="saveShippingMethod">
                      <div class="mb-3">
                        <label for="methodName" class="form-label">Method Name*</label>
                        <input 
                          type="text" 
                          class="form-control" 
                          id="methodName" 
                          v-model="currentMethod.name"
                          required
                        >
                      </div>
                      
                      <div class="mb-3">
                        <label for="methodDescription" class="form-label">Description</label>
                        <textarea 
                          class="form-control" 
                          id="methodDescription" 
                          v-model="currentMethod.description"
                          rows="2"
                        ></textarea>
                      </div>
                      
                      <div class="row">
                        <div class="col-md-6 mb-3">
                          <label for="methodBaseCost" class="form-label">Base Cost ($)*</label>
                          <input 
                            type="number" 
                            class="form-control" 
                            id="methodBaseCost" 
                            v-model.number="currentMethod.baseCost"
                            step="0.01"
                            min="0"
                            required
                          >
                        </div>
                        <div class="col-md-6 mb-3">
                          <label for="methodEstimatedDelivery" class="form-label">Estimated Delivery*</label>
                          <input 
                            type="text" 
                            class="form-control" 
                            id="methodEstimatedDelivery" 
                            v-model="currentMethod.estimatedDelivery"
                            placeholder="e.g. 2-3 business days"
                            required
                          >
                        </div>
                      </div>
                      
                      <div class="form-check mb-3">
                        <input 
                          class="form-check-input" 
                          type="checkbox" 
                          id="methodIsActive" 
                          v-model="currentMethod.isActive"
                        >
                        <label class="form-check-label" for="methodIsActive">
                          Active
                        </label>
                      </div>
                      
                      <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="submit" class="btn btn-primary">Save</button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Shipping Zones Tab -->
          <div v-if="activeTab === 'zones'">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <h3 class="mb-0">Shipping Zones</h3>
              <button class="btn btn-primary" @click="openZoneForm()">
                Add New Zone
              </button>
            </div>
            
            <!-- Shipping Zones Table -->
            <div class="card">
              <div class="card-body">
                <div class="table-responsive">
                  <table class="table table-hover">
                    <thead>
                      <tr>
                        <th>Zone Name</th>
                        <th>Countries/Regions</th>
                        <th>Available Methods</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="zone in shippingZones" :key="zone.id">
                        <td>{{ zone.name }}</td>
                        <td>
                          <span class="badge bg-light text-dark me-1 mb-1" v-for="(country, index) in zone.countries" :key="index">
                            {{ country }}
                          </span>
                        </td>
                        <td>{{ zone.availableMethods.length }}</td>
                        <td>
                          <div class="btn-group">
                            <button 
                              class="btn btn-sm btn-outline-primary" 
                              @click="openZoneForm(zone)"
                            >
                              Edit
                            </button>
                            <button 
                              class="btn btn-sm btn-outline-danger" 
                              @click="deleteShippingZone(zone.id)"
                            >
                              Delete
                            </button>
                          </div>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            
            <!-- Add/Edit Shipping Zone Modal -->
            <div class="modal fade" id="shippingZoneModal" tabindex="-1" aria-hidden="true" ref="zoneModal">
              <div class="modal-dialog modal-lg">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title">{{ isEditingZone ? 'Edit Shipping Zone' : 'Add Shipping Zone' }}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <form @submit.prevent="saveShippingZone">
                      <div class="mb-3">
                        <label for="zoneName" class="form-label">Zone Name*</label>
                        <input 
                          type="text" 
                          class="form-control" 
                          id="zoneName" 
                          v-model="currentZone.name"
                          required
                        >
                      </div>
                      
                      <div class="mb-3">
                        <label class="form-label">Countries/Regions*</label>
                        <select 
                          class="form-select" 
                          multiple 
                          v-model="currentZone.countries"
                          required
                          size="5"
                        >
                          <option v-for="country in availableCountries" :key="country" :value="country">
                            {{ country }}
                          </option>
                        </select>
                        <small class="form-text text-muted">Hold Ctrl/Cmd to select multiple countries</small>
                      </div>
                      
                      <div class="mb-3">
                        <label class="form-label">Available Shipping Methods*</label>
                        <div class="card">
                          <div class="card-body">
                            <div class="form-check mb-2" v-for="method in shippingMethods" :key="method.id">
                              <input 
                                class="form-check-input" 
                                type="checkbox" 
                                :id="`method-${method.id}`" 
                                :value="method.id"
                                v-model="currentZone.availableMethods"
                              >
                              <label class="form-check-label" :for="`method-${method.id}`">
                                {{ method.name }} - ${{ method.baseCost.toFixed(2) }}
                              </label>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div class="mb-3">
                        <label class="form-label">Method Rate Adjustments</label>
                        <div class="card mb-2" v-for="methodId in currentZone.availableMethods" :key="methodId">
                          <div class="card-body">
                            <h6>{{ getMethodName(methodId) }}</h6>
                            <div class="row">
                              <div class="col-md-6 mb-2">
                                <label :for="`adjustment-${methodId}`" class="form-label">Additional Cost ($)</label>
                                <input 
                                  type="number" 
                                  class="form-control" 
                                  :id="`adjustment-${methodId}`" 
                                  v-model.number="currentZone.rateAdjustments[methodId]"
                                  step="0.01"
                                  min="0"
                                >
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="submit" class="btn btn-primary">Save</button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Settings Tab -->
          <div v-if="activeTab === 'settings'">
            <h3 class="mb-3">Shipping Settings</h3>
            
            <div class="card mb-4">
              <div class="card-body">
                <form @submit.prevent="saveSettings">
                  <h5 class="card-title mb-3">General Settings</h5>
                  
                  <div class="mb-3">
                    <label for="defaultShippingMethod" class="form-label">Default Shipping Method</label>
                    <select 
                      class="form-select" 
                      id="defaultShippingMethod" 
                      v-model="settings.defaultMethodId"
                    >
                      <option value="">Select a default method</option>
                      <option 
                        v-for="method in activeShippingMethods" 
                        :key="method.id" 
                        :value="method.id"
                      >
                        {{ method.name }}
                      </option>
                    </select>
                  </div>
                  
                  <div class="mb-3 form-check">
                    <input 
                      class="form-check-input" 
                      type="checkbox" 
                      id="enableFreeShipping" 
                      v-model="settings.enableFreeShipping"
                    >
                    <label class="form-check-label" for="enableFreeShipping">
                      Enable Free Shipping
                    </label>
                  </div>
                  
                  <div class="mb-3" v-if="settings.enableFreeShipping">
                    <label for="freeShippingThreshold" class="form-label">Free Shipping Threshold ($)</label>
                    <input 
                      type="number" 
                      class="form-control" 
                      id="freeShippingThreshold" 
                      v-model.number="settings.freeShippingThreshold"
                      step="0.01"
                      min="0"
                    >
                    <small class="form-text text-muted">Orders above this amount qualify for free shipping</small>
                  </div>
                  
                  <h5 class="card-title mb-3 mt-4">Calculation Settings</h5>
                  
                  <div class="mb-3">
                    <label for="calculationMethod" class="form-label">Shipping Cost Calculation</label>
                    <select 
                      class="form-select" 
                      id="calculationMethod" 
                      v-model="settings.calculationMethod"
                    >
                      <option value="flat">Flat Rate</option>
                      <option value="weight">Based on Weight</option>
                      <option value="price">Based on Order Price</option>
                      <option value="items">Based on Number of Items</option>
                    </select>
                  </div>
                  
                  <div class="mb-3 form-check">
                    <input 
                      class="form-check-input" 
                      type="checkbox" 
                      id="taxIncluded" 
                      v-model="settings.taxIncluded"
                    >
                    <label class="form-check-label" for="taxIncluded">
                      Include Tax in Shipping Cost
                    </label>
                  </div>
                  
                  <div class="mb-3 form-check">
                    <input 
                      class="form-check-input" 
                      type="checkbox" 
                      id="showEstimatedDelivery" 
                      v-model="settings.showEstimatedDelivery"
                    >
                    <label class="form-check-label" for="showEstimatedDelivery">
                      Show Estimated Delivery Time
                    </label>
                  </div>
                  
                  <button type="submit" class="btn btn-primary">Save Settings</button>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue';

export default {
  name: 'ManageShipping',
  components: {
    AdminSidebar
  },
  data() {
    return {
      activeTab: 'methods',
      shippingMethods: [
        {
          id: 1,
          name: 'Standard Shipping',
          description: 'Regular postal service',
          baseCost: 5.99,
          estimatedDelivery: '3-5 business days',
          isActive: true
        },
        {
          id: 2,
          name: 'Express Shipping',
          description: 'Faster delivery option',
          baseCost: 12.99,
          estimatedDelivery: '1-2 business days',
          isActive: true
        },
        {
          id: 3,
          name: 'Economy Shipping',
          description: 'Slower but cheaper option',
          baseCost: 3.99,
          estimatedDelivery: '5-7 business days',
          isActive: true
        },
        {
          id: 4,
          name: 'International Shipping',
          description: 'For international orders',
          baseCost: 19.99,
          estimatedDelivery: '7-14 business days',
          isActive: false
        }
      ],
      currentMethod: this.getEmptyMethod(),
      isEditingMethod: false,
      editingMethodId: null,
      
      shippingZones: [
        {
          id: 1,
          name: 'Domestic',
          countries: ['USA'],
          availableMethods: [1, 2, 3],
          rateAdjustments: {
            1: 0,
            2: 0,
            3: 0
          }
        },
        {
          id: 2,
          name: 'Canada & Mexico',
          countries: ['Canada', 'Mexico'],
          availableMethods: [1, 2],
          rateAdjustments: {
            1: 5,
            2: 8
          }
        },
        {
          id: 3,
          name: 'Europe',
          countries: ['United Kingdom', 'France', 'Germany', 'Italy', 'Spain'],
          availableMethods: [4],
          rateAdjustments: {
            4: 10
          }
        },
        {
          id: 4,
          name: 'Asia Pacific',
          countries: ['Japan', 'China', 'Australia', 'South Korea'],
          availableMethods: [4],
          rateAdjustments: {
            4: 15
          }
        }
      ],
      currentZone: this.getEmptyZone(),
      isEditingZone: false,
      editingZoneId: null,
      
      availableCountries: [
        'USA', 'Canada', 'Mexico', 'United Kingdom', 'France', 'Germany', 
        'Italy', 'Spain', 'Japan', 'China', 'Australia', 'South Korea',
        'Brazil', 'India', 'Russia', 'South Africa'
      ],
      
      settings: {
        defaultMethodId: 1,
        enableFreeShipping: true,
        freeShippingThreshold: 50,
        calculationMethod: 'flat',
        taxIncluded: true,
        showEstimatedDelivery: true
      },
      
      // Stats for dashboard
      averageShippingCost: 7.99,
      freeShippingPercentage: 35
    }
  },
  computed: {
    activeShippingMethods() {
      return this.shippingMethods.filter(method => method.isActive);
    }
  },
  methods: {
    getEmptyMethod() {
      return {
        name: '',
        description: '',
        baseCost: 0,
        estimatedDelivery: '',
        isActive: true
      };
    },
    getEmptyZone() {
      return {
        name: '',
        countries: [],
        availableMethods: [],
        rateAdjustments: {}
      };
    },
    openMethodForm(method = null) {
      if (method) {
        this.isEditingMethod = true;
        this.editingMethodId = method.id;
        this.currentMethod = { ...method };
      } else {
        this.isEditingMethod = false;
        this.editingMethodId = null;
        this.currentMethod = this.getEmptyMethod();
      }
      
      // Open modal using Bootstrap
      const modal = new bootstrap.Modal(this.$refs.methodModal);
      modal.show();
    },
    saveShippingMethod() {
      if (this.isEditingMethod) {
        // Update existing method
        const index = this.shippingMethods.findIndex(method => method.id === this.editingMethodId);
        if (index !== -1) {
          this.shippingMethods[index] = {
            ...this.currentMethod,
            id: this.editingMethodId
          };
        }
      } else {
        // Add new method
        const newId = this.shippingMethods.length > 0 
          ? Math.max(...this.shippingMethods.map(method => method.id)) + 1 
          : 1;
        
        const newMethod = {
          ...this.currentMethod,
          id: newId
        };
        
        this.shippingMethods.push(newMethod);
      }
      
      // Reset form and close modal
      this.currentMethod = this.getEmptyMethod();
      bootstrap.Modal.getInstance(this.$refs.methodModal).hide();
      
      // In a real app, you would save to the backend here
      this.saveToBackend();
    },
    deleteShippingMethod(id) {
      if (confirm('Are you sure you want to delete this shipping method?')) {
        // Check if method is used in any zones
        const usedInZones = this.shippingZones.some(zone => 
          zone.availableMethods.includes(id)
        );
        
        if (usedInZones) {
          alert('This shipping method is used in one or more shipping zones. Please remove it from zones first.');
          return;
        }
        
        // Remove the method
        const index = this.shippingMethods.findIndex(method => method.id === id);
        if (index !== -1) {
          this.shippingMethods.splice(index, 1);
          
          // In a real app, you would delete from the backend here
          this.deleteFromBackend('method', id);
        }
      }
    },
    openZoneForm(zone = null) {
      if (zone) {
        this.isEditingZone = true;
        this.editingZoneId = zone.id;
        this.currentZone = JSON.parse(JSON.stringify(zone)); // Deep copy
      } else {
        this.isEditingZone = false;
        this.editingZoneId = null;
        this.currentZone = this.getEmptyZone();
      }
      
      // Open modal using Bootstrap
      const modal = new bootstrap.Modal(this.$refs.zoneModal);
      modal.show();
    },
    saveShippingZone() {
      // Ensure rate adjustments exist for all selected methods
      this.currentZone.availableMethods.forEach(methodId => {
        if (!this.currentZone.rateAdjustments[methodId]) {
          this.currentZone.rateAdjustments[methodId] = 0;
        }
      });
      
      // Remove rate adjustments for unselected methods
      Object.keys(this.currentZone.rateAdjustments).forEach(methodId => {
        if (!this.currentZone.availableMethods.includes(parseInt(methodId))) {
          delete this.currentZone.rateAdjustments[methodId];
        }
      });
      
      if (this.isEditingZone) {
        // Update existing zone
        const index = this.shippingZones.findIndex(zone => zone.id === this.editingZoneId);
        if (index !== -1) {
          this.shippingZones[index] = {
            ...this.currentZone,
            id: this.editingZoneId
          };
        }
      } else {
        // Add new zone
        const newId = this.shippingZones.length > 0 
          ? Math.max(...this.shippingZones.map(zone => zone.id)) + 1 
          : 1;
        
        const newZone = {
          ...this.currentZone,
          id: newId
        };
        
        this.shippingZones.push(newZone);
      }
      
      // Reset form and close modal
      this.currentZone = this.getEmptyZone();
      bootstrap.Modal.getInstance(this.$refs.zoneModal).hide();
      
      // In a real app, you would save to the backend here
      this.saveToBackend();
    },
    deleteShippingZone(id) {
      if (confirm('Are you sure you want to delete this shipping zone?')) {
        const index = this.shippingZones.findIndex(zone => zone.id === id);
        if (index !== -1) {
          this.shippingZones.splice(index, 1);
          
          // In a real app, you would delete from the backend here
          this.deleteFromBackend('zone', id);
        }
      }
    },
    getMethodName(methodId) {
      const method = this.shippingMethods.find(m => m.id === methodId);
      return method ? method.name : 'Unknown Method';
    },
    saveSettings() {
      // In a real app, you would save settings to the backend here
      alert('Settings saved successfully!');
      this.saveToBackend();
    },
    saveToBackend() {
      // This would be an API call to save changes
      console.log('Saving data to backend:', {
        methods: this.shippingMethods,
        zones: this.shippingZones,
        settings: this.settings
      });
    },
    deleteFromBackend(type, id) {
      // This would be an API call to delete an item
      console.log(`Deleting ${type} from backend, ID:`, id);
    }
  },
  mounted() {
    // Import Bootstrap JavaScript for modals
    // In a real app, you would import this in your main.js or via CDN
    // For this example, we assume Bootstrap JS is already available
  }
}
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 20px;
}

.manage-shipping-container {
  background-color: #f8f9fa;
  min-height: 100vh;
}

.card {
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 1rem;
}

.nav-tabs .nav-link {
  color: #6c757d;
}

.nav-tabs .nav-link.active {
  color: #212529;
  font-weight: 500;
}

.table th {
  font-weight: 600;
}

.badge {
  font-weight: 500;
}

.display-4 {
  font-size: 2.5rem;
  font-weight: 500;
}
</style>