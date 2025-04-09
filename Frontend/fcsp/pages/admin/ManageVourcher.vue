<template>
    <div class="container-fluid mt-4">
      <div class="card">
        <div class="card-header bg-primary text-white">
          <h4>dbo.Vouchers</h4>
        </div>
        <div class="card-body">
          <!-- Voucher Form -->
          <form @submit.prevent="saveVoucher">
            <div class="row mb-3">
              <div class="col-md-6">
                <div class="form-group mb-3">
                  <label for="voucherName">VoucherName</label>
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
                  <label for="voucherValue">VoucherValue</label>
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
                  <label for="startDate">StartDate</label>
                  <input 
                    type="datetime-local" 
                    class="form-control" 
                    id="startDate" 
                    v-model="voucher.startDate"
                    required
                  >
                </div>
                
                <div class="form-group mb-3">
                  <label for="expirationDate">ExpirationDate</label>
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
                  <th>Id</th>
                  <th>VoucherName</th>
                  <th>VoucherValue</th>
                  <th>Description</th>
                  <th>StartDate</th>
                  <th>ExpirationDate</th>
                  <th>Status</th>
                  <th>CreatedAt</th>
                  <th>UpdatedAt</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="v in vouchers" :key="v.id">
                  <td>{{ v.id }}</td>
                  <td>{{ v.voucherName }}</td>
                  <td>{{ v.voucherValue }}</td>
                  <td>{{ v.description }}</td>
                  <td>{{ formatDate(v.startDate) }}</td>
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
                  <td colspan="10" class="text-center">No vouchers found</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'VoucherPage',
    data() {
      return {
        voucher: {
          id: null,
          voucherName: '',
          voucherValue: '',
          description: '',
          startDate: '',
          expirationDate: '',
          status: 1,
          createdAt: null,
          updatedAt: null
        },
        vouchers: [],
        isEditing: false
      }
    },
    mounted() {
      // Load vouchers when component is mounted
      this.loadVouchers()
    },
    methods: {
      loadVouchers() {
        // In a real application, this would be an API call
        // For demo purposes, we'll use mock data
        this.vouchers = [
          {
            id: 1,
            voucherName: 'Summer Discount',
            voucherValue: '20%',
            description: 'Summer season discount for all products',
            startDate: '2023-06-01T00:00:00',
            expirationDate: '2023-08-31T23:59:59',
            status: 1,
            createdAt: '2023-05-15T10:30:00',
            updatedAt: '2023-05-15T10:30:00'
          },
          {
            id: 2,
            voucherName: 'New User Bonus',
            voucherValue: '$10',
            description: 'Welcome bonus for new users',
            startDate: '2023-01-01T00:00:00',
            expirationDate: '2023-12-31T23:59:59',
            status: 1,
            createdAt: '2023-01-01T08:00:00',
            updatedAt: '2023-01-01T08:00:00'
          },
          {
            id: 3,
            voucherName: 'Holiday Special',
            voucherValue: '15%',
            description: 'Special discount for holiday season',
            startDate: '2022-12-01T00:00:00',
            expirationDate: '2023-01-15T23:59:59',
            status: 2,
            createdAt: '2022-11-20T14:45:00',
            updatedAt: '2023-01-16T00:00:01'
          }
        ]
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
            updatedAt: now
          }
          this.vouchers.push(newVoucher)
        }
        
        // Reset form after saving
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
          startDate: '',
          expirationDate: '',
          status: 1,
          createdAt: null,
          updatedAt: null
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