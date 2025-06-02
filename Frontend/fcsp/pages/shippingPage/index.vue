<template>
  <Header/>
    <div class="shipping-address-container">
      <div class="container py-4">
        <h2 class="mb-4">Shipping Address</h2>
        
        <!-- Loading Indicator -->
        <div v-if="loading" class="text-center my-4">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
        
        <!-- Error Alert -->
        <div v-if="error" class="alert alert-danger mb-4">
          {{ error }}
        </div>
        
        <!-- Address List -->
        <div v-if="!loading && addresses.length > 0" class="mb-4">
          <div class="address-card" v-for="address in sortedAddresses" :key="address.id">
            <div class="address-content">
              <div class="address-header">
                <h3 class="receiver-name">{{ address.receiverName || 'No Name' }}</h3>
                <div class="phone-number">{{ address.phoneNumber }}</div>
              </div>
              <div class="address-details">
                <p class="address-line">{{ address.address }}</p>
                <p class="address-line">{{ address.ward }}, {{ address.district }}, {{ address.city }}</p>
              </div>
              <div class="address-actions">
                <div class="default-badge" v-if="address.isDefault">
                  Default
                </div>
                <div class="action-buttons">
                  <button 
                    class="btn-update"
                    @click="openEditModal(address)"
                    :disabled="loading || isDeleting"
                  >
                    Update
                  </button>
                  <button 
                    v-if="!address.isDefault"
                    class="btn-delete"
                    @click="deleteAddress(address.id)"
                    :disabled="loading || isDeleting"
                  >
                    <span v-if="isDeleting" class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
                    Delete
                  </button>
                  <button 
                    v-if="!address.isDefault"
                    class="btn-set-default"
                    @click="setAsDefault(address.id)"
                    :disabled="loading || isDeleting"
                  >
                    Set as Default
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <div v-if="!loading && addresses.length === 0" class="alert alert-info">
          You don't have any addresses yet. Add your first address below.
        </div>

        <!-- Edit Address Modal -->
        <div class="modal fade" id="editAddressModal" tabindex="-1" ref="editModal">
          <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">{{ isEditing ? 'Update Address' : 'Add New Address' }}</h5>
                <button type="button" class="btn-close" @click="closeModal"></button>
              </div>
              <div class="modal-body">
                <form @submit.prevent="saveAddress">
                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <label for="receiverName" class="form-label">Receiver Name*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="receiverName" 
                        v-model="currentAddress.receiverName"
                        required
                        :disabled="loading"
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
                        :disabled="loading"
                      >
                    </div>
                  </div>
                  
                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <label for="country" class="form-label">Country</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="country" 
                        v-model="currentAddress.country"
                        readonly
                        :disabled="loading"
                      >
                    </div>
                  </div>
                  
                  <div class="row">
                    <div class="col-12 mb-3">
                      <label for="address" class="form-label">Address*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="address" 
                        v-model="currentAddress.address"
                        required
                        :disabled="loading"
                      >
                    </div>
                  </div>
                  
                  <div class="row">
                    <div class="col-md-4 mb-3">
                      <label for="ward" class="form-label">Ward*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="ward" 
                        v-model="currentAddress.ward"
                        required
                        :disabled="loading"
                      >
                    </div>
                    <div class="col-md-4 mb-3">
                      <label for="district" class="form-label">District*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="district" 
                        v-model="currentAddress.district"
                        required
                        :disabled="loading"
                      >
                    </div>
                    <div class="col-md-4 mb-3">
                      <label for="city" class="form-label">City*</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="city" 
                        v-model="currentAddress.city"
                        required
                        :disabled="loading"
                      >
                    </div>
                  </div>
                  
                  <div class="row">
                    <div class="col-12 mb-3">
                      <div class="form-check">
                        <input 
                          class="form-check-input" 
                          type="checkbox" 
                          id="isDefault" 
                          v-model="currentAddress.isDefault"
                          :disabled="loading"
                        >
                        <label class="form-check-label" for="isDefault">
                          Set as default address
                        </label>
                      </div>
                    </div>
                  </div>
                  
                  <div class="modal-footer">
                    <button 
                      type="button" 
                      class="btn btn-secondary" 
                      @click="closeModal"
                      :disabled="loading"
                    >
                      Cancel
                    </button>
                    <button 
                      type="submit" 
                      class="btn btn-primary"
                      :disabled="loading"
                    >
                      {{ isEditing ? 'Update' : 'Add New' }}
                      <span v-if="loading" class="spinner-border spinner-border-sm ms-1" role="status" aria-hidden="true"></span>
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>

        <!-- Add New Address Button -->
        <button 
          class="btn btn-primary mb-4" 
          @click="openAddModal"
          :disabled="loading"
        >
          Add New Address
        </button>
      </div>
    </div>
    <Footer/>
  </template>
  
  <script>
  import { postShippingInfo, shippingInfo, deleteShippingInfo, putShippingInfo } from '@/server/shipping-service'
import Header from '~/components/Header.vue';

  export default {
    name: 'ShippingAddress',
    data() {
      return {
        addresses: [],
        currentAddress: this.getEmptyAddress(),
        isEditing: false,
        editingId: null,
        error: null,
        loading: false,
        isDeleting: false,
        modal: null
      }
    },
    computed: {
      sortedAddresses() {
        // Sắp xếp địa chỉ: địa chỉ mặc định lên đầu
        return [...this.addresses].sort((a, b) => {
          if (a.isDefault === b.isDefault) return 0;
          return a.isDefault ? -1 : 1;
        });
      }
    },
    mounted() {
      // Khởi tạo modal khi component được mount
      this.$nextTick(() => {
        this.initModal();
      });
    },
    created() {
      this.fetchAddresses();
    },
    methods: {
      initModal() {
        if (typeof window !== 'undefined' && window.bootstrap) {
          this.modal = new window.bootstrap.Modal(this.$refs.editModal);
        }
      },
      getUserId() {
        const userId = localStorage.getItem('userId');
        if (!userId) {
          this.error = 'User not logged in';
          return null;
        }
        return parseInt(userId); // Chuyển userId thành số
      },
      getEmptyAddress() {
        return {
          userId: this.getUserId(),
          phoneNumber: '',
          address: '',
          city: '',
          district: '',
          ward: '',
          country: 'Vietnam',
          isDefault: false,
          receiverName: ''
        };
      },
      async fetchAddresses() {
        try {
          this.loading = true;
          const userId = this.getUserId();
          if (!userId) return;

          const response = await shippingInfo(userId);
          console.log("Response from API:", response);
          
          if (Array.isArray(response)) {
            this.addresses = response;
          } else if (response && Array.isArray(response.shippingInfos)) {
            this.addresses = response.shippingInfos;
          } else {
            this.addresses = [];
          }
          this.error = null;
        } catch (error) {
          console.error('Error fetching addresses:', error);
          this.error = 'Failed to load shipping addresses. Please try again.';
          this.addresses = [];
        } finally {
          this.loading = false;
        }
      },
      openEditModal(address) {
        this.isEditing = true;
        this.editingId = address.id;
        this.currentAddress = { ...address };
        this.$nextTick(() => {
          if (typeof window !== 'undefined' && window.bootstrap) {
            const modal = new window.bootstrap.Modal(this.$refs.editModal);
            modal.show();
          }
        });
      },
      openAddModal() {
        this.isEditing = false;
        this.editingId = null;
        this.currentAddress = this.getEmptyAddress();
        this.$nextTick(() => {
          if (typeof window !== 'undefined' && window.bootstrap) {
            const modal = new window.bootstrap.Modal(this.$refs.editModal);
            modal.show();
          }
        });
      },
      closeModal() {
        if (typeof window !== 'undefined' && window.bootstrap) {
          const modal = window.bootstrap.Modal.getInstance(this.$refs.editModal);
          if (modal) {
            modal.hide();
          }
        }
        this.error = null;
      },
      async saveAddress() {
        try {
          const userId = this.getUserId();
          if (!userId) return;

          if (!this.currentAddress.phoneNumber || 
              !this.currentAddress.address || !this.currentAddress.city || 
              !this.currentAddress.district || !this.currentAddress.ward) {
            this.error = 'Please fill in all required fields';
            return;
          }

          this.loading = true;
          const addressData = {
            ...this.currentAddress,
            userId: userId
          };

          if (this.isEditing && this.editingId) {
            addressData.id = this.editingId;
          }

          // If this is the first address, automatically set as default
          if (this.addresses.length === 0) {
            addressData.isDefault = true;
          }

          let response;
          if (this.isEditing) {
            // Call putShippingInfo for update
            response = await putShippingInfo(addressData);
            console.log("Response from API:", response);
          } else {
            // Call postShippingInfo for add new
            response = await postShippingInfo(addressData);
          }

          console.log("Response from API:", response);
          
          // Check if the API call was successful based on the structure of your service function
          // Both postShippingInfo and putShippingInfo in your service seem to return data on success
          if (response) {
            await this.fetchAddresses();
            this.closeModal();
            this.currentAddress = this.getEmptyAddress();
            this.isEditing = false;
            this.editingId = null;
            this.error = null;
            alert(this.isEditing ? 'Address updated successfully' : 'New address added successfully');
          }
        } catch (error) {
          console.error('Error saving address:', error);
          this.error = error.message || 'Could not save address. Please try again.';
        } finally {
          this.loading = false;
        }
      },
      async deleteAddress(id) {
        try {
          const address = this.addresses.find(addr => addr.id === id);
          if (!address) {
            this.error = 'Address does not exist';
            return;
          }

          if (address.isDefault) {
            this.error = 'Cannot delete default address. Please set another address as default first.';
            return;
          }

          if (!confirm('Are you sure you want to delete this address?')) {
            return;
          }

          this.isDeleting = true;
          this.error = null;

          await deleteShippingInfo(id);
          this.addresses = this.addresses.filter(addr => addr.id !== id);
          alert('Address deleted successfully');

        } catch (error) {
          console.error('Error deleting address:', error);
          this.error = error.message || 'Could not delete address. Please try again.';
        } finally {
          this.isDeleting = false;
        }
      },
      async setAsDefault(id) {
        try {
          this.loading = true;
          const address = this.addresses.find(addr => addr.id === id);
          if (address) {
            const updatedAddress = { 
              ...address, 
              isDefault: true,
              userId: this.getUserId()
            };
            await postShippingInfo(updatedAddress);
            await this.fetchAddresses();
            alert('Set as default address successfully');
          }
        } catch (error) {
          console.error('Error setting default address:', error);
          this.error = 'Could not set default address. Please try again.';
        } finally {
          this.loading = false;
        }
      }
    }
  }
  </script>
  
  <style scoped>
  .shipping-address-container {
    background-color: #f8f9fa;
    min-height: 100vh;
    padding: 20px 0;
  }
  
  .address-card {
    background: #fff;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    margin-bottom: 16px;
    padding: 20px;
  }
  
  .address-content {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }
  
  .address-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #eee;
    padding-bottom: 12px;
  }
  
  .receiver-name {
    font-size: 18px;
    font-weight: 600;
    margin: 0;
  }
  
  .phone-number {
    color: #666;
  }
  
  .address-details {
    padding: 8px 0;
  }
  
  .address-line {
    margin: 0;
    color: #333;
    line-height: 1.5;
  }
  
  .address-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 8px;
  }
  
  .default-badge {
    background-color: #ff4d4f;
    color: white;
    padding: 4px 12px;
    border-radius: 4px;
    font-size: 14px;
  }
  
  .action-buttons {
    display: flex;
    gap: 12px;
    align-items: center;
  }
  
  .btn-update,
  .btn-delete,
  .btn-set-default {
    padding: 6px 16px;
    border-radius: 4px;
    border: 1px solid;
    cursor: pointer;
    font-size: 14px;
    transition: all 0.3s ease;
  }
  
  .btn-update {
    border-color: #1890ff;
    color: #1890ff;
    background: transparent;
  }
  
  .btn-update:hover {
    background: #1890ff;
    color: white;
  }
  
  .btn-delete {
    border-color: #ff4d4f;
    color: #ff4d4f;
    background: transparent;
  }
  
  .btn-delete:hover {
    background: #ff4d4f;
    color: white;
  }
  
  .btn-set-default {
    border-color: #52c41a;
    color: #52c41a;
    background: transparent;
  }
  
  .btn-set-default:hover {
    background: #52c41a;
    color: white;
  }
  
  .address-form-card {
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  }
  
  .form-header {
    padding: 16px 24px;
    border-bottom: 1px solid #f0f0f0;
    background-color: #fafafa;
  }
  
  .form-body {
    padding: 24px;
  }
  
  button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
  }
  
  /* Responsive adjustments */
  @media (max-width: 768px) {
    .address-header {
      flex-direction: column;
      align-items: flex-start;
      gap: 8px;
    }
    
    .address-actions {
      flex-direction: column;
      align-items: flex-start;
      gap: 12px;
    }
    
    .action-buttons {
      width: 100%;
      justify-content: flex-start;
    }
  }

  .modal-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #dee2e6;
  }

  .modal-footer {
    background-color: #f8f9fa;
    border-top: 1px solid #dee2e6;
  }

  .btn-primary {
    background-color: #1890ff;
    border-color: #1890ff;
  }

  .btn-primary:hover {
    background-color: #096dd9;
    border-color: #096dd9;
  }

  .btn-secondary {
    background-color: #f0f0f0;
    border-color: #d9d9d9;
    color: #595959;
  }

  .btn-secondary:hover {
    background-color: #d9d9d9;
    border-color: #bfbfbf;
    color: #262626;
  }
  </style>