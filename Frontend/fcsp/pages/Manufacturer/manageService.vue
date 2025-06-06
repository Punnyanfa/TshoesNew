<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="manufacturer-layout">
      <div class="main-content">
        <div class="container-fluid mt-4">
          <!-- Default Custom Fees Section -->
          <div class="card mb-4">
            <div class="card-header bg-warning text-dark">
              <h4 class="mb-0">Default Custom Fees Table</h4>
            </div>
            <div class="card-body">
              <div class="table-responsive">
                <table class="table table-bordered text-center align-middle">
                  <thead class="table-primary">
                    <tr>
                      <th>Component</th>
                      <th>Color Fee (₫)</th>
                      <th>Image Fee (₫)</th>
                      <th>Delete</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(fee, idx) in defaultCustomFees" :key="fee.part">
                      <td>{{ fee.part }}</td>
                      <td>
                        <input type="number" min="0" class="form-control text-end" v-model.number="fee.colorFee" placeholder="0" />
                      </td>
                      <td>
                        <input type="number" min="0" class="form-control text-end" v-model.number="fee.imageFee" placeholder="0" />
                      </td>
                      <td>
                        <button class="btn btn-danger btn-sm" @click="deleteFee(fee)">
                          <span>×</span>
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <div class="d-flex justify-content-end">
                <button class="btn btn-warning" @click="saveDefaultCustomFees">Save Default Fees</button>
              </div>
              <div class="alert alert-info mt-3" style="font-size: 0.95em">
                <b>Note:</b> These fees will be used as default when adding new services.
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import HeaderManu from '@/components/HeaderManu.vue';
import axios from 'axios';

export default {
  name: 'ManageService',
  components: { HeaderManu },
  setup() {
    return {};
  },
  data() {
    return {
      defaultCustomFees: [
        { part: 'Base', colorFee: 0, imageFee: 0, colorServiceId: null, imageServiceId: null },
        { part: 'Lace', colorFee: 0, imageFee: 0, colorServiceId: null, imageServiceId: null },
        { part: 'Sole', colorFee: 0, imageFee: 0, colorServiceId: null, imageServiceId: null },
        { part: 'Accent', colorFee: 0, imageFee: 0, colorServiceId: null, imageServiceId: null },
        { part: 'Details', colorFee: 0, imageFee: 0, colorServiceId: null, imageServiceId: null }
      ],
      showServiceModal: false,
      isEditing: false,
      currentService: {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1,
        customFees: [
          { part: 'Base', colorFee: 0, imageFee: 0 },
          { part: 'Lace', colorFee: 0, imageFee: 0 },
          { part: 'Sole', colorFee: 0, imageFee: 0 },
          { part: 'Accent', colorFee: 0, imageFee: 0 },
          { part: 'Details', colorFee: 0, imageFee: 0 }
        ]
      }
    };
  },
  methods: {
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
    async deleteFee(fee) {
      if (confirm(`Are you sure you want to delete fees for ${fee.part}?`)) {
        try {
          // Delete color fee if it exists
          if (fee.colorServiceId) {
            const colorResponse = await axios.delete(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/${fee.colorServiceId}`, {
              headers: { 'accept': '*/*' }
            });
            if (!(colorResponse.status === 200 && colorResponse.data.data.success)) {
              throw new Error('Failed to delete color fee');
            }
          }
          // Delete image fee if it exists
          if (fee.imageServiceId) {
            const imageResponse = await axios.delete(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/${fee.imageServiceId}`, {
              headers: { 'accept': '*/*' }
            });
            if (!(imageResponse.status === 200 && imageResponse.data.data.success)) {
              throw new Error('Failed to delete image fee');
            }
          }
          // Update local state
          this.defaultCustomFees = this.defaultCustomFees.filter(f => f.part !== fee.part);
          // Reload the page
          if (typeof window !== 'undefined') {
            window.location.reload();
          }
        } catch (error) {
          console.error('Error deleting fee:', error);
          alert('Error deleting fees!');
        }
      }
    },
    showAddServiceModal() {
      this.isEditing = false;
      this.currentService = {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1,
        customFees: this.defaultCustomFees.map(fee => ({ ...fee }))
      };
      this.showServiceModal = true;
    },
    editService(service) {
      this.isEditing = true;
      this.currentService = { ...service, customFees: service.customFees.map(fee => ({ ...fee })) };
      this.showServiceModal = true;
    },
    hideServiceModal() {
      this.showServiceModal = false;
      this.currentService = {
        id: null,
        name: '',
        description: '',
        price: 0,
        status: 1,
        customFees: this.defaultCustomFees.map(fee => ({ ...fee }))
      };
    },
    async saveService() {
      if (this.isEditing) {
        const index = this.services.findIndex(s => s.id === this.currentService.id);
        if (index !== -1) {
          this.services[index] = { ...this.currentService, customFees: this.currentService.customFees.map(fee => ({ ...fee })) };
        }
      } else {
        const newService = {
          ...this.currentService,
          id: this.services.length > 0 ? Math.max(...this.services.map(s => s.id)) + 1 : 1,
          customFees: this.currentService.customFees.map(fee => ({ ...fee }))
        };
        this.services.push(newService);
      }
      await this.submitServiceToBE();
      this.hideServiceModal();
    },
    async deleteService(serviceId) {
      if (confirm('Are you sure you want to delete this service?')) {
        try {
          const response = await axios.delete(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/${serviceId}`, {
            headers: {
              'accept': '*/*'
            }
          });
          if (response.status === 200 && response.data.data.success) {
            this.services = this.services.filter(s => s.id !== serviceId);
            alert('Service deleted successfully!');
          } else {
            alert('Failed to delete service!');
          }
        } catch (error) {
          console.error('Error deleting service:', error);
          alert('Error deleting service!');
        }
      }
    },
    logout() {
      if (typeof window !== 'undefined') {
        localStorage.removeItem('userToken');
        localStorage.removeItem('userEmail');
        localStorage.removeItem('role');
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        localStorage.removeItem('userRole');
        localStorage.removeItem('username');
        localStorage.removeItem('ManufacturerId');
      }
      if (typeof window !== 'undefined') {
        window.location.href = '/loginPage';
      }
    },
    async saveDefaultCustomFees() {
      let manufacturerId = null;
      if (typeof window !== 'undefined') {
        manufacturerId = localStorage.getItem('ManufacturerId') || 1;
      } else {
        manufacturerId = 1;
      }

      try {
        // Step 1: Fetch existing services
        let existingServices = [];
        try {
          const existingServicesResponse = await axios.get(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/manufacturer/${manufacturerId}`, {
            headers: { 'accept': '*/*' }
          });
          if (existingServicesResponse.status === 200 && Array.isArray(existingServicesResponse.data.data)) {
            existingServices = existingServicesResponse.data.data;
          }
        } catch (error) {
          if (error.response && error.response.status === 404) {
            // No services found, treat as empty list
            existingServices = [];
          } else {
            throw error; // Rethrow other errors
          }
        }

        // Step 2: Collect services to add and identify existing ones to delete
        const addServices = [];
        const servicesToDelete = [];

        this.defaultCustomFees.forEach(fee => {
          const componentLower = fee.part.toLowerCase();

          // Check for existing color fee
          if (fee.colorFee > 0) {
            const existingColorService = existingServices.find(
              s => s.component === componentLower && s.type === 'colorapplication' && !s.isDeleted
            );
            if (existingColorService) {
              servicesToDelete.push(existingColorService.id);
            }
            addServices.push({
              component: componentLower,
              type: 'colorapplication',
              price: Number(fee.colorFee),
              manufacturerId: manufacturerId
            });
          }

          // Check for existing image fee
          if (fee.imageFee > 0) {
            const existingImageService = existingServices.find(
              s => s.component === componentLower && s.type === 'imageapplication' && !s.isDeleted
            );
            if (existingImageService) {
              servicesToDelete.push(existingImageService.id);
            }
            addServices.push({
              component: componentLower,
              type: 'imageapplication',
              price: Number(fee.imageFee),
              manufacturerId: manufacturerId
            });
          }
        });

        // Step 3: Delete existing services
        for (const serviceId of servicesToDelete) {
          const deleteResponse = await axios.delete(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/${serviceId}`, {
            headers: { 'accept': '*/*' }
          });
          if (!(deleteResponse.status === 200 && deleteResponse.data.data.success)) {
            throw new Error(`Failed to delete service with ID ${serviceId}`);
          }
        }

        // Step 4: Add new services
        if (addServices.length > 0) {
          const response = await axios.post('https://fcspwebapi20250527114117.azurewebsites.net/api/Service', {
            addServices: addServices
          }, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });

          if (response.status === 201 && response.data.data.success) {
            alert('Default fees saved successfully!');
            // Reload the page to sync the new fees
            if (typeof window !== 'undefined') {
              window.location.reload();
            }
          } else {
            throw new Error('Failed to save default fees');
          }
        } else {
          alert('No changes to save!');
        }
      } catch (error) {
        console.error('Error saving default fees:', error);
        alert('Error saving default fees!');
      }
    },
    async submitServiceToBE() {
      let manufacturerId = null;
      if (typeof window !== 'undefined') {
        manufacturerId = localStorage.getItem('ManufacturerId') || 1;
      } else {
        manufacturerId = 1;
      }
      const addServices = [];

      this.currentService.customFees.forEach(fee => {
        addServices.push({
          component: fee.part.toLowerCase(),
          type: 'colorapplication',
          price: Number(fee.colorFee) || 0,
          manufacturerId: manufacturerId
        });
        addServices.push({
          component: fee.part.toLowerCase(),
          type: 'imageapplication',
          price: Number(fee.imageFee) || 0,
          manufacturerId: manufacturerId
        });
      });

      try {
        console.log('Sending data:', addServices);
        const response = await axios.post('https://fcspwebapi20250527114117.azurewebsites.net/api/Service', {
          addServices: addServices
        }, {
          headers: {
            'accept': '*/*',
            'Content-Type': 'application/json'
          }
        });
        if (response.status === 201 && response.data.data.success) {
          alert('Service added successfully!');
        } else {
          throw new Error('Failed to add service');
        }
      } catch (error) {
        console.error('Error submitting service:', error);
        alert('Error adding service!');
      }
    },
    async syncDefaultCustomFees() {
      let manufacturerId = null;
      if (typeof window !== 'undefined') {
        manufacturerId = localStorage.getItem('ManufacturerId');
        if (!manufacturerId) {
          alert('Manufacturer ID not found!');
          return;
        }
      } else {
        return;
      }

      try {
        const res = await axios.get(`https://fcspwebapi20250527114117.azurewebsites.net/api/Service/manufacturer/${manufacturerId}`, {
          headers: { 'accept': '*/*' }
        });
        if (res.status === 200 && Array.isArray(res.data.data)) {
          const colorMap = {};
          const imageMap = {};
          const colorIdMap = {};
          const imageIdMap = {};
          res.data.data.forEach(s => {
            if (s.isDeleted) return; // Skip deleted services
            if (s.type === 'colorapplication') {
              colorMap[s.component] = s.price;
              colorIdMap[s.component] = s.id;
            }
            if (s.type === 'imageapplication') {
              imageMap[s.component] = s.price;
              imageIdMap[s.component] = s.id;
            }
          });
          this.defaultCustomFees = this.defaultCustomFees.map(fee => ({
            ...fee,
            colorFee: colorMap[fee.part.toLowerCase()] ?? 0,
            imageFee: imageMap[fee.part.toLowerCase()] ?? 0,
            colorServiceId: colorIdMap[fee.part.toLowerCase()] ?? null,
            imageServiceId: imageIdMap[fee.part.toLowerCase()] ?? null
          }));
        }
      } catch (error) {
        if (error.response && error.response.status === 404) {
          // No services found, reset fees to default (all 0)
          this.defaultCustomFees = this.defaultCustomFees.map(fee => ({
            ...fee,
            colorFee: 0,
            imageFee: 0,
            colorServiceId: null,
            imageServiceId: null
          }));
        } else {
          console.error('Error syncing default fees:', error);
          alert('Error syncing fees!');
        }
      }
    }
  },
  mounted() {
    if (typeof window !== 'undefined') {
      this.syncDefaultCustomFees();
    }
  }
};
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