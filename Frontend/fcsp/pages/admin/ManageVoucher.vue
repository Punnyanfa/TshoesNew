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
                    <input 
                      type="number" 
                      class="form-control" 
                      id="discountAmount" 
                      v-model="voucher.discountAmount"
                      placeholder="Enter discount amount"
                      required
                    >
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
                      <button class="btn btn-sm btn-danger" @click="deleteVoucher(v.id)">
                        <i class="bi bi-trash"></i>
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
      isEditing: false
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
        if (this.isEditing) {
          console.log('Update functionality not implemented yet');
          return;
        }

        const response = await postVoucher(this.voucher);
        if (response.code === 200) {
          // Add new voucher to the list
          await this.loadVouchers(); // Reload the list to get fresh data
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
    async deleteVoucher(id) {
      if (confirm('Are you sure you want to delete this voucher?')) {
        try {
          const response = await deleteVoucher(id);
          if (response.code === 200) {
            await this.loadVouchers(); // Reload the list to get fresh data
            alert('Voucher deleted successfully!');
          } else {
            throw new Error(response.message || 'Failed to delete voucher');
          }
        } catch (error) {
          console.error('Error deleting voucher:', error);
          alert(error.message || 'An error occurred while deleting the voucher');
        }
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
</style>