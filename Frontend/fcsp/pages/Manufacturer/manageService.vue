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
import { addManufacture, getManufacturerById, updateManufacturer } from '@/server/manuService-service.js';

export default {
  name: 'ManageService',
  components: { HeaderManu },
  setup() {
    return {};
  },
  data() {
    return {
      defaultCustomFees: [
        { part: 'Base', colorFee: 0, imageFee: 0 },
        { part: 'Lace', colorFee: 0, imageFee: 0 },
        { part: 'Sole', colorFee: 0, imageFee: 0 },
        { part: 'Accent', colorFee: 0, imageFee: 0 },
        { part: 'Details', colorFee: 0, imageFee: 0 }
      ],
      services: [
        { id: 1, name: 'Custom Design', description: 'Create custom shoe designs', price: 150, status: 1, customFees: [
          { part: 'Base', colorFee: 0, imageFee: 0 },
          { part: 'Lace', colorFee: 0, imageFee: 0 },
          { part: 'Sole', colorFee: 0, imageFee: 0 },
          { part: 'Accent', colorFee: 0, imageFee: 0 },
          { part: 'Details', colorFee: 0, imageFee: 0 }
        ] },
        { id: 2, name: 'Bulk Order', description: 'Order in bulk quantities', price: 100, status: 1, customFees: [
          { part: 'Base', colorFee: 0, imageFee: 0 },
          { part: 'Lace', colorFee: 0, imageFee: 0 },
          { part: 'Sole', colorFee: 0, imageFee: 0 },
          { part: 'Accent', colorFee: 0, imageFee: 0 },
          { part: 'Details', colorFee: 0, imageFee: 0 }
        ] }
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
    deleteService(serviceId) {
      if (confirm('Are you sure you want to delete this service?')) {
        this.services = this.services.filter(s => s.id !== serviceId);
      }
    },
    logout() {
      // Clear all localStorage items
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

      // Redirect to login page
      if (typeof window !== 'undefined') {
        window.location.href = '/loginPage';
      }
    },
    async saveDefaultCustomFees() {
      let manufacturerId = null;
      if (typeof window !== 'undefined') {
        manufacturerId = localStorage.getItem('ManufacturerId') || 1;
      } else {
        manufacturerId = 1; // Fallback for SSR
      }

      const res = await getManufacturerById(manufacturerId);
      const currentServices = (res && res.data && Array.isArray(res.data.services)) ? res.data.services : [];

      const idMap = {};
      currentServices.forEach(s => {
        idMap[`${s.component}_${s.type}`] = s.id;
      });

      const updateServices = [];
      this.defaultCustomFees.forEach(fee => {
        if (fee.colorFee > 0) {
          updateServices.push({
            id: idMap[`${fee.part.toLowerCase()}_colorapplication`],
            price: Number(fee.colorFee)
          });
        }
        if (fee.imageFee > 0) {
          updateServices.push({
            id: idMap[`${fee.part.toLowerCase()}_imageapplication`],
            price: Number(fee.imageFee)
          });
        }
      });

      try {
        if (currentServices.length > 0 && updateServices.length > 0) {
          // Use updateManufacturer for updating existing services
          await updateManufacturer(updateServices);
          alert('Default fees updated successfully!');
        } else if (currentServices.length === 0) {
          // Add new services if none exist
          const addServices = [];
          this.defaultCustomFees.forEach(fee => {
            if (fee.colorFee > 0) {
              addServices.push({
                component: fee.part.toLowerCase(),
                type: 'colorapplication',
                price: Number(fee.colorFee),
                manufacturerId: manufacturerId
              });
            }
            if (fee.imageFee > 0) {
              addServices.push({
                component: fee.part.toLowerCase(),
                type: 'imageapplication',
                price: Number(fee.imageFee),
                manufacturerId: manufacturerId
              });
            }
          });
          await addManufacture({ addServices });
          alert('Default fees saved successfully!');
        } else {
          alert('No changes to save!');
        }
      } catch (error) {
        console.error('Error saving default fees:', error);
        alert('Error saving/updating default fees!');
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
        await addManufacture({ addServices });
        alert('Service added successfully!');
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
        // Skip during SSR or use a default/fallback value
        return;
      }

      try {
        const res = await getManufacturerById(manufacturerId);
        console.log(res);
        if (!res || !res.data || !Array.isArray(res.data.services)) {
          alert('No service data from API!');
          return;
        }
        const colorMap = {};
        const imageMap = {};
        res.data.services.forEach(s => {
          if (s.type === 'colorapplication') colorMap[s.component] = s.currentAmount;
          if (s.type === 'imageapplication') imageMap[s.component] = s.currentAmount;
        });
        this.defaultCustomFees = this.defaultCustomFees.map(fee => ({
          ...fee,
          colorFee: colorMap[fee.part.toLowerCase()] ?? 0,
          imageFee: imageMap[fee.part.toLowerCase()] ?? 0
        }));
      } catch (error) {
        console.error('Error syncing default fees:', error);
        alert('Error syncing fees!');
      }
    }
  },
  mounted() {
    // Only call syncDefaultCustomFees on the client side
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
  outline: 0;
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