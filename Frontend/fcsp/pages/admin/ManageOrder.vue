<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="container-fluid py-4">
        <div class="row">
          <div class="col-12">
            <h1 class="mb-4 text-primary fw-bold">Order Management</h1>
            
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
                        placeholder="Search orders"
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
                        <option value="">All Status</option>
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
                        placeholder="Date Range"
                        readonly
                        @click="toggleDatePicker"
                      >
                    </div>
                    <div v-if="datePickerVisible" class="date-picker-dropdown card mt-1">
                      <div class="card-body p-2">
                        <div class="d-flex gap-2 mb-2">
                          <input type="date" class="form-control" v-model="dateRange[0]">
                          <input type="date" class="form-control" v-model="dateRange[1]">
                        </div>
                        <div class="d-flex justify-content-end">
                          <button class="btn btn-sm btn-primary" @click="applyDateFilter">Apply</button>
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
                        <th>Order ID</th>
                        <th>Customer</th>
                        <th>Total</th>
                        <th>Status</th>
                        <th>Order Date</th>
                        <th class="text-end">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="order in filteredOrders" :key="order.id">
                        <td class="fw-medium">{{ order.id }}</td>
                        <td>{{ order.userId }}</td>
                        <td class="total-amount">{{ formatCurrency(order.totalPrice) }}</td>
                        <td>
                          <span :class="['badge', getStatusBadgeClass(order.status)]">
                            {{ getStatusText(order.status) }}
                          </span>
                        </td>
                        <td class="date-text">{{ formatDate(order.createdAt) }}</td>
                        <td class="text-end">
                          <button 
                            class="btn btn-sm btn-outline-primary me-1" 
                            data-bs-toggle="tooltip" 
                            title="View Details"
                            @click="viewOrderDetails(order)"
                          >
                            <i class="bi bi-eye"></i>
                          </button>
                          <button 
                            class="btn btn-sm btn-outline-success" 
                            data-bs-toggle="tooltip" 
                            title="Update Status"
                            @click="updateOrderStatus(order)"
                          >
                            <i class="bi bi-pencil"></i>
                          </button>
                        </td>
                      </tr>
                      <tr v-if="loading">
                        <td colspan="6" class="text-center py-4">
                          <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Loading...</span>
                          </div>
                        </td>
                      </tr>
                      <tr v-if="!loading && filteredOrders.length === 0">
                        <td colspan="6" class="text-center py-4">
                          No orders found
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
                    <h5 class="modal-title">Order Details #{{ selectedOrder?.id }}</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedOrder">
                    <div class="row g-4">
                      <div class="col-12 col-md-6">
                        <div class="card h-100 border-0 bg-light">
                          <div class="card-body">
                            <h5 class="card-title text-primary mb-3">
                              <i class="bi bi-person me-2"></i>Customer Information
                            </h5>
                            <ul class="list-group list-group-flush bg-transparent">
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Name:</strong> {{ selectedOrder.userId }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Email:</strong> {{ selectedOrder.userId }}@example.com
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Phone:</strong> {{ selectedOrder.userId }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Address:</strong> {{ selectedOrder.userId }}
                              </li>
                            </ul>
                          </div>
                        </div>
                      </div>
                      <div class="col-12 col-md-6">
                        <div class="card h-100 border-0 bg-light">
                          <div class="card-body">
                            <h5 class="card-title text-primary mb-3">
                              <i class="bi bi-box-seam me-2"></i>Order Information
                            </h5>
                            <ul class="list-group list-group-flush bg-transparent">
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Order Date:</strong> {{ formatDate(selectedOrder.createdAt) }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Status:</strong> 
                                <span :class="['badge', getStatusBadgeClass(selectedOrder.status)]">
                                  {{ getStatusText(selectedOrder.status) }}
                                </span>
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Total:</strong> 
                                <span class="total-amount">{{ formatCurrency(selectedOrder.totalPrice) }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Payment Method:</strong> {{ getPaymentMethodText(selectedOrder.paymentMethod) }}
                              </li>
                            </ul>
                          </div>
                        </div>
                      </div>
                      <div class="col-12">
                        <h5 class="text-primary mb-3">
                          <i class="bi bi-cart3 me-2"></i>Products
                        </h5>
                        <div class="table-responsive">
                          <table class="table table-striped">
                            <thead class="table-light">
                              <tr>
                                <th>Product</th>
                                <th>Price</th>
                                <th>Quantity</th>
                                <th>Total</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr v-for="(product, index) in selectedOrder.orderDetails" :key="index">
                                <td>{{ product.name }}</td>
                                <td>{{ formatCurrency(product.price) }}</td>
                                <td>{{ product.quantity }}</td>
                                <td class="total-amount">{{ formatCurrency(product.price * product.quantity) }}</td>
                              </tr>
                            </tbody>
                            <tfoot class="table-light">
                              <tr>
                                <td colspan="3" class="text-end fw-bold">Total:</td>
                                <td class="total-amount fw-bold">{{ formatCurrency(selectedOrder.totalPrice) }}</td>
                              </tr>
                            </tfoot>
                          </table>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                  </div>
                </div>
              </div>
            </div>

            <!-- Update Status Modal -->
            <div class="modal fade" id="updateStatusModal" tabindex="-1" ref="updateStatusModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header bg-success text-white">
                    <h5 class="modal-title">Update Order Status</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <div class="mb-3">
                      <label for="newStatus" class="form-label">New Status</label>
                      <select class="form-select" id="newStatus" v-model="newStatus">
                        <option v-for="status in orderStatuses" :key="status" :value="status">
                          {{ status }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" @click="confirmStatusUpdate">Update</button>
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
import { getAllOrders, getOrderById } from '@/server/order-service';
import AdminSidebar from '@/components/AdminSidebar.vue';

export default {
  name: 'AdminOrders',
  components: {
    AdminSidebar
  },
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
        'Pending',
        'Confirmed',
        'Processing',
        'Shipping',
        'Delivered',
        'Cancelled'
      ],
      orders: []
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
        result = result.filter(order => this.getStatusText(order.status) === this.statusFilter);
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
          order.id.toString().includes(searchLower) ||
          order.userId.toString().includes(searchLower)
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
    getStatusText(status) {
      const statusMap = {
        0: 'Pending',
        1: 'Confirmed',
        2: 'Processing',
        3: 'Shipping',
        4: 'Delivered',
        5: 'Cancelled'
      };
      return statusMap[status] || 'Unknown';
    },
    getStatusBadgeClass(status) {
      const statusText = this.getStatusText(status);
      const classes = {
        'Pending': 'bg-warning text-dark',
        'Confirmed': 'bg-info',
        'Processing': 'bg-primary',
        'Shipping': 'bg-info',
        'Delivered': 'bg-success',
        'Cancelled': 'bg-danger'
      };
      return classes[statusText] || 'bg-secondary';
    },
    getPaymentMethodText(method) {
      const methodMap = {
        0: 'Cash on Delivery',
        1: 'Bank Transfer',
        2: 'E-Wallet'
      };
      return methodMap[method] || 'Unknown';
    },
    toggleDatePicker() {
      this.datePickerVisible = !this.datePickerVisible;
    },
    applyDateFilter() {
      this.datePickerVisible = false;
    },
    async fetchOrders() {
      this.loading = true;
      try {
        const response = await getAllOrders();
        if (response) {
          this.orders = Array.isArray(response) ? response : [response];
        }
      } catch (error) {
        this.showMessage('Có lỗi xảy ra khi tải danh sách đơn hàng: ' + error.message, 'danger');
      } finally {
        this.loading = false;
      }
    },
    async viewOrderDetails(order) {
      try {
        const detailedOrder = await getOrderById(order.id);
        this.selectedOrder = detailedOrder;
      } catch (error) {
        this.showMessage('Có lỗi xảy ra khi tải chi tiết đơn hàng: ' + error.message, 'danger');
      }
    },
    updateOrderStatus(order) {
      this.selectedOrder = order;
      this.newStatus = order.status;
    },
    async confirmStatusUpdate() {
      try {
        // TODO: Implement API call to update order status
        this.selectedOrder.status = this.newStatus;
        
        // Show success message
        this.showMessage('Cập nhật trạng thái đơn hàng thành công', 'success');
      } catch (error) {
        // Show error message
        this.showMessage('Có lỗi xảy ra khi cập nhật trạng thái', 'danger');
      }
    },
    showMessage(message, type = 'success') {
      // Implement your own message display logic here
      console.log(`${type}: ${message}`);
    }
  },
  mounted() {
  },
  async created() {
    await this.fetchOrders();
  }
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