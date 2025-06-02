<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="container-fluid mt-4">
        <div class="card">
          <div class="card-header bg-primary text-white">
            <h4>Voucher Management</h4>
          </div>
          <div class="card-body">
            <!-- Voucher Form -->
            <form @submit.prevent="saveVoucher">
              <div class="row mb-3">
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <label for="code">Voucher Code</label>
                    <input 
                      type="text" 
                      class="form-control" 
                      id="code" 
                      v-model="voucher.code"
                      placeholder="Enter voucher code"
                      required
                    >
                  </div>
                  
                  <div class="form-group mb-3">
                    <label for="discountAmount">Discount Amount</label>
                    <div class="input-group">
                      <input 
                        type="number" 
                        class="form-control" 
                        id="discountAmount" 
                        v-model="voucher.discountAmount"
                        placeholder="Enter discount amount"
                        min="1"
                        max="1000000"
                        required
                      >
                      <span class="input-group-text">VND</span>
                    </div>
                  </div>
                </div>
                
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <label for="expiryDate">Expiry Date</label>
                    <input 
                      type="datetime-local" 
                      class="form-control" 
                      id="expiryDate" 
                      v-model="voucher.expiryDate"
                      required
                    >
                  </div>
                </div>
              </div>
              
              <div class="d-flex justify-content-end">
                <button type="button" class="btn btn-secondary me-2" @click="resetForm">Reset</button>
                <button type="submit" class="btn btn-primary">Save Voucher</button>
              </div>
            </form>
            
            <hr class="my-4">
            
            <!-- Vouchers Table -->
            <div class="table-responsive">
              <table class="table table-striped table-hover">
                <thead>
                  <tr>
                    
                    <th>Code</th>
                    <th>Discount Amount</th>
                    <th>Expiry Date</th>
                    <th>Status</th>
                    <th>Is Used</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="v in vouchers" :key="v.id">
                   
                    <td>{{ v.code }}</td>
                    <td>{{ v.discountAmount }}</td>
                    <td>{{ formatDate(v.expiryDate) }}</td>
                    <td>
                      <span 
                        :class="{
                          'badge bg-success': v.voucherStatusName === 'Active',
                          'badge bg-danger': v.voucherStatusName === 'Inactive',
                          'badge bg-warning': v.voucherStatusName === 'Expired'
                        }"
                      >
                        {{ v.voucherStatusName }}
                      </span>
                    </td>
                    <td>
                      <span :class="{'text-success': !v.isUsed, 'text-danger': v.isUsed}">
                        {{ v.isUsed ? 'Used' : 'Not Used' }}
                      </span>
                    </td>
                    <td>
                      <button class="btn btn-sm btn-info me-1" @click="editVoucher(v)">
                        <i class="bi bi-pencil"></i>
                      </button>
                      <button 
                        class="delete-btn" 
                        @click="confirmDelete(v)" 
                        title="Delete"
                      > 
                        <i class="fas fa-trash-alt"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="vouchers.length === 0">
                    <td colspan="7" class="text-center">No vouchers found</td>
                  </tr>
                </tbody>
              </table>
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
                  <h3>Xác nhận xóa voucher</h3>
                  <button class="modal-close" @click="showDeleteModal = false">×</button>
                </div>
                <div class="modal-body delete-modal-body">
                  <p v-if="selectedVoucher" class="delete-modal-title">Bạn có chắc chắn muốn xóa voucher <b>"{{ selectedVoucher.code }}"</b>?</p>
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
  </div>
</template>

<script>
import { getAllVouchers, postVoucher, deleteVoucher } from '~/server/ManageVoucher-service';
import AdminSidebar from '@/components/AdminSidebar.vue';

export default {
  name: 'VoucherPage',
  components: {
    AdminSidebar
  },
  data() {
    return {
      voucher: {
        code: '',
        discountAmount: 0,
        expiryDate: ''
      },
      vouchers: [],
      isEditing: false,
      showDeleteModal: false,
      selectedVoucher: null
    }
  },
  mounted() {
    this.loadVouchers()
  },
  methods: {
    async loadVouchers() {
      try {
        const response = await getAllVouchers();
        if (response.code === 200) {
          this.vouchers = response.data;
          console.log('Loaded vouchers:', this.vouchers);
        } else {
          throw new Error(response.message || 'Failed to load vouchers');
        }
      } catch (error) {
        console.error('Error loading vouchers:', error);
        alert('Error loading vouchers: ' + (error.message || 'Unknown error'));
      }
    },
    async saveVoucher() {
      try {
        // Validate discount amount
        if (this.voucher.discountAmount < 1 || this.voucher.discountAmount > 1000000) {
          alert('Discount amount must be between 1 and 1000000');
          return;
        }

        if (this.isEditing) {
          console.log('Update functionality not implemented yet');
          return;
        }

        const response = await postVoucher(this.voucher);
        console.log('Save voucher response:', response); // Debug log

        if ( response.code === 201) {
          await this.loadVouchers();
          this.resetForm();
          alert('Voucher created successfully!');
        } else {
          throw new Error(response.message || 'Failed to create voucher');
        }
      } catch (error) {
        console.error('Error saving voucher:', error);
        alert(error.message || 'An error occurred while saving the voucher');
      }
    },
    editVoucher(voucher) {
      this.voucher = {
        code: voucher.code,
        discountAmount: voucher.discountAmount,
        expiryDate: voucher.expiryDate
      }
      this.isEditing = true
    },
    confirmDelete(voucher) {
      this.selectedVoucher = voucher;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      try {
        if (!this.selectedVoucher) {
          alert('Không có voucher nào được chọn để xóa');
          return;
        }
        
        const response = await deleteVoucher(this.selectedVoucher.id);
        if (response.code === 200) {
          await this.loadVouchers();
          this.showDeleteModal = false;
          this.selectedVoucher = null;
        } else {
          throw new Error(response.message || 'Xóa thất bại!');
        }
      } catch (error) {
        console.error('Error deleting voucher:', error);
        alert('Có lỗi xảy ra khi xóa voucher');
      }
    },
    resetForm() {
      this.voucher = {
        code: '',
        discountAmount: 0,
        expiryDate: new Date().toISOString().slice(0, 16) // Set default to current date-time
      };
      this.isEditing = false;
    },
    formatDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleString()
    }
  }
}
</script>

<style>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 20px;
  background-color: white;
}

.card-header {
  background-color: #2c6da3 !important;
}

.table th {
  background-color: #f8f9fa;
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