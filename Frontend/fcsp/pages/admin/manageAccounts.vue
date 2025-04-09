<template>
    <div class="container-fluid py-4">
      <div class="row">
        <div class="col-12">
          <h1 class="mb-4 text-primary fw-bold">Quản lý tài khoản</h1>
          
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
                      placeholder="Tìm kiếm tài khoản"
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
                      <option value="">Tất cả vai trò</option>
                      <option v-for="role in userRoles" :key="role" :value="role">
                        {{ role }}
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
                      <option value="">Tất cả trạng thái</option>
                      <option value="active">Đang hoạt động</option>
                      <option value="inactive">Đã khóa</option>
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
                      <th>Tài khoản</th>
                      <th>Họ tên</th>
                      <th>Vai trò</th>
                      <th>Trạng thái</th>
                      <th>Ngày tạo</th>
                      <th class="text-end">Thao tác</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="account in filteredAccounts" :key="account.id">
                      <td class="fw-medium">{{ account.id }}</td>
                      <td>
                        <div class="d-flex align-items-center">
                          <div class="avatar-wrapper me-2">
                            <img 
                              :src="account.avatar || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(account.fullName) + '&background=random'" 
                              class="avatar rounded-circle"
                              alt="Avatar"
                            >
                          </div>
                          <div>
                            <div class="fw-medium">{{ account.username }}</div>
                            <div class="text-muted small">{{ account.email }}</div>
                          </div>
                        </div>
                      </td>
                      <td>{{ account.fullName }}</td>
                      <td>
                        <span :class="['badge', getRoleBadgeClass(account.role)]">
                          {{ account.role }}
                        </span>
                      </td>
                      <td>
                        <span :class="['badge', account.status === 'active' ? 'bg-success' : 'bg-danger']">
                          {{ account.status === 'active' ? 'Đang hoạt động' : 'Đã khóa' }}
                        </span>
                      </td>
                      <td class="date-text">{{ formatDate(account.createdAt) }}</td>
                      <td class="text-end">
                        <button 
                          class="btn btn-sm btn-outline-primary me-1" 
                          data-bs-toggle="tooltip" 
                          title="Xem chi tiết"
                          @click="viewAccountDetails(account)"
                        >
                          <i class="bi bi-eye"></i>
                        </button>
                        <button 
                          class="btn btn-sm btn-outline-success me-1" 
                          data-bs-toggle="tooltip" 
                          title="Chỉnh sửa"
                          @click="editAccount(account)"
                        >
                          <i class="bi bi-pencil"></i>
                        </button>
                        <button 
                          class="btn btn-sm"
                          :class="account.status === 'active' ? 'btn-outline-danger' : 'btn-outline-success'"
                          data-bs-toggle="tooltip" 
                          :title="account.status === 'active' ? 'Khóa tài khoản' : 'Mở khóa tài khoản'"
                          @click="toggleAccountStatus(account)"
                        >
                          <i class="bi" :class="account.status === 'active' ? 'bi-lock' : 'bi-unlock'"></i>
                        </button>
                      </td>
                    </tr>
                    <tr v-if="loading">
                      <td colspan="7" class="text-center py-4">
                        <div class="spinner-border text-primary" role="status">
                          <span class="visually-hidden">Đang tải...</span>
                        </div>
                      </td>
                    </tr>
                    <tr v-if="!loading && filteredAccounts.length === 0">
                      <td colspan="7" class="text-center py-4">
                        Không tìm thấy tài khoản nào
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            <div class="card-footer bg-white d-flex justify-content-between align-items-center">
              <div>
                Hiển thị <span class="fw-medium">{{ filteredAccounts.length }}</span> trên tổng số <span class="fw-medium">{{ accounts.length }}</span> tài khoản
              </div>
              <button class="btn btn-primary" @click="showAddAccountModal">
                <i class="bi bi-plus-circle me-1"></i> Thêm tài khoản mới
              </button>
            </div>
          </div>
  
          <!-- Account Details Modal -->
          <div class="modal fade" id="accountDetailsModal" tabindex="-1" ref="accountDetailsModal">
            <div class="modal-dialog modal-lg">
              <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                  <h5 class="modal-title">Chi tiết tài khoản</h5>
                  <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body" v-if="selectedAccount">
                  <div class="row">
                    <div class="col-md-4 text-center mb-4 mb-md-0">
                      <div class="avatar-container mb-3">
                        <img 
                          :src="selectedAccount.avatar || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(selectedAccount.fullName) + '&background=random&size=200'" 
                          class="img-fluid rounded-circle avatar-large"
                          alt="Avatar"
                        >
                      </div>
                      <h4 class="mb-1">{{ selectedAccount.fullName }}</h4>
                      <p class="text-muted mb-2">{{ selectedAccount.username }}</p>
                      <span :class="['badge', getRoleBadgeClass(selectedAccount.role)]">
                        {{ selectedAccount.role }}
                      </span>
                    </div>
                    <div class="col-md-8">
                      <div class="card border-0 bg-light">
                        <div class="card-body">
                          <h5 class="card-title text-primary mb-3">
                            <i class="bi bi-person-badge me-2"></i>Thông tin tài khoản
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
                              <span class="fw-medium">Số điện thoại:</span>
                              <span>{{ selectedAccount.phone }}</span>
                            </li>
                            <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                              <span class="fw-medium">Ngày tạo:</span>
                              <span>{{ formatDate(selectedAccount.createdAt) }}</span>
                            </li>
                            <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                              <span class="fw-medium">Đăng nhập cuối:</span>
                              <span>{{ formatDateTime(selectedAccount.lastLogin) }}</span>
                            </li>
                            <li class="list-group-item bg-transparent px-0 d-flex justify-content-between">
                              <span class="fw-medium">Trạng thái:</span>
                              <span :class="['badge', selectedAccount.status === 'active' ? 'bg-success' : 'bg-danger']">
                                {{ selectedAccount.status === 'active' ? 'Đang hoạt động' : 'Đã khóa' }}
                              </span>
                            </li>
                          </ul>
                        </div>
                      </div>
                      
                      <div class="card border-0 bg-light mt-3">
                        <div class="card-body">
                          <h5 class="card-title text-primary mb-3">
                            <i class="bi bi-shield-lock me-2"></i>Quyền hạn
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
                  <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                  <button type="button" class="btn btn-primary" @click="editAccount(selectedAccount)">Chỉnh sửa</button>
                </div>
              </div>
            </div>
          </div>
  
          <!-- Edit Account Modal -->
          <div class="modal fade" id="editAccountModal" tabindex="-1" ref="editAccountModal">
            <div class="modal-dialog modal-lg">
              <div class="modal-content">
                <div class="modal-header bg-success text-white">
                  <h5 class="modal-title">{{ isNewAccount ? 'Thêm tài khoản mới' : 'Chỉnh sửa tài khoản' }}</h5>
                  <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                  <form @submit.prevent="saveAccount" id="accountForm">
                    <div class="row g-3">
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="username" class="form-label">Tên đăng nhập</label>
                          <input type="text" class="form-control" id="username" v-model="editedAccount.username" required>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="email" class="form-label">Email</label>
                          <input type="email" class="form-control" id="email" v-model="editedAccount.email" required>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="fullName" class="form-label">Họ tên</label>
                          <input type="text" class="form-control" id="fullName" v-model="editedAccount.fullName" required>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="phone" class="form-label">Số điện thoại</label>
                          <input type="tel" class="form-control" id="phone" v-model="editedAccount.phone">
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="role" class="form-label">Vai trò</label>
                          <select class="form-select" id="role" v-model="editedAccount.role" required>
                            <option v-for="role in userRoles" :key="role" :value="role">
                              {{ role }}
                            </option>
                          </select>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label for="status" class="form-label">Trạng thái</label>
                          <select class="form-select" id="status" v-model="editedAccount.status" required>
                            <option value="active">Đang hoạt động</option>
                            <option value="inactive">Đã khóa</option>
                          </select>
                        </div>
                      </div>
                      <div class="col-12" v-if="isNewAccount">
                        <div class="mb-3">
                          <label for="password" class="form-label">Mật khẩu</label>
                          <input type="password" class="form-control" id="password" v-model="editedAccount.password" :required="isNewAccount">
                        </div>
                      </div>
                      <div class="col-12">
                        <div class="mb-3">
                          <label class="form-label">Quyền hạn</label>
                          <div class="permission-checkboxes bg-light p-3 rounded">
                            <div class="row g-2">
                              <div class="col-md-4" v-for="(permission, index) in availablePermissions" :key="index">
                                <div class="form-check">
                                  <input 
                                    class="form-check-input" 
                                    type="checkbox" 
                                    :id="'permission-' + index"
                                    :value="permission"
                                    v-model="editedAccount.permissions"
                                  >
                                  <label class="form-check-label" :for="'permission-' + index">
                                    {{ permission }}
                                  </label>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </form>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                  <button type="submit" form="accountForm" class="btn btn-success">Lưu</button>
                </div>
              </div>
            </div>
          </div>
  
          <!-- Confirm Status Change Modal -->
          <div class="modal fade" id="confirmStatusModal" tabindex="-1" ref="confirmStatusModal">
            <div class="modal-dialog">
              <div class="modal-content">
                <div class="modal-header" :class="selectedAccount && selectedAccount.status === 'active' ? 'bg-danger text-white' : 'bg-success text-white'">
                  <h5 class="modal-title">{{ selectedAccount && selectedAccount.status === 'active' ? 'Khóa tài khoản' : 'Mở khóa tài khoản' }}</h5>
                  <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body" v-if="selectedAccount">
                  <p>Bạn có chắc chắn muốn {{ selectedAccount.status === 'active' ? 'khóa' : 'mở khóa' }} tài khoản <strong>{{ selectedAccount.username }}</strong>?</p>
                  <p v-if="selectedAccount.status === 'active'">
                    <i class="bi bi-exclamation-triangle-fill text-warning me-2"></i>
                    Tài khoản sẽ không thể đăng nhập sau khi bị khóa.
                  </p>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                  <button 
                    type="button" 
                    :class="selectedAccount && selectedAccount.status === 'active' ? 'btn btn-danger' : 'btn btn-success'"
                    @click="confirmStatusChange"
                  >
                    {{ selectedAccount && selectedAccount.status === 'active' ? 'Khóa tài khoản' : 'Mở khóa tài khoản' }}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  
  export default {
    name: 'AccountManagement',
    data() {
      return {
        search: '',
        roleFilter: '',
        statusFilter: '',
        loading: false,
        selectedAccount: null,
        editedAccount: {},
        isNewAccount: false,
        accountDetailsModal: null,
        editAccountModal: null,
        confirmStatusModal: null,
        userRoles: [
          'Admin',
          'Quản lý',
          'Nhân viên',
          'Khách hàng'
        ],
        availablePermissions: [
          'Quản lý tài khoản',
          'Quản lý sản phẩm',
          'Quản lý đơn hàng',
          'Quản lý danh mục',
          'Quản lý khuyến mãi',
          'Quản lý thanh toán',
          'Xem báo cáo',
          'Xuất báo cáo',
          'Cấu hình hệ thống'
        ],
        accounts: [
          {
            id: 'ACC001',
            username: 'admin',
            email: 'admin@example.com',
            fullName: 'Nguyễn Quản Trị',
            phone: '0123456789',
            role: 'Admin',
            status: 'active',
            createdAt: '2023-01-15',
            lastLogin: '2024-03-28T08:45:30',
            avatar: 'https://ui-avatars.com/api/?name=Nguyen+Quan+Tri&background=0D8ABC&color=fff',
            permissions: [
              'Quản lý tài khoản',
              'Quản lý sản phẩm',
              'Quản lý đơn hàng',
              'Quản lý danh mục',
              'Quản lý khuyến mãi',
              'Quản lý thanh toán',
              'Xem báo cáo',
              'Xuất báo cáo',
              'Cấu hình hệ thống'
            ]
          },
          {
            id: 'ACC002',
            username: 'manager',
            email: 'manager@example.com',
            fullName: 'Trần Văn Quản Lý',
            phone: '0987654321',
            role: 'Quản lý',
            status: 'active',
            createdAt: '2023-02-20',
            lastLogin: '2024-03-27T14:30:15',
            avatar: 'https://ui-avatars.com/api/?name=Tran+Van+Quan+Ly&background=4CAF50&color=fff',
            permissions: [
              'Quản lý sản phẩm',
              'Quản lý đơn hàng',
              'Quản lý danh mục',
              'Quản lý khuyến mãi',
              'Xem báo cáo'
            ]
          },
          {
            id: 'ACC003',
            username: 'staff1',
            email: 'staff1@example.com',
            fullName: 'Lê Thị Nhân Viên',
            phone: '0369852147',
            role: 'Nhân viên',
            status: 'active',
            createdAt: '2023-03-10',
            lastLogin: '2024-03-28T09:15:45',
            avatar: 'https://ui-avatars.com/api/?name=Le+Thi+Nhan+Vien&background=FF5722&color=fff',
            permissions: [
              'Quản lý sản phẩm',
              'Quản lý đơn hàng'
            ]
          },
          {
            id: 'ACC004',
            username: 'staff2',
            email: 'staff2@example.com',
            fullName: 'Phạm Văn Nhân Viên',
            phone: '0258741369',
            role: 'Nhân viên',
            status: 'inactive',
            createdAt: '2023-04-05',
            lastLogin: '2024-03-15T10:20:30',
            avatar: null,
            permissions: [
              'Quản lý sản phẩm',
              'Quản lý đơn hàng'
            ]
          },
          {
            id: 'ACC005',
            username: 'customer1',
            email: 'customer1@example.com',
            fullName: 'Hoàng Thị Khách Hàng',
            phone: '0147852369',
            role: 'Khách hàng',
            status: 'active',
            createdAt: '2023-05-12',
            lastLogin: '2024-03-26T16:40:10',
            avatar: null,
            permissions: []
          },
          {
            id: 'ACC006',
            username: 'customer2',
            email: 'customer2@example.com',
            fullName: 'Mai Văn Khách Hàng',
            phone: '0321654987',
            role: 'Khách hàng',
            status: 'active',
            createdAt: '2023-06-18',
            lastLogin: '2024-03-25T11:35:20',
            avatar: null,
            permissions: []
          },
          {
            id: 'ACC007',
            username: 'customer3',
            email: 'customer3@example.com',
            fullName: 'Đỗ Thị Khách Hàng',
            phone: '0963258741',
            role: 'Khách hàng',
            status: 'inactive',
            createdAt: '2023-07-22',
            lastLogin: '2024-02-15T09:10:05',
            avatar: null,
            permissions: []
          }
        ]
      }
    },
    computed: {
      filteredAccounts() {
        let result = [...this.accounts];
        
        // Apply role filter
        if (this.roleFilter) {
          result = result.filter(account => account.role === this.roleFilter);
        }
        
        // Apply status filter
        if (this.statusFilter) {
          result = result.filter(account => account.status === this.statusFilter);
        }
        
        // Apply search filter
        if (this.search) {
          const searchLower = this.search.toLowerCase();
          result = result.filter(account => 
            account.id.toLowerCase().includes(searchLower) ||
            account.username.toLowerCase().includes(searchLower) ||
            account.email.toLowerCase().includes(searchLower) ||
            account.fullName.toLowerCase().includes(searchLower) ||
            account.phone.includes(searchLower)
          );
        }
        
        return result;
      }
    },
    methods: {
      formatDate(date) {
        if (!date) return '';
        return new Date(date).toLocaleDateString('vi-VN');
      },
      formatDateTime(dateTime) {
        if (!dateTime) return '';
        return new Date(dateTime).toLocaleString('vi-VN');
      },
      getRoleBadgeClass(role) {
        const classes = {
          'Admin': 'bg-danger',
          'Quản lý': 'bg-warning text-dark',
          'Nhân viên': 'bg-info',
          'Khách hàng': 'bg-secondary'
        };
        return classes[role] || 'bg-secondary';
      },
      viewAccountDetails(account) {
        this.selectedAccount = account;
        this.accountDetailsModal.show();
      },
      editAccount(account) {
        this.isNewAccount = false;
        this.editedAccount = JSON.parse(JSON.stringify(account)); // Deep copy
        
        // Close details modal if open
        if (this.accountDetailsModal._isShown) {
          this.accountDetailsModal.hide();
        }
        
        this.editAccountModal.show();
      },
      showAddAccountModal() {
        this.isNewAccount = true;
        this.editedAccount = {
          id: 'ACC' + (this.accounts.length + 1).toString().padStart(3, '0'),
          username: '',
          email: '',
          fullName: '',
          phone: '',
          role: 'Khách hàng',
          status: 'active',
          createdAt: new Date().toISOString().split('T')[0],
          lastLogin: null,
          avatar: null,
          permissions: [],
          password: ''
        };
        this.editAccountModal.show();
      },
      toggleAccountStatus(account) {
        this.selectedAccount = account;
        this.confirmStatusModal.show();
      },
      async confirmStatusChange() {
        try {
          // Toggle the status
          this.selectedAccount.status = this.selectedAccount.status === 'active' ? 'inactive' : 'active';
          
          // Close the modal
          this.confirmStatusModal.hide();
          
          // Show success message
          this.showToast(
            `Tài khoản ${this.selectedAccount.username} đã được ${this.selectedAccount.status === 'active' ? 'mở khóa' : 'khóa'} thành công`,
            this.selectedAccount.status === 'active' ? 'success' : 'warning'
          );
        } catch (error) {
          this.showToast('Có lỗi xảy ra khi thay đổi trạng thái tài khoản', 'danger');
        }
      },
      async saveAccount() {
        try {
          if (this.isNewAccount) {
            // Add new account
            this.accounts.push(this.editedAccount);
            this.showToast('Tạo tài khoản mới thành công', 'success');
          } else {
            // Update existing account
            const index = this.accounts.findIndex(acc => acc.id === this.editedAccount.id);
            if (index !== -1) {
              this.accounts[index] = { ...this.editedAccount };
            }
            this.showToast('Cập nhật tài khoản thành công', 'success');
          }
          
          // Close the modal
          this.editAccountModal.hide();
        } catch (error) {
          this.showToast('Có lỗi xảy ra khi lưu thông tin tài khoản', 'danger');
        }
      },
      showToast(message, type = 'success') {
        // Create a Bootstrap toast programmatically
        const toastContainer = document.getElementById('toast-container') || this.createToastContainer();
        
        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-white bg-${type} border-0`;
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        toastEl.setAttribute('aria-atomic', 'true');
        
        toastEl.innerHTML = `
          <div class="d-flex">
            <div class="toast-body">
              ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
          </div>
        `;
        
        toastContainer.appendChild(toastEl);
        
        // Initialize and show the toast
        const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 3000 });
        toast.show();
        
        // Remove the toast element after it's hidden
        toastEl.addEventListener('hidden.bs.toast', () => {
          toastEl.remove();
        });
      },
      createToastContainer() {
        const container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        container.style.zIndex = '1050';
        document.body.appendChild(container);
        return container;
      },
      async fetchAccounts() {
        this.loading = true;
        try {
          // TODO: Implement API call to fetch accounts
          // Using mock data for demonstration
          await new Promise(resolve => setTimeout(resolve, 500)); // Simulate API delay
          // Accounts are already set in data()
        } catch (error) {
          this.showToast('Có lỗi xảy ra khi tải danh sách tài khoản', 'danger');
        } finally {
          this.loading = false;
        }
      },
      
    },
    mounted() {
      
    },
    created() {
      this.fetchAccounts();
    }
  }
  </script>
  
  <style scoped>
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