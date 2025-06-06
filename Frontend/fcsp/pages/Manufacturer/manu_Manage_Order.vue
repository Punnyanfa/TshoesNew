<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="admin-layout">
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
                          <option value="">All Statuses</option>
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
                          placeholder="Date"
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
                          <th>Customer Name</th>
                          <th>Total Amount</th>
                          <th>Status</th>
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
                                  <strong>Name: </strong> {{ selectedOrder.userName }}
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
                                  <strong>Phone: </strong> {{ getShippingInfo(selectedOrder.shippingInfoId).phoneNumber }}
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>Address: </strong> {{ getShippingInfo(selectedOrder.shippingInfoId).address }}
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>City: </strong> {{ getShippingInfo(selectedOrder.shippingInfoId).city }}
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>District: </strong> {{ getShippingInfo(selectedOrder.shippingInfoId).district }}
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>Ward: </strong> {{ getShippingInfo(selectedOrder.shippingInfoId).ward }}
                                </li>
                              </ul>
                              <div v-else class="text-muted">
                                No shipping information found
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
                                  <strong>Order Date: </strong> {{ formatDate(selectedOrder.createdAt) }}
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>Status: </strong> 
                                  <span :class="['badge', getStatusBadgeClass(selectedOrder.statusName)]">
                                    {{ getStatusText(selectedOrder.statusName) }}
                                  </span>
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>Total Amount: </strong> 
                                  <span class="total-amount">{{ formatCurrency(selectedOrder.totalPrice) }}</span>
                                </li>
                                <li class="list-group-item bg-transparent px-0">
                                  <strong>Payment Method: </strong> {{ getPaymentMethodText(selectedOrder.paymentMethodName) }}
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
                                  <th>Image</th>
                                  <th>Product</th>
                                  <th>Price</th>
                                  <th>Quantity</th>
                                  <th>Size</th>
                                  <th>Total</th>
                                  <th>Actions</th>
                                </tr>
                              </thead>
                              <tbody>
                                <tr v-for="(product, index) in selectedOrder.orderDetails" :key="index">
                                  <td>
                                    <img
                                      v-if="product.firstPreviewImageUrl"
                                      :src="product.firstPreviewImageUrl"
                                      alt="Product Image"
                                      style="width: 50px; height: 50px; object-fit: cover; border-radius: 4px;"
                                      @error="() => (product.firstPreviewImageUrl = null)"
                                    />
                                    <span v-else class="text-muted">No image</span>
                                  </td>
                                  <td>{{ product.customShoeDesignName }}</td>
                                  <td>{{ formatCurrency(product.unitPrice) }}</td>
                                  <td>{{ product.quantity }}</td>
                                  <td>{{ product.sizeValue }}</td>
                                  <td class="total-amount">{{ formatCurrency(product.unitPrice * product.quantity) }}</td>
                                  <td>
                                    <button 
                                      class="btn btn-sm btn-outline-info me-1" 
                                      data-bs-toggle="tooltip" 
                                      title="Preview Images"
                                      @click="viewPreviewImages(product.customShoeDesignId)"
                                    >
                                      <i class="bi bi-images"></i>
                                    </button>
                                  </td>
                                </tr>
                              </tbody>
                              <tfoot class="table-light">
                                <tr>
                                  <td colspan="6" class="text-end fw-bold">Total Amount:</td>
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

              <!-- Order Item Details Modal -->
              <div class="modal fade" id="orderItemModal" tabindex="-1" ref="orderItemModal">
                <div class="modal-dialog">
                  <div class="modal-content">
                    <div class="modal-header bg-primary text-white">
                      <h5 class="modal-title">Product Details</h5>
                      <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body" v-if="selectedOrderItem">
                      <div class="card border-0 bg-light mb-3">
                        <div class="card-body">
                          <h6 class="text-primary mb-3">Product Information</h6>
                          <ul class="list-group list-group-flush bg-transparent">
                            <li class="list-group-item bg-transparent d-flex justify-content-between">
                              <strong>Product Name:</strong>
                              <span>{{ selectedOrderItem.customShoeDesignName }}</span>
                            </li>
                            <li class="list-group-item bg-transparent d-flex justify-content-between">
                              <strong>Size:</strong>
                              <span>{{ selectedOrderItem.sizeValue }}</span>
                            </li>
                            <li class="list-group-item bg-transparent d-flex justify-content-between">
                              <strong>Unit Price:</strong>
                              <span>{{ formatCurrency(selectedOrderItem.unitPrice) }}</span>
                            </li>
                            <li class="list-group-item bg-transparent d-flex justify-content-between">
                              <strong>Quantity:</strong>
                              <span>{{ selectedOrderItem.quantity }}</span>
                            </li>
                            <li class="list-group-item bg-transparent d-flex justify-content-between">
                              <strong>Total Amount:</strong>
                              <span class="text-primary fw-bold">
                                {{ formatCurrency(selectedOrderItem.unitPrice * selectedOrderItem.quantity) }}
                              </span>
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                    <div class="modal-footer">
                      <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Preview Images Modal -->
              <div class="modal fade" id="previewImagesModal" tabindex="-1" ref="previewImagesModal">
                <div class="modal-dialog modal-lg">
                  <div class="modal-content">
                    <div class="modal-header bg-info text-white">
                      <h5 class="modal-title">Preview Images</h5>
                      <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                      <div v-if="loadingPreviewImages" class="text-center">
                        <div class="spinner-border text-info" role="status">
                          <span class="visually-hidden">Loading...</span>
                        </div>
                      </div>
                      <div v-else-if="previewImages.length === 0" class="text-center text-muted">
                        No preview images available
                      </div>
                      <div v-else class="row">
                        <div v-for="(image, index) in previewImages" :key="image.id" class="col-md-4 mb-3">
                          <div class="card preview-card">
                            <img :src="image.previewImageUrl" class="card-img-top" alt="Preview Image" @error="handleImageError(index)">
                            <div class="card-body">
                              <p class="card-text text-muted small">
                                Created: {{ formatDate(image.createdAt) }}
                              </p>
                            </div>
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
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getAllShippingInfo } from '@/server/shipping-service';
import AdminSidebar from '@/components/AdminSidebar.vue';
import { onMounted, ref } from 'vue';

const getManufacturerByUserId = async (userId) => {
  try {
    const response = await fetch(`https://fcspwebapi20250527114117.azurewebsites.net/api/Manufacturer/user/${userId}`, {
      method: 'GET',
      headers: {
        'accept': '*/*',
        'Authorization': `Bearer ${localStorage.getItem('userToken')}`
      }
    });
    const result = await response.json();
    if (result.code === 200 && Array.isArray(result.data) && result.data.length > 0) {
      return result.data[0].id;
    } else {
      throw new Error('No manufacturer found for this user');
    }
  } catch (error) {
    console.error('Error fetching manufacturer ID:', error);
    throw error;
  }
};

const getOrdersByManufacturerId = async (manufacturerId) => {
  try {
    const response = await fetch(`https://fcspwebapi20250527114117.azurewebsites.net/api/Order/manufacturer/${manufacturerId}`, {
      method: 'GET',
      headers: {
        'accept': '*/*',
        'Authorization': `Bearer ${localStorage.getItem('userToken')}`
      }
    });
    const result = await response.json();
    if (result.code === 200 && result.data && Array.isArray(result.data.orders)) {
      return result.data.orders;
    } else {
      throw new Error('No orders found for this manufacturer');
    }
  } catch (error) {
    console.error('Error fetching orders by manufacturer ID:', error);
    throw error;
  }
};

const updateOrderStatusApi = async (orderId, statusCode) => {
  try {
    const response = await fetch(`https://fcspwebapi20250527114117.azurewebsites.net/api/Order/${orderId}`, {
      method: 'PUT',
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('userToken')}`
      },
      body: JSON.stringify({
        id: orderId,
        status: statusCode
      })
    });
    const result = await response.json();
    if (result.code === 200 && result.data.success) {
      return true;
    } else {
      throw new Error(result.message || 'Failed to update order status');
    }
  } catch (error) {
    console.error('Error updating order status:', error);
    throw error;
  }
};

const getPreviewImages = async (customShoeDesignId) => {
  try {
    const response = await fetch(`https://fcspwebapi20250527114117.azurewebsites.net/api/DesignPreview/design/${customShoeDesignId}`, {
      method: 'GET',
      headers: {
        'accept': '*/*',
        'Authorization': `Bearer ${localStorage.getItem('userToken')}`
      }
    });
    const result = await response.json();
    if (response.status === 200 && Array.isArray(result)) {
      return result;
    } else {
      throw new Error('Failed to fetch preview images');
    }
  } catch (error) {
    console.error('Error fetching preview images:', error);
    throw error;
  }
};

export default {
  name: 'ManufacturerOrders',
  components: {
    AdminSidebar
  },
  setup() {
    const orderItemModal = ref(null);
    const orderDetailsModal = ref(null);
    const updateStatusModal = ref(null);
    const previewImagesModal = ref(null);
    const modalRefs = ref({
      orderItemModal: null,
      orderDetailsModal: null,
      updateStatusModal: null,
      previewImagesModal: null
    });

    onMounted(async () => {
      if (typeof window !== 'undefined') {
        const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js');
        modalRefs.value = {
          orderItemModal: new bootstrap.Modal(document.getElementById('orderItemModal')),
          orderDetailsModal: new bootstrap.Modal(document.getElementById('orderDetailsModal')),
          updateStatusModal: new bootstrap.Modal(document.getElementById('updateStatusModal')),
          previewImagesModal: new bootstrap.Modal(document.getElementById('previewImagesModal'))
        };
      }
    });

    return {
      orderItemModal,
      orderDetailsModal,
      updateStatusModal,
      previewImagesModal,
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
      loadingPreviewImages: false,
      selectedOrder: null,
      selectedOrderItem: null,
      newStatus: '',
      previewImages: [],
      orderStatuses: [
        'Pending',
        'Confirmed',
        'Processing',
        'Shipping',
        'Delivered',
        'Cancelled'
      ],
      orders: [],
      shippingInfos: [],
      currentPage: 1,
      itemsPerPage: 7
    };
  },
  computed: {
    dateRangeText() {
      if (!this.dateRange[0] || !this.dateRange[1]) return '';
      return `${this.formatDate(this.dateRange[0])} - ${this.formatDate(this.dateRange[1])}`;
    },
    filteredOrders() {
      let result = [...this.orders];
      
      if (this.statusFilter) {
        result = result.filter(order => {
          const orderStatus = this.getStatusText(order.statusName);
          return orderStatus === this.statusFilter;
        });
      }
      
      if (this.dateRange[0] && this.dateRange[1]) {
        const startDate = new Date(this.dateRange[0]);
        const endDate = new Date(this.dateRange[1]);
        endDate.setHours(23, 59, 59, 999);
        
        result = result.filter(order => {
          const orderDate = new Date(order.createdAt);
          return orderDate >= startDate && orderDate <= endDate;
        });
      }
      
      if (this.search) {
        const searchLower = this.search.toLowerCase();
        result = result.filter(order => 
          order.id.toString().includes(searchLower) ||
          order.userName.toString().toLowerCase().includes(searchLower)
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
        for (let i = 1; i <= this.totalPages; i++) {
          pages.push(i);
        }
      } else {
        pages.push(1);
        
        let startPage = Math.max(2, this.currentPage - 1);
        let endPage = Math.min(this.totalPages - 1, this.currentPage + 1);
        
        if (startPage > 2) {
          pages.push('...');
        }
        
        for (let i = startPage; i <= endPage; i++) {
          pages.push(i);
        }
        
        if (endPage < this.totalPages - 1) {
          pages.push('...');
        }
        
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
      const statusMap = {
        'Pending': 'Pending',
        'Confirmed': 'Confirmed',
        'Processing': 'Processing',
        'Shipping': 'Shipping',
        'Delivered': 'Delivered',
        'Cancelled': 'Cancelled',
        'Completed': 'Completed'
      };
      return statusMap[status] || status || 'Unknown';
    },
    getStatusBadgeClass(status) {
      const classes = {
        'Pending': 'bg-warning text-dark',
        'Confirmed': 'bg-info',
        'Processing': 'bg-primary',
        'Shipping': 'bg-info',
        'Delivered': 'bg-success',
        'Completed': 'bg-success',
        'Cancelled': 'bg-danger'
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
    async fetchShippingInfos() {
      try {
        const response = await getAllShippingInfo();
        console.log('Shipping API Response:', response);
        
        if (response && Array.isArray(response.shippingInfos)) {
          this.shippingInfos = response.shippingInfos;
        } else if (Array.isArray(response)) {
          this.shippingInfos = response;
        } else {
          console.error('Unexpected shipping info response structure:', response);
          this.shippingInfos = [];
        }
        
        console.log('Processed shipping infos:', this.shippingInfos);
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
        const userId = localStorage.getItem('userId');
        if (!userId) {
          throw new Error('User ID not found in localStorage');
        }

        const manufacturerId = await getManufacturerByUserId(userId);
        console.log('Manufacturer ID:', manufacturerId);

        const ordersResponse = await getOrdersByManufacturerId(manufacturerId);
        console.log('Orders API Response:', ordersResponse);

        this.orders = ordersResponse.map(order => ({
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
          orderDetails: Array.isArray(order.orderDetail) ? order.orderDetail.map(detail => ({
            customShoeDesignName: detail.customShoeDesignName,
            customShoeDesignDescription: detail.customShoeDesignDescription,
            firstPreviewImageUrl: detail.firstPreviewImageUrl,
            quantity: detail.quantity,
            unitPrice: detail.unitPrice,
            sizeValue: detail.sizeValue,
            customShoeDesignId: detail.customShoeDesignId
          })) : [order.orderDetail]
        }));
        console.log('Transformed orders:', this.orders);

        await this.fetchShippingInfos();
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
      this.newStatus = this.getStatusText(order.statusName);
      if (this.modalRefs.updateStatusModal) {
        this.modalRefs.updateStatusModal.show();
      }
    },
    async confirmStatusUpdate() {
      try {
        const statusToCodeMap = {
          'Pending': 0,
          'Confirmed': 1,
          'Processing': 2,
          'Shipping': 3,
          'Delivered': 4,
          'Cancelled': 5
        };

        const statusCode = statusToCodeMap[this.newStatus];
        if (statusCode === undefined) {
          throw new Error('Invalid status');
        }

        await updateOrderStatusApi(this.selectedOrder.id, statusCode);

        const reverseStatusMap = {
          'Pending': 'Pending',
          'Confirmed': 'Confirmed',
          'Processing': 'Processing',
          'Shipping': 'Shipping',
          'Delivered': 'Delivered',
          'Cancelled': 'Cancelled',
          'Completed': 'Completed'
        };
        this.selectedOrder.statusName = reverseStatusMap[this.newStatus] || this.newStatus;

        if (this.modalRefs.updateStatusModal) {
          this.modalRefs.updateStatusModal.hide();
        }

        this.showMessage('Order status updated successfully', 'success');
      } catch (error) {
        this.showMessage('Error updating order status: ' + error.message, 'danger');
      }
    },
    async viewPreviewImages(customShoeDesignId) {
      this.loadingPreviewImages = true;
      this.previewImages = [];
      try {
        const images = await getPreviewImages(customShoeDesignId);
        this.previewImages = images;
        if (this.modalRefs.previewImagesModal) {
          this.modalRefs.previewImagesModal.show();
        }
      } catch (error) {
        this.showMessage('Error loading preview images: ' + error.message, 'danger');
      } finally {
        this.loadingPreviewImages = false;
      }
    },
    handleImageError(index) {
      this.previewImages[index].previewImageUrl = null;
    },
    showMessage(message, type = 'success') {
      console.log(`${type}: ${message}`);
    },
    viewOrderItemDetails(item) {
      this.selectedOrderItem = item;
      if (this.modalRefs.orderItemModal) {
        this.modalRefs.orderItemModal.show();
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
        window.location.href = '/loginPage';
      }
    }
  },
  async created() {
    await this.fetchOrders();
  }
};
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  flex: 1;
  padding: 2rem 0;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.search-card,
.orders-card {
  width: 100%;
  max-width: 1100px;
  margin: 0 auto 2rem auto;
  border-radius: 15px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  background: white;
}

.table {
  margin-bottom: 0;
}

.table th {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.8rem;
  letter-spacing: 0.5px;
  background-color: #f8f9fa;
  padding: 1rem;
  border-bottom: 2px solid #e9ecef;
}

.table td {
  vertical-align: middle;
  padding: 1rem;
  border-bottom: 1px solid #e9ecef;
  transition: background-color 0.2s ease;
}

.table tbody tr:hover {
  background-color: #f8f9fa;
}

.total-amount {
  font-weight: 600;
  color: #0d6efd;
  font-size: 1.1rem;
}

.date-text {
  color: #6c757d;
  font-size: 0.9rem;
}

.date-picker-dropdown {
  position: absolute;
  z-index: 1000;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
  border-radius: 10px;
  background: white;
  padding: 1rem;
}

.badge {
  padding: 0.6em 1em;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-size: 0.75rem;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.badge:hover {
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.modal-content {
  border: none;
  border-radius: 15px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
}

.modal-header {
  border-bottom: none;
  padding: 1.5rem;
  border-radius: 15px 15px 0 0;
}

.modal-body {
  padding: 1.5rem;
}

.modal-footer {
  border-top: none;
  padding: 1.5rem;
  border-radius: 0 0 15px 15px;
}

.card-title {
  font-weight: 600;
  font-size: 1.2rem;
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.list-group-item {
  border-color: rgba(0, 0, 0, 0.05);
  padding: 1rem 0;
  background: transparent;
}

.card {
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  transition: all 0.3s ease;
  background: white;
}

.preview-card {
  border-radius: 8px;
  overflow: hidden;
}

.preview-card img {
  height: 150px;
  object-fit: cover;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1);
}

.btn {
  border-radius: 8px;
  padding: 0.5rem 1rem;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-sm {
  padding: 0.4rem 0.8rem;
  font-size: 0.85rem;
  border-radius: 6px;
}

.btn-outline-primary, .btn-outline-success, .btn-outline-info {
  transition: all 0.3s ease;
  border-width: 2px;
}

.btn-outline-primary:hover, .btn-outline-success:hover, .btn-outline-info:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.form-control, .form-select {
  border-radius: 8px;
  padding: 0.6rem 1rem;
  border: 2px solid #e9ecef;
  transition: all 0.3s ease;
}

.form-control:focus, .form-select:focus {
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.15);
}

.input-group-text {
  border-radius: 8px;
  background-color: #f8f9fa;
  border: 2px solid #e9ecef;
}

.badge {
  transition: all 0.3s ease;
}

@media (max-width: 1200px) {
  .search-card,
  .orders-card {
    max-width: 98vw;
  }
}

@media (max-width: 768px) {
  .main-content {
    padding: 1rem 0;
  }
  h1 {
    font-size: 1.5rem;
  }
}

::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb {
  background: #c1c1c1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}

.spinner-border {
  width: 2rem;
  height: 2rem;
  border-width: 0.2em;
}

.text-center {
  color: #6c757d;
  font-size: 1.1rem;
  padding: 2rem;
}

h1 {
  text-align: center;
  font-size: 2.5rem;
  font-weight: bold;
  margin-bottom: 2rem;
  color: #AAAAAA;
  letter-spacing: 1px;
}
</style>