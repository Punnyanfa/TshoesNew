<template>
    <div class="shipping-address-container">
      <div class="container py-4">
        <h2 class="mb-4">Shipping Addresses</h2>
        
        <!-- Address List -->
        <div v-if="addresses.length > 0" class="mb-4">
          <div class="card mb-3" v-for="address in addresses" :key="address.id">
            <div class="card-body">
              <div class="d-flex justify-content-between align-items-start">
                <div>
                  <h5 class="card-title">{{ address.streetAddress }}</h5>
                  <p class="card-text mb-1">
                    {{ address.ward }}, {{ address.district }}, {{ address.city }}, {{ address.country }}
                  </p>
                  <p class="card-text mb-1">
                    <strong>Phone:</strong> {{ address.phoneNumber }}
                  </p>
                  <p class="card-text">
                    <strong>Contact:</strong> {{ address.contactNumber }}
                  </p>
                  <span v-if="address.isDefault" class="badge bg-success">Default</span>
                </div>
                <div class="d-flex flex-column">
                  <button 
                    class="btn btn-sm btn-outline-primary mb-2" 
                    @click="editAddress(address)"
                  >
                    Edit
                  </button>
                  <button 
                    class="btn btn-sm btn-outline-danger mb-2" 
                    @click="deleteAddress(address.id)"
                  >
                    Delete
                  </button>
                  <button 
                    v-if="!address.isDefault" 
                    class="btn btn-sm btn-outline-success" 
                    @click="setAsDefault(address.id)"
                  >
                    Set as Default
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div v-else class="alert alert-info">
          You don't have any shipping addresses yet. Add your first address below.
        </div>
        
        <!-- Add/Edit Address Form -->
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">{{ isEditing ? 'Edit Address' : 'Add New Address' }}</h5>
          </div>
          <div class="card-body">
            <form @submit.prevent="saveAddress">
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="streetAddress" class="form-label">Street Address*</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="streetAddress" 
                    v-model="currentAddress.streetAddress"
                    required
                  >
                </div>
                <div class="col-md-6 mb-3">
                  <label for="ward" class="form-label">Ward*</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="ward" 
                    v-model="currentAddress.ward"
                    required
                  >
                </div>
              </div>
              
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="district" class="form-label">District*</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="district" 
                    v-model="currentAddress.district"
                    required
                  >
                </div>
                <div class="col-md-6 mb-3">
                  <label for="city" class="form-label">City*</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="city" 
                    v-model="currentAddress.city"
                    required
                  >
                </div>
              </div>
              
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="country" class="form-label">Country*</label>
                  <input 
                    type="text" 
                    class="form-control" 
                    id="country" 
                    v-model="currentAddress.country"
                    required
                  >
                </div>
                <div class="col-md-6 mb-3">
                  <label for="phoneNumber" class="form-label">Phone Number*</label>
                  <input 
                    type="tel" 
                    class="form-control" 
                    id="phoneNumber" 
                    v-model="currentAddress.phoneNumber"
                    required
                  >
                </div>
              </div>
              
              <div class="row">
                <div class="col-md-6 mb-3">
                  <label for="contactNumber" class="form-label">Contact Number</label>
                  <input 
                    type="tel" 
                    class="form-control" 
                    id="contactNumber" 
                    v-model="currentAddress.contactNumber"
                  >
                </div>
                <div class="col-md-6 mb-3 d-flex align-items-end">
                  <div class="form-check">
                    <input 
                      class="form-check-input" 
                      type="checkbox" 
                      id="isDefault" 
                      v-model="currentAddress.isDefault"
                    >
                    <label class="form-check-label" for="isDefault">
                      Set as default address
                    </label>
                  </div>
                </div>
              </div>
              
              <div class="d-flex justify-content-end gap-2">
                <button 
                  v-if="isEditing" 
                  type="button" 
                  class="btn btn-secondary" 
                  @click="cancelEdit"
                >
                  Cancel
                </button>
                <button type="submit" class="btn btn-primary">
                  {{ isEditing ? 'Update Address' : 'Add Address' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'ShippingAddress',
    data() {
      return {
        addresses: [],
        currentAddress: this.getEmptyAddress(),
        isEditing: false,
        editingId: null
      }
    },
    created() {
      // Fetch addresses when component is created
      this.fetchAddresses();
    },
    methods: {
      getEmptyAddress() {
        return {
          userId: 1, // This would typically come from your auth system
          streetAddress: '',
          ward: '',
          district: '',
          city: '',
          country: '',
          phoneNumber: '',
          contactNumber: '',
          isDefault: false,
          isDeleted: false,
          orders: []
        };
      },
      fetchAddresses() {
        // This would typically be an API call
        // For demo purposes, I'll add a sample address
        this.addresses = [
          {
            userId: 1,
            streetAddress: "123 Main St",
            ward: "Ward 1",
            district: "District 1",
            city: "New York",
            country: "USA",
            phoneNumber: "1234567890",
            contactNumber: "1234567890",
            isDefault: true,
            isDeleted: false,
            orders: [],
            id: 1,
            createdAt: "2025-03-31T11:19:36.1633333",
            updatedAt: "2025-03-31T11:19:36.1633333",
            version: ""
          }
        ];
      },
      saveAddress() {
        if (this.isEditing) {
          // Update existing address
          const index = this.addresses.findIndex(addr => addr.id === this.editingId);
          if (index !== -1) {
            // If setting this address as default, unset others
            if (this.currentAddress.isDefault) {
              this.addresses.forEach(addr => {
                if (addr.id !== this.editingId) {
                  addr.isDefault = false;
                }
              });
            }
            
            // Update the address with current date
            this.addresses[index] = {
              ...this.currentAddress,
              id: this.editingId,
              updatedAt: new Date().toISOString()
            };
            
            // Reset form
            this.isEditing = false;
            this.editingId = null;
            this.currentAddress = this.getEmptyAddress();
          }
        } else {
          // Add new address
          const newId = this.addresses.length > 0 
            ? Math.max(...this.addresses.map(addr => addr.id)) + 1 
            : 1;
          
          // If setting this address as default, unset others
          if (this.currentAddress.isDefault) {
            this.addresses.forEach(addr => {
              addr.isDefault = false;
            });
          }
          
          // Create new address
          const now = new Date().toISOString();
          const newAddress = {
            ...this.currentAddress,
            id: newId,
            createdAt: now,
            updatedAt: now,
            version: ""
          };
          
          this.addresses.push(newAddress);
          
          // Reset form
          this.currentAddress = this.getEmptyAddress();
        }
        
        // In a real app, you would save to the backend here
        this.saveToBackend();
      },
      editAddress(address) {
        this.isEditing = true;
        this.editingId = address.id;
        this.currentAddress = { ...address };
      },
      cancelEdit() {
        this.isEditing = false;
        this.editingId = null;
        this.currentAddress = this.getEmptyAddress();
      },
      deleteAddress(id) {
        if (confirm('Are you sure you want to delete this address?')) {
          // Soft delete in the UI
          const index = this.addresses.findIndex(addr => addr.id === id);
          if (index !== -1) {
            // If it's a real delete, you might want to set isDeleted to true
            // and handle it on the backend
            this.addresses.splice(index, 1);
            
            // In a real app, you would delete from the backend here
            this.deleteFromBackend(id);
          }
        }
      },
      setAsDefault(id) {
        // Update all addresses
        this.addresses.forEach(addr => {
          addr.isDefault = addr.id === id;
        });
        
        // In a real app, you would update the backend here
        this.saveToBackend();
      },
      saveToBackend() {
        // This would be an API call to save changes
        console.log('Saving addresses to backend:', this.addresses);
      },
      deleteFromBackend(id) {
        // This would be an API call to delete an address
        console.log('Deleting address from backend, ID:', id);
      }
    }
  }
  </script>
  
  <style scoped>
  .shipping-address-container {
    background-color: #f8f9fa;
    min-height: 100vh;
  }
  
  .card {
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
  
  .card-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #e9ecef;
  }
  
  .badge {
    font-size: 0.75rem;
  }
  </style>