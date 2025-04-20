<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="manage-shipping-container">
        <div class="container py-4">
          <h2 class="mb-4">Shipping Management</h2>
          
          <!-- Shipping Information Table -->
          <div class="card">
            <div class="card-body">
              <div class="table-responsive">
                <table class="table table-hover">
                  <thead>
                    <tr>
                      <th>receiverName</th>
                      <th>Phone Number</th>
                      <th>Address</th>
                      <th>City</th>
                      <th>District</th>
                      <th>Ward</th>
                      <th>Default</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="info in shippingInfos" :key="info.id">
                      <td>{{ info.receiverName }}</td>
                      <td>{{ info.phoneNumber }}</td>
                      <td>{{ info.address }}</td>
                      <td>{{ info.city }}</td>
                      <td>{{ info.district }}</td>
                      <td>{{ info.ward }}</td>
                      <td>
                        <span 
                          class="badge"
                          :class="info.isDefault ? 'bg-success' : 'bg-secondary'"
                        >
                          {{ info.isDefault ? 'Yes' : 'No' }}
                        </span>
                      </td>
                      <td>
                        <div class="btn-group">
                          <!-- <button 
                            class="btn btn-sm btn-outline-primary" 
                            @click="openShippingForm(info)"
                          >
                            Edit
                          </button> -->
                          <button 
                            class="btn btn-sm btn-outline-danger" 
                            @click="deleteShipping(info.id)"
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

          <!-- Add/Edit Shipping Modal -->
          <div class="modal fade" id="shippingModal" tabindex="-1" aria-hidden="true" ref="shippingModal">
            <div class="modal-dialog">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title">{{ isEditing ? 'Edit Shipping Information' : 'Add Shipping Information' }}</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                  <form @submit.prevent="saveShipping">
                    <div class="mb-3">
                      <label for="userId" class="form-label">User ID*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="userId" 
                        v-model="currentShipping.userId"
                        required
                      >
                    </div>

                    <div class="mb-3">
                      <label for="phoneNumber" class="form-label">Phone Number*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="phoneNumber" 
                        v-model="currentShipping.phoneNumber"
                        required
                      >
                    </div>

                    <div class="mb-3">
                      <label for="address" class="form-label">Address*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="address" 
                        v-model="currentShipping.address"
                        required
                      >
                    </div>

                    <div class="mb-3">
                      <label for="city" class="form-label">City*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="city" 
                        v-model="currentShipping.city"
                        required
                      >
                    </div>

                    <div class="mb-3">
                      <label for="district" class="form-label">District*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="district" 
                        v-model="currentShipping.district"
                        required
                      >
                    </div>

                    <div class="mb-3">
                      <label for="ward" class="form-label">Ward*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="ward" 
                        v-model="currentShipping.ward"
                        required
                      >
                    </div>

                    <div class="form-check mb-3">
                      <input 
                        class="form-check-input" 
                        type="checkbox" 
                        id="isDefault" 
                        v-model="currentShipping.isDefault"
                      >
                      <label class="form-check-label" for="isDefault">
                        Set as Default Address
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
      </div>
    </div>
  </div>
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue';
import { getAllShippingInfo, postShippingInfo, deleteShippingInfo } from '@/server/shipping-service';

export default {
  name: 'ManageShipping',
  components: {
    AdminSidebar
  },
  data() {
    return {
      shippingInfos: [],
      currentShipping: this.getEmptyShipping(),
      isEditing: false,
    }
  },
  methods: {
    getEmptyShipping() {
      return {
        userId: '',
        phoneNumber: '',
        address: '',
        city: '',
        district: '',
        ward: '',
        isDefault: false
      };
    },

    async loadShippingInfo() {
      try {
        const data = await getAllShippingInfo();
        this.shippingInfos = data.shippingInfos || [];
      } catch (error) {
        console.error('Error loading shipping info:', error);
        // Handle error appropriately (show notification, etc)
      }
    },

    openShippingForm(info = null) {
      if (info) {
        this.isEditing = true;
        this.currentShipping = { ...info };
      } else {
        this.isEditing = false;
        this.currentShipping = this.getEmptyShipping();
      }
      
      const modal = new bootstrap.Modal(this.$refs.shippingModal);
      modal.show();
    },

    async saveShipping() {
      try {
        await postShippingInfo(this.currentShipping);
        await this.loadShippingInfo(); // Reload the list
        bootstrap.Modal.getInstance(this.$refs.shippingModal).hide();
        this.currentShipping = this.getEmptyShipping();
      } catch (error) {
        console.error('Error saving shipping info:', error);
        // Handle error appropriately (show notification, etc)
      }
    },

    async deleteShipping(id) {
      if (confirm('Are you sure you want to delete this shipping information?')) {
        try {
          await deleteShippingInfo(id);
          await this.loadShippingInfo(); // Reload the list
        } catch (error) {
          console.error('Error deleting shipping info:', error);
          // Handle error appropriately (show notification, etc)
        }
      }
    }
  },
  mounted() {
    this.loadShippingInfo();
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

.table th {
  font-weight: 600;
}

.badge {
  font-weight: 500;
}
</style>