<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <!-- Toast Notification -->
      <div class="toast-container position-fixed top-0 end-0 p-3">
        <div class="toast align-items-center text-white bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true" ref="toast">
          <div class="d-flex">
            <div class="toast-body">
              <i class="bi bi-check-circle me-2"></i>
              Size added successfully!
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
          </div>
        </div>
      </div>
      <div class="container-fluid py-4">
        <h1 class="mb-4 text-primary fw-bold">Size Management</h1>
        <div class="card mb-4">
          <div class="card-body d-flex justify-content-between align-items-center">
            <div class="fw-bold">All Sizes</div>
            <button class="btn btn-primary" @click="openAddModal">
              <i class="bi bi-plus-circle me-1"></i> Add New Size
            </button>
          </div>
        </div>
        <div class="card">
          <div class="card-body p-0">
            <div v-if="loading" class="text-center py-4">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>
            <div v-else-if="error" class="text-center py-4 text-danger">
              {{ error }}
            </div>
            <div v-else class="table-responsive">
              <table class="table table-hover align-middle mb-0">
                <thead class="table-light">
                  <tr>
                    <th>ID</th>
                    <th>Size Value</th>
                    <th class="text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="size in sizes" :key="size.id">
                    <td>{{ size.id }}</td>
                    <td>{{ size.sizeValue }}</td>
                    <td class="text-end">
                      
                      <button class="btn btn-sm btn-outline-danger" @click="confirmDelete(size)"><i class="bi bi-trash"></i></button>
                    </td>
                  </tr>
                  <tr v-if="sizes.length === 0">
                    <td colspan="3" class="text-center py-4">No sizes found</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
        <!-- Add/Edit Modal -->
        <div class="modal fade" :class="{ show: showModal }" style="display: block;" v-if="showModal">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header bg-primary text-white">
                <h5 class="modal-title">{{ isEdit ? 'Edit Size' : 'Add New Size' }}</h5>
                <button type="button" class="btn-close btn-close-white" @click="closeModal"></button>
              </div>
              <div class="modal-body">
                <form @submit.prevent="saveSize">
                  <div class="mb-3">
                    <label class="form-label">Size Value</label>
                    <input type="text" class="form-control" v-model="form.sizeValue" required />
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="closeModal">Cancel</button>
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
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue';
import { getAllSizes, createSizes, DeletedSize } from '@/server/manageSize-service.js';

export default {
  name: 'ManageSize',
  components: { AdminSidebar },
  data() {
    return {
      sizes: [],
      loading: false,
      error: '',
      showModal: false,
      isEdit: false,
      form: { id: null, sizeValue: '' },
      toast: null,
    };
  },
  methods: {
    showToast() {
      if (this.toast) {
        this.toast.show();
      }
    },
    async fetchSizes() {
      this.loading = true;
      this.error = '';
      try {
        const res = await getAllSizes();
        console.log(res);
        if (res && res.code === 200 && Array.isArray(res.data)) {
          this.sizes = res.data.filter(s => !s.isDeleted).sort((a, b) => {
            const sizeA = parseFloat(a.sizeValue);
            const sizeB = parseFloat(b.sizeValue);
            return sizeA - sizeB;
          });
        } else {
          this.error = res?.message || 'Failed to load sizes.';
        }
      } catch (e) {
        this.error = 'Error loading sizes.';
      } finally {
        this.loading = false;
      }
    },
    openAddModal() {
      this.isEdit = false;
      this.form = { id: null, sizeValue: '' };
      this.showModal = true;
    },
    openEditModal(size) {
      this.isEdit = true;
      this.form = { ...size };
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
    },
    async saveSize() {
      if (this.isEdit) {
        const idx = this.sizes.findIndex(s => s.id === this.form.id);
        if (idx !== -1) this.sizes[idx] = { ...this.form };
        this.closeModal();
      } else {
        try {
          this.loading = true;
          this.error = '';
          const res = await createSizes({ sizeValue: this.form.sizeValue });
          console.log('API Create Size Response:', res);
          if (res && (res.code === 200 || res.code === 201)) {
            this.closeModal();
            await this.fetchSizes();
            this.showToast();
          } else {
            this.error = res?.message || 'Failed to create size.';
          }
        } catch (e) {
          console.error('Error creating size:', e);
          this.error = 'Error creating size: ' + (e.message || e);
        } finally {
          this.loading = false;
        }
      }
    },
    async confirmDelete(size) {
      if (confirm(`Are you sure you want to delete size ${size.sizeValue}?`)) {
        try {
          this.loading = true;
          this.error = '';
          const res = await DeletedSize(size.id);
          console.log('API Delete Size Response:', res);

          if (res && res.code === 200) {
            await this.fetchSizes();
            alert('Size deleted successfully!');
          } else {
            this.error = res?.message || 'Failed to delete size.';
          }
        } catch (e) {
          console.error('Error deleting size:', e);
          this.error = 'Error deleting size: ' + (e.message || e);
        } finally {
          this.loading = false;
        }
      }
    },
  },
  mounted() {
    this.fetchSizes();
    this.$nextTick(() => {
      if (process.client && this.$refs.toast) {
        import('bootstrap').then(({ Toast }) => {
          this.toast = new Toast(this.$refs.toast);
        }).catch(e => console.error('Failed to load Bootstrap Toast:', e));
      }
    });
  },
};
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
.table th {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.8rem;
  letter-spacing: 0.5px;
}
.table td {
  vertical-align: middle;
  padding: 0.75rem 1rem;
}
.btn {
  min-width: 36px;
}
.modal {
  background: rgba(0,0,0,0.3);
}
.modal-dialog {
  margin-top: 10vh;
}
.toast-container {
  z-index: 1050;
}
.toast {
  min-width: 250px;
}
</style> 