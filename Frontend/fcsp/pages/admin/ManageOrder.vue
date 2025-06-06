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
                        <th>Shipping Status</th>
                        <th>Order Date</th>
                        <th class="text-end">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="order in paginatedOrders" :key="order.id">
                        <td class="fw-medium">{{ order.id }}</td>
                        <td>{{ order.userName }}</td>
                        <td class="total-amount">{{ formatCurrency(order.totalPrice) }}</td>
                        <td>
                          <span :class="['badge', getStatusBadgeClass(order.statusName)]">
                            {{ getStatusText(order.statusName) }}
                          </span>
                        </td>
                        <td>{{ order.shippingStatusName }}</td>
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
                <div class="card-footer bg-white">
                  <div class="d-flex flex-column align-items-center">
                    <nav aria-label="Page navigation">
                      <ul class="pagination mb-0">
                        <li class="page-item" :class="{ disabled: currentPage === 1 }">
                          <a class="page-link" href="#" @click.prevent="currentPage = 1">
                            <i class="bi bi-chevron-double-left"></i>
                          </a>
                        </li>
                        <li class="page-item" :class="{ disabled: currentPage === 1 }">
                          <a class="page-link" href="#" @click.prevent="currentPage--">
                            <i class="bi bi-chevron-left"></i>
                          </a>
                        </li>
                        <li v-for="page in displayedPages" :key="page" class="page-item" :class="{ active: currentPage === page }">
                          <a v-if="page !== '...'" class="page-link" href="#" @click.prevent="currentPage = page">{{ page }}</a>
                          <span v-else class="page-link">...</span>
                        </li>
                        <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                          <a class="page-link" href="#" @click.prevent="currentPage++">
                            <i class="bi bi-chevron-right"></i>
                          </a>
                        </li>
                        <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                          <a class="page-link" href="#" @click.prevent="currentPage = totalPages">
                            <i class="bi bi-chevron-double-right"></i>
                          </a>
                        </li>
                      </ul>
                    </nav>
                  </div>
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
                                <strong>Name:</strong> {{ selectedOrder.userName }}
                              </li>
                            </ul>
                          </div>
                        </div>
                      </div>
                      <div class="col-12 col-md-6">
                        <div class="card h-100 border-0 bg-light">
                          <div class="card-body">
                            <h5 class="card-title text-primary mb-3">
                              <i class="bi bi-truck me-2"></i>Shipping Information
                            </h5>
                            <ul class="list-group list-group-flush bg-transparent" v-if="getShippingInfo(selectedOrder.shippingInfoId)">
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Phone:</strong> {{ getShippingInfo(selectedOrder.shippingInfoId).phoneNumber }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Address:</strong> {{ getShippingInfo(selectedOrder.shippingInfoId).address }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>City:</strong> {{ getShippingInfo(selectedOrder.shippingInfoId).city }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>District:</strong> {{ getShippingInfo(selectedOrder.shippingInfoId).district }}
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Ward:</strong> {{ getShippingInfo(selectedOrder.shippingInfoId).ward }}
                              </li>
                            </ul>
                            <div v-else class="text-muted">
                              No shipping information available
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="col-12">
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
                                <span :class="['badge', getStatusBadgeClass(selectedOrder.statusName)]">
                                  {{ getStatusText(selectedOrder.statusName) }}
                                </span>
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Total:</strong> 
                                <span class="total-amount">{{ formatCurrency(selectedOrder.totalPrice) }}</span>
                              </li>
                              <li class="list-group-item bg-transparent px-0">
                                <strong>Payment Method:</strong> {{ getPaymentMethodText(selectedOrder.paymentMethodName) }}
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
                                <th class="text-center">Image</th>
                                <th>Name</th>
                                <th class="text-end">Unit Price</th>
                                <th class="text-center">Quantity</th>
                                <th class="text-center">Size</th>
                                <th class="text-end">Total</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr v-for="(product, index) in selectedOrder.orderDetails" :key="index">
                                <td class="text-center align-middle">
                                  <img v-if="product.firstPreviewImageUrl" :src="product.firstPreviewImageUrl" style="max-width:50px;max-height:50px; border-radius:4px;">
                                  <span v-else class="text-muted">No image</span>
                                </td>
                                <td class="align-middle">{{ product.customShoeDesignName }}</td>
                                <td class="text-end align-middle">{{ formatCurrency(product.unitPrice) }}</td>
                                <td class="text-center align-middle">{{ product.quantity }}</td>
                                <td class="text-center align-middle">{{ product.sizeValue }}</td>
                                <td class="text-end align-middle total-amount">{{ formatCurrency(product.unitPrice * product.quantity) }}</td>
                              </tr>
                            </tbody>
                            <!-- <tfoot class="table-light">
                              <tr>
                                <td colspan="6" class="text-end fw-bold">Total:</td>
                                <td class="total-amount fw-bold">{{ formatCurrency(selectedOrder.totalPrice) }}</td>
                              </tr>
                            </tfoot> -->
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
                        <option v-for="status in findValidStatuses(newStatus)" :key="status" :value="status">
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

            <!-- Order Item Details Modal -->
            <div class="modal fade" id="orderItemModal" tabindex="-1" ref="orderItemModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Chi tiết sản phẩm</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedOrderItem">
                    <div class="card border-0 bg-light mb-3">
                      <div class="card-body">
                        <h6 class="text-primary mb-3">Product Information (Full Detail)</h6>
                        <ul class="list-group list-group-flush bg-transparent">
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>customShoeDesignDescription:</strong>
                            <span>{{ selectedOrderItem.customShoeDesignDescription }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>customShoeDesignId:</strong>
                            <span>{{ selectedOrderItem.customShoeDesignId }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>customShoeDesignName:</strong>
                            <span>{{ selectedOrderItem.customShoeDesignName }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>designerMarkup:</strong>
                            <span>{{ selectedOrderItem.designerMarkup }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>firstPreviewImageUrl:</strong>
                            <span>
                              <img v-if="selectedOrderItem.firstPreviewImageUrl" :src="selectedOrderItem.firstPreviewImageUrl" alt="Preview" style="max-width:80px;max-height:80px;">
                              <span v-else class="text-muted">No image</span>
                            </span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>previewImageUrls:</strong>
                            <span>{{ selectedOrderItem.previewImageUrls }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>quantity:</strong>
                            <span>{{ selectedOrderItem.quantity }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>servicePrice:</strong>
                            <span>{{ formatCurrency(selectedOrderItem.servicePrice) }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>sizeValue:</strong>
                            <span>{{ selectedOrderItem.sizeValue }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>templatePrice:</strong>
                            <span>{{ formatCurrency(selectedOrderItem.templatePrice) }}</span>
                          </li>
                          <li class="list-group-item bg-transparent d-flex justify-content-between">
                            <strong>unitPrice:</strong>
                            <span>{{ formatCurrency(selectedOrderItem.unitPrice) }}</span>
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
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
import { getAllOrders, getOrderById, putOrderStatus } from '@/server/order-service';
import { getAllShippingInfo } from '@/server/shipping-service';
import AdminSidebar from '@/components/AdminSidebar.vue';
import { onMounted, ref } from 'vue';

export default {
  name: 'AdminOrders',
  components: {
    AdminSidebar
  },
  setup() {
    const orderItemModal = ref(null);
    const orderDetailsModal = ref(null);
    const updateStatusModal = ref(null);
    const modalRefs = ref({
      orderItemModal: null,
      orderDetailsModal: null,
      updateStatusModal: null
    });

    onMounted(async () => {
      if (process.client) {
        const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js');
        modalRefs.value = {
          orderItemModal: new bootstrap.Modal(document.getElementById('orderItemModal')),
          orderDetailsModal: new bootstrap.Modal(document.getElementById('orderDetailsModal')),
          updateStatusModal: new bootstrap.Modal(document.getElementById('updateStatusModal'))
        };
      }
    });

    return {
      orderItemModal,
      orderDetailsModal,
      updateStatusModal,
      modalRefs
    };
  },
  data() {
    return {
      search: '',
      statusFilter: '',
      datePickerVisible: false,
      dateRange: ['', ''],
      loading: false,
      selectedOrder: null,
      selectedOrderItem: null,
      newStatus: '',
      currentPage: 1,
      itemsPerPage: 7,
      orderStatuses: [
        'Pending',
        'Confirmed',
        'Processing',
        'Completed',
        'Cancelled',
        'Refunded'
      ],
      orders: [],
      shippingInfos: [],
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
        result = result.filter(order => order.statusName === this.statusFilter);
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
          order.userName.toString().includes(searchLower)
        );
      }
      
      return result;
    },
    paginatedOrders() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredOrders.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.filteredOrders.length / this.itemsPerPage);
    },
    displayedPages() {
      const pages = [];
      const maxDisplayedPages = 5;
      
      if (this.totalPages <= maxDisplayedPages) {
        // Nếu tổng số trang ít hơn hoặc bằng maxDisplayedPages, hiển thị tất cả
        for (let i = 1; i <= this.totalPages; i++) {
          pages.push(i);
        }
      } else {
        // Luôn hiển thị trang đầu tiên
        pages.push(1);
        
        // Tính toán các trang ở giữa
        let startPage = Math.max(2, this.currentPage - 1);
        let endPage = Math.min(this.totalPages - 1, this.currentPage + 1);
        
        // Điều chỉnh để luôn hiển thị 3 trang ở giữa
        if (startPage > 2) {
          pages.push('...');
        }
        
        for (let i = startPage; i <= endPage; i++) {
          pages.push(i);
        }
        
        // Thêm dấu ... nếu cần
        if (endPage < this.totalPages - 1) {
          pages.push('...');
        }
        
        // Luôn hiển thị trang cuối cùng
        pages.push(this.totalPages);
      }
      
      return pages;
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
      return status || 'Unknown';
    },
    getStatusBadgeClass(status) {
      const classes = {
        'Pending': 'bg-warning text-dark',
        'Confirmed': 'bg-info',
        'Processing': 'bg-primary',
        'Completed': 'bg-success',
        'Cancelled': 'bg-danger',
        'Refunded': 'bg-secondary'
      };
      return classes[status] || 'bg-secondary';
    },
    getPaymentMethodText(method) {
      return method || 'Unknown';
    },
    toggleDatePicker() {
      this.datePickerVisible = !this.datePickerVisible;
    },
    applyDateFilter() {
      this.datePickerVisible = false;
    },
    findValidStatuses(status) {
      const index = this.orderStatuses.findIndex(item => item === status);
      return this.orderStatuses.slice(index);
    },
    async fetchShippingInfos() {
      try {
        const response = await getAllShippingInfo();
     
        
        // Check if response has the expected structure
        if (response && Array.isArray(response.shippingInfos)) {
          this.shippingInfos = response.shippingInfos;
        } else if (Array.isArray(response)) {
          this.shippingInfos = response;
        } else {
          console.error('Unexpected shipping info response structure:', response);
          this.shippingInfos = [];
        }
        
        
      } catch (error) {
        console.error('Error fetching shipping info:', error);
        this.showMessage('Có lỗi xảy ra khi tải thông tin giao hàng', 'danger');
        this.shippingInfos = [];
      }
    },
    getShippingInfo(shippingInfoId) {
      return this.shippingInfos.find(info => info.id === shippingInfoId) || null;
    },
    async fetchOrders() {
      this.loading = true;
      try {
        const response = await getAllOrders();
        console.log('API Response:', response);
        
        if (response && Array.isArray(response)) {
          this.orders = response.map(order => ({
            id: order.id,
            userName: order.userName,
            shippingInfoId: order.shippingInfoId,
            voucherCode: order.voucherCode,
            totalPrice: order.totalPrice,
            statusName: order.status,
            shippingStatusName: order.shippingStatus,
            paymentMethodName: order.paymentMethod,
            createdAt: order.createdAt,
            updatedAt: order.updatedAt,
            orderDetails: Array.isArray(order.orderDetail) ? order.orderDetail : [order.orderDetail]
          }));
         
          
          // Fetch shipping information after orders are loaded
          await this.fetchShippingInfos();
        } else {
          throw new Error('Invalid response format from server');
        }
      } catch (error) {
        console.error('Error fetching orders:', error);
        this.showMessage('Có lỗi xảy ra khi tải danh sách đơn hàng: ' + error.message, 'danger');
      } finally {
        this.loading = false;
      }
    },
    viewOrderDetails(order) {
      this.selectedOrder = order;
      if (this.modalRefs.orderDetailsModal) {
        this.modalRefs.orderDetailsModal.show();
      }
    },
    updateOrderStatus(order) {
      this.selectedOrder = order;
      this.newStatus = order.statusName;
      if (this.modalRefs.updateStatusModal) {
        this.modalRefs.updateStatusModal.show();
      }
    },
    async confirmStatusUpdate() {
      try {
        const statusMapping = {
          'Pending': 0,
          'Confirmed': 1,
          'Processing': 2,
          'Completed': 3,
          'Cancelled': 4,
          'Refunded': 5
        };

        const newStatusId = statusMapping[this.newStatus];
        if (newStatusId === undefined) {
          throw new Error('Invalid status selected');
        }

        console.log(this.selectedOrder.id, newStatusId);
        const response = await putOrderStatus(this.selectedOrder.id, newStatusId);
        console.log(response);

        if (response && response.code === 200) {
          // Update the status in the local orders array
          const orderIndex = this.orders.findIndex(order => order.id === this.selectedOrder.id);
          if (orderIndex !== -1) {
            this.orders[orderIndex].statusName = this.newStatus;
          }

          // Show success message
          this.showMessage('Cập nhật trạng thái đơn hàng thành công', 'success');
          
          // Close the modal
          if (this.modalRefs.updateStatusModal) {
            this.modalRefs.updateStatusModal.hide();
          }
        } else {
           throw new Error(response?.message || 'Failed to update order status');
        }

      } catch (error) {
        // Show error message
        console.error('Error updating order status:', error.response?.data || error.message || error);
        this.showMessage('Có lỗi xảy ra khi cập nhật trạng thái', 'danger');
      }
    },
    showMessage(message, type = 'success') {
      // Implement your own message display logic here

    },
    viewOrderItemDetails(item) {
      this.selectedOrderItem = item;
      if (this.modalRefs.orderItemModal) {
        this.modalRefs.orderItemModal.show();
      }
    }
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
  background-color: white;
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

.table th, .table td {
  vertical-align: middle;
  padding: 0.75rem 1rem;
  font-size: 1rem;
}

.table .total-amount {
  font-weight: 600;
  color: #777777;
  font-size: 1.1rem;
  text-align: right;
}

.table tfoot .total-amount,
.table tfoot td.total-amount {
  font-weight: 700;
  color: #444;
  font-size: 1.15rem;
  text-align: right;
  background: #f8f9fa;
  border-top: 2px solid #e9ecef;
}

.table tfoot td {
  background: #f8f9fa;
  border-top: 2px solid #e9ecef;
  font-size: 1.1rem;
  font-weight: 600;
  color: #444;
  text-align: right;
}

.table tbody tr:last-child td {
  border-bottom: 2px solid #e9ecef;
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
  border-bottom: none;
}

.modal-footer {
  border-top: none;
}

.card-title {
  font-weight: 600;
  font-size: 1.1rem;
}

.list-group-item {
  border-color: rgba(0, 0, 0, 0.05);
  padding: 0.75rem 0;
}

.card {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
}

.modal .card {
  box-shadow: none;
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

.pagination {
  margin-bottom: 0;
  justify-content: center;
}

.card-footer {
  padding: 1rem;
}

.card-footer .d-flex {
  width: 100%;
}

.page-link {
  padding: 0.375rem 0.75rem;
  color: #6c757d;
  background-color: #fff;
  border: 1px solid #dee2e6;
}

.page-item.active .page-link {
  background-color: #0d6efd;
  border-color: #0d6efd;
  color: #fff;
}

.page-item.disabled .page-link {
  color: #6c757d;
  pointer-events: none;
  background-color: #fff;
  border-color: #dee2e6;
}

.page-link:hover {
  z-index: 2;
  color: #0d6efd;
  background-color: #e9ecef;
  border-color: #dee2e6;
}
</style>