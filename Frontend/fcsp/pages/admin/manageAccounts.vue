<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="container-fluid py-4">
        <div class="row">
          <div class="col-12">
            <h1 class="mb-4 text-primary fw-bold">Account Management</h1>
            
            <!-- Search and Filter Section -->
            <div class="card mb-4 search-card">
              <div class="card-body">
                <div class="row g-3">
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-search"></i>
                      </span>
                      <input 
                        type="text" 
                        class="form-control" 
                        v-model="search" 
                        placeholder="Search accounts"
                      >
                      <button 
                        v-if="search" 
                        class="btn btn-outline-secondary" 
                        type="button"
                        @click="search = ''"
                      >
                        <i class="bi bi-x"></i>
                      </button>
                    </div>
                  </div>
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-funnel"></i>
                      </span>
                      <select class="form-select" v-model="roleFilter">
                        <option value="">All Roles</option>
                        <option v-for="role in userRoles" :key="role.value" :value="role.value">
                          {{ role.label }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-toggle-on"></i>
                      </span>
                      <select class="form-select" v-model="statusFilter">
                        <option value="">All Status</option>
                        <option value="active">Active</option>
                        <option value="inactive">Inactive</option>
                      </select>
                    </div>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Accounts Table -->
            <div class="card accounts-card">
              <div class="card-body p-0">
                <div class="table-responsive">
                  <table class="table table-hover align-middle mb-0">
                    <thead class="table-light">
                      <tr>
                        <th>ID</th>
                        <th>Account</th>
                        <th>Name</th>
                        <th>Role</th>
                        <th>Status</th>
                        <th>Created Date</th>
                        <th class="text-end">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="account in filteredAccounts" :key="account.id">
                        <td class="fw-medium">{{ account.id }}</td>
                        <td>
                          <div class="d-flex align-items-center">
                            <div class="avatar-wrapper me-2">
                              <img 
                                :src="'https://ui-avatars.com/api/?name=' + encodeURIComponent(account.name) + '&background=random'" 
                                class="avatar rounded-circle"
                                alt="Avatar"
                              >
                            </div>
                            <div>
                              <div class="fw-medium">{{ account.name }}</div>
                              <div class="text-muted small">{{ account.email }}</div>
                            </div>
                          </div>
                        </td>
                        <td>{{ account.name }}</td>
                        <td>
                          <span :class="['badge', getRoleBadgeClass(userRoles.find(r => r.value === account.role)?.label)]">
                            {{ userRoles.find(r => r.value === account.role)?.label || account.role }}
                          </span>
                        </td>
                        <td>
                          <span :class="['badge', account.status === 'active' ? 'bg-success' : 'bg-danger']">
                            {{ account.status === 'active' ? 'Active' : 'Inactive' }}
                          </span>
                        </td>
                        <td class="date-text">{{ formatDate(account.createdAt) }}</td>
                        <td class="text-end">
                          <button 
                            class="btn btn-sm btn-outline-primary me-1" 
                            data-bs-toggle="tooltip" 
                            title="View Details"
                            @click="viewAccountDetails(account)"
                          >
                            <i class="bi bi-eye"></i>
                          </button>
                          <button 
                            class="btn btn-sm btn-outline-success me-1" 
                            data-bs-toggle="tooltip" 
                            title="Edit"
                            @click="editAccount(account)"
                          >
                            <i class="bi bi-pencil"></i>
                          </button>
                          <button 
                            v-if="isAdmin"
                            class="btn btn-sm"
                            :class="account.status === 'active' ? 'btn-outline-danger' : 'btn-outline-success'"
                            data-bs-toggle="tooltip" 
                            :title="account.status === 'active' ? 'Lock Account' : 'Unlock Account'"
                            @click="toggleAccountStatus(account)"
                          >
                            <i class="bi" :class="account.status === 'active' ? 'bi-lock' : 'bi-unlock'"></i>
                          </button>
                        </td>
                      </tr>
                      <tr v-if="loading">
                        <td colspan="7" class="text-center py-4">
                          <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Loading...</span>
                          </div>
                        </td>
                      </tr>
                      <tr v-if="!loading && filteredAccounts.length === 0">
                        <td colspan="7" class="text-center py-4">
                          No accounts found
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div class="card-footer bg-white d-flex justify-content-between align-items-center">
                <div>
                  Showing <span class="fw-medium">{{ filteredAccounts.length }}</span> of <span class="fw-medium">{{ accounts.length }}</span> accounts
                </div>
                <!-- <button class="btn btn-primary" @click="showAddAccountModal">
                  <i class="bi bi-plus-circle me-1"></i> Add New Account
                </button> -->
              </div>
            </div>
    
            <!-- Account Details Modal -->
            <div class="modal fade" id="accountDetailsModal" tabindex="-1" ref="accountDetailsModal">
              <div class="modal-dialog modal-lg">
                <div class="modal-content">
                  <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Account Details</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedAccount">
                    <div class="row">
                      <div class="col-md-4 text-center mb-4 mb-md-0">
                        <div class="avatar-container mb-3">
                          <img 
                            :src="selectedAccount.avatar || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(selectedAccount.name) + '&background=random&size=200'" 
                            class="img-fluid rounded-circle avatar-large"
                            alt="Avatar"
                          >
                        </div>
                        <h4 class="mb-1">{{ selectedAccount.name }}</h4>
                        <p class="text-muted mb-2">{{ selectedAccount.username }}</p>
                        <span :class="['badge', getRoleBadgeClass(userRoles.find(r => r.value === selectedAccount.role)?.label)]">
                          {{ userRoles.find(r => r.value === selectedAccount.role)?.label || selectedAccount.role }}
                        </span>
                      </div>
                      <div class="col-md-8">
                        <div class="card border-0 bg-light">
                          <div class="card-body">
                            <h5 class="card-title text-primary mb-3">
                              <i class="bi bi-person-badge me-2"></i>Account Information
                            </h5>
                            <ul class="list-group list-group-flush bg-transparent">
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">ID:</span>
                                <span>{{ selectedAccount.id }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">Email:</span>
                                <span>{{ selectedAccount.email }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">Phone:</span>
                                <span>{{ selectedAccount.phone }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">Created Date:</span>
                                <span>{{ formatDate(selectedAccount.createdAt) }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">Last Login:</span>
                                <span>{{ formatDateTime(selectedAccount.lastLogin) }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                                <span class="fw-medium">Status:</span>
                                <span :class="['badge', selectedAccount.status === 'active' ? 'bg-success' : 'bg-danger']">
                                  {{ selectedAccount.status === 'active' ? 'Active' : 'Inactive' }}
                                </span>
                              </li>
                            </ul>
                          </div>
                        </div>
                        
                        <div class="card border-0 bg-light mt-3">
                          <div class="card-body">
                            <h5 class="card-title text-primary mb-3">
                              <i class="bi bi-shield-lock me-2"></i>Permissions
                            </h5>
                            <div class="permission-list">
                              <div v-for="(permission, index) in selectedAccount.permissions" :key="index" class="permission-item">
                                <span class="badge bg-info me-2">
                                  <i class="bi bi-check-circle me-1"></i>
                                </span>
                                {{ permission }}
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" @click="editAccount(selectedAccount)">Edit</button>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Edit Account Modal (customized for role/commissionRate) -->
            <div class="modal fade" :class="{ show: showEditModal }" style="display: block;" v-if="showEditModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header bg-success text-white">
                    <h5 class="modal-title">Edit Account Role & Commission</h5>
                    <button type="button" class="btn-close btn-close-white" @click="showEditModal = false"></button>
                  </div>
                  <div class="modal-body">
                    <form @submit.prevent="saveAccount">
                      <div class="mb-3">
                        <label class="form-label">ID</label>
                        <input type="text" class="form-control" v-model="editedAccount.id" disabled />
                      </div>
                      <div class="mb-3">
                        <label class="form-label">Role</label>
                        <select class="form-select" v-model.number="editedAccount.role" required>
                          <option v-for="role in userRoles" :key="role.value" :value="role.value">
                            {{ role.label }} ({{ role.value }})
                          </option>
                        </select>
                      </div>
                      <div class="mb-3">
                        <label class="form-label">Commission Rate</label>
                        <input type="number" class="form-control" v-model.number="editedAccount.commissionRate" min="0" step="0.01" />
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" @click="showEditModal = false">Cancel</button>
                        <button type="submit" class="btn btn-success">Save</button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Confirm Status Change Modal -->
            <div class="modal fade" id="confirmStatusModal" tabindex="-1" ref="confirmStatusModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header" :class="selectedAccount && selectedAccount.status === 'active' ? 'bg-danger text-white' : 'bg-success text-white'">
                    <h5 class="modal-title">{{ selectedAccount && selectedAccount.status === 'active' ? 'Lock Account' : 'Unlock Account' }}</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedAccount">
                    <p>Are you sure you want to {{ selectedAccount.status === 'active' ? 'lock' : 'unlock' }} account <strong>{{ selectedAccount.name }}</strong>?</p>
                    <p v-if="selectedAccount.status === 'active'">
                      <i class="bi bi-exclamation-triangle-fill text-warning me-2"></i>
                      The account will not be able to log in after being locked.
                    </p>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button 
                      type="button" 
                      :class="selectedAccount && selectedAccount.status === 'active' ? 'btn btn-danger' : 'btn btn-success'"
                      @click="confirmStatusChange"
                    >
                      {{ selectedAccount && selectedAccount.status === 'active' ? 'Lock Account' : 'Unlock Account' }}
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getAllUser, updateStatus, updateRole } from '@/server/manageAccounts-service';
import AdminSidebar from '@/components/AdminSidebar.vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import { useRouter } from 'vue-router';

export default {
  name: 'AccountManagement',
  components: {
    AdminSidebar
  },
  setup() {
    const router = useRouter();
    return { router };
  },
  data() {
    return {
      search: '',
      roleFilter: '',
      statusFilter: '',
      loading: false,
      selectedAccount: null,
      editedAccount: {},
      showEditModal: false,
      userRoles: [
        { label: 'Admin', value: 3 },
        { label: 'Designer', value: 2 },
        { label: 'Manufacturer', value: 1 },
        { label: 'Customer', value: 0 }
      ],
      availablePermissions: [
        'Manage Accounts',
        'Manage Products',
        'Manage Orders',
        'Manage Categories',
        'Manage Promotions',
        'Manage Payments',
        'View Reports',
        'Export Reports',
        'System Configuration'
      ],
      accounts: []
    }
  },
  computed: {
    filteredAccounts() {
      let result = [...this.accounts];
      
      // Apply role filter
      if (this.roleFilter) {
        result = result.filter(account => Number(account.role) === Number(this.roleFilter));
      }
      
      // Apply status filter
      if (this.statusFilter) {
        result = result.filter(account => account.status === this.statusFilter);
      }
      
      // Apply search filter
      if (this.search) {
        const searchLower = this.search.toLowerCase();
        result = result.filter(account => {
          const roleLabel = this.userRoles.find(r => r.value === account.role)?.label?.toLowerCase() || '';
          return (
            account.id.toString().toLowerCase().includes(searchLower) ||
            account.name.toLowerCase().includes(searchLower) ||
            account.email.toLowerCase().includes(searchLower) ||
            account.role.toString().toLowerCase().includes(searchLower) ||
            roleLabel.includes(searchLower)
          );
        });
      }
      
      return result;
    },
    isAdmin() {
      try {
        return typeof window !== 'undefined' && localStorage && localStorage.getItem('role') === 'Admin';
      } catch (e) {
        return false;
      }
    }
  },
  methods: {
    checkAdminAccess() {
      try {
        if (!this.isAdmin) {
          ElMessage({
            type: 'error',
            message: 'Access denied. Only administrators can access this page.'
          });
          this.router.push('/');
          return false;
        }
        return true;
      } catch (e) {
        this.router.push('/');
        return false;
      }
    },
    formatDate(date) {
      if (!date) return '';
      return new Date(date).toLocaleDateString('en-US');
    },
    formatDateTime(dateTime) {
      if (!dateTime) return '';
      return new Date(dateTime).toLocaleString('vi-VN');
    },
    getRoleBadgeClass(role) {
      const classes = {
        'Admin': 'bg-danger',
        'Designer': 'bg-warning text-dark',
        'Manufacturer': 'bg-info'
      };
      return classes[role] || 'bg-secondary';
    },
    viewAccountDetails(account) {
      this.selectedAccount = account;
    },
    toggleAccountStatus(account) {
      if (!account) return;
      
      // Check if current user is admin
      if (!this.isAdmin) {
        ElMessage({
          type: 'warning',
          message: 'Only administrators can lock/unlock accounts'
        });
        return;
      }

      // Don't allow admin to lock themselves
      try {
        const currentUserId = typeof window !== 'undefined' && localStorage ? localStorage.getItem('userId') : null;
        if (currentUserId === account.id) {
          ElMessage({
            type: 'warning',
            message: 'You cannot lock your own account'
          });
          return;
        }
      } catch (e) {
        console.error('Error accessing localStorage:', e);
        return;
      }
      
      const action = account.status === 'active' ? 'lock' : 'unlock';
      
      ElMessageBox.confirm(
        `Are you sure you want to ${action} account "${account.name}"?`,
        `${action.charAt(0).toUpperCase() + action.slice(1)} Account`,
        {
          confirmButtonText: action.charAt(0).toUpperCase() + action.slice(1),
          cancelButtonText: 'Cancel',
          type: action === 'lock' ? 'warning' : 'success'
        }
      )
      .then(async () => {
        try {
          const newStatus = account.status === 'active' ? 'inactive' : 'active';
          // Call API with correct parameters
          await updateStatus(account.id, account.status === 'active');
          
          // Update local state
          account.status = newStatus;
          
          // Refresh the accounts list
          await this.fetchAccounts();
          
          // Show success message
          ElMessage({
            type: 'success',
            message: `Account has been ${newStatus === 'active' ? 'unlocked' : 'locked'} successfully`
          });
        } catch (error) {
          console.error('Error updating status:', error);
          ElMessage({
            type: 'error',
            message: error.message || 'Failed to update account status'
          });
        }
      })
      .catch(() => {
        // User cancelled the operation
      });
    },
    editAccount(account) {
      this.editedAccount = JSON.parse(JSON.stringify(account));
      this.showEditModal = true;
    },
    async saveAccount() {
      try {
        // Gọi API updateRole
        const roleNumber = typeof this.editedAccount.role === 'number' ? this.editedAccount.role : this.userRoles.find(r => r.value === this.editedAccount.role)?.value;
        await updateRole(this.editedAccount.id, roleNumber, this.editedAccount.commissionRate || 0);
        await this.fetchAccounts();
        ElMessage({
          type: 'success',
          message: 'Account updated successfully'
        });
        this.showEditModal = false;
      } catch (error) {
        ElMessage({
          type: 'error',
          message: error.message || 'Failed to save account'
        });
      }
    },
    async fetchAccounts() {
      this.loading = true;
      try {
        const response = await getAllUser();
        this.accounts = response.map(user => ({
          id: user.id,
          name: user.name,
          email: user.email,
          role: typeof user.role === 'string'
            ? this.userRoles.find(r => r.label.toLowerCase() === user.role.toLowerCase())?.value
            : user.role,
          status: user.status === 'Active' ? 'active' : 'inactive',
          createdAt: user.createdAt
        }));
      } catch (error) {
        console.error('Error loading accounts:', error);
        ElMessage({
          type: 'error',
          message: 'Error loading accounts'
        });
      } finally {
        this.loading = false;
      }
    },
  },
  async created() {
    if (!this.checkAdminAccess()) return;
  },
  async mounted() {
    if (!this.checkAdminAccess()) return;

    try {
      await this.fetchAccounts();
    } catch (error) {
      console.error('Error in mounted hook:', error);
      ElMessage({
        type: 'error',
        message: 'Error initializing the page'
      });
    }
  },
}
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

/* Custom styling */
.search-card {
  border: none;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  transition: all 0.3s ease;
}

.search-card:hover {
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.1);
}

.accounts-card {
  border: none;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  overflow: hidden;
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

.avatar {
  width: 40px;
  height: 40px;
  object-fit: cover;
}

.avatar-large {
  width: 150px;
  height: 150px;
  object-fit: cover;
  border: 5px solid #fff;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.avatar-container {
  position: relative;
  display: inline-block;
}

.date-text {
  color: #6c757d;
  font-size: 0.9rem;
}

/* Status badge styling */
.badge {
  padding: 0.5em 0.75em;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-size: 0.75rem;
}

/* Modal styling */
.modal-header {
  border-bottom: 0;
}

.modal-footer {
  border-top: 0;
}

.card-title {
  font-weight: 600;
  font-size: 1.1rem;
}

.list-group-item {
  padding-top: 0.75rem;
  padding-bottom: 0.75rem;
  border-color: rgba(0, 0, 0, 0.05);
}

/* Button styling */
.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.8rem;
}

.btn-outline-primary, .btn-outline-success, .btn-outline-danger {
  transition: all 0.2s ease;
}

.btn-outline-primary:hover, .btn-outline-success:hover, .btn-outline-danger:hover {
  transform: translateY(-2px);
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
}

/* Permission styling */
.permission-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.permission-item {
  background-color: #f8f9fa;
  padding: 0.5rem 0.75rem;
  border-radius: 0.25rem;
  font-size: 0.875rem;
}

.permission-checkboxes {
  max-height: 200px;
  overflow-y: auto;
}

/* Animation for status changes */
.badge {
  transition: background-color 0.3s ease;
}

/* Responsive improvements */
@media (max-width: 768px) {
  .card-body {
    padding: 1rem;
  }
  
  .table th, .table td {
    padding: 0.5rem;
  }
  
  .btn-sm {
    padding: 0.2rem 0.4rem;
  }
  
  .avatar-large {
    width: 100px;
    height: 100px;
  }
}
</style>