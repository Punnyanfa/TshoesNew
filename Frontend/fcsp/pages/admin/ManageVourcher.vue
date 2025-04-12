<template>
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
                  <label for="voucherName">Voucher Name</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="voucherName" 
                    v-model="voucher.voucherName"
                    placeholder="Enter voucher name"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="voucherValue">Voucher Value</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="voucherValue" 
                    v-model="voucher.voucherValue"
                    placeholder="Enter voucher value"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="description">Description</label>
                  <textarea 
                    class="form-control" 
                    id="description" 
                    v-model="voucher.description"
                    placeholder="Enter description"
                    rows="3"
                  ></textarea>
                </div>
              </div>
              
              <div class="col-md-6">
                <div class="form-group mb-3">
                  <label for="expirationDate">Expiration Date</label>
                  <input 
                    type="datetime-local" 
                    class="form-control" 
                    id="expirationDate" 
                    v-model="voucher.expirationDate"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="status">Status</label>
                  <select 
                    class="form-select" 
                    id="status" 
                    v-model="voucher.status"
                    required
                  >
                    <option value="1">Active</option>
                    <option value="0">Inactive</option>
                    <option value="2">Expired</option>
                  </select>
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
                  <th>ID</th>
                  <th>Name</th>
                  <th>Value</th>
                  <th>Description</th>
                  <th>Expiration Date</th>
                  <th>Status</th>
                  <th>Created At</th>
                  <th>Updated At</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="v in vouchers" :key="v.id">
                  <td>{{ v.id }}</td>
                  <td>{{ v.voucherName }}</td>
                  <td>{{ v.voucherValue }}</td>
                  <td>{{ v.description }}</td>
                  <td>{{ formatDate(v.expirationDate) }}</td>
                  <td>
                    <span 
                      :class="{
                        'badge bg-success': v.status === 1,
                        'badge bg-danger': v.status === 0,
                        'badge bg-warning': v.status === 2
                      }"
                    >
                      {{ getStatusText(v.status) }}
                    </span>
                  </td>
                  <td>{{ formatDate(v.createdAt) }}</td>
                  <td>{{ formatDate(v.updatedAt) }}</td>
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
                  <td colspan="9" class="text-center">No vouchers found</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import { getAllVouchers } from '@/server/ManageVoucher-service';
  
  export default {
    name: 'VoucherPage',
    data() {
      return {
        voucher: {
          id: null,
          voucherName: '',
          voucherValue: '',
          description: '',
          expirationDate: '',
          status: 0,
          orders: [],
          createdAt: null,
          updatedAt: null,
          version: ''
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
          this.vouchers = response;
        } catch (error) {
          console.error('Error loading vouchers:', error);
          // You might want to show an error message to the user here
        }
      },
      saveVoucher() {
        if (this.isEditing) {
          // Update existing voucher
          const index = this.vouchers.findIndex(v => v.id === this.voucher.id)
          if (index !== -1) {
            this.voucher.updatedAt = new Date().toISOString()
            this.vouchers[index] = { ...this.voucher }
          }
        } else {
          // Create new voucher
          const now = new Date().toISOString()
          const newVoucher = {
            ...this.voucher,
            id: this.vouchers.length > 0 ? Math.max(...this.vouchers.map(v => v.id)) + 1 : 1,
            createdAt: now,
            updatedAt: now,
            orders: [],
            version: ''
          }
          this.vouchers.push(newVoucher)
        }
        
        this.resetForm()
      },
      editVoucher(voucher) {
        this.voucher = { ...voucher }
        this.isEditing = true
      },
      deleteVoucher(id) {
        if (confirm('Are you sure you want to delete this voucher?')) {
          this.vouchers = this.vouchers.filter(v => v.id !== id)
        }
      },
      resetForm() {
        this.voucher = {
          id: null,
          voucherName: '',
          voucherValue: '',
          description: '',
          expirationDate: '',
          status: 0,
          orders: [],
          createdAt: null,
          updatedAt: null,
          version: ''
        }
        this.isEditing = false
      },
      formatDate(dateString) {
        if (!dateString) return ''
        const date = new Date(dateString)
        return date.toLocaleString()
      },
      getStatusText(status) {
        switch (status) {
          case 0: return 'Inactive'
          case 1: return 'Active'
          case 2: return 'Expired'
          default: return 'Unknown'
        }
      }
    }
  }
  </script>
  
  <style>
  .card-header {
    background-color: #2c6da3 !important;
  }
  
  .table th {
    background-color: #f8f9fa;
  }
  </style>