<template>
  <Header/>
    <div class="container py-5">
      <h1 class="mb-4">Order History</h1>
      
      <!-- Filter section -->
      <div class="card mb-4">
        <div class="card-body">
          <div class="row g-3">
            <div class="col-md-4">
              <label for="status" class="form-label">Status</label>
              <select v-model="filters.status" class="form-select" id="status">
                <option value="">All Status</option>
                <option value="completed">Completed</option>
                <option value="processing">Processing</option>
                <option value="cancelled">Cancelled</option>
              </select>
            </div>
            <div class="col-md-4">
              <label for="dateRange" class="form-label">Time Range</label>
              <select v-model="filters.dateRange" class="form-select" id="dateRange">
                <option value="all">All Time</option>
                <option value="30days">Last 30 Days</option>
                <option value="6months">Last 6 Months</option>
                <option value="1year">Last Year</option>
              </select>
            </div>
            <div class="col-md-4 d-flex align-items-end">
              <button @click="applyFilters" class="btn btn-primary w-100">Filter Orders</button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Orders list -->
      <div v-if="filteredOrders.length > 0">
        <div class="card mb-3" v-for="order in paginatedOrders" :key="order.id">
          <div class="card-header d-flex justify-content-between align-items-center">
            <div>
              <span class="fw-bold">Order #{{ order.id }}</span>
              <span class="ms-3 text-muted">{{ formatDate(order.createdAt) }}</span>
            </div>
            <div>
              <span :class="getStatusBadgeClass(order.status)">{{ getStatusText(order.status) }}</span>
            </div>
          </div>
          <div class="card-body">
            <div class="row">
              <div class="col-md-8">
                <div v-if="order.orderDetail">
                  <table class="table table-bordered table-sm mb-0">
                    <tbody>
                      <tr>
                        <th>Name</th>
                        <td>{{ order.orderDetail.customShoeDesignName }}</td>
                      </tr>
                      <tr>
                        <th>Description</th>
                        <td>{{ order.orderDetail.customShoeDesignDescription }}</td>
                      </tr>
                     
                      <tr>
                        <th>Preview Image</th>
                        <td>
                          <img v-if="order.orderDetail.firstPreviewImageUrl" :src="order.orderDetail.firstPreviewImageUrl" alt="Preview" style="max-width:80px;max-height:80px;">
                          <span v-else class="text-muted">No image</span>
                        </td>
                      </tr>
                      <tr>
                        <th>Quantity</th>
                        <td>{{ order.orderDetail.quantity }}</td>
                      </tr>
                      <tr>
                        <th>Service Price</th>
                        <td>{{ formatCurrency(order.orderDetail.servicePrice) }}</td>
                      </tr>
                      <tr>
                        <th>Size</th>
                        <td>{{ order.orderDetail.sizeValue }}</td>
                      </tr>
                      <tr>
                        <th>Template Price</th>
                        <td>{{ formatCurrency(order.orderDetail.templatePrice) }}</td>
                      </tr>
                      <tr>
                        <th>Unit Price</th>
                        <td>{{ formatCurrency(order.orderDetail.unitPrice) }}</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <div v-else>
                  <span class="text-muted">No product details available</span>
                </div>
              </div>
              <div class="col-md-4 border-start">
                <div class="d-flex justify-content-between mb-2">
                  <span>Total Payment:</span>
                  <span>{{ formatCurrency(order.totalPrice) }}</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Payment Method:</span>
                  <span>{{ order.paymentMethod }}</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Shipping Status:</span>
                  <span>{{ order.shippingStatus }}</span>
                </div>
                <div class="mt-3">
                  <button class="btn btn-outline-primary btn-sm w-100 mb-2">View Details</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- Pagination -->
        <nav aria-label="Page navigation">
          <ul class="pagination justify-content-center">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">Previous</a>
            </li>
            <li v-for="page in totalPages" :key="page" class="page-item" :class="{ active: page === currentPage }">
              <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
            </li>
            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">Next</a>
            </li>
          </ul>
        </nav>
      </div>
      
      <!-- Empty state -->
      <div v-else class="text-center py-5">
        <div class="mb-3">
          <i class="bi bi-bag-x" style="font-size: 3rem;"></i>
        </div>
        <h4>No Orders Found</h4>
        <p class="text-muted">You don't have any orders or no orders match the current filters.</p>
        <button class="btn btn-primary">Continue Shopping</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import { getOrdersByUserId } from '@/server/order-service';
import Header from '~/components/Header.vue';
  
  const orders = ref([]);
  const loading = ref(true);
  const error = ref(null);
  
  // Lấy userId từ localStorage
  const userId = localStorage.getItem('userId');
  
  onMounted(async () => {
    if (!userId) {
      error.value = 'Không tìm thấy userId.';
      loading.value = false;
      return;
    }
    try {
      const res = await getOrdersByUserId(userId);
      console.log('res', res);
      orders.value = res.orders || [];
    } catch (err) {
      error.value = err.message || 'Lỗi khi tải đơn hàng.';
    } finally {
      loading.value = false;
    }
  });
  
  // Filters
  const filters = ref({
    status: '',
    dateRange: 'all'
  });
  
  // Pagination
  const currentPage = ref(1);
  const itemsPerPage = 3;
  
  // Apply filters
  const applyFilters = () => {
    currentPage.value = 1;
  };
  
  // Filter orders based on selected filters
  const filteredOrders = computed(() => {
    let result = [...orders.value];
    
    // Filter by status
    if (filters.value.status) {
      result = result.filter(order => order.status === filters.value.status);
    }
    
    // Filter by date range
    if (filters.value.dateRange !== 'all') {
      const now = new Date();
      let startDate;
      
      switch (filters.value.dateRange) {
        case '30days':
          startDate = new Date(now.setDate(now.getDate() - 30));
          break;
        case '6months':
          startDate = new Date(now.setMonth(now.getMonth() - 6));
          break;
        case '1year':
          startDate = new Date(now.setFullYear(now.getFullYear() - 1));
          break;
        default:
          startDate = null;
      }
      
      if (startDate) {
        result = result.filter(order => new Date(order.createdAt) >= startDate);
      }
    }
    
    return result;
  });
  
  // Calculate total pages
  const totalPages = computed(() => {
    return Math.ceil(filteredOrders.value.length / itemsPerPage);
  });
  
  // Get paginated orders
  const paginatedOrders = computed(() => {
    const startIndex = (currentPage.value - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return filteredOrders.value.slice(startIndex, endIndex);
  });
  
  // Change page
  const changePage = (page) => {
    if (page >= 1 && page <= totalPages.value) {
      currentPage.value = page;
    }
  };
  
  // Format date
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };
  
  // Format currency
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };
  
  // Get status text
  const getStatusText = (status) => {
    switch (status) {
      case 'completed':
      case 'Processing':
        return 'Processing';
      case 'cancelled':
      case 'Cancelled':
        return 'Cancelled';
      case 'confirmed':
      case 'Completed':
        return 'Completed';
      default:
        return status;
    }
  };
  
  // Get status badge class
  const getStatusBadgeClass = (status) => {
    let baseClass = 'badge ';
    
    switch (status) {
      case 'completed':
      case 'Completed':
        return baseClass + 'bg-success';
      case 'processing':
      case 'Processing':
        return baseClass + 'bg-primary';
      case 'cancelled':
      case 'Cancelled':
        return baseClass + 'bg-danger';
      default:
        return baseClass + 'bg-secondary';
    }
  };
  </script>
  
  <style scoped>
  /* Add Bootstrap icons */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css");
  
  /* Add Bootstrap CSS */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css");
  
  .card {
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  }
  
  .card-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #eaeaea;
  }
  
  .img-thumbnail {
    border-radius: 6px;
  }
  
  .pagination .page-link {
    color: #777777;
  }
  
  .pagination .page-item.active .page-link {
    background-color: #777777;
    border-color: #777777;
  }
  </style>