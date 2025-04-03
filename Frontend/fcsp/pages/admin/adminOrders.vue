<template>
  <div class="container-fluid py-4">
    <div class="row">
      <div class="col-12">
        <h1 class="mb-4 text-primary fw-bold">Quản lý đơn hàng</h1>
        
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
                    placeholder="Tìm kiếm đơn hàng"
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
                  <select class="form-select" v-model="statusFilter">
                    <option value="">Tất cả trạng thái</option>
                    <option v-for="status in orderStatuses" :key="status" :value="status">
                      {{ status }}
                    </option>
                  </select>
                </div>
              </div>
              <div class="col-12 col-md-4">
                <div class="input-group">
                  <span class="input-group-text bg-light">
                    <i class="bi bi-calendar3"></i>
                  </span>
                  <input 
                    type="text" 
                    class="form-control" 
                    v-model="dateRangeText" 
                    placeholder="Khoảng thời gian"
                    readonly
                    @click="toggleDatePicker"
                  >
                </div>
                <div v-if="datePickerVisible" class="date-picker-dropdown card mt-1">
                  <div class="card-body p-2">
                    <!-- Simple date range picker implementation -->
                    <div class="d-flex gap-2 mb-2">
                      <input type="date" class="form-control" v-model="dateRange[0]">
                      <input type="date" class="form-control" v-model="dateRange[1]">
                    </div>
                    <div class="d-flex justify-content-end">
                      <button class="btn btn-sm btn-primary" @click="applyDateFilter">Áp dụng</button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Orders Table -->
        <div class="card orders-card">
          <div class="card-body p-0">
            <div class="table-responsive">
              <table class="table table-hover align-middle mb-0">
                <thead class="table-light">
                  <tr>
                    <th>Mã đơn hàng</th>
                    <th>Khách hàng</th>
                    <th>Tổng tiền</th>
                    <th>Trạng thái</th>
                    <th>Ngày đặt</th>
                    <th class="text-end">Thao tác</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="order in filteredOrders" :key="order.orderId">
                    <td class="fw-medium">{{ order.orderId }}</td>
                    <td>{{ order.customerName }}</td>
                    <td class="total-amount">{{ formatCurrency(order.total) }}</td>
                    <td>
                      <span :class="['badge', getStatusBadgeClass(order.status)]">
                        {{ order.status }}
                      </span>
                    </td>
                    <td class="date-text">{{ formatDate(order.createdAt) }}</td>
                    <td class="text-end">
                      <button 
                        class="btn btn-sm btn-outline-primary me-1" 
                        data-bs-toggle="tooltip" 
                        title="Xem chi tiết"
                        @click="viewOrderDetails(order)"
                      >
                        <i class="bi bi-eye"></i>
                      </button>
                      <button 
                        class="btn btn-sm btn-outline-success" 
                        data-bs-toggle="tooltip" 
                        title="Cập nhật trạng thái"
                        @click="updateOrderStatus(order)"
                      >
                        <i class="bi bi-pencil"></i>
                      </button>
                    </td>
                  </tr>
                  <tr v-if="loading">
                    <td colspan="6" class="text-center py-4">
                      <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Đang tải...</span>
                      </div>
                    </td>
                  </tr>
                  <tr v-if="!loading && filteredOrders.length === 0">
                    <td colspan="6" class="text-center py-4">
                      Không tìm thấy đơn hàng nào
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <!-- Order Details Modal -->
        <div class="modal fade" id="orderDetailsModal" tabindex="-1" ref="orderDetailsModal">
          <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
              <div class="modal-header bg-primary text-white">
                <h5 class="modal-title">Chi tiết đơn hàng #{{ selectedOrder?.orderId }}</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body" v-if="selectedOrder">
                <div class="row g-4">
                  <div class="col-12 col-md-6">
                    <div class="card h-100 border-0 bg-light">
                      <div class="card-body">
                        <h5 class="card-title text-primary mb-3">
                          <i class="bi bi-person me-2"></i>Thông tin khách hàng
                        </h5>
                        <ul class="list-group list-group-flush bg-transparent">
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Tên:</strong> {{ selectedOrder.customerName }}
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Email:</strong> {{ selectedOrder.customerEmail }}
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Số điện thoại:</strong> {{ selectedOrder.customerPhone }}
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Địa chỉ:</strong> {{ selectedOrder.shippingAddress }}
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>
                  <div class="col-12 col-md-6">
                    <div class="card h-100 border-0 bg-light">
                      <div class="card-body">
                        <h5 class="card-title text-primary mb-3">
                          <i class="bi bi-box-seam me-2"></i>Thông tin đơn hàng
                        </h5>
                        <ul class="list-group list-group-flush bg-transparent">
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Ngày đặt:</strong> {{ formatDate(selectedOrder.createdAt) }}
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Trạng thái:</strong> 
                            <span :class="['badge', getStatusBadgeClass(selectedOrder.status)]">
                              {{ selectedOrder.status }}
                            </span>
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Tổng tiền:</strong> 
                            <span class="total-amount">{{ formatCurrency(selectedOrder.total) }}</span>
                          </li>
                          <li class="list-group-item bg-transparent px-0">
                            <strong>Phương thức thanh toán:</strong> {{ selectedOrder.paymentMethod }}
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>
                  <div class="col-12">
                    <h5 class="text-primary mb-3">
                      <i class="bi bi-cart3 me-2"></i>Sản phẩm
                    </h5>
                    <div class="table-responsive">
                      <table class="table table-striped">
                        <thead class="table-light">
                          <tr>
                            <th>Sản phẩm</th>
                            <th>Giá</th>
                            <th>Số lượng</th>
                            <th>Tổng</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="(product, index) in selectedOrder.products" :key="index">
                            <td>{{ product.name }}</td>
                            <td>{{ formatCurrency(product.price) }}</td>
                            <td>{{ product.quantity }}</td>
                            <td class="total-amount">{{ formatCurrency(product.price * product.quantity) }}</td>
                          </tr>
                        </tbody>
                        <tfoot class="table-light">
                          <tr>
                            <td colspan="3" class="text-end fw-bold">Tổng cộng:</td>
                            <td class="total-amount fw-bold">{{ formatCurrency(selectedOrder.total) }}</td>
                          </tr>
                        </tfoot>
                      </table>
                    </div>
                  </div>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
              </div>
            </div>
          </div>
        </div>

        <!-- Update Status Modal -->
        <div class="modal fade" id="updateStatusModal" tabindex="-1" ref="updateStatusModal">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header bg-success text-white">
                <h5 class="modal-title">Cập nhật trạng thái đơn hàng</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body">
                <div class="mb-3">
                  <label for="newStatus" class="form-label">Trạng thái mới</label>
                  <select class="form-select" id="newStatus" v-model="newStatus">
                    <option v-for="status in orderStatuses" :key="status" :value="status">
                      {{ status }}
                    </option>
                  </select>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                <button type="button" class="btn btn-success" @click="confirmStatusUpdate">Cập nhật</button>
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
  name: 'AdminOrders',
  data() {
    return {
      search: '',
      statusFilter: '',
      datePickerVisible: false,
      dateRange: ['', ''],
      loading: false,
      selectedOrder: null,
      newStatus: '',
      orderDetailsModal: null,
      updateStatusModal: null,
      orderStatuses: [
        'Chờ xác nhận',
        'Đã xác nhận',
        'Đang xử lý',
        'Đang giao hàng',
        'Đã giao hàng',
        'Đã hủy'
      ],
      orders: [
        {
          orderId: 'ORD001',
          customerName: 'Nguyễn Văn A',
          customerEmail: 'nguyenvana@example.com',
          customerPhone: '0123456789',
          shippingAddress: '123 Đường ABC, Quận 1, TP.HCM',
          total: 1500000,
          status: 'Chờ xác nhận',
          createdAt: '2024-03-20',
          paymentMethod: 'Thanh toán khi nhận hàng',
          products: [
            {
              name: 'Giày Nike Air Max',
              price: 1000000,
              quantity: 1
            },
            {
              name: 'Tất thể thao',
              price: 500000,
              quantity: 1
            }
          ]
        },
        {
          orderId: 'ORD002',
          customerName: 'Trần Thị B',
          customerEmail: 'tranthib@example.com',
          customerPhone: '0987654321',
          shippingAddress: '456 Đường XYZ, Quận 3, TP.HCM',
          total: 2500000,
          status: 'Đã xác nhận',
          createdAt: '2024-03-19',
          paymentMethod: 'Chuyển khoản',
          products: [
            {
              name: 'Giày Adidas Ultra Boost',
              price: 2000000,
              quantity: 1
            },
            {
              name: 'Băng đeo cổ chân',
              price: 500000,
              quantity: 1
            }
          ]
        },
        {
          orderId: 'ORD003',
          customerName: 'Lê Văn C',
          customerEmail: 'levanc@example.com',
          customerPhone: '0369852147',
          shippingAddress: '789 Đường DEF, Quận 7, TP.HCM',
          total: 3500000,
          status: 'Đang xử lý',
          createdAt: '2024-03-18',
          paymentMethod: 'Ví điện tử',
          products: [
            {
              name: 'Giày Puma RS-X',
              price: 2500000,
              quantity: 1
            },
            {
              name: 'Băng quấn cổ chân',
              price: 1000000,
              quantity: 1
            }
          ]
        },
        {
          orderId: 'ORD004',
          customerName: 'Phạm Thị D',
          customerEmail: 'phamthid@example.com',
          customerPhone: '0147852369',
          shippingAddress: '321 Đường GHI, Quận 10, TP.HCM',
          total: 4500000,
          status: 'Đang giao hàng',
          createdAt: '2024-03-17',
          paymentMethod: 'Thanh toán khi nhận hàng',
          products: [
            {
              name: 'Giày New Balance 574',
              price: 3000000,
              quantity: 1
            },
            {
              name: 'Tất thể thao cao cấp',
              price: 1500000,
              quantity: 1
            }
          ]
        },
        {
          orderId: 'ORD005',
          customerName: 'Hoàng Văn E',
          customerEmail: 'hoangvane@example.com',
          customerPhone: '0258963147',
          shippingAddress: '654 Đường JKL, Quận 5, TP.HCM',
          total: 5500000,
          status: 'Đã giao hàng',
          createdAt: '2024-03-16',
          paymentMethod: 'Chuyển khoản',
          products: [
            {
              name: 'Giày Jordan 1',
              price: 4000000,
              quantity: 1
            },
            {
              name: 'Băng đeo cổ chân chuyên nghiệp',
              price: 1500000,
              quantity: 1
            }
          ]
        },
        {
          orderId: 'ORD006',
          customerName: 'Mai Thị F',
          customerEmail: 'maithif@example.com',
          customerPhone: '0369852147',
          shippingAddress: '987 Đường MNO, Quận 2, TP.HCM',
          total: 2000000,
          status: 'Đã hủy',
          createdAt: '2024-03-15',
          paymentMethod: 'Ví điện tử',
          products: [
            {
              name: 'Giày Converse Chuck 70',
              price: 2000000,
              quantity: 1
            }
          ]
        }
      ]
    }
  },
  computed: {
    dateRangeText() {
      if (!this.dateRange[0] || !this.dateRange[1]) return '';
      return `${this.formatDate(this.dateRange[0])} - ${this.formatDate(this.dateRange[1])}`;
    },
    filteredOrders() {
      let result = [...this.orders];
      
      // Apply status filter
      if (this.statusFilter) {
        result = result.filter(order => order.status === this.statusFilter);
      }
      
      // Apply date range filter
      if (this.dateRange[0] && this.dateRange[1]) {
        const startDate = new Date(this.dateRange[0]);
        const endDate = new Date(this.dateRange[1]);
        endDate.setHours(23, 59, 59, 999); // End of day
        
        result = result.filter(order => {
          const orderDate = new Date(order.createdAt);
          return orderDate >= startDate && orderDate <= endDate;
        });
      }
      
      // Apply search filter
      if (this.search) {
        const searchLower = this.search.toLowerCase();
        result = result.filter(order => 
          order.orderId.toLowerCase().includes(searchLower) ||
          order.customerName.toLowerCase().includes(searchLower) ||
          order.customerEmail.toLowerCase().includes(searchLower)
        );
      }
      
      return result;
    }
  },
  methods: {
    formatCurrency(value) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(value);
    },
    formatDate(date) {
      if (!date) return '';
      return new Date(date).toLocaleDateString('vi-VN');
    },
    getStatusBadgeClass(status) {
      const classes = {
        'Chờ xác nhận': 'bg-warning text-dark',
        'Đã xác nhận': 'bg-info',
        'Đang xử lý': 'bg-primary',
        'Đang giao hàng': 'bg-info',
        'Đã giao hàng': 'bg-success',
        'Đã hủy': 'bg-danger'
      };
      return classes[status] || 'bg-secondary';
    },
    toggleDatePicker() {
      this.datePickerVisible = !this.datePickerVisible;
    },
    applyDateFilter() {
      this.datePickerVisible = false;
    },
    viewOrderDetails(order) {
      this.selectedOrder = order;
      this.orderDetailsModal.show();
    },
    updateOrderStatus(order) {
      this.selectedOrder = order;
      this.newStatus = order.status;
      this.updateStatusModal.show();
    },
    async confirmStatusUpdate() {
      try {
        // TODO: Implement API call to update order status
        this.selectedOrder.status = this.newStatus;
        this.updateStatusModal.hide();
        
        // Show success message with Bootstrap toast
        this.showToast('Cập nhật trạng thái đơn hàng thành công', 'success');
      } catch (error) {
        // Show error message
        this.showToast('Có lỗi xảy ra khi cập nhật trạng thái', 'danger');
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
    async fetchOrders() {
      this.loading = true;
      try {
        // TODO: Implement API call to fetch orders
        // Using mock data for demonstration
        await new Promise(resolve => setTimeout(resolve, 500)); // Simulate API delay
        // Orders are already set in data()
      } catch (error) {
        this.showToast('Có lỗi xảy ra khi tải danh sách đơn hàng', 'danger');
      } finally {
        this.loading = false;
      }
    },
    initBootstrapComponents() {
      // Initialize tooltips
      const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
      tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
      });
      
      // Initialize modals
      this.orderDetailsModal = new bootstrap.Modal(this.$refs.orderDetailsModal);
      this.updateStatusModal = new bootstrap.Modal(this.$refs.updateStatusModal);
    }
  },
  mounted() {
    this.initBootstrapComponents();
  },
  created() {
    this.fetchOrders();
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

.orders-card {
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

.total-amount {
  font-weight: 600;
  color: #0d6efd;
}

.date-text {
  color: #6c757d;
  font-size: 0.9rem;
}

.date-picker-dropdown {
  position: absolute;
  z-index: 1000;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
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

.btn-outline-primary, .btn-outline-success {
  transition: all 0.2s ease;
}

.btn-outline-primary:hover, .btn-outline-success:hover {
  transform: translateY(-2px);
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
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
}
</style>