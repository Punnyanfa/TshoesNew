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
                      <button 
                        class="delete-btn" 
                        @click="confirmDelete(size)" 
                        title="Delete"
                      > 
                        <i class="fas fa-trash-alt"></i>
                      </button>
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
                  <h3>Confirm delete size</h3>
                  <button class="modal-close" @click="showDeleteModal = false">×</button>
                </div>
                <div class="modal-body delete-modal-body">
                  <p v-if="selectedSize" class="delete-modal-title">Are you sure you want to delete size<b>"{{ selectedSize.sizeValue }}"</b>?</p>
                  <p class="text-danger delete-modal-warning">This action cannot be undone.</p>
                </div>
                <div class="modal-footer delete-modal-footer">
                  <button class="btn-cancel" @click="showDeleteModal = false">Cancel</button>
                  <button class="btn-delete" @click="handleDelete">Delete</button>
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
      showDeleteModal: false,
      selectedSize: null
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
    confirmDelete(size) {
      this.selectedSize = size;
      this.showDeleteModal = true;
    },
    async handleDelete() {
      try {
        if (!this.selectedSize) {
          alert('No size selected for deletion');
          return;
        }
        
        this.loading = true;
        this.error = '';
        const res = await DeletedSize(this.selectedSize.id);
        console.log('API Delete Size Response:', res);

        if (res && res.code === 200) {
          await this.fetchSizes();
          this.showDeleteModal = false;
          this.selectedSize = null;
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