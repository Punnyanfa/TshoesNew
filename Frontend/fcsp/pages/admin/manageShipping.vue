<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="manage-shipping-container">
        <div class="container py-4">
          <h1 class="mb-4 text-primary fw-bold">Shipping Management</h1>
          
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
                          <button 
                            class="delete-btn" 
                            @click="confirmDelete(info)" 
                            title="Delete"
                          > 
                            <i class="fas fa-trash-alt"></i>
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
                <h3>Xác nhận xóa địa chỉ giao hàng</h3>
                <button class="modal-close" @click="showDeleteModal = false">×</button>
              </div>
              <div class="modal-body delete-modal-body">
                <p v-if="selectedShipping" class="delete-modal-title">Bạn có chắc chắn muốn xóa địa chỉ giao hàng này?</p>
                <p class="text-danger delete-modal-warning">Hành động này không thể hoàn tác.</p>
              </div>
              <div class="modal-footer delete-modal-footer">
                <button class="btn-cancel" @click="showDeleteModal = false">Hủy</button>
                <button class="btn-delete" @click="handleDelete">Xóa</button>
              </div>
            </div>
          </div>
        </div>
      </transition>
    </teleport>
  </client-only>
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
      showDeleteModal: false,
      selectedShipping: null,
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

    confirmDelete(shipping) {
      this.selectedShipping = shipping;
      this.showDeleteModal = true;
    },

    async handleDelete() {
      try {
        if (!this.selectedShipping) {
          alert('No shipping addresses selected for deletion');
          return;
        }
        
        await deleteShippingInfo(this.selectedShipping.id);
        await this.loadShippingInfo();
        this.showDeleteModal = false;
        this.selectedShipping = null;
      } catch (error) {
        console.error('Error deleting shipping info:', error);
        alert('An error occurred while deleting the shipping address.');
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